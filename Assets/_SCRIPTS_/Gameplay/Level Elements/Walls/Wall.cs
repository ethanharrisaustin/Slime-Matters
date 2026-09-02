using UnityEngine;

[ExecuteAlways]
public class Wall : MonoBehaviour
{
    public Vector2Int size;

    [SerializeField] Transform boxCollider;
    [SerializeField] GameObject middleWallPiece, topWallPiece;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RemovedCurrentWall();
        ShowWall();
    }

     #if UNITY_EDITOR

    Vector2Int prevSize = new Vector2Int(int.MaxValue, int.MinValue);

    // Update is called once per frame
    void Update()
    {
        if (!LevelPreview.Showing()) 
        {
            RemovedCurrentWall();

            prevSize = new Vector2Int(int.MaxValue, int.MinValue);

            return;
        }
        
        if (prevSize == size) return;
        
        prevSize = size;

        RemovedCurrentWall();
        ShowWall();
    }

    #endif

    void RemovedCurrentWall()
    {
        WallPiece[] wallPieces = GetComponentsInChildren<WallPiece>();

        for (int i = 0; i < wallPieces.Length; ++i)
        {
            #if UNITY_EDITOR
            DestroyImmediate(wallPieces[i].gameObject);
            #else
            Destroy(wallPieces[i].gameObject);
            #endif
        }
    }

    void ShowWall()
    {
        for (int x = 0; x < size.x; ++x)
        {
            for (int y = 0; y < size.y; ++y)
            {
                GameObject prefabToSpawn = topWallPiece;

                if (y < size.y - 1) prefabToSpawn = middleWallPiece;

                GameObject newWallPiece = Instantiate(prefabToSpawn, transform);

                newWallPiece.transform.localPosition = new Vector3(x, y);
            }
        }

        boxCollider.localScale = new Vector3(size.x, size.y);
    }
}
