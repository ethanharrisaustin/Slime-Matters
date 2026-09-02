using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class Seasaw : MonoBehaviour
{
    [Range(2f, 25f),SerializeField] float plankLength;
    [Range(-180f, 180f),SerializeField] float plankStartRotation;
    [Space(20)]
    [Range(-90f, 90f),SerializeField] float maxLeftFall;
    [SerializeField] bool showLeftWoodenBit = true;
    [Space(10)]
    [Range(-90f, 90f),SerializeField] float maxRightFall;

    [SerializeField] bool showRightWoodenBit = true;

    [HideInInspector, SerializeField] Transform pivot;
    [HideInInspector, SerializeField] SpriteRenderer plank;
    [HideInInspector, SerializeField] BoxCollider2D plankCollider;
    [SerializeField] BoxCollider2D playerTrigger;

    [HideInInspector, SerializeField] Transform maxLeftPivot, maxLeftWood;
    [HideInInspector, SerializeField] SpriteRenderer maxLeftSprite;
    [HideInInspector, SerializeField] Transform maxRightPivot, maxRightWood;
    [HideInInspector, SerializeField] SpriteRenderer maxRightSprite;

    List<PlayerController> playersOnPlank = new List<PlayerController>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MakeSeasaw();

        playersOnPlank.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            MakeSeasaw();
        }

        #endif

        if (!Application.isPlaying) return;

        int numPlayersOnPlank = NumPlayersOnPlank();

        if (numPlayersOnPlank == 0)
        {
            RotateToNeutral();
        }
        else
        {
            RotateWithPlayers();
        }
    }

    void FixedUpdate()
    {
        ManageBounceBuffer();
    }

    float bounceBuffer = .5f;

    void ManageBounceBuffer()
    {
        bounceBuffer -= Time.fixedDeltaTime;
    }

    bool JustDidABounce()
    {
        return bounceBuffer >= 0f;
    }

    void MakeSeasaw()
    {
        plank.size = new Vector2(plankLength, plank.size.y);
        plankCollider.size = plank.size;
        playerTrigger.size = new Vector2(plankLength - 0.25f, plank.size.y);

        maxLeftSprite.enabled = showLeftWoodenBit;
        maxRightSprite.enabled = showRightWoodenBit;

        maxLeftPivot.localEulerAngles = new Vector3(0,0,maxLeftFall);
        maxLeftWood.localPosition = new Vector3(-plankLength/2f + 0.3f, maxLeftWood.localPosition.y);
    
        maxRightPivot.localEulerAngles = new Vector3(0,0,-maxRightFall);
        maxRightWood.localPosition = new Vector3(plankLength/2f - 0.3f, maxLeftWood.localPosition.y);
    
        pivot.localEulerAngles = new Vector3(0,0, plankStartRotation);

        zRotation = plankStartRotation;
    }

    float currentRotateSpeed = 0f;
    float zRotation = 0f;
    [SerializeField] float friction = 0.1f;
    [SerializeField] float bounceAmount = -0.8f;
    [SerializeField] float rotateSpeed = 20f;
    void RotateToNeutral()
    {
        float targetAngle = 0f;

        float difference = Mathf.DeltaAngle(zRotation, targetAngle);
        float rotateDirection = Mathf.Sign(difference);

        RotateSeasaw(rotateDirection);
    }

    void RotateWithPlayers()
    {
        float leftWeight = LeftWeight();
        float rightWeight = RightWeight();

        if (Mathf.Approximately(leftWeight, rightWeight))
        {
            RotateToNeutral();
            return;
        }

        float rotateDirection = -1;
        if (leftWeight > rightWeight) rotateDirection = 1;

        RotateSeasaw(rotateDirection * Mathf.Abs(leftWeight - rightWeight));
    }

    void RotateSeasaw(float direction)
    {
        currentRotateSpeed += direction * Time.deltaTime;

        // Apply friction
        currentRotateSpeed = Mathf.MoveTowards(currentRotateSpeed, 0f, Time.deltaTime * friction);

        zRotation += currentRotateSpeed * Time.deltaTime * rotateSpeed;

        if (SeasawNeedsToBounce())
        {
            currentRotateSpeed *= bounceAmount;

            bounceBuffer = .5f;
        }

        zRotation = Mathf.Clamp(zRotation, -maxRightFall, maxLeftFall);

        pivot.localEulerAngles = new Vector3(0,0, zRotation);
    }

    bool SeasawNeedsToBounce()
    {
        if (zRotation > 0 && currentRotateSpeed > 0 && zRotation > maxLeftFall) return true;

        if (zRotation < 0 && currentRotateSpeed < 0 && zRotation < -maxRightFall) return true;
        
        return false;
    }

    [SerializeField] float playerVelocityImpact = 30f;
    [SerializeField] float playerVelocityLaunchImpact= 30f;

    public void OnPlayerEnter(int player)
    {
        var pc = PlayerController.GetPlayer(player);

        playersOnPlank.Add(pc);

        if (playersOnPlank.Count > 1)
        {
            StartCoroutine(LaunchOtherPlayers(pc));
        }
    }

    public void OnPlayerExit(int player)
    {
        playersOnPlank.Remove(PlayerController.GetPlayer(player));
    }

    int NumPlayersOnPlank()
    {
        int number = 0;

        for (int i = 0; i < playersOnPlank.Count; ++i)
        {
            if (playersOnPlank[i] == null || playersOnPlank[i].IsDead()) continue;

            number++;
        }

        return number;
    }

    float LeftWeight()
    {
        float totalWeight = 0f;

        for (int i = 0; i < playersOnPlank.Count; ++i)
        {
            if (playersOnPlank[i] == null || playersOnPlank[i].IsDead()) continue;

            if (!PlayerIsOnLeft(playersOnPlank[i])) continue;

            totalWeight += PlayerWeight(playersOnPlank[i]);
        }

        return totalWeight;
    }

    float RightWeight()
    {
        float totalWeight = 0f;

        for (int i = 0; i < playersOnPlank.Count; ++i)
        {
            if (playersOnPlank[i] == null || playersOnPlank[i].IsDead()) continue;

            if (!PlayerIsOnRight(playersOnPlank[i])) continue;

            totalWeight += PlayerWeight(playersOnPlank[i]);
        }

        return totalWeight;
    }


    bool PlayerIsOnLeft(PlayerController player)
    {
        return player.transform.position.x < pivot.position.x;
    }

    bool PlayerIsOnRight(PlayerController player)
    {
        return player.transform.position.x > pivot.position.x;
    }

    float PlayerDistanceFromPivot(PlayerController player)
    {
        return Vector2.Distance(player.collision.lower.position, pivot.position);
    }

    float PlayerWeight(PlayerController player)
    {
        return PlayerDistanceFromPivot(player) * player.playerChangingSize.GetScale();
    }

    readonly WaitForFixedUpdate waitForFixedUpdate = new();

    IEnumerator LaunchOtherPlayers(PlayerController playerJustLanded)
    {
        float force = -playerJustLanded.movement.ySpeed;

        force -= 0.2f;

        force *= playerJustLanded.playerChangingSize.GetScale();

        if (force <= 0) yield break; // Exit out of coroutine

        float dir = PlayerIsOnLeft(playerJustLanded) ? +1 : -1;

        currentRotateSpeed = dir * force * playerVelocityImpact;

        var otherPlayers = PlayersToLaunch(playerJustLanded);

        int count = 200;
        while(count>0)
        {
            yield return waitForFixedUpdate;

            if (JustDidABounce())
            {
                break;
            }

            count--;
        }

        if (count <= 0) 
        {
            Debug.Log("Never did do any launching");;
            yield break;
        }

        Vector2 launchVelocity = new Vector2(-0.05f, 0.2f * (1f / otherPlayers.Count) * force * playerVelocityLaunchImpact);

        for (int i = 0; i < otherPlayers.Count; ++i)
        {
            float scale = otherPlayers[i].playerChangingSize.GetScale();
            float cappedScale = scale + 0.2f; // To remove dividing by 0 or 0.0001 (results in huge number)
            float distanceFromPivot = PlayerDistanceFromPivot(otherPlayers[i]);
            float multiplyAmount = 1f / cappedScale;

            otherPlayers[i].movement.Launch(launchVelocity * multiplyAmount * distanceFromPivot);
        }
    }

    List<PlayerController> PlayersToLaunch(PlayerController playerJustLanded)
    {
        if (PlayerIsOnLeft(playerJustLanded))
        {
            return PlayersOnRightExluding(playerJustLanded);
        }
        else if (PlayerIsOnRight(playerJustLanded))
        {
            return PlayersOnLeftExluding(playerJustLanded);
        }

        // return empty list without creating new object
        playersOnPlankExludingCahedResult.Clear();
        return playersOnPlankExludingCahedResult;
    }

    static List<PlayerController> playersOnPlankExludingCahedResult = new();
    List<PlayerController> PlayersOnPlankExluding(PlayerController playerToBeExluded)
    {
        playersOnPlankExludingCahedResult.Clear();

        for (int i = 0; i < playersOnPlank.Count; ++i)
        {
            if (playersOnPlank[i] == playerToBeExluded) continue;

            playersOnPlankExludingCahedResult.Add(playersOnPlank[i]);
        }

        return playersOnPlankExludingCahedResult;
    }

    List<PlayerController> PlayersOnLeft()
    {
        return PlayersOnLeftExluding(null);
    }

    List<PlayerController> PlayersOnLeftExluding(PlayerController playerToBeExluded)
    {
        playersOnPlankExludingCahedResult.Clear();

        for (int i = 0; i < playersOnPlank.Count; ++i)
        {
            if (!PlayerIsOnLeft(playersOnPlank[i])) continue;

            if (playersOnPlank[i] == playerToBeExluded) continue;

            playersOnPlankExludingCahedResult.Add(playersOnPlank[i]);
        }

        return playersOnPlankExludingCahedResult;
    }

    List<PlayerController> PlayersOnRight()
    {
        return PlayersOnRightExluding(null);
    }

    List<PlayerController> PlayersOnRightExluding(PlayerController playerToBeExluded)
    {
        playersOnPlankExludingCahedResult.Clear();

        for (int i = 0; i < playersOnPlank.Count; ++i)
        {
            if (!PlayerIsOnRight(playersOnPlank[i])) continue;

            if (playersOnPlank[i] == playerToBeExluded) continue;

            playersOnPlankExludingCahedResult.Add(playersOnPlank[i]);
        }

        return playersOnPlankExludingCahedResult;
    }
}   
