using UnityEngine;

public class SpriteAnimationGroup : MonoBehaviour
{
    [SerializeField] string defaultAnimation;

    SpriteAnimationBase[] spriteAnimations;
    SpriteAnimationBase currentAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteAnimations = GetComponentsInChildren<SpriteAnimationBase>();

        PlayAnimation(defaultAnimation);
    }

    public void PlayAnimation(string key)
    {
        if (AlreadyPlaying(key)) return;

        StopCurrent();

        for (int i = 0; i < spriteAnimations.Length; ++i)
        {
            if (spriteAnimations[i].animationKey == key)
            {
                currentAnimation = spriteAnimations[i];
                break;
            }
        }

        PlayCurrent();
    }

    void StopCurrent()
    {
        if (currentAnimation == null) return;
        
        currentAnimation.Stop();
        currentAnimation.Hide();
    }

    void PlayCurrent()
    {
        if (currentAnimation == null) return;
        
        currentAnimation.Play();
        currentAnimation.Show();
    }

    bool AlreadyPlaying(string key)
    {
        if (currentAnimation == null) return false;
        
        return currentAnimation.animationKey == key;
    }
}
