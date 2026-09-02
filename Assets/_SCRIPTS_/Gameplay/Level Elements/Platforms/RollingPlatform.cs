using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RollingPlatform : MonoBehaviour
{
    [HideInInspector] [SerializeField] float rollSpeed = 10;
    [Range(-1, 1)] public int currentDirection = 0;
    [Tooltip("When ChangeDirection() is called and the platform is stopped and not currently rolling in either direction, 'preferred direction' is chosen.")]
    [Range(-1, 1)] public int preferredDirection = 0;
    
    [Range(2, 40)] public int length = 4;
    public GameObject cogPrefab;
    public ButtonColour colour;
    [HideInInspector] public SpinningCog[] cogs;

    bool player1On, player2On = false;

    static List<RollingPlatform> rollingPlatforms = new();

    void Awake()
    {
        rollingPlatforms.Clear();
    }

    void Start()
    {
        if (!rollingPlatforms.Contains(this)) 
            rollingPlatforms.Add(this);

        CreateCogs();
    }

    int previousLength = -1;
    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        
        if (LevelPreview.Showing())
        {
            if (length != previousLength) CreateCogs();
            previousLength = length;
        }
        else
        {
            RemoveCogs();
            previousLength = -1;
        }

        if (Application.isPlaying && !rollingPlatforms.Contains(this))
        {
            rollingPlatforms.Add(this);
        }
        
        #endif

        if (cogs == null) return;

        Vector3 rotateAmount = -0.7f * currentDirection * rollSpeed * Time.deltaTime * Vector3.forward;

        for (int i = 0; i < cogs.Length; ++i)
        {
            if (cogs[i] == null) continue;

            cogs[i].Spin(rotateAmount);
        }
    }

    void CreateCogs()
    {
        if (cogPrefab == null) return;

        RemoveCogs();

        for (int i = 2; i < length; ++i)
        {
            GameObject newCog = Instantiate(cogPrefab, transform);

            newCog.transform.localPosition = 0.8f * i * Vector2.right;
        }

        cogs = GetComponentsInChildren<SpinningCog>();

        BoxCollider2D[] boxCollider2Ds = GetComponentsInChildren<BoxCollider2D>();

        for (int i = 0; i < boxCollider2Ds.Length; ++i)
        {
            boxCollider2Ds[i].transform.localScale = new Vector3(length, 1);
        }
    }


    void RemoveCogs()
    {
        SpinningCog[] spriteRenderers = GetComponentsInChildren<SpinningCog>(true);

        for (int i = 2; i < spriteRenderers.Length; ++i)
        {
            #if UNITY_EDITOR
            DestroyImmediate(spriteRenderers[i].gameObject);
            #else
            Destroy(spriteRenderers[i].gameObject);
            #endif
        }

    }

    void FixedUpdate()
    {
        if (player1On)
        {
            MovePlayer(PlayerController.GetPlayer(1));
        }

        if (player2On)
        {
            MovePlayer(PlayerController.GetPlayer(2));
        }
    }

    void MovePlayer(PlayerController playerController)
    {
        if (currentDirection > 0)
        {
            playerController.collision.cantCollideLeft = true;
            playerController.collision.cantCollideRight = false;

            if (playerController.collision.RightHitting()) return;
        }
        else
        {
            playerController.collision.cantCollideRight = true;
            playerController.collision.cantCollideLeft = false;

            if (playerController.collision.LeftHitting()) return;
        }
        

        playerController.transform.Translate(
            currentDirection * Time.fixedDeltaTime * rollSpeed * 0.01f,
            0f,
            0f 
        );
    }

    public void PlayerEntered(int player)
    {
        if (player == 1)
        {
            player1On = true;
        }
        else
        {
            player2On = true;
        }
    }

    public void PlayerExited(int player)
    {
        if (player == 1)
        {
            player1On = false;
        }
        else
        {
            player2On = false;
        }
    }

    public void SetDirectionToLeft()
    {
        currentDirection = -1;
    }

    public void SetDirectionToRight()
    {
        currentDirection = 1;
    }

    public void ChangeDirection()
    {
        if (currentDirection == 0)
        {
            currentDirection = preferredDirection;
            return;
        }

        currentDirection *= -1;
    }

    public void Stop()
    {
        currentDirection = 0;
    }

    public static void SwitchPlatforms(ButtonColour colour)
    {
        if (rollingPlatforms == null) return;

        for (int i = 0; i < rollingPlatforms.Count; ++i)
        {
            if (rollingPlatforms[i] == null) continue;

            if (rollingPlatforms[i].colour != colour) continue;

            rollingPlatforms[i].ChangeDirection();
        }
    }
}
