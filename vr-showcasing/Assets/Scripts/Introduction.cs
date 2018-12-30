using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//hånterer veksling mellom panelene til introduksjonen

public class Introduction : MonoBehaviour {

    public GameObject firstPanel;
    public GameObject secondPanel;
    public GameObject thirdPanel;
    public GameObject fourthPanel;

    public GameObject introMenuTarget;

    public Text timer;
    public Slider slider;

    private bool firstPanelBool = true;
    private bool secondPanelBool = false;
	
	// Update is called once per frame
	void Update ()
    {
        if(firstPanelBool == true)
        {
            timer.text = ChaseLocation.timeLeft.ToString("F2") + "s";

            if (ChaseLocation.timed == false)
            {
                firstPanelBool = false;

                StartSecond();
            }
        }

        else if(secondPanelBool == true)
        {
            if(OVRInput.GetDown(OVRInput.Button.Two))
            {
                StartThird();
                secondPanelBool = false;
            }
        }
	}

    public void StartSecond()
    {
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
        secondPanelBool = true;
    }

    public void StartThird()
    {
        secondPanel.SetActive(false);
        thirdPanel.SetActive(true);
    }

    public void StartFourth()
    {
        if(slider.value != 0)
        {
            thirdPanel.SetActive(false);
            fourthPanel.SetActive(true);
        }
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
        Destroy(introMenuTarget);
    }
}
