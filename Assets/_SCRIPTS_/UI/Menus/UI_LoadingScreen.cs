using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LoadingScreen : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image tempBlackScreen;
    [SerializeField] Animator animator;
    [SerializeField] float closeTime, openTime;
    [SerializeField] Image slime;

    [SerializeField] Sprite greenSlime, orangeSlime;

    [Space, SerializeField] StageLevels stages;

    public static UI_LoadingScreen instance;

    public static bool restartingLevel = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (restartingLevel)
        {
            slime.enabled = false;
        }
        
        canvas.enabled = true;
        tempBlackScreen.enabled = true;

        instance = this;
        
        CloseLoadingScreen();
    }

    void CloseLoadingScreen()
    {
        if (restartingLevel)
        {
            restartingLevel = false;

            tempBlackScreen.DOFade(0f, 0.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                slime.enabled = false;
                if (opening) return;

                canvas.enabled = false;
            });
            return;
        }

        StartCoroutine(CloseLoadingScreenRoutine());
    }

    IEnumerator CloseLoadingScreenRoutine()
    {
        slime.sprite = orangeSlime;

        yield return null;

        tempBlackScreen.enabled = false;

        yield return new WaitForSecondsRealtime(closeTime);

        if (!opening) canvas.enabled = false;
    }

    public void OpenLoadingScreen(string sceneToLoad)
    {
        if (opening) return;

        StartCoroutine(OpenLoadingScreenRoutine(sceneToLoad));
    }

    public void OpenLoadingScreen(int sceneToLoad)
    {
        if (opening) return;

        StartCoroutine(OpenLoadingScreenRoutine(sceneToLoad));
    }

    IEnumerator OpenLoadingScreenRoutine(string sceneToLoad)
    {
        yield return new WaitForSeconds(OpenAnimation());

        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator OpenLoadingScreenRoutine(int sceneToLoad)
    {
        yield return new WaitForSeconds(OpenAnimation());

        SceneManager.LoadScene(sceneToLoad);
    }

    public async void QuitGame()
    {
        if (opening) return;

       StartCoroutine(QuitGameRoutine());
    }

    IEnumerator QuitGameRoutine()
    {
        yield return new WaitForSecondsRealtime(OpenAnimation());

        Application.Quit();
    }

    bool opening = false;
    float OpenAnimation()
    {
        opening = true;

        slime.enabled = true;

        restartingLevel = false;

        canvas.enabled = true;

        slime.sprite = greenSlime;

        animator.SetBool("loading", true);

        return openTime;
    }

    public void RestartLevel()
    {
        if (opening) return;

        tempBlackScreen.enabled = true;

        canvas.enabled = true;

        slime.enabled = false;

        restartingLevel = true;

        tempBlackScreen.color = new Color(0f,0f,0f,0);

        tempBlackScreen.DOFade(1f, 0.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}
