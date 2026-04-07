using UnityEngine;

using TMPro;

using UnityEngine.UI;



public class DemoChoice : MonoBehaviour

{

    public TMP_Text resultText;

    public Image moralityBar;



    int morality = 0;



    void Start()

    {

        resultText.text = "Press Q to do good, or R to cause trouble";

        moralityBar.fillAmount = 0.5f;

    }



    void Update()

    {

        if (Input.GetKeyDown(KeyCode.Q))

        {

            morality += 1;

            resultText.text = "You did good. Morality: " + morality;

            moralityBar.fillAmount = Mathf.Clamp01(0.5f + morality * 0.1f);

        }



        if (Input.GetKeyDown(KeyCode.R))

        {

            morality -= 1;

            resultText.text = "You caused trouble. Morality: " + morality;

            moralityBar.fillAmount = Mathf.Clamp01(0.5f + morality * 0.1f);

        }

    }

}