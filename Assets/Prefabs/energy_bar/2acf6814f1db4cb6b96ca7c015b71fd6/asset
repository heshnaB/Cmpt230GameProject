using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image fillImage;

    public float maxEnergy = 100f;
    public float currentEnergy = 100f;

    public void SetEnergy(float value)
    {
        currentEnergy = Mathf.Clamp(value, 0f, maxEnergy);
        if (fillImage != null)
        {
            fillImage.fillAmount = currentEnergy / maxEnergy;
        }
    }
}
