using UnityEngine;

[ExecuteAlways]
public class Platform : MonoBehaviour
{
    [SerializeField, Range(1, 20)] int length = 2;
    [SerializeField] GameObject leftEnd, rightEnd, onlyMiddle, middlePrefab, middleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ShowPlatform();
    }

    int previousLength = -1;

    #if UNITY_EDITOR

    // Update is called once per frame
    void Update()
    {
        if (!LevelPreview.Showing()) 
        {
            RemoveCurrentMiddlePieces();
            return;
        }
        
        if (previousLength == length) return;
        
        previousLength = length;

        ShowPlatform();
    }

    #endif

    void ShowPlatform()
    {
        RemoveCurrentMiddlePieces();

        switch(length)
        {
            case 1:
            ShowOnlyOneLength();
            break;

            case 2:
            ShowOnlyTwoLength();
            break;

            default:
            ShowThreeOrMoreLength();
            break;
        }
    }

    void ShowOnlyOneLength()
    {
        leftEnd.SetActive(false);
        rightEnd.SetActive(false);
        onlyMiddle.SetActive(true);
        middleCollider.SetActive(false);
    }

    void ShowOnlyTwoLength()
    {
        leftEnd.SetActive(true);
        rightEnd.SetActive(true);
        onlyMiddle.SetActive(false);
        middleCollider.SetActive(false);

        rightEnd.transform.localPosition = new Vector3(1, 0);
    }

    void ShowThreeOrMoreLength()
    {
        leftEnd.SetActive(true);
        rightEnd.SetActive(true);
        onlyMiddle.SetActive(false);
        middleCollider.SetActive(true);

        rightEnd.transform.localPosition = new Vector3(length - 1, 0);

        middleCollider.transform.localScale = new Vector3(length - 2, 1f);

        for (int i = 0; i < length - 2; ++i)
        {
            GameObject newMiddlePiece = Instantiate(middlePrefab, transform);

            newMiddlePiece.transform.localPosition = new Vector3(i + 1, 0f);
        }
    }

    void RemoveCurrentMiddlePieces()
    {
        MiddlePlatformPiece[] pieces = GetComponentsInChildren<MiddlePlatformPiece>();

        for (int i = 0; i < pieces.Length; ++i)
        {
            #if UNITY_EDITOR
            DestroyImmediate(pieces[i].gameObject);
            #else
            Destroy(pieces[i].gameObject);
            #endif
        }

        previousLength = -1;
    }
}
