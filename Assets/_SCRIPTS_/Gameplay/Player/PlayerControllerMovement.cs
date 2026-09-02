using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(PlayerControllerInput))]
public class PlayerControllerMovement : MonoBehaviour
{
    PlayerControllerInput input;
    PlayerController controller;
    PlayerControllerCollision collision;
    PlayerOnPlayerHead onPlayerHead;

    [SerializeField] float maxLaunchX, maxlaunchY;


    void Awake()
    {
        input = GetComponent<PlayerControllerInput>();
        controller = GetComponent<PlayerController>();
    }

    void Start()
    {
        collision = controller.collision;
        onPlayerHead = controller.onPlayerHead;
    }

    [HideInInspector] public float xSpeed, ySpeed = 0f;
    [HideInInspector] public float xLaunchSpeed, yLaunchSpeed = 0f;
    float launchBuffer = 0f;

    public void Move()
    {
        MoveX();
        MoveY();
        TranslatePlayer();

        IsGroundedTimer();

        ManageJumpBuffer();
    }

    void MoveX()
    {
        float xTarget = 0f;

        bool rightHitting = collision.RightHitting();
        bool leftHitting = collision.LeftHitting();

        if (input.LeftInput() && !leftHitting) xTarget = -controller.maxMoveSpeed;
        if (input.RightInput() && !rightHitting) xTarget = controller.maxMoveSpeed;

        if (!input.LeftInput() && !input.RightInput())
        {
            if (collision.NeedsToSlideOffEdge(out int dir))
            {
                transform.Translate(0.02f * dir * Vector2.right);
            }
        }

        xSpeed = Mathf.MoveTowards(xSpeed, xTarget, controller.acceleration);

        if (xSpeed + xLaunchSpeed > 0 && rightHitting)
        {
            xSpeed = 0;
            xLaunchSpeed = 0;
        }

        if (xSpeed + xLaunchSpeed< 0 && leftHitting)
        {
            xSpeed = 0;
            xLaunchSpeed = 0;
        }
    }


    void MoveY()
    {
        launchBuffer -= Time.fixedDeltaTime;

        float targetY = -controller.maxYSpeed;

        ySpeed = Mathf.MoveTowards(ySpeed, targetY, controller.gravity);

        if (yLaunchSpeed > 0) yLaunchSpeed = Mathf.MoveTowards(yLaunchSpeed, 0f, controller.gravity);

        if (collision.HittingLower()) 
        {
            ySpeed = 0f;
            if (yLaunchSpeed < 0 || launchBuffer < 0f) 
            {
                yLaunchSpeed = 0f;
                xLaunchSpeed = 0f;
            }
        }

        if (input.JumpInput() && CanJump())
        {
            Jump();
        }

        bool hittingAbove = HittingAbove();

        if (IsJumping() && hittingAbove)
        {
            MakeFalling();
        }

        if (hittingAbove)
        {
            yLaunchSpeed = 0;
        }
    }

    bool CanJump()
    {
        if (IsJumping()) 
        {
            return false;
        }

        if (timeOnGround <= 2) 
        {
            Debug.Log("Cannot jump because time on ground is too small.");
            return false;
        }

        if (HittingAbove()) 
        {
            Debug.Log("Cannot jump because our head is hitting something above us.");
            return false;
        }

        /*var otherPlayer = PlayerController.GetOtherPlayer(controller);

        if (otherPlayer != null && otherPlayer.onPlayerHead.IsOnPlayersHead()) 
        {
            Debug.Log("Cannot jump because we have a player on our head.");
            return false;
        }*/

        return true;
    }

    const float bufferBetweenJumps = 0.3f;
    float jumpBuffer = 0f;
    void ManageJumpBuffer()
    {
        jumpBuffer -= Time.fixedDeltaTime;
    }
    public void Jump()
    {
        if (jumpBuffer > 0f) return;

        ySpeed = controller.jumpHeight;

        jumpBuffer = bufferBetweenJumps;
    }

    public void MakeFalling()
    {
        const float fallingAmount = -0.01f;

        if (ySpeed > fallingAmount)
        {
            ySpeed = fallingAmount;
        }

        jumpBuffer = 0f;
    }


    public void SetYSpeedAs0()
    {
        ySpeed = 0;
    }

    public void SetXSpeedAs(float value)
    {
        xSpeed = value;
    }

    bool HittingAbove()
    {
        return collision.HittingHigher();
    }

    [HideInInspector] public int timeOnGround = 0;
    [HideInInspector] public int timeOffGrouond = 0;
    void IsGroundedTimer()
    {
        if (collision.HittingLower())
        {
            timeOnGround++;

            timeOffGrouond = 0;
        }
        else
        {
            timeOnGround = 0;

            timeOffGrouond++;
        }
    }

    void TranslatePlayer()
    {
        transform.Translate(new Vector3(xSpeed, ySpeed));
        transform.Translate(new Vector3(xLaunchSpeed, yLaunchSpeed));
    }

    public bool IsJumping()
    {
        return ySpeed > 0;
    }

    int shrinkingBuffer = 0;
    public bool NeedsToShrink()
    {
        if (input.LeftInput() || input.RightInput())
        {
            shrinkingBuffer = 8;
        }

        shrinkingBuffer--;

        return shrinkingBuffer >= 0;
    }

    public void Launch(Vector2 launchVelocity)
    {
        xLaunchSpeed = Mathf.Clamp(launchVelocity.x, -maxLaunchX, maxLaunchX);
        yLaunchSpeed = Mathf.Min(launchVelocity.y, maxlaunchY);

        launchBuffer = 0.3f;
    }
}
