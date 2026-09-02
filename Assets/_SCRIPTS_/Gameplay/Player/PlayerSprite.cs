using DG.Tweening;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] SpriteAnimationGroup animationGroup;

    [SerializeField] PlayerController playerController;

    public Transform actualSprite;
    [SerializeField] Transform tweenObjectHolder;
    public Transform tweenObject;
    [SerializeField]  SpriteRenderer spriteRenderer;
    public Vector3 offset;

    // Update is called once per frame
    [HideInInspector] public Vector2 refVelocity;
    Vector3 refAngleVel;
    void Update()
    {
        tweenObjectHolder.position = playerController.collision.lower.position;

        FollowPlayer();
        DoAnimations();
    }

    void FollowPlayer()
    {
        transform.position = Vector2.SmoothDamp(
            transform.position,
            TargetGraphic().position,
            ref refVelocity,
            1f/40f,
            999999f,
            Time.deltaTime
        );
        
        transform.rotation = Quateniextras.SmoothDamp(
            transform.rotation,
            TargetGraphic().rotation,
            ref refAngleVel,
            1f/40f,
            999999f,
            Time.deltaTime
        );
    }
    

    void DoAnimations()
    {
        if (playerController.movement.xSpeed != 0)
        {
            animationGroup.PlayAnimation("walking");
        }
        else
        {
            animationGroup.PlayAnimation("idle");
        }
    }

    public PlayerSpriteInfo CurrentSprite()
    {
        return new PlayerSpriteInfo(spriteRenderer);
    }

    public void HidePlayer()
    {
        //spriteRenderer.enabled = false;
    }

    public void ShowPlayer()
    {
        tweenObjectHolder.position = playerController.collision.lower.position;

        spriteRenderer.enabled = true;
    }

    public void ShowPlayer(FakePlayer fakePlayer)
    {
        ShowPlayer();

        if (fakePlayer == null) return;

        tweenObject.DOKill(false);
        tweenObject.position = fakePlayer.transform.position;
        tweenObject.rotation = fakePlayer.transform.rotation;

        tweenObject.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad);
        tweenObject.DOLocalRotate(Vector3.zero, 0.45f).SetEase(Ease.InOutQuad);
    }

    public Transform TargetGraphic()
    {
        if (FakePlayer.HasFakePlayer(playerController, out Transform targetGraphic))
        {
            return targetGraphic;
        }

        return tweenObject;
    }
}

public class PlayerSpriteInfo
{
    public Sprite sprite;
    public Color colour;

    public PlayerSpriteInfo(SpriteRenderer spriteRenderer)
    {
        sprite = spriteRenderer.sprite;
        colour = spriteRenderer.color;
    }

    public PlayerSpriteInfo(Sprite sprite, Color colour)
    {
        this.sprite = sprite;
        this.colour = colour;
    }
}