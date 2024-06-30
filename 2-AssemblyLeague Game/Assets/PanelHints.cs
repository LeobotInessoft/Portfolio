using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelHints : MonoBehaviour
{
   // public RobotOwnerLookup TheLookup;
    public Text TextHeading;
    public Text TextDetail;
    float WaitTime = 1f;
    float currentTime = 0;
    // Use this for initialization
    void Start()
    {
        currentTime = WaitTime + 1;
        if (RobotOwnerLookup.PublicAccess != null)
        {
            ShowRandomHint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RobotOwnerLookup.PublicAccess != null)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= WaitTime)
            {
                currentTime = 0;
                ShowRandomHint();
            }
        }
    }
    public void ShowRandomHint()
    {

        if (RobotOwnerLookup.PublicAccess != null)
        {
            List<xHint> hints = RobotOwnerLookup.PublicAccess.GetHints();
            if (hints.Count > 0)
            {
                int hintNumber = Random.Range(0, hints.Count);
                xHint aHint = hints[hintNumber];
                TextHeading.text = aHint.TipHeading;
                TextDetail.text = aHint.TipText;
                WaitTime = aHint.TipText.Length * 0.3f;
                if (WaitTime <= 5) WaitTime = 5;
            }
        }
    }
    void OnEnable()
    {
        currentTime = WaitTime + 1; 
        ShowRandomHint();
    }

   
}
