using UnityEngine;

[ExecuteAlways]
public class LevelPreview : MonoBehaviour
{
    [SerializeField] bool showPreviews = true;
    static bool show;

    void Update()
    {
        show = showPreviews;
    }

    public static bool Showing()
    {
        return Application.isPlaying || show;
    }
}
