using UnityEngine;

public class UI_PauseMenu : UI_PopUpMenu
{
    public static UI_PauseMenu instance;

    bool open = false;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    public override void Open()
    {
        base.Open();

        open = true;
    }

    public static bool IsOpen()
    {
        if (instance == null) return false;

        return instance.open;
    }
}
