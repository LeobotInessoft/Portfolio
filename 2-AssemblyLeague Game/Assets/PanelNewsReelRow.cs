using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelNewsReelRow : MonoBehaviour
{
    public Text TextNews;
    int maxLengthAtaTime = 50;
    string origText = "";
    string displayText = "";
    float delayTime =0.03f;
    int currentIndex = -1;
    float curTime = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (origText.Length >= maxLengthAtaTime)
        {
          curTime+=Time.deltaTime;
          if (curTime >= delayTime)
          {
              if (displayText.Length > 0 && displayText.Length >= maxLengthAtaTime)
              {
                  curTime = 0;
                  currentIndex++;
                  displayText = origText.Substring(currentIndex);
                  if (displayText.Length >= maxLengthAtaTime)
                  {
                      displayText = displayText.Substring(0, maxLengthAtaTime);
                  }
              }
              else
              {

              }
              TextNews.text = displayText;
          }
        }
        else
        {
            TextNews.text = origText;
        }
    }
    public void SetNews(xNews newsText)
    {
        curTime = -3f;
        currentIndex = 0;
        origText = newsText.NewsText;
        displayText = "";
        TextNews.text = displayText;
        DoDisplay();
    }
    private void DoDisplay()
    {
        displayText = origText.Substring(currentIndex);
        if (displayText.Length >= maxLengthAtaTime)
        {
            displayText = displayText.Substring(0, maxLengthAtaTime);
        }
        TextNews.text = displayText;
    }
}
