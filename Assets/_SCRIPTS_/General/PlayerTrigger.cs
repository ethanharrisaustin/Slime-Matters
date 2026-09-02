using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent<int> onPlayerEntered;
    public UnityEvent<int> onPlayerStay;
    public UnityEvent<int> onPlayerExited;
    [Space]
    public bool onlyLowerPartOfPlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.layer = 7;
    }

    class PlayerEntered
    {
        public int player;
        public int buffer = bufferStart;
        const int bufferStart = 2;

        public PlayerEntered(int player)
        {
            this.player = player;
            buffer = bufferStart;
        }

        public static void Add(ref List<PlayerEntered> players, int player)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].player == player) 
                {
                    players[i].buffer = bufferStart;
                    return;
                }
            }

            players.Add(new PlayerEntered(player));
        }

        public static bool Contains(List<PlayerEntered> players, int player)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                if (players[i].player == player) return true;
            }
            return false;
        }

        public static bool Remove(ref List<PlayerEntered> players, int player)
        {
            bool didRemove = false;

            for (int i = 0; i < players.Count;)
            {
                if (players[i].player == player) 
                {
                    players.RemoveAt(i);
                    didRemove = true;
                    continue;
                }

                ++i;
            }
            return didRemove;
        }
    }

    List<PlayerEntered> playersEntered = new();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Notlower(collision)) return;

        OnPlayerEnter(collision.GetComponentInParent<PlayerController>());
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Notlower(collision)) return;

        var pc = collision.GetComponentInParent<PlayerController>();

        if (pc == null) return;

        onPlayerStay.Invoke(pc.player);

        OnPlayerEnter(pc);
    }

    void OnPlayerEnter(PlayerController playerController)
    {
        if (playerController == null) return;

        if (!PlayerEntered.Contains(playersEntered, playerController.player))
        {
            onPlayerEntered.Invoke(playerController.player);
        }

        PlayerEntered.Add(ref playersEntered, playerController.player);
    }

    void FixedUpdate()
    {
        ManagePlayersEntered();
    }

    void ManagePlayersEntered()
    {
        for (int i = 0; i < playersEntered.Count;)
        {
            playersEntered[i].buffer --;

            if (playersEntered[i].buffer < 0)
            {
                onPlayerExited.Invoke(playersEntered[i].player);

                playersEntered.RemoveAt(i);
                continue;
            }

            ++i;
        }
    }

    bool Notlower(Collider2D collision)
    {
        return onlyLowerPartOfPlayer && collision.gameObject.tag != "Lower Player";
    }
}
