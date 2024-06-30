using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelTopLeaders : MonoBehaviour
{
    public List<LeaderStatsGeneralRow> TheRows;
    public List<xLeagueLeaderboard> currentXLeaders;
    public Text TextHeading;
    public Text TextColumnName;
    public Text TextScoreTypeName;
    public Text TextYourHeading;
    public LeaderStatsGeneralRow StatYourStat;
    public xLeagueLeaderboard PersonalRobot;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetInfo(string heading, string columnName, string scoreName, string yourText, List<xLeagueLeaderboard> xLeaders, LeaderStatsGeneralRow.EnumScoreType scoreType, int viewAsPlayerID)
    {
        PersonalRobot = new xLeagueLeaderboard();
        currentXLeaders = xLeaders;
        TextHeading.text = heading;
        TextColumnName.text = columnName;
        TextScoreTypeName.text = scoreName;
        TextYourHeading.text = yourText;
        for (int c = 0; c < TheRows.Count; c++)
        {
            if (c >= xLeaders.Count)
            {
                TheRows[c].gameObject.SetActive(false);
            }
            else
            {
                TheRows[c].SetInfo(currentXLeaders[c], scoreType);
                TheRows[c].gameObject.SetActive(true);

            }

        }
        if (xLeaders.Count > 0)
        {
            if (viewAsPlayerID == xLeaders[xLeaders.Count - 1].PlayerID)
            {
                PersonalRobot = xLeaders[xLeaders.Count - 1];
                StatYourStat.SetInfo(xLeaders[xLeaders.Count - 1], scoreType);
                StatYourStat.gameObject.SetActive(true);
                TextYourHeading.gameObject.SetActive(true);
            }
            else
            {
                StatYourStat.gameObject.SetActive(false);
                TextYourHeading.gameObject.SetActive(false);

            }
        }
        else
        {
            StatYourStat.gameObject.SetActive(false);
            TextYourHeading.gameObject.SetActive(false);

        }
    }
    public void ButtonPlayAllClick()
    {
        List<int> ids = new List<int>();
       
        int myRobotID = PersonalRobot.RobotID;

        for (int c = 0; c < TheRows.Count; c++)
        {
            if (TheRows[c].gameObject.activeSelf)
            {
                ids.Add(TheRows[c].cureLeader.RobotID);


            }
        }
        IDECanvasManager.PublicAccess.CreateLeagueMatchRequest(ids, true, myRobotID, myRobotID);

    }

}
