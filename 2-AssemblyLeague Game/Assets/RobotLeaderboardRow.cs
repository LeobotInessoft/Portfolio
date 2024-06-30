using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotLeaderboardRow : MonoBehaviour
{
    public Text TextRobotName;
    public Text TextRobotOwner;
    public Text TextType1;
    public Text TextType2;
    public Text TextShotsFired;
    //public Text TextShotsHit;
    public Text TextKills;
    public Text TextRamDamage;
    public Text TextSurviveTime;
    public Text TextWorldRank;
    public Text TextFinalScore;
    public Text TextHealth;



    public RobotConstructor.RobotTemplate TheRobotTemplate;
    public RobotMeta aMeta;
    ComponentType mainComponent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

       
    }
    public void SetRow(RobotMeta aRobot)
    {
        aMeta = aRobot;
        mainComponent = aMeta.gameObject.GetComponent<ComponentType>();
        TextRobotName.text = aRobot.Template.RobotName;
        TextType1.text = aRobot.Template.ModuleList.Count + " modules";
        TextType2.text = mainComponent.DeviceName;
        TextShotsFired.text = (((aMeta.ShotsHit+1.0f)/(aMeta.ShotsFired+1.0f))*100.0f).ToString("f1")+"%" ;
        TextKills.text = aMeta.Kills + "";
        TextRamDamage.text = aMeta.DamageGiven.ToString("f0") + "";
        TextSurviveTime.text = aMeta.MinutesSurvived.ToString("f0") + "";
        TextWorldRank.text = aMeta.RuntimeRank.ToString("f0") + "";
        TextFinalScore.text = aMeta.RuntimeScore.ToString("f0") + "";
        TextHealth.text = aMeta.Health.ToString("f0") + "";
        TextRobotOwner.text = aRobot.RuntimeRobotOwnerName;
     

        SetCodePanelState(aRobot);

    }
    public void SetCodePanelState(RobotMeta aRobot)
    {
        if (GameObjectFollower.PublicAccess.GameObjectToFollow != null && aRobot.gameObject.transform == GameObjectFollower.PublicAccess.GameObjectToFollow.transform)
        {
            MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer = aMeta.GetComponent<Computer>();
            if (Match.IsLeagueMatch)
            {

                if (MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.myRobotMeta != null)
                {
                    if (WwwLeagueInterface.LoggedInUserID == MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.myRobotMeta.RuntimeRobotOwnderID)
                    {
                        MatchCanvasManager.PublicAccess.TheCodePanel.ShowCodeForRobot = true;
                    }
                    else
                    {
                        MatchCanvasManager.PublicAccess.TheCodePanel.ShowCodeForRobot = false;

                    }
                }
            }
            else
            {
                MatchCanvasManager.PublicAccess.TheCodePanel.ShowCodeForRobot = true;

            }

            if (Match.IsLeagueMatch)
            {
                MatchCanvasManager.PublicAccess.TheCodePanel.IsAllowedToChangeCode = false;
            }
            else
            {
                MatchCanvasManager.PublicAccess.TheCodePanel.IsAllowedToChangeCode = true;

            }
            if (MatchCanvasManager.PublicAccess.TheCodePanel.IsAllowedToChangeCode)
            {


                if (MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.isFocused == false)
                {
                    MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.text = MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.CodeText;
                }
            }
            else
            {
                MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.text = MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.CodeText;

            }
        }
    }
    public void ButtonStatsClick()
    {
        MatchCanvasManager.PublicAccess.TheStatsPanel.TheRobotMeta = aMeta;
        MatchCanvasManager.PublicAccess.TheStatsPanel.gameObject.SetActive(!MatchCanvasManager.PublicAccess.TheStatsPanel.gameObject.activeSelf);
        MatchCanvasManager.PublicAccess.TheLogPanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheCodePanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheOverviewPanel.gameObject.SetActive(false);

    }
    public void ButtonLogClick()
    {
        MatchCanvasManager.PublicAccess.TheLogPanel.gameObject.SetActive(!MatchCanvasManager.PublicAccess.TheLogPanel.gameObject.activeSelf);
        MatchCanvasManager.PublicAccess.TheOverviewPanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheCodePanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheStatsPanel.gameObject.SetActive(false);
    }
    public void ButtonCodeClick()
    {
        MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer = aMeta.GetComponent<Computer>();
        MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.text = MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.CodeText;
        MatchCanvasManager.PublicAccess.TheCodePanel.gameObject.SetActive(!MatchCanvasManager.PublicAccess.TheCodePanel.gameObject.activeSelf);
        MatchCanvasManager.PublicAccess.TheLogPanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheOverviewPanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheStatsPanel.gameObject.SetActive(false);
    }
    public void ButtonOverviewClick()
    {
        MatchCanvasManager.PublicAccess.TheOverviewPanel.TheRobotMeta = aMeta;
        MatchCanvasManager.PublicAccess.TheOverviewPanel.gameObject.SetActive(!MatchCanvasManager.PublicAccess.TheOverviewPanel.gameObject.activeSelf);
        MatchCanvasManager.PublicAccess.TheLogPanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheCodePanel.gameObject.SetActive(false);
        MatchCanvasManager.PublicAccess.TheStatsPanel.gameObject.SetActive(false);


    }
}
