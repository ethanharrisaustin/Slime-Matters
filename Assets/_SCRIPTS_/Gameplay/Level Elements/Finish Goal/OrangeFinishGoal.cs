using DG.Tweening;
using UnityEngine;

public class OrangeFinishGoal : MonoBehaviour
{
    [SerializeField] SpriteAnimationGroup flag;
    [SerializeField] Transform centre;
    public static bool playerInGoal = false;

    void Awake()
    {
        playerInGoal = false;
    }

    public void OnPlayerEnteredGoal(int player)
    {
        if (player != 2) return;
        
        playerInGoal = true;

        flag.PlayAnimation("up");
        
        PlayerController pc = PlayerController.GetPlayer(player);
        pc.SetAsActive(false);

        pc.transform.parent.DOScale(0f, 0.4f);
        pc.transform.parent.DOMove(centre.position, 0.3f);

        if (GreenFinishGoal.playerInGoal)
        {
            UI_CompletionMenu.instance.Open();
        }
    }
}
