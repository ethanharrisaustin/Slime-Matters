using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControllerInput))]
[RequireComponent(typeof(PlayerControllerMovement))]
[RequireComponent(typeof(PlayerControllerCollision))]
[RequireComponent(typeof(PlayerOnPlayerHead))]
public class PlayerController : MonoBehaviour, ISlimeStickOnto
{
    [Header("Player Settings")]
    public int player = 1;
    public float acceleration = 0.01f;
    public float maxMoveSpeed = 0.1f;
    public float gravity;
    public float maxYSpeed;
    public float jumpHeight;

    static List<PlayerController> players = new List<PlayerController>();

    [HideInInspector] public PlayerControllerInput input;
    [HideInInspector] public PlayerControllerMovement movement;
    [HideInInspector] public PlayerControllerCollision collision;
    [HideInInspector] public PlayerOnPlayerHead onPlayerHead;
    [HideInInspector] public PlayerMoveWithSwing moveWithSwing;
    public PlayerChangingSize playerChangingSize;
    public PlayerSprite playerSprite;

    

    void Awake()
    {
        players.Clear();

        input = GetComponent<PlayerControllerInput>();
        movement = GetComponent<PlayerControllerMovement>();
        collision = GetComponent<PlayerControllerCollision>();
        onPlayerHead = GetComponent<PlayerOnPlayerHead>();
        moveWithSwing = GetComponent<PlayerMoveWithSwing>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var childSprite = GetComponentInChildren<SpriteRenderer>();
        if (childSprite != null) childSprite.enabled = false;

        players.Add(this);
    }

    void FixedUpdate()
    {
        if (IsDead()) return;

        if (moveWithSwing.OnSwing())
        {
            moveWithSwing.OnSwingInput();
            return;
        }

        if (!IsActive()) return;

        collision.ResetShouldColliders();

        movement.Move();

        collision.Collide();

        onPlayerHead.Move();

        MoveDelta();
    }

    Vector3 previousPos = Vector3.one;
    [HideInInspector] public Vector3 moveDelta;
    void MoveDelta()
    {
        moveDelta = transform.position - previousPos;
        
        previousPos = transform.position;
    }

    bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public void SetAsDead(bool dead = true)
    {
        isDead = dead;

        if (dead) transform.parent.gameObject.SetActive(false);
    }

    bool isActive = true;

    public bool IsActive()
    {
        return isActive;
    }

    public void SetAsActive(bool active = true)
    {
        isActive = active;
    }

    public static void DeactivateAllPlayers()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] == null) continue;

            players[i].SetAsActive(false);
        }
    }

    public static PlayerController GetPlayer(int player)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].player == player) return players[i];
        }
        return null;
    }

    public static PlayerController GetOtherPlayer(PlayerController playerController)
    {
        if (playerController.player == 1)
        {
            return GetPlayer(2);
        }

        return GetPlayer(1);
    }

    public static PlayerController GetOnHeadPlayer()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].onPlayerHead.IsOnPlayersHead()) return players[i];
        }
        return null;
    }

    public static PlayerController GetUnderneathPlayer()
    {
        bool oneIsOnHead = false;
        PlayerController notOnHead = null;

        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].onPlayerHead.IsOnPlayersHead()) 
            {
                oneIsOnHead = true;
            }

            else
            {
                notOnHead = players[i];
            }
        }

        if (oneIsOnHead == false) return null;

        return notOnHead;
    }

    public Transform GetSlimeParent()
    {
        return playerSprite.actualSprite;
    }

    public SlimeColour ColourOfPlayer()
    {
        if (player == 1) return SlimeColour.green;

        return SlimeColour.orange;
    }
}
