using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LaserDoor : MonoBehaviour
{
    [HideInInspector] 
    [SerializeField] ButtonColour colour;

    [SerializeField, Range(1f, 45f)] float length;

    [SerializeField, Range(0f, 360f)] float rotation;


    [HideInInspector, SerializeField] Transform laserPivot;
    [HideInInspector, SerializeField] Transform topGun;

    static List<LaserDoor> lasers = new List<LaserDoor>();

    public bool isOn = true;

    GameObject laserGO;

    void Awake()
    {
        lasers.Clear();
    }

    void Start()
    {
        if (!lasers.Contains(this)) 
            lasers.Add(this);

        laserGO = transform.GetChild(0).gameObject;

        SetLength();
        SetRotation();
    }

    #if UNITY_EDITOR
    float previousLength = -1f;
    float previousRotation = -1f;
    void Update()
    {
        if (laserGO == null) laserGO = transform.GetChild(0).gameObject;
        if (laserGO.activeSelf != isOn) laserGO.SetActive(isOn);

        if (previousLength != length)
        {
            SetLength();
            previousLength = length;
        }

        if (previousRotation != rotation)
        {
            SetRotation();
            previousRotation = rotation;
        }
    }
    #endif

    void SetLength()
    {
        laserPivot.localScale = new Vector3(1f, length);
        topGun.localPosition = new Vector3(0f, length);
    }

    void SetRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -rotation);
    }

    public void Switch()
    {
        isOn = !isOn;

        if (laserGO == null) laserGO = transform.GetChild(0).gameObject;
        
        laserGO.SetActive(isOn);
    }

    public static void SwitchLaserDoors(ButtonColour colour)
    {
        if (lasers == null) return;

        for (int i = 0; i < lasers.Count; ++i)
        {
            if (lasers[i] == null) continue;

            if (lasers[i].colour != colour) continue;

            lasers[i].Switch();
        }
    }
}
