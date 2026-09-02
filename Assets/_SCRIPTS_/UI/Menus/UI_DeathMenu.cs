using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeathMenu : UI_PopUpMenu
{
    [SerializeField] Transform title;
    [SerializeField] Image playerImg;
    [SerializeField] Sprite greenPlayer, orangePlayer;

    [SerializeField] Image[] eyes;
    [SerializeField] Color greenEyeColour, orangeEyeColour;

    public static UI_DeathMenu instance;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }  

    bool open = false;
   
    public override void Open()
    {
        base.Open();

        open = true;

        playerImg.sprite = PlayerSprite();
        for (int i = 0; i < eyes.Length; ++i) eyes[i].color = EyeColour();

        title.localScale = Vector2.zero;

        title.DOScale(1.58f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.3f);
    }

    public bool IsOpen()
    {
        return open;
    }
    
    bool Player1Dead()
    {
        PlayerController player = PlayerController.GetPlayer(1);

        return player.IsDead();
    }

    Sprite PlayerSprite()
    {
        if (Player1Dead()) return greenPlayer;
        return orangePlayer;
    }

    Color EyeColour()
    {
        if (Player1Dead()) return greenEyeColour;
        return orangeEyeColour;
    }
}
