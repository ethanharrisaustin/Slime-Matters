using UnityEngine;

[ExecuteAlways]
public class MushroomPlatform : MonoBehaviour
{
    [SerializeField, Range(3, 10)] int length = 3;
    [SerializeField, Range(-3, 3)] int stalkOffset = 0;
    [SerializeField, Range(2, 10)] int height = 3;

    [HideInInspector, SerializeField] Transform stalk, top;
    [HideInInspector, SerializeField] Transform stalkTop;
    [HideInInspector, SerializeField] Transform platformLeft, platformRight, platformStalk;
    [HideInInspector, SerializeField] GameObject middleStalkPrefab, middlePlatformPrefab;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowPlatform();
    }

    

    #if UNITY_EDITOR

    int previousLength = -1;
    int previousHeight =-1;
    int previousStalkOffset = -1;

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) return;

        if (!LevelPreview.Showing()) 
        {
            RemoveCurrentMiddlePieces();
            return;
        }
        
        if (previousLength == length && height == previousHeight && previousStalkOffset == stalkOffset) return;
        
        previousLength = length;
        previousHeight = height;
        previousStalkOffset = stalkOffset;

        ShowPlatform();
    }

    #endif

    void ShowPlatform()
    {
        RemoveCurrentMiddlePieces();

        MakeStalk();
        MakePlatform();

        previousLength = length;
        previousHeight = height;
    }

    void MakeStalk()
    {
        stalk.localPosition = new Vector3(PosOfStalk(), -height - 0.28f);
        stalkTop.localPosition = new Vector3(0f, height - 0.5f);

        int numberOfMiddleStalks = height - 2;

        for (int i = 0; i < numberOfMiddleStalks; ++i)
        {
            GameObject newStalk = Instantiate(middleStalkPrefab, stalk);

            newStalk.transform.localPosition = Vector3.up * (i + 1.5f);
        }

       
    }

    void MakePlatform()
    {
        int numberOfMiddlePlatforms = length - 3;

        int posOfStalk = PosOfStalk() - 1;

        for (int i = 0; i < numberOfMiddlePlatforms; ++i)
        {
            GameObject newPlatform = Instantiate(middlePlatformPrefab, top);

            int xPos = i + 1;

            if (i >= posOfStalk) xPos++;

            newPlatform.transform.localPosition = Vector3.right * xPos;
        }

        platformStalk.localPosition = new Vector3(PosOfStalk(), 0f);

        platformRight.localPosition = Vector2.right * (length - 1);
    }

    int PosOfStalk()
    {
        return Mathf.Clamp((length/2) + stalkOffset, 1, length - 2);
    }

    void RemoveCurrentMiddlePieces()
    {
        previousLength = -1;
        previousHeight = -1;

        DestroyMiddlePieces<MushroomMiddlePlatform>();
        DestroyMiddlePieces<MushroomMiddleStalk>();
    }

    void DestroyMiddlePieces<T>() where T : Component
    {
        var pieces = GetComponentsInChildren<T>(true);

        for (int i = 0; i < pieces.Length; ++i)
        {
            #if UNITY_EDITOR
            DestroyImmediate(pieces[i].gameObject);
            #else
            Destroy(pieces[i].gameObject);
            #endif
        }
    }
}
