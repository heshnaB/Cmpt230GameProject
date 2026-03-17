using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CatHUD : MonoBehaviour
{
    public CatGameManager gameManager;

    public Text moralityText;
    public Text goalText;
    public Text messageText;

    public Slider yellowBar;
    public Slider greenBar;

    public float messageDuration = 4f;

    private Coroutine messageRoutine;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<CatGameManager>();
        }
    }

    void OnEnable()
    {
        if (gameManager == null)
        {
            return;
        }

        gameManager.StateChanged += RefreshState;
        gameManager.MessageRaised += ShowMessage;
        gameManager.GoalChanged += SetGoal;
    }

    void Start()
    {
        RefreshState();
    }

    void OnDisable()
    {
        if (gameManager == null)
        {
            return;
        }

        gameManager.StateChanged -= RefreshState;
        gameManager.MessageRaised -= ShowMessage;
        gameManager.GoalChanged -= SetGoal;
    }

    void RefreshState()
    {
        if (gameManager == null)
        {
            return;
        }

        if (moralityText != null)
        {
            moralityText.text = "Morality: " + gameManager.morality;
        }

        if (yellowBar != null)
        {
            yellowBar.maxValue = gameManager.maxStat;
            yellowBar.value = gameManager.yellowStat;
        }

        if (greenBar != null)
        {
            greenBar.maxValue = gameManager.maxStat;
            greenBar.value = gameManager.greenStat;
        }
    }

    void SetGoal(string goal)
    {
        if (goalText == null)
        {
            return;
        }

        goalText.text = "Goal: " + goal;
    }

    void ShowMessage(string message)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = message;

        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
        }

        messageRoutine = StartCoroutine(ClearMessageAfterDelay());
    }

    IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        if (messageText != null)
        {
            messageText.text = "";
        }
    }
}