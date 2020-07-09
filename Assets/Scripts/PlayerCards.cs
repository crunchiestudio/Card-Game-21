using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCards : MonoBehaviour
{
    [SerializeField]
    CardsScalePos cardsScalePos;
    [HideInInspector]
    public List<Card> cards = new List<Card>();
    //Количество очков на руках
    [HideInInspector]
    public int amount = 0;
    //Количество текущих побед
    [HideInInspector]
    public int winCount = 0;
    //Золотое очко
    [HideInInspector]
    public bool goldenWin = false;

    private void OnEnable()
    {
        GameController.OnNewGame += ResetWins;
        GameController.OnGameRestart += ResetCards;
    }

    private void OnDisable()
    {
        GameController.OnNewGame -= ResetWins;
        GameController.OnGameRestart -= ResetCards;
    }

    //Добавление карты на руки
    public (Card, Image) AddCard()
    {
        //Создание новой карты на сцене
        Image cardObj = Instantiate(GameController.instance.cardPrefab, transform, false).GetComponent<Image>();
        Card card = DeckController.instance.GetCardFromDeck();
        cards.Add(card);
        //Определние новых позиций и размеров карт
        cardsScalePos.CalcPositions(GetCardsTransforms(), cards.Count);
        //Подсчёт очков на руках
        CalcPoints();
        return (card, cardObj);
    }

    //Рестарт карт на руках
    public void ResetCards()
    {
        //Удаление всех карт
        cards.Clear();
        RemoveCards();
        goldenWin = false;
    }

    //Проверка на золотое очко
    public bool CheckGoldenPoint()
    {
        if (cards.Count == 2)
        {
            if (cards[0].type == CardType.Ace && cards[1].type == CardType.Ace)
            {
                goldenWin = true;
                return true;
            }
        }
        return false;
    }

    //Подтсёт очков
    public void CalcPoints()
    {
        if(CheckGoldenPoint())
        {
            amount = 21;
        }
        else
        {
            amount = 0;
            foreach (Card card in cards)
            {
                amount += (int)card.type;
            }
        }
    }

    //Расскрытие карт
    public void OpenCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = cards[i].sprite;
        }
    }

    private RectTransform[] GetCardsTransforms()
    {
        RectTransform[] cards = new RectTransform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cards[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
        return cards;
    }

    //Удаление карт со сцены
    private void RemoveCards()
    {
        int length = transform.childCount;
        for (int i = 0; i < length; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject,false);
            //Destroy(transform.GetChild(0).gameObject);
        }
    }

    //Рестарт количества побед
    public void ResetWins()
    {
        winCount = 0;
    }

    //Добавление победы
    public void Win()
    {
        winCount++;
    }
}
