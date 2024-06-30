using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNewsReel : MonoBehaviour
{
    public PanelNewsReelRow TheRow;
    public Vector3 ShowPoint;
    public float WaitTime = 55f;
    public float TransitionTime = 2f;
    float currentWaitTime = 55f;
    float currentTransitionTime = 0;
    int currentIndex = -1;
    // Use this for initialization
    void Start()
    {
    }
    List<xNews> NewsEventList;
    void Update()
    {
        if (RobotOwnerLookup.PublicAccess != null)
        {
            NewsEventList = RobotOwnerLookup.PublicAccess.GetNews();
            if (NewsEventList.Count > 0)
            {
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime >= WaitTime)
                {
                    currentWaitTime = 0;
                    currentIndex++;
                    if (currentIndex >= NewsEventList.Count)
                    {
                        currentIndex = 0;
                    }
                    StartTransitionToNext(NewsEventList[currentIndex]);


                }
            }
        }
    }
    public void StartTransitionToNext(xNews newsEvent)
    {
        TheRow.SetNews(newsEvent);
        WaitTime = newsEvent.NewsText.Length * 0.15f;
        if (WaitTime <= 10) WaitTime = 10;
    }
    public void DoTransitionToNext()
    {
    }
    public void EndTransitionToNext()
    {
    }

}
