using System.Collections;
using System.Threading.Tasks;

using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(Canvas))]
public class UI_CompletionMenu : MonoBehaviour
{
    public static bool isOpen;
    public static UI_CompletionMenu instance;

    GraphicRaycaster graphicRaycaster;
    Canvas canvas;

    [SerializeField] RectTransform holder;
    [SerializeField] RectTransform bannerThing;
    [SerializeField] RectTransform menu;
    [SerializeField] Image darkness;

    [Space]

    [SerializeField] float openTime = 0.3f;
    [SerializeField] AnimationCurve holderXAnimCurve;
    [SerializeField] AnimationCurve holderYAnimCurve;
    [SerializeField] AnimationCurve bannerXAnimCurve;
    [SerializeField] AnimationCurve bannerYAnimCurve;

    [Space]

    [SerializeField] float menuOpenTime;
    [SerializeField] float menuOpenDelay = 0.3f;
    [SerializeField] AnimationCurve menuOpenAnimCurve;
    [SerializeField] RectTransform menuOffPos, menuOnPos;

    [Space]

    [SerializeField] float closeTime = 0.3f;
    [SerializeField] Transform closedMenuPosition;

    Vector2 bannerSizeDelta;

    static float openBuffer = 0f;

    [Space] 
    public StageLevels[] stages;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Init();
    }

    void Init()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        canvas = GetComponent<Canvas>();

        bannerSizeDelta = bannerThing.sizeDelta;

        instance = this;

        graphicRaycaster.enabled = false;
        isOpen = false;
        holder.localScale = Vector2.zero;
        canvas.enabled = false;
    }

    void Update()
    {
        openBuffer -= Time.unscaledDeltaTime;
    }

    public async void Open()
    {
        if (openBuffer > 0f) return;

        isOpen = true;

        openBuffer = 1.5f;

        StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);

        isOpen = true;
        graphicRaycaster.enabled = true;
        canvas.enabled = true;

        holder.DOKill(false);
        bannerThing.DOKill(false);

        holder.localScale = Vector2.zero;
        holder.position = new Vector3(Screen.width/2f, Screen.height/2f);
        holder.DOScaleX(1f, openTime).SetEase(holderXAnimCurve);
        holder.DOScaleY(1f, openTime).SetEase(holderYAnimCurve);

        bannerThing.sizeDelta = new Vector2(0f, 230f);
        bannerThing.DOSizeDelta(bannerSizeDelta, openTime).SetEase(bannerXAnimCurve);

        bannerThing.localScale = new Vector2(1f, 0f);
        bannerThing.DOScale(1f, openTime).SetEase(bannerYAnimCurve);

        menu.DOKill(false);
        menu.localPosition = menuOffPos.localPosition;
        menu.DOLocalMove(menuOnPos.localPosition, menuOpenTime).SetEase(menuOpenAnimCurve).SetDelay(menuOpenDelay);

        darkness.DOKill(false);
        darkness.DOFade(0.7f, openTime);
    }

    public void Close()
    {
        isOpen = false;

        holder.DOKill(false);
        bannerThing.DOKill(false);
        menu.DOKill(false);
        darkness.DOKill(false);

        darkness.DOFade(0f, closeTime);

        holder.DOMove(closedMenuPosition.position, closeTime).SetEase(Ease.InQuad).OnComplete(() =>
        {
            graphicRaycaster.enabled = false;
            canvas.enabled = false;
        });

        openBuffer = 5f;
    }

    public void NextLevelBtn()
    {
        //int c_scene = SceneManager.GetActiveScene().buildIndex;
        //int nextScene = c_scene + 1;
        
        string nextLevel;

        if (!StageLevels.GetNextLevel(stages, out nextLevel)) return;

        UI_LoadingScreen.instance.OpenLoadingScreen(nextLevel);
    }

    public static bool CannotOpen()
    {
        return openBuffer > 0f;
    }
}
