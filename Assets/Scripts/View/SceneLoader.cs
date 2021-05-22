#region

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

/*
 * Class in charge of animation of scene
 * change and scene transition
 */

public class SceneLoader : MonoBehaviour
{
    private const float FadeTime = 0.9f;
    private const float LogoTime = 0.4f;
    public GameObject crossfadeObj;
    private CanvasGroup crossfade;

    private void Start()
    {
        crossfade = crossfadeObj.GetComponent<CanvasGroup>();

        AnimateFadeOut();
    }

    public void LoadSceneClick(int id)
    {
        AudioManager.Instance.Click();

        AnimateFadeIn();

        LoadSceneAfterFade(id);
    }

    private void LoadSceneAfterFade(int id)
    {
        StartCoroutine(Co_Load(id));
    }

    private void AnimateFadeIn()
    {
        crossfadeObj.SetActive(true);
        crossfade.DOFade(1f, FadeTime);
    }

    private void AnimateFadeOut()
    {
        StartCoroutine(Co_FadeOutAfterSeconds(crossfadeObj));
    }

    private IEnumerator Co_FadeOutAfterSeconds(GameObject g)
    {
        var waitForLogo = new WaitForSeconds(LogoTime);
        var waitForFadeOut = new WaitForSeconds(FadeTime + 0.1f);

        yield return waitForLogo;

        crossfade.DOFade(0f, FadeTime);

        yield return waitForFadeOut;

        g.SetActive(false);
    }

    private IEnumerator Co_Load(int indexScene)
    {
        var waitForFadeIn = new WaitForSeconds(FadeTime + LogoTime);

        yield return waitForFadeIn;

        LoadScene(indexScene);
    }

    private void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}