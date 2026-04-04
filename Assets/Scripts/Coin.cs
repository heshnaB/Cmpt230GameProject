using UnityEngine;

public class Coin : MonoBehaviour
{
    public Popup popup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popup.ShowPopup();
            Destroy(gameObject);
        }
    }
}
