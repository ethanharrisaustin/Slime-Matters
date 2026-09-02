using System.Collections.Generic;
using UnityEngine;

public class FakePlayerGroup : MonoBehaviour
{
    [SerializeField] GameObject fakePlayerPrefab;

    List<FakePlayer> fakePlayers = new List<FakePlayer>();


    public bool ShowFakePlayer(PlayerController playerController)
    {
        if (AlreadyShowingPlayer(playerController)) return false;
        
        FakePlayer fakePlayer = GetFakePlayer(playerController);

        if (fakePlayer == null) return false;

        fakePlayer.ShowFakePlayer(playerController);

        return true;
    }

    public bool HideFakePlayer(PlayerController playerController)
    {
        FakePlayer fakePlayer = GetFakePlayer(playerController);

        if (fakePlayer == null) return false;

        fakePlayer.HideFakePlayer(playerController);

        fakePlayer.gameObject.SetActive(false);

        return true;
    }

    public FakePlayer GetFakePlayer(PlayerController playerController)
    {
        for (int i = 0; i < fakePlayers.Count; ++i)
        {
            if (fakePlayers[i].gameObject.activeSelf 
                && !fakePlayers[i].IsShowingThisPlayer(playerController)) continue;

            fakePlayers[i].gameObject.SetActive(true);
            
            return fakePlayers[i];
        }

        FakePlayer fakePlayer = Instantiate(fakePlayerPrefab, transform).GetComponent<FakePlayer>();

        fakePlayers.Add(fakePlayer);

        return fakePlayer;
    }

    public FakePlayer GetAlreadyShowingFakePlayer(PlayerController playerController)
    {
        for (int i = 0; i < fakePlayers.Count; ++i)
        {
            if (!fakePlayers[i].IsShowingThisPlayer(playerController)) continue;

            return fakePlayers[i];
        }

        return null;
    }

    bool AlreadyShowingPlayer(PlayerController playerController)
    {
        for (int i = 0; i < fakePlayers.Count; ++i)
        {
            if (!fakePlayers[i].IsShowingThisPlayer(playerController)) continue;

            return true;
        }

        return false;
    }
}
