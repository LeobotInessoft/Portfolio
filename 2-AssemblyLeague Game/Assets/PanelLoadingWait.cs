using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelLoadingWait : MonoBehaviour {
    public Text TextLoading;

    float waiTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        waiTime += Time.deltaTime;
        SetText();
	}
    private void SetText()
    {
        string txt = "Loading";
        for (int c = 0; c < waiTime; c++)
        {
            txt += ".";
        }
        TextLoading.text = txt;
    }
    void OnEnable()
    {
        waiTime = 0;
    }
}
