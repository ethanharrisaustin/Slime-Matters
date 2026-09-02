using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

[ExecuteAlways]
public class HangingPlatform : MonoBehaviour, ISlimeStickOnto, IAttachedButtonTo
{
    List<PlayerController> playersOnPlatform = new List<PlayerController>();

    List<PlayerToIgnore> playersToIgnore = new List<PlayerToIgnore>();

    class PlayerToIgnore
    {
        public PlayerController controller;
        public int buffer;
        public const int startBuffer = 10;
        public bool makePlayerJump;

        public PlayerToIgnore(PlayerController controller)
        {
            this.controller = controller;
            buffer = startBuffer;
            makePlayerJump = controller.movement.IsJumping();
        }
    }

    public Transform bottom;
    public Transform playerFollowPos;
    [SerializeField] SpriteRenderer rope;
    [Range(0f, 20f)] public float length = 4f;
    [SerializeField] float swingAmount = 1f;
    [SerializeField] float speed = 1f;
    float timer = 0f;

    float c_bounce = 0f;
    float bounce_movement = 0f;

    public FakePlayerGroup fakePlayerGroup;

    // Update is called once per frame
    void Update()
    {
        Vector3 bottomPos = bottom.position;

        SetLength();
        Swing();
    }

    void FixedUpdate()
    {
        ManagePlayersToIgnore();
    }

    void SetLength()
    {
        bottom.localPosition = new Vector3(0f, -LengthToShow());

        rope.transform.localPosition = new Vector3(0f, -LengthToShow()*.5f);
        rope.size = new Vector2(rope.size.x, LengthToShow());
    }

    float LengthToShow()
    {
        return length + c_bounce;
    }

    float xVelocity;
    void Swing()
    {
        if (!Application.isPlaying) return;

        float prevPlayerPosX = playerFollowPos.position.x;

        timer += Time.deltaTime * speed;

        transform.rotation = Quaternion.Euler(0f,0f, swingAmount * Mathf.Sin(timer));

        float newPlayerPosX = playerFollowPos.position.x;

        xVelocity = newPlayerPosX - prevPlayerPosX;
    }

    public void OnPlayerEnter(int player)
    {
        PlayerController pc = PlayerController.GetPlayer(player);

        if (NeedsToIgnorePlayer(pc))
        {
            Debug.Log("Ignored player");
            return;
        } 

        if (pc.moveWithSwing.OnSwing()) return;

        fakePlayerGroup.ShowFakePlayer(pc);

        playersOnPlatform.Add(pc);

        pc.moveWithSwing.OnPlayerLandOnSwing(this);

        pc.SetAsActive(false);
    }

    public void OnPlayerExit(int player)
    {
        //PlayerController pc = PlayerController.GetPlayer(player);

        //playersOnPlatform.Remove(pc);

       // pc.moveWithSwing.OnPlayerGoOffSwing(this);
    }

    bool ContainsPlayer(PlayerController playerController)
    {
        for (int i = 0; i < playersOnPlatform.Count; ++i)
        {
            if (playersOnPlatform[i].IsDead()) continue;

            if (playersOnPlatform[i] == playerController)
            {
                return true;
            }
        }

        return false;
    }

    bool ContainsIgnoredPlayer(PlayerController playerController)
    {
        for (int i = 0; i < playersToIgnore.Count; ++i)
        {
            if (playersToIgnore[i].controller.IsDead()) continue;

            if (playersToIgnore[i].controller == playerController)
            {
                return true;
            }
        }

        return false;
    }

    void MakePlayerOffSwing(PlayerController playerController)
    {
        playerController.playerSprite.ShowPlayer(fakePlayerGroup.GetAlreadyShowingFakePlayer(playerController));
        playersOnPlatform.Remove(playerController);
        fakePlayerGroup.HideFakePlayer(playerController);
        playerController.moveWithSwing.OnPlayerGoOffSwing(this);
        playersToIgnore.Add(new PlayerToIgnore(playerController));
    }

    public void JumpInput(PlayerController playerController)
    {
        playerController.SetAsActive(true);

        playerController.movement.Jump();

        MakePlayerOffSwing(playerController);
    }

    public void FallOffSwing(PlayerController playerController)
    {
        playerController.SetAsActive(true);

        playerController.movement.MakeFalling();

        MakePlayerOffSwing(playerController);
    }
    
    public void MoveRightInput()
    {
        
    }

    public void MoveLeftInput()
    {
        
    }

    public float CurrentWeight()
    {
        float weight = 0f;
        
        for (int i = 0; i < playersOnPlatform.Count; ++i)
        {
            weight += playersOnPlatform[i].playerChangingSize.CurrentWeight();
        }

        return weight;
    }

    public Transform GetSlimeParent()
    {
        return bottom;
    }

    bool NeedsToIgnorePlayer(PlayerController controller)
    {
        for (int i = 0; i < playersToIgnore.Count; ++i)
        {
            if (playersToIgnore[i].controller == controller) return true;
        }

        return false;
    }

    void ManagePlayersToIgnore()
    {
        for (int i = 0; i < playersToIgnore.Count;)
        {
            if (playersToIgnore[i].buffer > PlayerToIgnore.startBuffer - 5)
            {
                if (playersToIgnore[i].makePlayerJump)
                {
                    playersToIgnore[i].controller.movement.Jump();
                }
                else
                {
                    playersToIgnore[i].controller.movement.MakeFalling();
                }
            }    

            playersToIgnore[i].buffer -= 1;

            if (playersToIgnore[i].buffer <= 0f)
            {
                playersToIgnore.RemoveAt(i);
                continue;
            }

            ++i;
        }
    }

    public float CurrentXVelocity()
    {
        return xVelocity;
    }

    public bool CanPressButton(PlayerController playerController)
    {
        return ContainsPlayer(playerController);
    }

    public bool PlayerToBeIgnored(PlayerController playerController)
    {
        return ContainsIgnoredPlayer(playerController);
    }

    public static bool DoNotCollideWithSwing(Collider2D collider2D, PlayerController controller)
    {
        if (collider2D == null) return false;

        HangingPlatform hangingPlatform = collider2D.GetComponentInParent<HangingPlatform>();

        if (hangingPlatform == null) return false;

        return hangingPlatform.PlayerToBeIgnored(controller);
    }
}
