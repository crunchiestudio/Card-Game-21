using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Типы игрыльных карт (их очки)
public enum CardType
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 2,
    Queen = 3,
    King = 4,
    Ace = 11
}

public class DeckController : MonoBehaviour
{
    //Все карты
    [SerializeField]
    Card[] allCards;
    //Карты в колоде
    List<Card> deckCards;

    //Рестарт колоды
    public void ResetDeck()
    {
        CreateDeck();
        DeckShuffle();
        GetCardFromDeck();
    }

    private static DeckController s_Instance;
    public static DeckController instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(DeckController)) as DeckController;

                if (s_Instance == null)
                {
                    var obj = new GameObject("DeckController");
                    s_Instance = obj.AddComponent<DeckController>();
                }
            }

            return s_Instance;
        }
    }

    //Создание новой колоды
    public void CreateDeck()
    {
        deckCards = new List<Card>(allCards);
    }

    //Перемешивание колоды
    public void DeckShuffle()
    {
        deckCards = deckCards.OrderBy(x => Guid.NewGuid()).ToList();
    }

    //Взятие карты с колоды
    public Card GetCardFromDeck()
    {
        var card = deckCards[0];
        deckCards.RemoveAt(0);
        return card;
    }
}

[Serializable]
public struct Card
{
    //картинка карты
    public Sprite sprite;
    //тип карты
    public CardType type;
}
