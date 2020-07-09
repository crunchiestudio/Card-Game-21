using System.Collections;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [Header("Пауза между раздачей карт ботам")]
    [SerializeField]
    float pauseBotGetCards = 0.5f;
    [Header("Минимальное количество очков необходимое для того, чтобы бот остановился")]
    [SerializeField]
    float minPointsBeforeStopGetCards = 15;

    private static BotController s_Instance;
    public static BotController instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(BotController)) as BotController;

                if (s_Instance == null)
                {
                    var obj = new GameObject("BotController");
                    s_Instance = obj.AddComponent<BotController>();
                }
            }

            return s_Instance;
        }
    }

    private void OnEnable()
    {
        UIController.OnMenuOpen += StopBotGame;
    }

    private void OnDisable()
    {
        UIController.OnMenuOpen -= StopBotGame;
    }

    //Запуск раздачи карт ботам
    public void StartBotGame()
    {
        StartCoroutine(BotGame());
    }

    private void StopBotGame()
    {
        StopAllCoroutines();
    }

    //Раздача карт ботам
    private IEnumerator BotGame()
    {
        for (int i = 0; i < 3; i++)
        {
            if(i!=0)
                yield return new WaitForSeconds(1.5f);
            else
                yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(BotGetCards(GameController.instance.botsCards[i], pauseBotGetCards));
        }
        //Расскрываем карты после раздачи
        for (int i = 0; i < 3; i++)
        {
            GameController.instance.botsCards[i].OpenCards();
        }
        //Завершаем игру
        GameController.instance.GameEnd();
    }

    /// <summary>
    /// Раздача карт конкретному боту
    /// </summary>
    /// <param name="botCards">Карты бота</param>
    /// <param name="pause">Пауза между раздачей карт</param>
    /// <returns></returns>
    private IEnumerator BotGetCards(PlayerCards botCards, float pause)
    {
        var max = 5;
        //Определяем сколько возьмёт карт бот
        var randomCardsCount = Random.Range(1, max);
        var check = 0;
        while (check < randomCardsCount)
        {
            //Даём карту боту
            botCards.AddCard();
            //Если очков меньше 21
            if (botCards.amount <= 21)
            {
                check++;
                //Если у бота золотое очко или 20-21 очков, то останавливаемся
                if (botCards.CheckGoldenPoint() || botCards.amount >= 20)
                {
                    check = randomCardsCount;
                    yield break;
                }
                else
                {
                    //Если раздали нужное количество карт, но очков меньше 15
                    if (check == randomCardsCount && botCards.amount < minPointsBeforeStopGetCards)
                    {
                        //Определяем новое количество карт для раздач
                        randomCardsCount = Random.Range(1, 6 - botCards.cards.Count);
                        check = 0;
                        yield return new WaitForSeconds(pause);
                    }
                    //Иначе просто делаем паузу перед следующей раздачей карты
                    else if (check != randomCardsCount)
                        yield return new WaitForSeconds(pause);
                }
            }
            //Иначе если больше, то останавливаемся
            else
            {
                check = randomCardsCount;
                yield break;
            }
        }
    }
}
