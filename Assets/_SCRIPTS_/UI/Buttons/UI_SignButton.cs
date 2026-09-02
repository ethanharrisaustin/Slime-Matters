using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SignButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Transform graphic;
    [SerializeField] float wobbleAmount;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float wobbleTime;

    [SerializeField] float scaleAmount = 1;

    float startScale = 1;


    Vector3 startRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRot = graphic.localEulerAngles;

        startScale = transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        DoWobble();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        EndWobble();
    }

    void DoWobble()
    {
        graphic.DOKill(false);
        transform.DOKill(false);

        graphic.DOLocalRotate(startRot +  Vector3.forward * wobbleAmount, wobbleTime).SetEase(curve).OnComplete(() =>
        {
            graphic.DOLocalRotate(startRot, 0.3f);
        });

        transform.DOScale(startScale * scaleAmount, wobbleTime * 0.5f).SetEase(Ease.InOutQuad);
    }

    void EndWobble()
    {
        transform.DOKill(false);
        transform.DOScale(startScale, wobbleTime * 0.5f).SetEase(Ease.InOutQuad);
    }
}
