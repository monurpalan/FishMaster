using UnityEngine;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    public static ScreensManager instance;

    [Header("Screens")]
    public GameObject endScreen;
    public GameObject mainScreen;
    public GameObject gameScreen;
    public GameObject returnScreen;

    [Header("Buttons")]
    public Button lengthButton;
    public Button strengthButton;
    public Button offlineEarningsButton;

    [Header("Texts")]
    public Text gameScreenMoney;
    public Text lengthCostText;
    public Text lengthValueText;
    public Text strengthCostText;
    public Text strengthValueText;
    public Text offlineEarningsCostText;
    public Text offlineEarningsValueText;
    public Text endScreenMoney;
    public Text returnScreenMoney;

    private GameObject currentScreen;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        currentScreen = mainScreen;
    }

    void Start()
    {
        CheckIdles();
        UpdateTexts();
    }

    public void ChangeScreen(Screens screen)
    {
        if (currentScreen != null)
            currentScreen.SetActive(false);

        switch (screen)
        {
            case Screens.MAIN:
                currentScreen = mainScreen;
                UpdateTexts();
                CheckIdles();
                break;
            case Screens.GAME:
                currentScreen = gameScreen;
                break;
            case Screens.END:
                currentScreen = endScreen;
                SetEndScreenMoney();
                break;
            case Screens.RETURN:
                currentScreen = returnScreen;
                SetReturnScreenMoney();
                break;
        }

        if (currentScreen != null)
            currentScreen.SetActive(true);
    }

    public void SetEndScreenMoney()
    {
        endScreenMoney.text = $"${IdleManager.instance.totalGain}";
    }

    public void SetReturnScreenMoney()
    {
        returnScreenMoney.text = $"${IdleManager.instance.totalGain} gained while waiting!";
    }

    public void CheckIdles()
    {
        int wallet = IdleManager.instance.wallet;

        lengthButton.interactable = wallet >= IdleManager.instance.lengthCost;
        strengthButton.interactable = wallet >= IdleManager.instance.strengthCost;
        offlineEarningsButton.interactable = wallet >= IdleManager.instance.offlineEarningsCost;
    }

    public void UpdateTexts()
    {
        gameScreenMoney.text = $"${IdleManager.instance.wallet}";
        lengthCostText.text = $"${IdleManager.instance.lengthCost}";
        lengthValueText.text = $"-{IdleManager.instance.length}m";
        strengthCostText.text = $"${IdleManager.instance.strengthCost}";
        strengthValueText.text = $"{IdleManager.instance.strength} fishes.";
        offlineEarningsCostText.text = $"${IdleManager.instance.offlineEarningsCost}";
        offlineEarningsValueText.text = $"${IdleManager.instance.offlineEarnings}/min";
    }
}

