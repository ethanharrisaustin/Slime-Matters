using UnityEngine;

public class PopUpMenuAnimationCurves : MonoBehaviour
{
    public AnimationCurve openX;
    public AnimationCurve openY;
    public float openTime;

    [Space]

    public AnimationCurve closeX;
    public AnimationCurve closeY;
    public float closeTime;

    public static PopUpMenuAnimationCurves instance;

    void Awake()
    {
        instance = this;
    }
}
    