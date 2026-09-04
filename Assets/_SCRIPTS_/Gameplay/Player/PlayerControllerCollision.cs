using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerControllerCollision : MonoBehaviour
{
    [Header("Hit Points")]
    public Transform higher;
    public Transform higherLeft, higherRight;
    public Transform lower, lowerLeft, lowerRight;
    public Transform left, leftLower, leftUpper;
    public Transform right, rightLower, rightUpper;
    public LayerMask collideMask;
    public LayerMask detectIgnoreCollisionMask;
    public LayerMask pondCollision;
    public float allowence = 0.1f;
    public float snapToGroundAllowence = 0.3f;
    public float slideOffEdgeAllowence = 0.2f;
    public float slideOffEdgeDistanceCheck = 0.5f;
    [HideInInspector] public Transform center;
    [HideInInspector] public Vector2 offset;

    PlayerController controller;
    PlayerControllerInput input;
    PlayerControllerMovement movement;
    PlayerOnPlayerHead onPlayerHead;

    void Start()
    {
        center = higher.parent;

        offset = new Vector2(right.localPosition.x, higher.localPosition.y);

        controller = GetComponent<PlayerController>();

        input = controller.input;
        movement = controller.movement;
        onPlayerHead = controller.onPlayerHead;
    }

    [HideInInspector] 
    public bool cantCollideLeft = false;
    public bool ShouldCollideLeft()
    {
        return !input.RightInput() && !cantCollideLeft;
    }

    [HideInInspector] 
    public bool cantCollideRight = false;
    public bool ShouldCollideRight()
    {
        return !input.LeftInput() && !cantCollideRight;
    }

    public void ResetShouldColliders()
    {
        if (input.LeftInput()) cantCollideLeft = false;

        if (input.RightInput()) cantCollideRight = false;
    }

    public void Collide()
    {
        CollideX();

        CollideY();
    }

    public void CollideX()
    {
        if (HittingIgnoreCollision(lower, allowence))
        {
            if (movement.IsJumping()) CollideXUpper();
            
            return;
        }

        CollideXMiddle();
        
        if (!Hitting(lower)) CollideXLower();

        if (!Hitting(higher)) CollideXUpper();
    }

    public void CollideXMiddle()
    {
        if (Hitting(left) && Hitting(right)) return;

        bool hittingLeft = ShouldCollideLeft() && Hitting(left);

        if (hittingLeft && HitPosition(left, out var leftPos))
        {
            transform.position = new Vector3(leftPos.x + Offset().x, transform.position.y);
        }

        bool hittingRight = ShouldCollideRight() && Hitting(right);

        if (hittingRight && HitPosition(right, out var rightPos))
        {
            transform.position = new Vector3(rightPos.x - Offset().x, transform.position.y);
        }
    }

    public void CollideXLower()
    {
        if (Hitting(leftLower) && Hitting(rightLower)) return;

        bool hittingLeft = ShouldCollideLeft() && Hitting(leftLower);

        if (hittingLeft && HitPosition(leftLower, out var leftPos))
        {
            transform.position = new Vector3(leftPos.x + Offset().x, transform.position.y);
        }

        bool hittingRight = ShouldCollideRight() && Hitting(rightLower);

        if (hittingRight && HitPosition(rightLower, out var rightPos))
        {
            transform.position = new Vector3(rightPos.x - Offset().x, transform.position.y);
        }
    }

    public void CollideXUpper()
    {
        if (Hitting(leftUpper) && Hitting(rightUpper)) return;

        bool hittingLeft = ShouldCollideLeft() && Hitting(leftUpper);

        if (hittingLeft && HitPosition(leftUpper, out var leftPos))
        {
            transform.position = new Vector3(leftPos.x + Offset().x, transform.position.y);
        }

        bool hittingRight = ShouldCollideRight() && Hitting(rightUpper);

        if (hittingRight && HitPosition(rightUpper, out var rightPos))
        {
            transform.position = new Vector3(rightPos.x - Offset().x, transform.position.y);
        }
    }

    public void CollideY()
    {
        if (onPlayerHead.IsOnPlayersHead()) return;

        var otherPlayer = PlayerController.GetOnHeadPlayer();
        bool hittingCeiling = otherPlayer == null && Hitting(higher) ;

        if (hittingCeiling && Hitting(lower)) return;

        CollideYMiddle();

        CollideYRight();

        CollideYLeft();
    }

    void CollideYMiddle()
    {
        Collider2D collider2D;

        if (Hitting(lower, out collider2D) && HitPosition(lower, out var downPos))
        {
            var playerController = collider2D.GetComponentInParent<PlayerController>();

            if (playerController != null) return;

            transform.position = new Vector3(transform.position.x, downPos.y + Offset().y);
        }

        if (Hitting(higher, out collider2D) && HitPosition(higher, out var upPos))
        {
            var playerController = collider2D.GetComponentInParent<PlayerController>();

            if (playerController != null) return;

            transform.position = new Vector3(transform.position.x, upPos.y - Offset().y);
        }
    }

    void CollideYRight()
    {
        if (Hitting(lowerRight) && Hitting(higherRight)) return;

        Collider2D collider2D;

        Vector3 raycastOrigin = lowerRight.position + Vector3.up * 0.5f;

        if (Hitting(lowerRight, out collider2D) && HitPosition(lowerRight, raycastOrigin, Vector2.down, out var downPos))
        {
            var playerController = collider2D.GetComponentInParent<PlayerController>();

            if (playerController != null) return;

            transform.position = new Vector3(transform.position.x, downPos.y + Offset().y);
        }

        if (Hitting(higherRight, out collider2D) && HitPosition(higherRight, raycastOrigin, Vector2.up, out var upPos))
        {
            var playerController = collider2D.GetComponentInParent<PlayerController>();

            if (playerController != null) return;

            transform.position = new Vector3(transform.position.x, upPos.y - Offset().y);
        }
    }

    void CollideYLeft()
    {
        if (Hitting(lowerLeft) && Hitting(higherLeft)) return;
        
        Collider2D collider2D;

        Vector3 raycastOrigin = lowerLeft.position + Vector3.up * 0.5f;

        if (Hitting(lowerLeft, out collider2D) && HitPosition(lowerLeft, raycastOrigin, Vector2.down, out var downPos))
        {
            var playerController = collider2D.GetComponentInParent<PlayerController>();

            if (playerController != null) return;

            transform.position = new Vector3(transform.position.x, downPos.y + Offset().y);
        }

        if (Hitting(higherLeft, out collider2D) && HitPosition(higherLeft, raycastOrigin, Vector2.up, out var upPos))
        {
            var playerController = collider2D.GetComponentInParent<PlayerController>();

            if (playerController != null) return;

            transform.position = new Vector3(transform.position.x, upPos.y - Offset().y);
        }
    }

    void SnapToGround()
    {
        if (movement.IsJumping()) return;
        if (movement.timeOnGround < 3 && movement.timeOffGrouond == 0) return;
        if (movement.timeOffGrouond > 5) return;

        HitPosition(lower, out var downPos);

        if (HittingBox(lower, 0.01f, snapToGroundAllowence))
        {
            transform.position = new Vector3(transform.position.x, downPos.y + Offset().y);
        }
    }

    public bool RightHitting()
    {
        // When in pond, only stop if right upper 
        if (HittingIgnoreCollision(lower, allowence))
        {
            if (!movement.IsJumping()) return false;

            return Hitting(rightUpper);
        }

        // When on player head, we are snapped on, allow movement
        // even when rightLower is hitting
        if (onPlayerHead.IsOnPlayersHead() || HittingIgnoreCollision(lower, allowence))
        {
            return Hitting(right) || Hitting(rightUpper);
        }

        // When we have player on our head, allow movement
        // even when rightUpper is being hit
        var otherPlayer = PlayerController.GetOtherPlayer(controller);

        if (otherPlayer.onPlayerHead.IsOnPlayersHead())
        {
            return Hitting(right) || Hitting(rightLower);
        }

        return Hitting(right) || Hitting(rightUpper) || Hitting(rightLower);
    }

    public bool LeftHitting()
    {
        // When in pond, only stop if right upper 
        if (HittingIgnoreCollision(lower, allowence))
        {
            if (!movement.IsJumping()) return false;

            return Hitting(leftUpper);
        }

        // When on player head, we are snapped on, allow movement
        // even when leftLower is hitting
        if (onPlayerHead.IsOnPlayersHead())
        {
            return Hitting(left) || Hitting(leftUpper);
        }
        
        // When we have player on our head, allow movement
        // even when leftUpper is being hit
        var otherPlayer = PlayerController.GetOtherPlayer(controller);

        if (otherPlayer.onPlayerHead.IsOnPlayersHead())
        {
            return Hitting(left) || Hitting(leftLower);
        }

        return Hitting(left) || Hitting(leftUpper) || Hitting(leftLower);
    }

    public bool HittingLower()
    {
        return Hitting(lower) || Hitting(lowerRight) || Hitting(lowerLeft);
    }

    public bool NeedsToSlideOffEdge(out int slideDirection)
    {
        slideDirection = 0;

        // Greater allowence to stop player getting 'stuck' at corner of platform.
        float greaterAllowence = allowence * 1.5f;

        bool hittingRight = Hitting(lowerRight, greaterAllowence);
        bool hittingLeft = Hitting(lowerLeft, greaterAllowence);
        bool hittingBothSides = hittingRight && hittingLeft;
        bool onEdge = !Hitting(lower) && !hittingBothSides;

        if (!onEdge) return false;

        if (!hittingRight && !hittingLeft) return false;

        float radius = slideOffEdgeAllowence * controller.playerChangingSize.GetActualScale();

        var hit = Physics2D.CircleCast(
            lower.position,
            radius, 
            Vector2.down, 
            slideOffEdgeDistanceCheck,
            collideMask);
        
        slideDirection = hittingRight ? -1 : 1;

        return hit.collider == null;
    }

    public bool HittingHigher()
    {
        return Hitting(higher) || Hitting(higherRight) || Hitting(higherLeft);
    }

    public bool Hitting(Transform transform, float allowence = 0)
    {
        if (!transform.gameObject.activeInHierarchy) return false;

        if (allowence == 0) allowence = this.allowence;

        LayerMask mask = collideMask;

        if (HittingIgnoreCollision(transform, allowence)) mask = pondCollision;

        Collider2D collider2D = Physics2D.OverlapCircle(transform.position, allowence, mask);

        if (HangingPlatform.DoNotCollideWithSwing(collider2D, controller)) return false;

        return collider2D != null;
    }

    public bool Hitting(Transform transform, out Collider2D collider2D, float allowence = 0)
    {
        if (!transform.gameObject.activeInHierarchy) 
        {
            collider2D = null;
            return false;
        }

        if (allowence == 0) allowence = this.allowence;

        LayerMask mask = collideMask;

        if (HittingIgnoreCollision(transform, allowence)) mask = pondCollision;

        collider2D = Physics2D.OverlapCircle(transform.position, allowence, mask);
        
        if (HangingPlatform.DoNotCollideWithSwing(collider2D, controller)) return false;
        
        return collider2D != null;
    }

    public bool HittingIgnoreCollision(Transform transform, float allowence)
    {
        return Physics2D.OverlapCircle(transform.position, allowence, detectIgnoreCollisionMask) != null;
    }

    public bool HittingBox(Transform transform, float width, float height)
    {
        return Physics2D.OverlapBox(transform.position, new Vector2(width, height), 0f, collideMask) != null;
    }

    public bool HitPosition(Transform transform, out Vector2 position)
    {
        Vector3 origin;
        Vector2 direction;

        // If we are doing y collide
        if (transform.localPosition.x == 0)
        {
            origin = center.position;
            direction = transform.localPosition;
        }
        // X Collide
        else
        {
            origin = new Vector2(center.position.x, transform.position.y);
            direction = new Vector2(transform.localPosition.x, 0f);
        }

        return HitPosition(transform, origin, direction, out position);
    }

    public bool HitPosition(Transform hitTestPoint, Vector3 origin, Vector2 direction, out Vector2 position)
    {
        const int playerCollisionLayer = 6;

        LayerMask mask = collideMask;

        if (HittingIgnoreCollision(hitTestPoint, allowence)) mask = pondCollision;

        var radius = allowence;
        var distance = 10f;

        RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction, distance, mask);

        position = hit.point;

        if (hit.collider == null) return false;

        if (hit.collider.gameObject.layer != playerCollisionLayer) return false;

        // Doing Y Collide
        if (Mathf.Approximately(direction.x, 0))
        {
            return Mathf.Abs(hit.point.y - center.position.y) <= Mathf.Abs(hitTestPoint.localPosition.y);
        }
        // Doing X Collide
        else
        {
            return Mathf.Abs(hit.point.x - center.position.x) <= Mathf.Abs(hitTestPoint.localPosition.y);
        }
    }

    public Vector2 Offset()
    {
        return 0.9f * transform.localScale.x * offset;
    }

    public bool IsGrounded()
    {
        return movement.timeOnGround > 2;
    }

    public bool IsInIgnoreCollision()
    {
        return HittingIgnoreCollision(lower, allowence);
    }
}
