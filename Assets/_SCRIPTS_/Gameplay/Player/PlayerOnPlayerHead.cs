using UnityEngine;

public class PlayerOnPlayerHead : MonoBehaviour
{
    PlayerControllerInput input;
    PlayerController controller;
    PlayerControllerCollision collision;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<PlayerController>();

        input = controller.input;
        collision = controller.collision;
    }

    public void Move()
    {
        CheckLandedOnPlayerHead();
        MoveWithUnderneathPlayer();
    }

    bool onPlayersHead = false;
    int onPlayersHeadBuffer = -1;
    int onPlayersHeadBufferAmount = 7;
    float onPlayerHeadXOffset;
    PlayerController underneathPlayer;

    void CheckLandedOnPlayerHead()
    {
        onPlayersHead = false;

        if (controller.movement.IsJumping()) 
        {
            onPlayersHeadBuffer = -1;
            underneathPlayer = null;
            return;
        }

        if (!HittingPlayer(out var playerController)) return;

        float otherPlayerPos = playerController.transform.position.y;
        float ourPos = transform.position.y;

        bool otherPlayerBelowUs = otherPlayerPos < ourPos - 0.2f;

        if (!otherPlayerBelowUs) return;

        underneathPlayer = playerController;

        onPlayersHead = true;

        if (!IsOnPlayersHead() || DoOnHeadCollisions())
        {
            onPlayerHeadXOffset = transform.position.x - playerController.transform.position.x;
        }
    }

    bool HittingPlayer(out PlayerController playerController)
    {
        if (HittingPlayer(controller.collision.lower, out playerController)) return true;
        if (HittingPlayer(controller.collision.lowerLeft, out playerController)) return true;
        if (HittingPlayer(controller.collision.lowerRight, out playerController)) return true;

        playerController = null;
        return false;
    }

    bool HittingPlayer(Transform transform, out PlayerController playerController)
    {
        if (collision.Hitting(transform, out Collider2D collider2D))
        {
            playerController = collider2D.GetComponentInParent<PlayerController>();

            return playerController != null;
        }

        playerController = null;
        return false;
    }

    void MoveWithUnderneathPlayer()
    {
        DoOnHeadBuffer();

        if (!IsOnPlayersHead()) return;

        if (underneathPlayer == null) return;

        if (input.AnyInput())
        {
            MoveWithUnderneathPlayerInAdditionToInput();
        }
        else 
        {
            SnapToUnderneathPlayerWithOffset();
        }

        ReducePlayerHeadOffsetAsItShrinks();
    }

    void MoveWithUnderneathPlayerInAdditionToInput()
    {
        transform.Translate(Vector3.right * underneathPlayer.moveDelta.x);

        // Snap Y
        if (!input.JumpInput())
        {
            transform.position = new Vector3(
                transform.position.x,
                underneathPlayer.collision.higher.position.y + collision.Offset().y
            );
        }

        onPlayerHeadXOffset = transform.position.x - underneathPlayer.transform.position.x;
    }

    void SnapToUnderneathPlayerWithOffset()
    {   
        if (DoOnHeadCollisions()) return;

        transform.position = new Vector3(
                underneathPlayer.transform.position.x + onPlayerHeadXOffset,
                underneathPlayer.collision.higher.position.y + collision.Offset().y
            );
    }

    bool DoOnHeadCollisions()
    {
        if (CollidingLeft())
        {
            return true;
        }

        if (CollidingRight())
        {
            return true;
        }

        return false;
    }

    bool CollidingLeft()
    {
        if (controller.moveDelta.x >= -0.001f) return false;

        if (collision.Hitting(collision.left)) return true;

        if (collision.Hitting(collision.leftUpper)) return true;

        return false;
    }

    bool CollidingRight()
    {
        if (controller.moveDelta.x <= 0.001f) return false;

        if (collision.Hitting(collision.right)) return true;

        if (collision.Hitting(collision.rightUpper)) return true;

        return false;
    }

    void ReducePlayerHeadOffsetAsItShrinks()
    {
        if (Mathf.Abs(controller.moveDelta.x) > 0.02f) 
        {
            onPlayerHeadXOffset = Mathf.MoveTowards(onPlayerHeadXOffset, 0f, 0.006f);
        }
    }

    void DoOnHeadBuffer()
    {
        if (!onPlayersHead) onPlayersHeadBuffer --;
        else onPlayersHeadBuffer = 1;
    }

    public bool IsOnPlayersHead()
    {
        return onPlayersHeadBuffer >= -onPlayersHeadBufferAmount;
    }

}
