using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerControllerInput : MonoBehaviour
{
    PlayerController playerController;

    int player { get { return playerController.player; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public bool LeftInput()
    {
        return player == 1 ?  Input.leftPressed1 : Input.leftPressed2;
    }

    public bool RightInput()
    {
        return player == 1 ?  Input.rightPressed1 : Input.rightPressed2;
    }

    public bool JumpInput()
    {
        return player == 1 ?  Input.upPressed1 : Input.upPressed2;
    }

    public bool DownInput()
    {
        return player == 1 ?  Input.downPressed1 : Input.downPressed2;
    }

    public bool AnyInput()
    {
        return LeftInput() || RightInput() || JumpInput();
    }
}
