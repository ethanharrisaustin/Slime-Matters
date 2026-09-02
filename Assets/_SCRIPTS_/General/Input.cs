using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    static Input instance;

    public static Vector2 movement;

    private PlayerInput playerInput;

    public ControlScheme currentControlScheme;

    public static Action<ControlScheme> OnControlSchemeChangedEvent;

    public bool mouseDown;

    public static Vector2 mousePosition;

    public static bool leftPressed1, rightPressed1, upPressed1, jumpPressed1, downPressed1;
    public static bool leftPressed2, rightPressed2, upPressed2, jumpPressed2, downPressed2;

    public enum ControlScheme { Controller, KeyboardMouse, XboxController, PlaystationController, NitendoSwitch,  Error }

    public static Input Find()
    {
        if (instance == null) instance = FindFirstObjectByType<Input>();
        return instance;
    }

    public static Input Main()
    {
        if (instance == null) instance = FindFirstObjectByType<Input>();
        
        return instance;
    }

    void Init()
    {
        if (playerInput != null) return;

        playerInput = GetComponent<PlayerInput>();
    }

    void LateUpdate()
    {
        if (GameEnded())
        {
            leftPressed1 = false;
            rightPressed1 = false;
            upPressed1 = false;
            jumpPressed1 = false;
            downPressed1 = false;

            leftPressed2 = false;
            rightPressed2 = false;
            upPressed2 = false;
            jumpPressed2 = false;
            downPressed2 = false;
        }
    }

    public static bool GameEnded()
    {  
        if (UI_CompletionMenu.isOpen) return true;

        if (UI_DeathMenu.instance != null && UI_DeathMenu.instance.IsOpen()) return true;

        if (UI_PauseMenu.IsOpen()) return true;
        
        return false;
    }

    public void OnControlSchemeChanged()
    {
        CheckCurrentControlScheme();
    }

    void CheckCurrentControlScheme()
    {
        Init();

        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse": currentControlScheme = ControlScheme.KeyboardMouse; break;
            case "Gamepad": currentControlScheme = ControlScheme.Controller; break;
            case "XboxController": currentControlScheme = ControlScheme.XboxController; break;
            case "PlaystationController": currentControlScheme = ControlScheme.PlaystationController; break;
            case "NitendoSwitch": currentControlScheme = ControlScheme.NitendoSwitch; break;
            default: currentControlScheme = ControlScheme.Error; break;
        }

        OnControlSchemeChangedEvent?.Invoke(currentControlScheme);
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        // Mouse down
        if (value == 1)
        {
            if (mouseDown) return;

            mouseDown = true;          
        }
        // Mouse Up
        else if (value == 0)
        {
            mouseDown = false;
        }
        else
        {
            mouseDown = false;
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (value == 1)
        {

        }
        
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        leftPressed1 = IsPressed(context);
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        rightPressed1 = IsPressed(context);
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        upPressed1 = IsPressed(context);
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        downPressed1 = IsPressed(context);
    }

    public void OnLeft2(InputAction.CallbackContext context)
    {
        leftPressed2 = IsPressed(context);
    }

    public void OnRight2(InputAction.CallbackContext context)
    {
        rightPressed2 = IsPressed(context);
    }

    public void OnUp2(InputAction.CallbackContext context)
    {
        upPressed2 = IsPressed(context);
    }

    public void OnDown2(InputAction.CallbackContext context)
    {
        downPressed2 = IsPressed(context);
    }

    bool IsPressed(InputAction.CallbackContext context)
    {
        return context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Started;
    }
}
