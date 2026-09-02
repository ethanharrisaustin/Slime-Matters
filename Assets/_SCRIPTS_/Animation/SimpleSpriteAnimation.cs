using UnityEngine;

public class SimpleSpriteAnimation : SpriteAnimationBase
{
    [SerializeField] int fps;
    [SerializeField] Sprite[] sprites;

    protected override void ShowFrame()
    {
        imageRenderer.SetSprite(sprites[frame]);
    }

    protected override float CurrentFPS()
    {
        return fps;
    }

    protected override int NumFrames()
    {
        return sprites.Length;
    }

    public override void Play()
    {
        playing = true;
    }
}
