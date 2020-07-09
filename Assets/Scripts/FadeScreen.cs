using UnityEngine;

//Затемняющий экран
public class FadeScreen : MonoBehaviour
{
    public delegate void FadeScreenDelegate();
    public static FadeScreenDelegate OnFadeOnEnd;

    public void FadeOnEnd()
    {
        OnFadeOnEnd();
    }

    //Удаление картинки
    public void FadeOffEnd()
    {
        Destroy(gameObject);
    }
}
