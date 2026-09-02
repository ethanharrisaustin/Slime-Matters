using UnityEngine;

public class PlayerMoveWithSwing : MonoBehaviour
{
    Transform player { get { return transform; } }
    [HideInInspector] public HangingPlatform currentSwing;
    PlayerController controller;
    PlayerControllerMovement movement;
    PlayerSprite playerSprite;
    PlayerControllerInput input;
    PlayerChangingSize changingSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<PlayerController>();
        input = GetComponent<PlayerControllerInput>();
        movement = GetComponent<PlayerControllerMovement>();
        playerSprite = GetComponent<PlayerController>().playerSprite;

        changingSize = controller.playerChangingSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerFollowSwing();
    }

    public void OnPlayerLandOnSwing(HangingPlatform hangingPlatform)
    {
        currentSwing = hangingPlatform;
    }

    public void OnPlayerGoOffSwing(HangingPlatform hangingPlatform)
    {
        if (hangingPlatform != currentSwing) return;

        currentSwing = null;
    }

    void PlayerFollowSwing()
    {
        //if (movement.IsJumping()) currentSwing = null;

        if (currentSwing == null) return;

        var fakePlayer = currentSwing.fakePlayerGroup.GetAlreadyShowingFakePlayer(controller);
        
        if (fakePlayer != null)
            player.position = fakePlayer.CenterPosition();
        
        changingSize.SetBuffer(-1);

        movement.SetYSpeedAs0();
    }

    public bool OnSwing()
    {
        return currentSwing != null;
    }

    public void OnSwingInput()
    {
        if (input.JumpInput())
        {
            currentSwing.JumpInput(controller);

            currentSwing = null;

            return;
        }

        if (input.LeftInput())
        {
            var fakePlayer = currentSwing.fakePlayerGroup.GetAlreadyShowingFakePlayer(controller);

            fakePlayer.transform.Translate(Vector2.left * controller.maxMoveSpeed, Space.Self);

            if (fakePlayer.transform.localPosition.x < -0.8f)
            {
                currentSwing.FallOffSwing(controller);
                return;
            }
        }

        if (input.RightInput())
        {
            var fakePlayer = currentSwing.fakePlayerGroup.GetAlreadyShowingFakePlayer(controller);

            fakePlayer.transform.Translate(Vector2.right * controller.maxMoveSpeed, Space.Self);

            if (fakePlayer.transform.localPosition.x > 0.8f)
            {
                currentSwing.FallOffSwing(controller);
                return;
            }
        }
    }
}
