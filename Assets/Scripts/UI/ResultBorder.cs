using UnityEngine;
using UnityEngine.UI;

public class ResultBorder : MonoBehaviour
{
    [Header("Текст результат игрока")]
    [SerializeField]
    private Text borderLabel;
    [Header("Текст очков игроков")]
    [SerializeField]
    private Text borderText;

    /// <summary>
    /// Обновление результирующей таблицы
    /// </summary>
    /// <param name="gr">Результат игрока</param>
    /// <param name="points">Количество всех очков</param>
    public void UpdateResultBorder(GameResult gr, int[] points)
    {
        if(gr == GameResult.Win)
        {
            borderLabel.text = "Вы победили!";
        }
        else if (gr == GameResult.Tie)
        {
            borderLabel.text = "Нет победителя.";
        }
        else if (gr == GameResult.Lose)
        {
            borderLabel.text = "Вы проиграли.";
        }
        else
        {
            borderLabel.text = "Нет победителя.";
        }

        borderText.text = $"<color=Red>Вы:\t\t{points[0]}</color>\t\tBot 1:\t{ points[1] }\nBot 2:\t{ points[2] }\t\tBot 3:\t{ points[3] }";
    }
}
