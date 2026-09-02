using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [SerializeField] Transform centerPos;
    PlayerController currentPlayer;
    Vector2 refVelocity;

    static List<FakePlayer> activeFakePlayers = new List<FakePlayer>();

    public void ShowFakePlayer(PlayerController playerController)
    {
        activeFakePlayers.Add(this);

        currentPlayer = playerController;

        refVelocity = playerController.playerSprite.refVelocity;

        transform.localScale = playerController.playerSprite.transform.localScale;

        playerController.playerSprite.HidePlayer();

        transform.SetPositionAndRotation(  
            playerController.playerSprite.transform.position, 
            playerController.playerSprite.actualSprite.rotation);

        float moveTime = 0.2f;

        float yVel = Mathf.Abs(playerController.moveDelta.y);

        Vector2 localPos = new Vector3(transform.localPosition.x, 0f);

        // Initially move it towards target so the player doesn't "stop" for a frame while falling
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, localPos, yVel);

        transform.DOLocalMove(localPos, moveTime * 0.75f).SetEase(Ease.OutQuad);
        transform.DOLocalRotate(Vector3.zero, moveTime).SetEase(Ease.InOutQuad);
    }

    public bool IsShowingThisPlayer(PlayerController playerController)
    {
        if (!gameObject.activeSelf) return false;

        return currentPlayer == playerController;
    }

    public void HideFakePlayer(PlayerController playerController)
    {
        activeFakePlayers.Remove(this);

        playerController.playerSprite.refVelocity = refVelocity;
    }

    public Vector3 CenterPosition()
    {
        return centerPos.position;
    }

    public bool IsShowing()
    {
        return gameObject.activeInHierarchy  && currentPlayer != null;
    }

    public static bool HasFakePlayer(PlayerController playerController, out Transform targetGraphic)
    {
        for (int i = 0; i < activeFakePlayers.Count; ++i)
        {
            if (activeFakePlayers[i] == null) continue;

            if (!activeFakePlayers[i].IsShowingThisPlayer(playerController)) continue;

            targetGraphic = activeFakePlayers[i].transform;
            return true;
        }

        targetGraphic = null;
        return false;
    }
}
