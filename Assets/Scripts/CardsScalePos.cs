using UnityEngine;

public class CardsScalePos : MonoBehaviour
{
    [Header("Отступ между картами")]
    [SerializeField]
    private float indent = 5;

    //Масштаб карт по вертикали в зависимости от их количества
    private float[] verticalScales = { 1, 1f, 1f, 0.85f, 0.8f };//, 0.7f, 0.6f };
    //Масштаб карт по горизонтали в зависимости от их количества
    private float[] horizontalScales = { 1, 1f, 1f, 1f, 0.9f };//, 0.88f, 0.76f};

    /// <summary>
    /// Определение позиций карт
    /// </summary>
    /// <param name="cards">Карты</param>
    /// <param name="length">Количество карт</param>
    public void CalcPositions(RectTransform[] cards, int length)
    {
        //Определяем размер карт в зависимости от их количества
        CalcScale(length);
        var card = cards[0];
        var width = card.rect.width * card.localScale.x;

        if(length == 0)
        {
            card.localPosition = new Vector2(0, 0);
        }
        else
        {
            var a = (length - 1.0f) / 2.0f;
            var defPos = -1 * (a * width + a * indent);
            var offset = width + indent;

            card.localPosition = new Vector2(defPos, 0);
            for (int i = 1; i < length; i++)
            {
                cards[i].localPosition = new Vector2(defPos + offset * i, 0);
            }
        }
    }

    /// <summary>
    /// Определение размера карт
    /// </summary>
    /// <param name="length">Количество карт</param>
    private void CalcScale(int length)
    {
        if(transform.localPosition.x == 0)
        {
            transform.localScale = new Vector2(horizontalScales[length - 1], horizontalScales[length - 1]);
        }
        else
        {
            transform.localScale = new Vector2(verticalScales[length - 1], verticalScales[length - 1]);
        }
    }
}
