using DG.Tweening;
using UnityEngine;

public class PlayerChangingSize : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform[] thingsToScale, thingsToScaleClamped;
    [SerializeField] float hitAllowenceAtStart = 0.15f;
    [SerializeField] float scaleAtStartOfLevel = 1;
    [SerializeField] float scaleDownRate = 0.01f;
    [SerializeField] float minScale = 0.2f;
    [SerializeField] float jumpHeightAtStart;
    [SerializeField] float jumpHeightOffset;

    [SerializeField] GameObject extraHitters;
    [SerializeField] float scaleToRemoveExtraHitters = 0.3f;
    
    [Space]
    [SerializeField] GameObject slimePrefab;

    [SerializeField] float spaceBetweenSlimes = 0.5f;

    float c_spaceBetweenSlime = 0f;

    float currentScale;

    int buffer = 0;

    public static float dropSlimeAllowence = 0.02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScale = scaleAtStartOfLevel;

        SetScale(currentScale);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerController.IsDead() || !playerController.IsActive()) return;

        buffer ++;

        if (buffer < 3) return;

        if (playerController.movement.NeedsToShrink()) 
        {
            currentScale -= scaleDownRate * Mathf.Abs(playerController.moveDelta.x);
            SetScale(currentScale);

            DropSlimePiece();

            if (currentScale < minScale)
            {
                playerController.SetAsDead();

                UI_DeathMenu.instance.Open();

                PlayerController.DeactivateAllPlayers();
            }
        }
    }

    public void SetBuffer(int buffer)
    {
        this.buffer = buffer;
    }

    void SetScale(float scale)
    {
        for (int i = 0; i < thingsToScale.Length; ++i)
            thingsToScale[i].localScale = Vector2.one * scale;
        
        for (int i = 0; i < thingsToScaleClamped.Length; ++i)
            thingsToScaleClamped[i].localScale = Vector2.one * Mathf.Max(scale, 0.3f);


        playerController.collision.allowence = hitAllowenceAtStart * scale;
        playerController.jumpHeight = jumpHeightAtStart * scale + jumpHeightOffset;

        extraHitters.SetActive(scale > scaleToRemoveExtraHitters);
    }

    public float GetScale()
    {
        return (currentScale - minScale) / (scaleAtStartOfLevel - minScale);
    }

    public float GetActualScale()
    {
        return currentScale;
    }

    float prevXPos = 0f;
    void DropSlimePiece()
    {
        float currentXPos = playerController.transform.position.x;

        if (Mathf.Abs(prevXPos - currentXPos) <= c_spaceBetweenSlime) return;
        
        prevXPos = currentXPos;

        bool droppedLeft = DropSlimePiece(playerController.collision.lowerLeft);
        bool droppedRight = DropSlimePiece(playerController.collision.lowerRight);
        bool droppedMiddle = DropSlimePiece(playerController.collision.lower);

        /* 
        if (!droppedLeft && !droppedRight)
        {
            droppedMiddle = DropSlimePiece(playerController.collision.lower);
        }*/

        int numDropped = NumDropped(droppedLeft, droppedMiddle, droppedRight);

        switch(numDropped)
        {
            case 0:
                c_spaceBetweenSlime = 0;
                break;

            case 1:
                c_spaceBetweenSlime = spaceBetweenSlimes * 0.3f;
                break;
            
            default:
                c_spaceBetweenSlime = spaceBetweenSlimes;
                break;
        }
    }

    bool DropSlimePiece(Transform groundHitPoint)
    {
        if (!playerController.collision.Hitting(groundHitPoint, dropSlimeAllowence)) return false;

        GameObject newSlimePiece = Instantiate(slimePrefab);

        newSlimePiece.transform.position = groundHitPoint.position;

        newSlimePiece.GetComponent<PlayerSlimePiece>().SetUp(playerController, this);

        return true;
    }

    int NumDropped(bool droppedLeft, bool droppedMiddle, bool droppedRight)
    {
        int result = 0;

        if (droppedLeft) result++;
        if (droppedMiddle) result++;
        if (droppedRight) result++;

        return result;
    }

    public float CurrentWeight()
    {
        return GetScale();
    }
}
