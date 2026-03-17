using System;
using System.Collections.Generic;
using UnityEngine;

public class CatGameManager : MonoBehaviour
{
    public int minMorality = -100;
    public int maxMorality = 100;
    public int morality = 0;

    public float maxStat = 100f;
    public float yellowStat = 100f;
    public float greenStat = 100f;

    public string introMessage = "Clementine can choose to behave well or cause a little chaos. Good actions raise morality, while mischievous actions lower it.";
    public List<string> goals = new List<string>();

    public event Action StateChanged;
    public event Action<string> MessageRaised;
    public event Action<string> GoalChanged;

    private int currentGoalIndex;

    public string CurrentGoal
    {
        get
        {
            if (currentGoalIndex >= goals.Count)
            {
                return "All goals done";
            }

            return goals[currentGoalIndex];
        }
    }

    void Start()
    {
        ClampState();
        StateChanged?.Invoke();
        MessageRaised?.Invoke(introMessage);
        GoalChanged?.Invoke(CurrentGoal);
    }

    public void ApplyAction(CatInteractable interactable)
    {
        if (interactable == null)
        {
            return;
        }

        morality += interactable.moralityDelta;
        yellowStat += interactable.yellowDelta;
        greenStat += interactable.greenDelta;
        ClampState();

        if (!string.IsNullOrWhiteSpace(interactable.message))
        {
            MessageRaised?.Invoke(interactable.message);
        }

        if (interactable.completesCurrentGoal)
        {
            CompleteGoal();
        }

        StateChanged?.Invoke();
    }

    void CompleteGoal()
    {
        if (goals.Count == 0)
        {
            GoalChanged?.Invoke(CurrentGoal);
            return;
        }

        if (currentGoalIndex < goals.Count)
        {
            currentGoalIndex++;
        }

        GoalChanged?.Invoke(CurrentGoal);
    }

    void ClampState()
    {
        morality = Mathf.Clamp(morality, minMorality, maxMorality);
        yellowStat = Mathf.Clamp(yellowStat, 0f, maxStat);
        greenStat = Mathf.Clamp(greenStat, 0f, maxStat);
    }
}