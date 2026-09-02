using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    public Vector3 rotateAmount;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAmount * Time.deltaTime);
    }
}
