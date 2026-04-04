using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject popup;

    public void ShowPopup()
    {
        popup.SetActive(true);
        Invoke("HidePopup", 1f);
    }

    void HidePopup()
    {
        popup.SetActive(false);
    }
}
