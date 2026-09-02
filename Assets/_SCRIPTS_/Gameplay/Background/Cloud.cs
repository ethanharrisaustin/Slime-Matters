using UnityEngine;

public class Cloud : MonoBehaviour
{
    
    [SerializeField] float moveSpeed;

    [HideInInspector] public float scale;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * scale;

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * scale);

        if (transform.position.x < -20)
        {
            gameObject.SetActive(false);
        }
    }
}
