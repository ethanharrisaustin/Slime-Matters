using DG.Tweening;
using UnityEngine;

public class UI_MobileJoystick : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] RectTransform joystick;

    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] bool player1 = true;

    [SerializeField] float maxDistance;

    [SerializeField] float allowence;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup.alpha = 0.5f;
    }


    public void SetJoystickPosition(Vector2 position)
    {
        rectTransform.position = position;

        canvasGroup.DOKill(false);
        joystick.DOKill(false);

        canvasGroup.alpha = 1f;

        inNeutral = false;
    }

    public void SetHandlePosition(Vector2 position)
    {
        joystick.position = position;

        var clampedPos = Vector2.ClampMagnitude(joystick.localPosition, maxDistance);
        joystick.localPosition = clampedPos;

        var handlePosition = joystick.localPosition;

        SetIsRight(handlePosition.x > allowence);

        SetIsLeft(handlePosition.x < -allowence);

        SetIsJumping(handlePosition.y > allowence);
    }

    bool inNeutral = false;    
    public void ReturnHandleToNeutral()
    {
        if (inNeutral) return;

        canvasGroup.DOKill(false);
        joystick.DOKill(false);
        
        canvasGroup.DOFade(0.5f, 0.35f);
        joystick.DOLocalMove(Vector2.zero, 0.4f).SetEase(Ease.OutBack);

        SetIsJumping(false);
        SetIsLeft(false);
        SetIsRight(false);

        inNeutral = true;
    }

    void SetIsJumping(bool jumping)
    {
        if (player1)
        {
            Input.upPressed1 = jumping;
        }
        else
        {
            Input.upPressed2 = jumping;
        }
    }

    void SetIsLeft(bool left)
    {
        if (player1)
        {
            Input.leftPressed1 = left;
        }
        else
        {
            Input.leftPressed2 = left;
        }
    }

    void SetIsRight(bool right)
    {
        if (player1)
        {
            Input.rightPressed1 = right;
        }
        else
        {
            Input.rightPressed2 = right;
        }
    }
}
