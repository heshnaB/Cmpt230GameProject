using UnityEngine;

public class CatInteractable : MonoBehaviour
{
    public int moralityDelta;
    public float yellowDelta;
    public float greenDelta;
    public string message;
    public bool completesCurrentGoal;

    public void Interact(CatGameManager gameManager)
    {
        if (gameManager == null)
        {
            return;
        }

        gameManager.ApplyAction(this);
    }
}