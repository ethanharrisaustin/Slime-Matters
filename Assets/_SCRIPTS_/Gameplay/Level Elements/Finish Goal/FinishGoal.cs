using DG.Tweening;
using UnityEngine;

public class FinishGoal : MonoBehaviour
{
    [SerializeField] SpriteAnimationGroup orangeFlag, greenFlag;
    [SerializeField] Transform centre;
    bool player1InGoal;
    bool player2InGoal;
    public void OnPlayerEnteredGoal(int player)
    {
        if (player == 1)
        {
            player1InGoal = true;

            greenFlag.PlayAnimation("up");
        }
        else
        {
            player2InGoal = true;

            orangeFlag.PlayAnimation("up");
        }

        if (player1InGoal && player2InGoal)
            UI_CompletionMenu.instance.Open();
        
        PlayerController pc = PlayerController.GetPlayer(player);
        pc.SetAsActive(false);

        pc.transform.parent.DOScale(0f, 0.4f);
        pc.transform.parent.DOMove(centre.position, 0.3f);
    }
}
