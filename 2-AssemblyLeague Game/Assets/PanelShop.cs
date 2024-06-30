using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShop : MonoBehaviour {
    public PanelShopViewComponent TheComponentView;
    public PanelAddFunds TheFundsAdd;

	// Use this for initialization
	void Start () {
        TheComponentView.gameObject.SetActive(false);
        TheFundsAdd.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ButtonAddFundsClick()
    {
        TheFundsAdd.gameObject.SetActive(true);
    }
}
