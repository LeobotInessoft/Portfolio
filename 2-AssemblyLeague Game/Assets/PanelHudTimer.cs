using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelHudTimer : MonoBehaviour {
    public Text TextTimeRemaining;
    public Text TextStatus;

    public Match TheMatch;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (TheMatch.HasMatchStarted == false)
        {
            System.TimeSpan time = TheMatch.MatchStartTime - System.DateTime.Now;
            if (time.TotalMinutes > 0)
            {
                TextTimeRemaining.text = time.TotalSeconds.ToString("f0");
                TextStatus.text = "Start";
            }
            else
            {
                TextTimeRemaining.text = "";
                TextStatus.text = "";
            }
          }
        else
        {
            if (TheMatch.IsInIntroductionMode)
            {
                System.TimeSpan time = TheMatch.MatchIntroductionEnd - System.DateTime.Now;
                if (time.TotalMinutes > 0)
                {
                    TextTimeRemaining.text = time.TotalSeconds.ToString("f0");
                    TextStatus.text = "Introduction";
                }
                else
                {
                    TextTimeRemaining.text = "";
                    TextStatus.text = "";
                }
                 
            }
            else
            {
                if (TheMatch.HasMatchEnded == false && TheMatch.HasMatchStarted == true)
                {
                    System.TimeSpan time = TheMatch.MatchEndTime - System.DateTime.Now;
                    if (time.TotalMinutes > 0)
                    {
                        TextTimeRemaining.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
                        TextStatus.text = "Battle";
                    }
                    else
                    {
                        TextTimeRemaining.text = "";
                        TextStatus.text = "";
                    }
                  }
            }
        }
	}
}
