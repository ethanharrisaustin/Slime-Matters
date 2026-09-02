using UnityEngine;

public class SpinningCog : MonoBehaviour
{
    [SerializeField] Transform cogGraphic;

    public void Spin(Vector3 rotateAmount)
    {
        cogGraphic.Rotate(rotateAmount);
    }
}
