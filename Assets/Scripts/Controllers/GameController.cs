using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameResult
{
    Win,
    Lose,
    Tie,
    LoseTie
}

public class GameController : MonoBehaviour
{
    public delegate void GameControllerDelegate();
    public static GameControllerDelegate OnGameRestart, OnGameOver, OnNewGame;

    private static GameController s_Instance;
    public static GameController instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameController)) as GameController;

                if (s_Instance == null)
                {
                    var obj = new GameObject("GameController");
                    s_Instance = obj.AddComponent<GameController>();
                }
            }

            return s_Instance;
        }
    }

    [Header("Префаб карты")]
    public GameObject cardPrefab;
    [Header("Карты игрока")]
    [SerializeField]
    public PlayerCards playerCards;
    [Header("Карты ботов")]
    [SerializeField]
    public PlayerCards[] botsCards;

    //Старт игры
    public void StartNewGame()
    {
        OnNewGame();
        RestartGame();
    }

    //Рестарт игры
    public void RestartGame()
    {
        //Рестартам колоду
        DeckController.instance.ResetDeck();
        OnGameRestart();
        //Рестартаем карты игрока
        playerCards.ResetCards();
        //Берём карту игроку
        GetCard();
        //Берём карту всем ботам
        for (int i = 0; i < 3; i++)
        {
            botsCards[i].AddCard();
        }
    }

    //Конец игры
    public void GameEnd()
    {
        UIController.instance.OnGameEnd();
    }

    //Взятие карты для игрока
    public void GetCard()
    {
        //Берём карту
        (Card card, Image image) = playerCards.AddCard();
        //Меняем у неё картинку
        image.sprite = card.sprite;
        //Обновляем таблицу очков
        UIController.instance.UpdateBorderText(new int[] { playerCards.amount, 0, 0, 0 });
        //Проверяем на проигрыш
        if(CheckGameOver() || CheckOverflow())
        {
            OnGameOver();
        }
    }

    //Определение результатов игры
    public GameResult GetGameResult()
    {
        var max = 0;
        List<PlayerCards> winners = new List<PlayerCards>();
        if (playerCards.amount > 21)
        {
            for (int i = 0; i < 3; i++)
            {
                if (max < botsCards[i].amount && botsCards[i].amount < 22)
                {
                    winners.Clear();
                    max = botsCards[i].amount;
                    winners.Add(botsCards[i]);
                }
                else if (max == botsCards[i].amount)
                {
                    winners.Add(botsCards[i]);
                }
            }
            for(int i = 0; i < winners.Count; i++)
            {
                winners[i].Win();
            }
            return GameResult.Lose;
        }
        else if (playerCards.goldenWin)
        {
            playerCards.Win();
            for (int i = 0; i < 3; i++)
            {
                if (botsCards[i].goldenWin)
                {
                    botsCards[i].Win();
                    return GameResult.Tie;
                }
            }
            return GameResult.Win;
        }
        else
        {
            max = playerCards.amount;
            for (int i = 0; i < 3; i++)
            {
                if (max < botsCards[i].amount && botsCards[i].amount < 22)
                {
                    winners.Clear();
                    max = botsCards[i].amount;
                    winners.Add(botsCards[i]);
                }
                else if (max == botsCards[i].amount)
                {
                    winners.Add(botsCards[i]);
                }
            }
            if (max == playerCards.amount && winners.Count == 0)
            {
                playerCards.Win();
                return GameResult.Win;
            }
            else if (winners.Count != 0)
            {
                for (int i = 0; i < winners.Count; i++)
                {
                    winners[i].Win();
                }
                return GameResult.Lose;
            }
            else return GameResult.LoseTie;
        }
    }

    //Передача шага ботам
    public void Pass()
    {
        BotController.instance.StartBotGame();
    }

    //Проверка перебора очков
    public bool CheckGameOver()
    {
        if(playerCards.amount > 21)
        {
            return true;
        }
        return false;
    }

    //Проверка перебора карт (макс = 5)
    public bool CheckOverflow()
    {
        if(playerCards.cards.Count == 5)
        {
            return true;
        }
        return false;
    }

    //Очки игроков
    public int[] GetPlayersPoints()
    {
        int[] points = { playerCards.amount, botsCards[0].amount, botsCards[1].amount, botsCards[2].amount };
        return points;
    }

    //Количество побед игроков
    public int[] GetPlayersWinsCount()
    {
        int[] points = { playerCards.winCount, botsCards[0].winCount, botsCards[1].winCount, botsCards[2].winCount };
        return points;
    }
}
