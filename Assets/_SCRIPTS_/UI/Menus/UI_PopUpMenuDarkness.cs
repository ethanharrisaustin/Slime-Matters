using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UI_PopUpMenuDarkness : MonoBehaviour
{
    CanvasGroup canvasGroup;
    float targetAlpha;

    public static bool doSequentially = true;

    bool init = false;
    void Init()
    {
        if (init) return;
        init = true;

        canvasGroup = GetComponent<CanvasGroup>();
        targetAlpha = canvasGroup.alpha;

        canvasGroup.alpha = 0f;
    }

    public void ShowDarkness(Action onComplete)
    {
        Init();

        canvasGroup.DOKill(false);
        canvasGroup.DOFade(targetAlpha, 0.2f).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
        {
            if (doSequentially) onComplete.Invoke();
        });

        if (!doSequentially) onComplete.Invoke();
    }

    public void RemovDarkness(Action onComplete)
    {
        Init();

        canvasGroup.DOKill(false);
        canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
        {
            if (doSequentially) onComplete.Invoke();
        });

        if (!doSequentially) onComplete.Invoke();
    }
}
