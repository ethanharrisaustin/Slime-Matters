using UnityEngine;

[ExecuteAlways]
public class TwoPlatformPully : MonoBehaviour
{
    [SerializeField, Range(2f, 10f)] float width;
    [SerializeField, Range(-5f, 5f)] float height;
    [SerializeField, Range(0f, 10f)] float leftLength, rightLength;
    float totalLength = 6f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] HangingPlatform leftHangingPlatform, rightHangingPlatform;
    [SerializeField] Transform rightWheel;
    [SerializeField] SpriteRenderer horizontalString;
    [SerializeField] Transform horizontalStringPivot;

    void Awake()
    {
        SetInitialLengths();
        SetHorizontalString();
    }

    void SetInitialLengths()
    {
        totalLength = leftLength + rightLength;

        leftHangingPlatform.length = leftLength;
        rightHangingPlatform.length = rightLength;

        rightHangingPlatform.transform.localPosition = new Vector3(width, height);
        rightWheel.position = rightHangingPlatform.transform.position;
    }

    void SetHorizontalString()
    {
        // Length
        float horizontalStringLength = Mathf.Sqrt(width * width + height * height);
        horizontalString.size = new Vector2(horizontalString.size.x, horizontalStringLength);
        horizontalString.transform.localPosition = new Vector3(horizontalStringLength*0.5f, 0f);
    
        // Rotation
        Vector2 target = rightHangingPlatform.transform.localPosition;
        
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        horizontalStringPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            SetInitialLengths();
            SetHorizontalString();
            return;
        }

        float leftWeight = LeftWeight();
        float rightWeight = RightWeight();

        if (leftWeight > rightWeight)
        {
            FavourLeft();
        }
        else if (rightWeight > leftWeight)
        {
            FavourRight();
        }
    }

    void EvenOutHeights()
    {
        float targetHeight = totalLength / 2f;

        ChangeHeight(leftHangingPlatform, targetHeight);
        ChangeHeight(rightHangingPlatform, targetHeight);
    }

    void FavourLeft()
    {
        float targetLeftHeight = totalLength;
        float targetRightHeight = 0f;

        ChangeHeight(leftHangingPlatform, targetLeftHeight);
        ChangeHeight(rightHangingPlatform, targetRightHeight);
    }

    void FavourRight()
    {
        float targetRightHeight = totalLength;
        float targetLeftHeight = 0f;

        ChangeHeight(leftHangingPlatform, targetLeftHeight);
        ChangeHeight(rightHangingPlatform, targetRightHeight);
    }

    void ChangeHeight(HangingPlatform platform, float targetHeight)
    {
        float newLength = Mathf.MoveTowards(platform.length, targetHeight, Speed());
        platform.length = newLength;
    }

    float LeftWeight()
    {
        return leftHangingPlatform.CurrentWeight();
    }

    float RightWeight()
    {
        return rightHangingPlatform.CurrentWeight();
    }

    float Speed()
    {
        float difference = Mathf.Abs(LeftWeight() - RightWeight());

        return Time.deltaTime * moveSpeed * (difference + 0.4f);
    }
}
