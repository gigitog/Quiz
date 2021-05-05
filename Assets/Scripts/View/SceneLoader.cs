using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Class in charge of animation of scene
 * change and scene transition
 */

public class SceneLoader : MonoBehaviour
{
    private const float Time = 1.5f;
    public GameObject crossfadeObj;
    private CanvasGroup crossfade;

    private void Start()
    {
        crossfade = crossfadeObj.GetComponent<CanvasGroup>();
        var y = SceneManager.GetActiveScene().buildIndex;
        Animate(true);
    }

    public void LoadSceneClick(int id)
    {
        AudioManager.Instance.Click();
        StartCoroutine(Load(id));
    }

    private IEnumerator Load(int indexScene)
    {
        Animate(false);

        yield return new WaitForSeconds(Time + 0.1f);

        StartCoroutine(LoadAsynch(indexScene));
    }

    private void Animate(bool isEnd)
    {
        if (!isEnd) crossfadeObj.SetActive(true);
        else StartCoroutine(DisableAfterTime(crossfadeObj));

        crossfade.DOFade(isEnd ? 0f : 1f, Time);
    }

    private IEnumerator DisableAfterTime(GameObject g)
    {
        yield return new WaitForSeconds(Time + 0.1f);
        g.SetActive(false);
    }

    private IEnumerator LoadAsynch(int scene)
    {
        var operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            if (operation.progress == 0.9f)
            {
                //CurrentView.indexScene = systemIndex;
            }

            yield return null;
        }
    }
}