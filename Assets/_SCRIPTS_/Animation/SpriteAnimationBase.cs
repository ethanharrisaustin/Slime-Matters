using UnityEngine;
using UnityEngine.UI;

public abstract class SpriteAnimationBase : MonoBehaviour
{
    public string animationKey;
    [SerializeField] bool playOnStart = true;
    [SerializeField] bool loop = true;

    protected float timer;
    protected int frame = 0;
    protected bool playing = true;

    protected ImageRenderer imageRenderer;

    protected class ImageRenderer
    {
        public SpriteRenderer spriteRenderer;
        public Image image;
        public Transform transform
        {
            get
            {
                if (spriteRenderer != null) return spriteRenderer.transform;

                if (image != null) return image.transform;

                return null;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            if (spriteRenderer != null) spriteRenderer.sprite = sprite;

            if (image != null) image.sprite = sprite;
        }

        public void Hide()
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;

            if (image != null) image.enabled = false;
        }

        public void Show()
        {
            if (spriteRenderer != null) spriteRenderer.enabled = true;

            if (image != null) image.enabled = true;
        }
    }

    protected virtual void Awake()
    {
        imageRenderer = new ImageRenderer();


        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        }

        Image image = null;
        if (spriteRenderer == null)
        {
            image = GetComponentInChildren<Image>();

            if (image == null)
            {
                image = transform.parent.GetComponentInChildren<Image>();
            }
        }

        imageRenderer.spriteRenderer = spriteRenderer;
        imageRenderer.image = image;

        playing = playOnStart;
    }

    void Start()
    {
        ShowFrame();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!playing) return;

        timer += Time.deltaTime;

        float fps = CurrentFPS();

        if (timer >= 1f / fps)
        {
            timer -= 1f / fps;

            frame = NextFrame();

            if (frame >= NumFrames()) 
            {
                if (loop) 
                {
                    frame = 0;
                }
                else
                {
                    frame = NumFrames() - 1;

                    playing = false;

                    return;
                }
            }

            ShowFrame();
        }
    }

    protected virtual float CurrentFPS()
    {
        return 10;
    }

    protected virtual int NumFrames()
    {
        return 0;
    }

    protected virtual void ShowFrame()
    {
        
    }

    protected virtual int NextFrame()
    {
        return frame + 1;
    }

    public virtual void GoToFrame(int frame)
    {
        this.frame = frame;

        ShowFrame();
    }

    public virtual void Pause()
    {
        playing = false;
    }

    public virtual void Stop()
    {
        frame = 0;
        playing = false;
        timer = 0f;
        ShowFrame();
    }

    public virtual void Play()
    {
        playing = true;

        ShowFrame();
    }

    public void Hide()
    {
        imageRenderer.Hide();
    }

    public void Show()
    {
        imageRenderer.Show();
    }
}
