using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFunds : MonoBehaviour {
    public Text TextFunds;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (WwwLeagueInterface.LoggedInFundsTotal > 0)
        {

            TextFunds.text = "$" + (WwwLeagueInterface.LoggedInFundsTotal - RobotOwnerLookup.PublicAccess.GetPlayerPurchases().TotalMoneySpent).ToString("0");
           
        }
        else
        {
            TextFunds.text = "MONEY";
        }
	}
}
