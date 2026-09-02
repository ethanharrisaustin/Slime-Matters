using UnityEngine;

public class ColouredButton : MonoBehaviour
{
    //[HideInInspector] 
    [SerializeField] ButtonColour colour;
    //[HideInInspector] 
    [SerializeField] Sprite pressedButton;
    [SerializeField] Collider2D buttonTrigger;

    IAttachedButtonTo attachedButtonTo = null;

    bool pressed  = false;
    public void OnPlayerEnter(int player)
    {
        var pc = PlayerController.GetPlayer(player);

        if (attachedButtonTo != null && !attachedButtonTo.CanPressButton(pc)) return;

        if (pressed) return;
        pressed = true;   

        OnPressed();
    }

    protected void OnPressed()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = pressedButton;

        LaserDoor.SwitchLaserDoors(colour);
        RollingPlatform.SwitchPlatforms(colour);

        RemoveAllTriggers();
    }

    public void RemoveButtonTrigger()
    {
        if (buttonTrigger == null) return;

        buttonTrigger.enabled = false;
    }

    public void RemoveAllTriggers()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].enabled = false;
        }
    }
    
    public void AddAttachedTo(IAttachedButtonTo attachedButtonTo)
    {
        this.attachedButtonTo = attachedButtonTo;
    }
}
