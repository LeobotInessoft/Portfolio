using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchLoadPrizePanel : MonoBehaviour {
    public Text TextMoney;
    // Use this for initialization
    void Start()
    {
        TextMoney.text = "$" + Match.PrizeMoneyLeague.ToString("0");
        if (Match.IsLeagueMatch == false)
        {
            TextMoney.text = "Practice Match";
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
       
       
	}
}
