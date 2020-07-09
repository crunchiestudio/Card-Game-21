using UnityEngine;
using UnityEngine.UI;

public class GlowEffect : MonoBehaviour
{
    [SerializeField]
    private Outline outline;
    [Header("Скорость мигания")]
    [SerializeField]
    private float glowSpeed = 1.5f;

    void Update()
    {
        ChangeGlow();
    }

    //Эффект мигания
    private void ChangeGlow()
    {
        Color color = outline.effectColor;
        color.a = Mathf.PingPong(glowSpeed * Time.time, 1);
        outline.effectColor = color;
    }
}
