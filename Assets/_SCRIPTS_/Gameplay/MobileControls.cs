using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class MobileControls : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] UI_MobileJoystick leftJoystick, rightJoystick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bool needsToShow = Application.isMobilePlatform && SceneManager.GetActiveScene().buildIndex != 0;
        gameObject.SetActive(needsToShow);
        canvas.enabled = true;
        canvasGroup.alpha = 0f;
    }

    void Start()
    {
        bool gameEnded = Input.GameEnded();
        canvas.enabled = !gameEnded;
        ForceShowHideMobileControls(!gameEnded);
        if (gameEnded) return;
    }

    Touchscreen touchscreen;

    // Update is called once per frame
    void Update()
    {
        bool gameEnded = Input.GameEnded();
        canvas.enabled = !gameEnded;
        ShowHideMobileControls(!gameEnded);
        if (gameEnded) return;

        if (touchscreen == null) 
        {
            touchscreen = Touchscreen.current;

            if (touchscreen == null) return;
        }

        ManageTouches();
    }

    void ManageTouches()
    {
        managedLeftTouch = false;
        managedRightTouch = false;

        var touches = touchscreen.touches.ToArray();

        for (int i = 0; i < touches.Length; ++i)
        {
            if (touches[i].ReadValue().startPosition == Vector2.zero) continue;

            if (touches[i].ReadValue().isNoneEndedOrCanceled) continue;

            ManageTouch(touches[i]);
        }

        if (!managedLeftTouch) ReturnLeftJoystickToNeutral();
        if (!managedRightTouch) ReturnRightJoystickToNeutral();
    }


    void ManageTouch(TouchControl touch)
    {
        if (touch.startPosition.x.ReadValue() < Screen.width * .5f)
        {
            ManageLeftScreenTouch(touch);
        }
        else
        {
            ManageRightScreenTouch(touch);
        }
    }

    bool managedLeftTouch = false;
    void ManageLeftScreenTouch(TouchControl touch)
    {
        if (managedLeftTouch) return;

        managedLeftTouch = true;

        leftJoystick.SetJoystickPosition(touch.startPosition.ReadValue());

        leftJoystick.SetHandlePosition(touch.position.ReadValue());
    }

    bool managedRightTouch = false;
    void ManageRightScreenTouch(TouchControl touch)
    {
        if (managedRightTouch) return;

        managedRightTouch = true;

        rightJoystick.SetJoystickPosition(touch.startPosition.ReadValue());

        rightJoystick.SetHandlePosition(touch.position.ReadValue());
    }

    void ReturnLeftJoystickToNeutral()
    {
        leftJoystick.ReturnHandleToNeutral();
    }

    void ReturnRightJoystickToNeutral()
    {
        rightJoystick.ReturnHandleToNeutral();
    }

    bool prevShowing = false;
    void ShowHideMobileControls(bool showing)
    {
        if (showing == prevShowing) return;

        ForceShowHideMobileControls(showing);
    }

    void ForceShowHideMobileControls(bool showing)
    {
        canvasGroup.DOKill(false);

        canvasGroup.DOFade(showing ? 1f : 0f, 0.35f).SetUpdate(true);
    }
}
