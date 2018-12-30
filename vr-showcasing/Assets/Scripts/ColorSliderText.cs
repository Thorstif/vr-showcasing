using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//For canvas (UI) with RGB color selector
//Attach to a scriptkeeper, maybe the very canvas used to select colors
//Sliders in the UI must have references to this script and trigger set-methods on change

public class ColorSliderText : MonoBehaviour {

    //must have references to RGB sliders to get RGB values
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    //must have reference to text for indicationg values
    public Text redValue;
    public Text greenValue;
    public Text blueValue;

    //must have reference to a plain white image which will be used to display the color
    public RawImage colorDisplay;

    // Use this for initialization
    void Start () {
		//Initial values
        redValue.text = "255";
        greenValue.text = "255";
        blueValue.text = "255";
        colorDisplay.GetComponent<RawImage>().color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 100);
    }
	
    //changes on the sliders cause changes on color image and value for text
    public void setRedValue()
    {
        redValue.text = redSlider.value.ToString();
        colorDisplay.GetComponent<RawImage>().color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 100);
    }

    public void setGreendValue()
    {
        greenValue.text = greenSlider.value.ToString();
        colorDisplay.GetComponent<RawImage>().color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 100);
    }

    public void setBlueValue()
    {
        blueValue.text = blueSlider.value.ToString();
        colorDisplay.GetComponent<RawImage>().color = new Color(redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255, 100);
    }
}
