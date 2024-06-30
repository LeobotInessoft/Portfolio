using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAddFunds : MonoBehaviour
{
    private static readonly bool UseSteamPayment = true;
    public InputField InputAmount;
    public Text TextReceive;
    public Text TextExchangeRateFor1Usd;
    //decimal exchangeRate = 100;
    // Use this for initialization
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshUI();
    }
    public void ButtonCloseClick()
    {

        gameObject.SetActive(false);


    }
    private void RefreshUI()
    {
        string dispText = "$0";
        if (RobotOwnerLookup.PublicAccess != null)
        {
            decimal exchangeRate = RobotOwnerLookup.PublicAccess.GetRateofExchange();
            TextExchangeRateFor1Usd.text = "$" + exchangeRate.ToString("f2");

            try
            {
                if (InputAmount.text.Length > 0)
                {
                    decimal valUsd = System.Convert.ToDecimal(InputAmount.text);
                    if (valUsd < (decimal)0.1)
                    {
                        valUsd = (decimal)0.1;
                        InputAmount.text = valUsd.ToString("f2");
                    }
                    decimal gameFunds = valUsd * exchangeRate;
                    dispText = "$" + gameFunds.ToString("f0");
                }
                else
                {

                }
            }
            catch
            {
                InputAmount.text = "0";
            }
        }
        TextReceive.text = dispText;
    }
    public void OnInputAmountChange()
    {
        RefreshUI();
    }
    public void ButtonFundNow()
    {
        if (UseSteamPayment == false)
        {
            string baseURL = "http://localhost:60811/AddFunds.aspx?";
            string dispText = "$0";
            if (InputAmount.text.Length > 0)
            {
                decimal exchangeRate = RobotOwnerLookup.PublicAccess.GetRateofExchange();
                decimal valUsd = System.Convert.ToDecimal(InputAmount.text);
                if (valUsd < (decimal)0.1) valUsd = (decimal)0.1;
                decimal gameFunds = valUsd * exchangeRate;

                baseURL += "uid=" + WwwLeagueInterface.LoggedInUserID;
                baseURL += "&am=" + valUsd.ToString();
              
                InputAmount.text = "";
                Application.OpenURL(baseURL);

            }
        }
        else
        {
            decimal exchangeRate = RobotOwnerLookup.PublicAccess.GetRateofExchange();
            decimal valUsd = System.Convert.ToDecimal(InputAmount.text);
            if (valUsd < (decimal)0.1) valUsd = (decimal)0.1;
            decimal gameFunds = valUsd * exchangeRate;

            DoSteamPayment(valUsd);
        }
    }
    private void DoSteamPayment(decimal amount)
    {
        IDECanvasManager.PublicAccess.www.SteamWalletPayInit(amount);

    }

}
