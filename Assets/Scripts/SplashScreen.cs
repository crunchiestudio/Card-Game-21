using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Загрузочный экран (банер с текстом)
public class SplashScreen : MonoBehaviour
{
    [Header("Время показа банера")]
    [SerializeField]
    private float minSceneLoadTime = 5f;
    [SerializeField]
    GameObject fadeScreen;
    private AsyncOperation asyncOp;
    float time;

    private void Start()
    {
        LoadFirstScene();
    }

    private void OnEnable()
    {
        FadeScreen.OnFadeOnEnd += OnFadeOnEnd;
    }

    private void OnDisable()
    {
        FadeScreen.OnFadeOnEnd -= OnFadeOnEnd;
    }

    public void LoadFirstScene()
    {
        StartCoroutine(LoadingFirstScene());
    }

    //Загрузка главной сцены
    private IEnumerator LoadingFirstScene()
    {
        time = Time.time;
        yield return new WaitForSeconds(0.5f);
        asyncOp = SceneManager.LoadSceneAsync(1);
        asyncOp.allowSceneActivation = false;

        while (asyncOp.progress < 0.89f || Time.time - time < minSceneLoadTime)
        {
            yield return null;
        }

        //Открываем затемняющий экран
        fadeScreen.SetActive(true);
    }

    //После того как затемнился экран запускаем главную сцену
    private void OnFadeOnEnd()
    {
        StartCoroutine(FadeOnEnd());
    }

    private IEnumerator FadeOnEnd()
    {
        asyncOp.allowSceneActivation = true;
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }
}
