using Unity.VisualScripting;
using UnityEngine;

public class DeadlyLaser : MonoBehaviour
{
    [SerializeField] GameObject laserBeam;
    public void SwitchLaser()
    {
        laserBeam.SetActive(!laserBeam.activeSelf);
    }
}
