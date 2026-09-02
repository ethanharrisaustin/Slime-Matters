using DG.Tweening;
using UnityEngine;

public class PlayerSlimePiece : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public void SetUp(PlayerController playerController, PlayerChangingSize playerChangingSize)
    {
        SetScale(playerChangingSize);

        SetParent(playerController);

        SetZOrder(playerChangingSize);

        AnimationScale();

        //if (target == null) enabled = false;
    }

    void SetScale(PlayerChangingSize playerChangingSize)
    {
        Vector3 targetScale = new Vector3(
            Random.Range(0.6f, 1f),
            Random.Range(0.9f, 1.4f)
        ) * (playerChangingSize.GetActualScale() + 0.5f);

        transform.localScale = targetScale;
    }

    void SetParent(PlayerController playerController)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, 
            PlayerChangingSize.dropSlimeAllowence, 
            playerController.collision.collideMask);

        for (int i = 0; i < colliders.Length; ++i)
        {
            var stickTo = GetSlimeStickOnto(colliders[i]);

            if (stickTo == null) continue;
            
            transform.parent = stickTo.GetSlimeParent();

            return;
        }
    }

    void SetZOrder(PlayerChangingSize playerChangingSize)
    {
        spriteRenderer.sortingOrder = 1000 - (int)(playerChangingSize.GetScale() * 500f);
    }

    void AnimationScale()
    {
        Vector3 targetScale = transform.localScale;

        transform.localScale = Vector3.zero;

        transform.DOScale(targetScale,  0.2f);
    }

    ISlimeStickOnto GetSlimeStickOnto(Collider2D collider2D)
    {
        ISlimeStickOnto result = collider2D.GetComponentInParent<ISlimeStickOnto>();

        return result;
    }
}
