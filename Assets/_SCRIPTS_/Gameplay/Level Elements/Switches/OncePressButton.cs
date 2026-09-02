using UnityEngine;
using UnityEngine.Events;

public class OncePressButton : MonoBehaviour
{
    [SerializeField] Sprite pressedButton;

    public UnityEvent onPressed;

    bool pressed  = false;
    public void OnPlayerEnter()
    {
        if (pressed) return;
        pressed = true;   

        onPressed.Invoke();

        GetComponentInChildren<SpriteRenderer>().sprite = pressedButton;
    }
}
