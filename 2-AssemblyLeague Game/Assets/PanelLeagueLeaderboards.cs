using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLeagueLeaderboards : MonoBehaviour
{
    public WwwLeagueInterface wwwLeague;
    public float TimeBetweenUpdates = 10f;
    float currentTime = 1000;
    public PanelTopLeaders TopAssemblers;
    public PanelTopLeaders TopRobots;
    public PanelTopLeaders TopRobotMostKills;
    public PanelTopLeaders TopRobotKillsToDeathRatio;
    public PanelTopLeaders TopRobotCodeToScoreRatio;
    public PanelTopLeaders TopRobotWinPercentRatio;
    public PanelTopLeaders TopRobotMostAccurateHitToMissRatio;
    public PanelTopLeaders TopRobotMultiMatchWinRatio;
    public GameObject PanelLoading;
    public static PanelLeagueLeaderboards PublicAccess;



    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        //  SetInfo();
        PanelLoading.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > TimeBetweenUpdates)
        {
            if (wwwLeague.IsBusy == false)
            {
                currentTime = 0;
                DoUpdateRequest();
            }

        }
    }
    private void DoUpdateRequest()
    {
        string msg = "";
        wwwLeague.UpdateLeagueLeaderboard();
    }
    UpdateLeagueLeaderboardResult currentLeaderboardResult;
    public void SetInfo(UpdateLeagueLeaderboardResult aFullLeaderboardResult)
    {
        currentLeaderboardResult = aFullLeaderboardResult;
        TopAssemblers.SetInfo("Top Assemblers", "Player", "Rank", "Your Rank", aFullLeaderboardResult.TopAssemblers, LeaderStatsGeneralRow.EnumScoreType.TopAssemblers, aFullLeaderboardResult.ViewingAsPlayerID);
      
        TopRobots.SetInfo("Top Robots", "Robot", "Rank", "Your Top Rank Robot", aFullLeaderboardResult.TopRobots, LeaderStatsGeneralRow.EnumScoreType.TopRobots, aFullLeaderboardResult.ViewingAsPlayerID);
       
        TopRobotMostKills.SetInfo("Top Killers", "Robot", "Kills", "Your top killing Robot", aFullLeaderboardResult.TopRobotMostKills, LeaderStatsGeneralRow.EnumScoreType.TopRobotMostKills, aFullLeaderboardResult.ViewingAsPlayerID);
      
        TopRobotKillsToDeathRatio.SetInfo("Most Effective", "Robot", "Kills/Deaths", "Your most effective Robot", aFullLeaderboardResult.TopRobotKillsToDeathRatio, LeaderStatsGeneralRow.EnumScoreType.TopRobotKillsToDeathRatio, aFullLeaderboardResult.ViewingAsPlayerID);
      
        TopRobotCodeToScoreRatio.SetInfo("Best Coded", "Robot", "Code/Score", "Your best coded Robot", aFullLeaderboardResult.TopRobotCodeToScoreRatio, LeaderStatsGeneralRow.EnumScoreType.TopRobotCodeToScoreRatio, aFullLeaderboardResult.ViewingAsPlayerID);
      
        TopRobotWinPercentRatio.SetInfo("Best Performing ", "Robot", "Wins/Loss", "Your best performing Robot", aFullLeaderboardResult.TopRobotWinPercentRatio, LeaderStatsGeneralRow.EnumScoreType.TopRobotWinPercentRatio, aFullLeaderboardResult.ViewingAsPlayerID);
      
        TopRobotMostAccurateHitToMissRatio.SetInfo("Most Accurate", "Robot", "Hit/Miss", "Your most accurate Robot", aFullLeaderboardResult.TopRobotMostAccurateHitToMissRatio, LeaderStatsGeneralRow.EnumScoreType.TopRobotMostAccurateHitToMissRatio, aFullLeaderboardResult.ViewingAsPlayerID);
      
        TopRobotMultiMatchWinRatio.SetInfo("Most Entertaining", "Robot", "Kills/Match", "Your most entertaining Robot", aFullLeaderboardResult.TopRobotMultiMatchWinRatio, LeaderStatsGeneralRow.EnumScoreType.TopRobotMultiMatchWinRatio, aFullLeaderboardResult.ViewingAsPlayerID);
        PanelLoading.gameObject.SetActive(false);
    }
    public int GetPersonalRobotIDInStat(LeaderStatsGeneralRow.EnumScoreType aType)
    {
        int ret = 0;

        switch (aType)
        {
            case LeaderStatsGeneralRow.EnumScoreType.TopAssemblers:
                {
                    ret = TopAssemblers.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobotCodeToScoreRatio:
                {
                    ret = TopRobotCodeToScoreRatio.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobotKillsToDeathRatio:
                {
                    ret = TopAssemblers.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobotMostAccurateHitToMissRatio:
                {
                    ret = TopRobotMostAccurateHitToMissRatio.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobotMostKills:
                {
                    ret = TopRobotMostKills.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobotMultiMatchWinRatio:
                {
                    ret = TopRobotMultiMatchWinRatio.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobots:
                {
                    ret = TopRobots.PersonalRobot.RobotID;
                    break;
                }
            case LeaderStatsGeneralRow.EnumScoreType.TopRobotWinPercentRatio:
                {
                    ret = TopRobotWinPercentRatio.PersonalRobot.RobotID;
                    break;
                }

        }

        return ret;
    }

}
