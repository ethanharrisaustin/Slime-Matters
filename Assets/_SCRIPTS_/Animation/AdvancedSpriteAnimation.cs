using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class AdvancedSpriteAnimation : SpriteAnimationBase
{
    [SerializeField] int defaultFps;
    [SerializeField] Frame[] frames;

    [Serializable]
    class Frame
    {
        public Sprite sprite;
        [Tooltip("Leave at 0 or less to use the default FPS.")]
        public float fpsOverride;
        public Vector2 scale = Vector2.one;
        public Ease scaleEasing = Ease.Linear;
        public Vector2 localOffset = Vector2.zero;
        public Ease offsetEasing = Ease.Linear;
        [Tooltip("Leave at -1 or less to be ignored.")]
        public int goToFrame = -1;
    }

    Vector2 positionOffset;

    protected override void Awake()
    {
        base.Awake();

        positionOffset = imageRenderer.transform.localPosition;
    }

    protected override float CurrentFPS()
    {
        float fps = defaultFps;

        if (frames[frame].fpsOverride > 0) fps = frames[frame].fpsOverride;

        return fps;
    }

    protected override int NextFrame()
    {
        int returnValue = frame + 1;

        if (CurrentFrame().goToFrame >= 0)
        {
            returnValue = CurrentFrame().goToFrame;
        }

        return returnValue;
    }

    protected override void ShowFrame()
    {
        imageRenderer.SetSprite(frames[frame].sprite);

        DoFrameBounce();
    }

    protected override int NumFrames()
    {
        return frames.Length;
    }

    public override void Stop()
    {
        base.Stop();

        imageRenderer.transform.DOKill(false);
    }

    void DoFrameBounce()
    {
        imageRenderer.transform.DOKill(false);

        float animationTime = 1f / CurrentFPS();

        Vector2 scale = CurrentFrame().scale;

        if (scale != (Vector2)imageRenderer.transform.localScale)
        {
            imageRenderer.transform.DOScale(scale, animationTime).SetEase(CurrentFrame().scaleEasing);
        }

        Vector2 localPosition = positionOffset + CurrentFrame().localOffset;

        if (localPosition != (Vector2)imageRenderer.transform.localPosition)
        {
            imageRenderer.transform.DOLocalMove(localPosition, animationTime).SetEase(CurrentFrame().offsetEasing);
        }
    }


    Frame CurrentFrame()
    {
        return frames[frame];
    }
}
