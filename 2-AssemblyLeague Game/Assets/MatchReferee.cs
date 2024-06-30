using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchReferee : MonoBehaviour
{
    public Match TheMatch;
    public int LastLeaderRuntimeID;
    // Use this for initialization
    float delayTime = 15f;
    void Start()
    {
        wasMoreThanOneAtLeastOnce = false;
        waitTime = 0;
    }
    float waitTime = 0;
    bool wasMoreThanOneAtLeastOnce = false;
    // Update is called once per frame
    void Update()
    {

        if (wasMoreThanOneAtLeastOnce)
        {
            if (delayTime > 0)
            {
                if (TheMatch.HasMatchStarted && TheMatch.IsInIntroductionMode == false)
                {
                    delayTime -= Time.deltaTime;
                }
            }
            else
            {

                if (TheMatch.HasMatchStarted && TheMatch.IsInIntroductionMode == false && TheMatch.HasMatchEnded == false)
                {
                    int winner = DetermineWinner();
                    if (winner >= 0)
                    {
                        waitTime += Time.deltaTime;
                        if (waitTime >= 3)
                        {
                            waitTime = 0;
                            print("Forcing Game Over");
                            TheMatch.IsForcedGameOver = true;
                        }
                    }
                }
            }
        }
        if (TheMatch.AllSpawnedRobots != null && TheMatch.AllSpawnedRobots.Count > 0)
        {
            wasMoreThanOneAtLeastOnce = true;
        }
    }

    public int DetermineWinner()
    {
        int retID = -1;
        List<RobotMeta> robotsRemaining = new List<RobotMeta>();
        float mostPts = 0;
        int ptsIndex = 0;
        for (int c = 0; c < TheMatch.AllSpawnedRobots.Count; c++)
        {
            RobotMeta aRobot = TheMatch.AllSpawnedRobots[c].GetComponent<RobotMeta>();
            if (aRobot.Health > 0)
            {
                aRobot.TimeDied = System.DateTime.UtcNow;
                aRobot.MinutesSurvived = (float)(aRobot.TimeDied - aRobot.TimeStarted).TotalMinutes;
                robotsRemaining.Add(aRobot);
            }
            else
            {
                if (aRobot.TimeDied == aRobot.TimeStarted)
                {
                    aRobot.TimeDied = System.DateTime.UtcNow;
                    aRobot.MinutesSurvived = (float)(aRobot.TimeDied - aRobot.TimeStarted).TotalMinutes;
                }
            }

            if (robotsRemaining.Count > 2)
            {
                break;
            }
            if (aRobot.RuntimeScore >= mostPts)
            {
                mostPts = aRobot.RuntimeScore;
                ptsIndex = aRobot.RuntimeRobotLeagueID;
            }


        }
        if (robotsRemaining.Count <= 1)
        {
            retID = ptsIndex;
            for (int c = 0; c < TheMatch.AllSpawnedRobots.Count; c++)
            {
                RobotMeta aRobot = TheMatch.AllSpawnedRobots[c].GetComponent<RobotMeta>();
                if (aRobot.RuntimeRobotLeagueID == retID)
                {
                    aRobot.TimeDied = System.DateTime.UtcNow;
                    aRobot.MinutesSurvived = (float)(aRobot.TimeDied - aRobot.TimeStarted).TotalMinutes;

                }
            }
          }

        return retID;
    }

    public float DeterminePlayerScore2(RobotMeta theRobot)
    {
        float ret = 0;
        float totalDeaths = 0;
        float totalHealth = 0;
        float totalKills = 0;
        ret = totalKills - totalDeaths + Random.Range(0, 100);
        theRobot.RuntimeScore = ret;

        return ret;
    }
    public int DeterminePlayerRank(RobotMeta theRobot)
    {
        int ret = 0;


        ret = 0;

        for (int c = 0; c < TheMatch.AllSpawnedRobots.Count; c++)
        {
            RobotMeta aRobotMeta = TheMatch.AllSpawnedRobots[c].GetComponent<RobotMeta>();
            if (theRobot.RuntimeScore < aRobotMeta.RuntimeScore)
            {
                ret++;
            }
        }
        if (ret == 0) LastLeaderRuntimeID = theRobot.RuntimeRobotID;
        theRobot.RuntimeRank = ret;


        return ret;
    }
}
