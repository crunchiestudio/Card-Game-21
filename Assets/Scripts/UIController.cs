using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text borderText;
    [SerializeField]
    private Button getCardButton, passButton;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private ResultBorder resultBorder;

    public delegate void UIControllerDelegate();
    public static UIControllerDelegate OnMenuOpen;

    private static UIController s_Instance;
    public static UIController instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(UIController)) as UIController;

                if (s_Instance == null)
                {
                    var obj = new GameObject("UIController");
                    s_Instance = obj.AddComponent<UIController>();
                }
            }

            return s_Instance;
        }
    }

    private void OnEnable()
    {
        GameController.OnNewGame += OnGameStart;
        GameController.OnGameOver += OnGameOver;
        GameController.OnGameRestart += OnGameRestart;
    }

    private void OnDisable()
    {
        GameController.OnNewGame -= OnGameStart;
        GameController.OnGameOver -= OnGameOver;
        GameController.OnGameRestart -= OnGameRestart;
    }

    public void GetCardButtonPress()
    {
        GameController.instance.GetCard();
    }

    public void PassButtonPress()
    {
        DisablesButtons();
        GameController.instance.Pass();
    }

    //Нажатие на кнопку меню
    public void MenuButtonPress()
    {
        OnMenuOpen();
        CancelInvoke();
        menu.SetActive(true);
    }

    //Обновление таблицы очков
    public void UpdateBorderText(int[] points)
    {
        borderText.text = $"<color=Red>Вы:\t\t{points[0]}</color>\nBot 1:\t{ (points[1]==0 ? "?": points[1].ToString()) }\nBot 2:\t{ (points[2] == 0 ? "?" : points[2].ToString()) }\nBot 3:\t{ (points[3] == 0 ? "?" : points[3].ToString()) }";
    }

    public void DisableGetCardButton(bool enable)
    {
        getCardButton.interactable = enable;
    }

    public void DisablesButtons()
    {
        DisableGetCardButton(false);
        passButton.interactable = false;
    }

    public void OnGameRestart()
    {
        DisableGetCardButton(true);
        passButton.interactable = true;
    }

    public void OnGameOver()
    {
        //Отключаем кнопки
        DisableGetCardButton(false);
    }

    public void OnGameStart()
    {
        //Закрываем меню
        menu.SetActive(false);
    }

    //Кнопка старта игры
    public void StartGamePressButton()
    {
        OnGameStart();
        resultBorder.gameObject.SetActive(false);
        GameController.instance.StartNewGame();
    }

    //Конец игры
    public void GameEnd()
    {
        UpdateBorderText(GameController.instance.GetPlayersPoints());
        resultBorder.UpdateResultBorder(GameController.instance.GetGameResult(), GameController.instance.GetPlayersWinsCount());
        resultBorder.gameObject.SetActive(true);
    }

    public void OnGameEnd()
    {
        Invoke("GameEnd", 1);
    }

    //Нажатие на кнопку ОК в окне
    public void OnResultBorderOkButtonPress()
    {
        resultBorder.gameObject.SetActive(false);
        GameController.instance.RestartGame();
    }
}
