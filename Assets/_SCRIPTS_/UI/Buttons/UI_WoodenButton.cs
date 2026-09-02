using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_WoodenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public UnityEvent onClick;
    [SerializeField] Image[] highlight;
    [SerializeField] Transform scale;

    ImageToHighlight[] imageToHighlights;

    const float highlightAmount = 0.1f;
    const float pressAmount = -0.1f;

    const float scaleHighlightAmount = 0.16f;
    const float scalePressedAmount = 0.1f;

    const float fadeDuration = 0.25f;

    const Ease fadeEase = Ease.OutQuad;

    Vector2 normalScale, highlightScale, pressedScale;

    private class ImageToHighlight
    {
        readonly Image highlightImg;
        Color normalColour, highlightColour, pressedColour;

        public ImageToHighlight(Image highlight)
        {
            highlightImg = highlight;

            normalColour = highlight.color;

            // Set up Highlight Colour
            highlightColour = new Color
            {
                b = normalColour.b + highlightAmount,
                g = normalColour.g + highlightAmount,
                r = normalColour.r + highlightAmount,
                a = 1f
            };

            // Set up Pressed Colour
            pressedColour = new Color
            {
                b = normalColour.b + pressAmount,
                g = normalColour.g + pressAmount,
                r = normalColour.r + pressAmount,
                a = 1f
            };
        }

        public void Highlight()
        {
            SetTo(highlightColour);
        }

        public void Normal()
        {
            SetTo(normalColour);
        }

        public void Pressed()
        {
            SetTo(pressedColour);
        }

        void SetTo(Color colour)
        {
            highlightImg.DOKill(false);
            highlightImg.DOColor(colour, fadeDuration).SetEase(fadeEase);
        }
    }

    void Awake()
    {
        SetColours();
        SetScales();
    }

    void SetColours()
    {
        imageToHighlights = new ImageToHighlight[highlight.Length];
        
        for (int i = 0; i < imageToHighlights.Length; ++i)
        {
            imageToHighlights[i] = new ImageToHighlight(highlight[i]);    
        }
    }

    void SetScales()
    {
        normalScale = scale.localScale;
        highlightScale = normalScale * (1 + scaleHighlightAmount);
        pressedScale = normalScale * (1 + scalePressedAmount);
    }

    bool mouseOver = false;
    public void OnPointerEnter(PointerEventData data)
    {
        SetColourToHighlight();

        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (!mouseDown) SetColourToNormal();

        mouseOver = false;
    }

    bool mouseDown = false;
    public void OnPointerUp(PointerEventData data)
    {
        if (mouseOver)
            SetColourToHighlight();
        else 
            SetColourToNormal();
        
        mouseDown = false;
    }
    public void OnPointerDown(PointerEventData data)
    {
        SetColourToPressed();

        mouseDown = true;
    }

    public void OnPointerClick(PointerEventData data)
    {
        onClick.Invoke();
    }

    void SetColourToHighlight()
    {
        for (int i = 0; i < imageToHighlights.Length; ++i)
        {
            imageToHighlights[i].Highlight();
        }

        ScaleTo(highlightScale);
    }

    void SetColourToNormal()
    {
        for (int i = 0; i < imageToHighlights.Length; ++i)
        {
            imageToHighlights[i].Normal();
        }

        ScaleTo(normalScale);
    }

    void SetColourToPressed()
    {
        for (int i = 0; i < imageToHighlights.Length; ++i)
        {
            imageToHighlights[i].Pressed();
        }

        ScaleTo(pressedScale);
    }

    void ScaleTo(Vector2 targetScale)
    {
        scale.DOKill(false);
        scale.DOScale(targetScale, fadeDuration).SetEase(fadeEase).SetUpdate(false);
    }
}

