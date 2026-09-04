using UnityEngine;

[ExecuteAlways]
public class SlimePond : MonoBehaviour
{
    [SerializeField, Range(0.5f, 15f)] float pondWidth = 3f;

    public SlimeColour slimeColour;
  
    [HideInInspector, SerializeField] float waveWidth = 0.45f;

    [HideInInspector, SerializeField] SpriteRenderer mask;

    [HideInInspector, SerializeField] Transform[] waveTransforms;
    [HideInInspector, SerializeField] BoxCollider2D[] collidersToGrow;
    [HideInInspector, SerializeField] PolygonCollider2D leftUphill, rightUphill;
    [HideInInspector, SerializeField] float[] waveMoveSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MakePond();
    }

    #if UNITY_EDITOR
    float previousPondWidth = -1;
    #endif

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR

        if (previousPondWidth != pondWidth)
        {
            previousPondWidth = pondWidth;

            MakePond();
        }

        if (!Application.isPlaying) return;

        #endif

        MoveWaves();
    }

    void MoveWaves()
    {
        for (int i = 0; i < waveTransforms.Length; ++i)
        {
            waveTransforms[i].Translate(Vector2.left * Time.deltaTime * waveMoveSpeed[i], Space.Self);

            if (waveTransforms[i].localPosition.x < -waveWidth * 0.5f)
            {
                waveTransforms[i].Translate(Vector2.right * waveWidth);
            }
        }
    }

    void MakePond()
    {
        mask.size = new Vector2(pondWidth, mask.size.y);

        for (int i = 0; i < waveTransforms.Length; ++i)
        {
            SpriteRenderer wave = waveTransforms[i].GetComponent<SpriteRenderer>();

            wave.size = new (pondWidth + waveWidth * 1.1f, wave.size.y);
        }

        for (int i = 0; i < collidersToGrow.Length; ++i)
        {
            collidersToGrow[i].size = new Vector2(pondWidth, collidersToGrow[i].size.y);
        }

        leftUphill.transform.localPosition = new Vector3(-pondWidth*.5f, leftUphill.transform.localPosition.y);
        rightUphill.transform.localPosition = new Vector3(pondWidth*.5f, rightUphill.transform.localPosition.y);
    }
}
