using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public void Kill(int player)
    {
        PlayerController.GetPlayer(player).SetAsDead();

        UI_DeathMenu.instance.Open();

        PlayerController.DeactivateAllPlayers();
    }
}
