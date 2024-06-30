using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComponentStatSlider : MonoBehaviour {
    public Text TextTop;
    public Text TextOver;
    public Slider SliderValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetData(string heading, string over, float value,float MinValue, float MaxValue)
    {
        gameObject.SetActive(true);
        TextTop.text = heading;
        TextOver.text = over;
        SliderValue.minValue = MinValue;
        SliderValue.maxValue = MaxValue;
        SliderValue.value = value;
          }
    public void Clear()
    {
        gameObject.SetActive(false);
  
    }
}

