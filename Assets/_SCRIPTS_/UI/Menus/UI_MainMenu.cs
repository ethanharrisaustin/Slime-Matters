using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void PlayBtn()
    {
        UI_LoadingScreen.instance.OpenLoadingScreen("LEVEL_00");
    }

    public void QuitBtn()
    {
        UI_LoadingScreen.instance.QuitGame();
    }
}
