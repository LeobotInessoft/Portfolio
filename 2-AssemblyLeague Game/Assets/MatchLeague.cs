using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLeague : MonoBehaviour {
    public static int MyRobotID;
    public List<xRobot> LeagueRobotsInMatch;
    public LeagueInterfaceState State;
    public WwwLeagueInterface wLeague;
    public enum LeagueInterfaceState
    {
        Ready,
        CreatingMatch,
        LoadRobots,
        DoMatch,


    }
    bool hasSetup = false;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (hasSetup == false)
        {
            wLeague.On_GetXData_Received_CreateRobotMatch += new WwwLeagueInterface.GetXData_Received(CreatedMatchResponse);
            hasSetup = true;
        }
        LeagueInterfaceState nxt = State;
	}

    public void CreateLeagueMatch()
    {
    }
    void CreatedMatchResponse(XData data)
    {

    }
}
