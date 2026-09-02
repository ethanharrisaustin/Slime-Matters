using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopUpMenu : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 targetSizeDelta;

    PopUpMenuAnimationCurves curves;

    Canvas canvas;

    bool calledAwake = false;

    RectMask2D rectMask2D;

    UI_PopUpMenuDarkness darkness;

    protected virtual void Awake()
    {
        CloseImmediately();
        
        canvas = GetComponentInParent<Canvas>(true);
        rectMask2D = GetComponentInChildren<RectMask2D>();

        darkness = GetComponentInChildren<UI_PopUpMenuDarkness>();

        UI_PopUpMenuGraphic graphic = GetComponentInChildren<UI_PopUpMenuGraphic>();

        if (graphic != null)
        {
            rectTransform = graphic.GetComponent<RectTransform>();
        }
        else
        {
            rectTransform = GetComponent<RectTransform>();
        }
        
        targetSizeDelta = rectTransform.sizeDelta;

        calledAwake = true;
    }

    bool calledStart = false;
    protected virtual void Start()
    {
        curves = PopUpMenuAnimationCurves.instance;

        calledStart = true;
    }

    public virtual void Open()
    {
        if (!calledAwake) Awake();
        if (!calledStart) Start();

        canvas.enabled = true;

        gameObject.SetActive(true);

        InitOpenScale();

        if (darkness != null)
        {
            darkness.ShowDarkness(OpenScaleAnimation);
        }
        else
        {
            OpenScaleAnimation();
        }
    }

    void InitOpenScale()
    {
        rectTransform.sizeDelta = Vector2.up * targetSizeDelta;
        rectTransform.localScale = Vector2.right;
    }

    void OpenScaleAnimation()
    {
        rectMask2D.enabled = true;

        rectTransform.DOKill(false);
        
        rectTransform.DOSizeDelta(targetSizeDelta, curves.openTime).SetEase(curves.openX).SetUpdate(true);

        rectTransform.DOScale(1f, curves.openTime).SetEase(curves.openY).SetUpdate(true).OnComplete(() =>
        {
            rectMask2D.enabled = false;
        });
    }

    public virtual void Close()
    {
        CloseScaleAnimation(RemoveDarkness);
    }

    void CloseScaleAnimation(Action onComplete)
    {
        rectMask2D.enabled = true;

        rectTransform.DOKill(false);

        rectTransform.DOSizeDelta(Vector2.up * targetSizeDelta, curves.closeTime).SetEase(curves.closeX).SetUpdate(true);

        rectTransform.DOScale(Vector2.right, curves.closeTime).SetEase(curves.closeY).SetUpdate(true).OnComplete(() => 
        {
            if (UI_PopUpMenuDarkness.doSequentially)
            {
                onComplete.Invoke();
            }
        });

        if (!UI_PopUpMenuDarkness.doSequentially)
        {
            onComplete.Invoke();
        }
    }

    void RemoveDarkness()
    {
        if (darkness != null && UI_PopUpMenuDarkness.doSequentially)
        {
            darkness.RemovDarkness(CloseImmediately);
        }
        else
        {
            CloseImmediately();
        }
    }

    public void CloseImmediately()
    {
        gameObject.SetActive(false);
    }
}
