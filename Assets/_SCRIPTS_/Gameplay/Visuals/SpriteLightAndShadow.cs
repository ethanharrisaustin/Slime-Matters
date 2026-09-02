using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLightAndShadow : MonoBehaviour
{
    [SerializeField] float lineThickness;
    [SerializeField] Color lightColour, shadowColour;

    SpriteRenderer spriteRenderer;

    new SpriteRenderer light;
    SpriteRenderer shadow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject lightGO = new GameObject();
        //lightGO.transform.parent = transform;
        lightGO.transform.localScale = Vector2.one;
        lightGO.transform.localPosition = new Vector3(-0.1f * lineThickness, 0.1f * lineThickness);

        light = lightGO.AddComponent<SpriteRenderer>();

        light.sortingLayerName = spriteRenderer.sortingLayerName;
        light.sortingOrder = spriteRenderer.sortingOrder - 1;
        
        
        GameObject shadowGO = new GameObject();
        //shadowGO.transform.parent = transform;
        shadowGO.transform.localScale = Vector2.one;
        shadowGO.transform.localPosition = new Vector3(0.1f * lineThickness, -0.1f * lineThickness);

        shadow = shadowGO.AddComponent<SpriteRenderer>();

        shadow.sortingLayerName = spriteRenderer.sortingLayerName;
        shadow.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        light.sprite = spriteRenderer.sprite;
        shadow.sprite = spriteRenderer.sprite;
        
        light.color = lightColour;
        shadow.color = shadowColour;

        light.transform.position = transform.position + new Vector3(-0.1f * lineThickness, 0.1f * lineThickness);
        light.transform.rotation = transform.rotation;
        light.transform.localScale = transform.lossyScale;

        shadow.transform.position = transform.position + new Vector3(0.1f * lineThickness, -0.1f * lineThickness);
        shadow.transform.rotation = transform.rotation;
        shadow.transform.localScale = transform.lossyScale;
    }
}
