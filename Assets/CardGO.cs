using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardGO : MonoBehaviour
{
    [SerializeField] Transform desk;

    [SerializeField] Transform icon;

    [SerializeField] float showPlayerYPos;

    [SerializeField] float cardMoveTime;

    Vector3 iconStartScale;

    void Awake()
    {
        iconStartScale = icon.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            FlipToShowPlayer();
        }

        if (Keyboard.current.backspaceKey.isPressed)
        {
            FlipOntoDesk();
        }
    }

    public void FlipOntoDesk()
    {
        transform.DOKill(false);

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            desk.position.y + 0.2f,
            transform.position.z
        );
        
        Vector3 targetEulerAngles = new Vector3(
            -90,
            0,
            0
        );

        float iconTargetScale = 0f;


        transform.DOMove(targetPosition, cardMoveTime).SetEase(Ease.InOutQuad);
        transform.DORotate(targetEulerAngles, cardMoveTime).SetEase(Ease.InOutQuad);

        icon.DOScale(iconTargetScale, cardMoveTime).SetEase(Ease.InOutQuad);
    }

    public void FlipToShowPlayer()
    {
        transform.DOKill(false);

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            showPlayerYPos,
            transform.position.z
        );
        
        Vector3 targetEulerAngles = new Vector3(
            12.2f,
            0,
            0
        );

        float iconTargetScale = 1f;

        transform.DOMove(targetPosition, cardMoveTime).SetEase(Ease.InOutQuad);
        transform.DORotate(targetEulerAngles, cardMoveTime).SetEase(Ease.InOutQuad);

        icon.DOScale(iconStartScale * iconTargetScale, cardMoveTime).SetEase(Ease.InOutQuad);
    }
}
