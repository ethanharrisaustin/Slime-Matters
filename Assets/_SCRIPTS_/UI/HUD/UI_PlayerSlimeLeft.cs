using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerSlimeLeft : MonoBehaviour
{
    [SerializeField] int player;
    [SerializeField] TMP_Text text;
    [SerializeField] Image fill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = PlayerController.GetPlayer(this.player);  

        if (player == null) 
        {
            enabled = false; 
            return;
        }

        float scale = player.playerChangingSize.GetScale();
        text.text = (scale*100f).ToString("0") + "<size=20>%";

        fill.fillAmount = scale;
    }
}
