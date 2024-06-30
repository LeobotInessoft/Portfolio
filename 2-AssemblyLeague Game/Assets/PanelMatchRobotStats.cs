using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PanelMatchRobotStats : MonoBehaviour {
    public InputField TheCode;
    public RobotMeta TheRobotMeta;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (TheRobotMeta != null)
        {
            TheCode.text = GenerateStats();
        }
	}
    private string GenerateStats()
    {
        string ret = "";
        ret += "Position: " + TheRobotMeta.gameObject.transform.position + "\n";
        ret += "Health: " + TheRobotMeta.Health + "\n";
        ret += "IsDead: " + TheRobotMeta.IsDead + "\n";
        ret += "IsOverheated: " + TheRobotMeta.IsOverheated + "\n";
        ret += "Kills: " + TheRobotMeta.Kills + "\n";
        ret += "MinutesSurvived: " + TheRobotMeta.MinutesSurvived + "\n";
        ret += "DamageGiven: " + TheRobotMeta.DamageGiven + "\n";
        ret += "RuntimeOverheat: " + TheRobotMeta.RuntimeOverheat + "\n";
        ret += "RuntimeRank: " + TheRobotMeta.RuntimeRank + "\n";
        ret += "RuntimeRobotID: " + TheRobotMeta.RuntimeRobotID + "\n";
        ret += "RuntimeRobotLeagueID: " + TheRobotMeta.RuntimeRobotLeagueID + "\n";
        ret += "RuntimeRobotOwnerName: " + TheRobotMeta.RuntimeRobotOwnerName + "\n";
        ret += "RuntimeScore: " + TheRobotMeta.RuntimeScore + "\n";
        ret += "ShotsFired: " + TheRobotMeta.ShotsFired + "\n";
        ret += "ShotsHit: " + TheRobotMeta.ShotsHit + "\n";
        ret += "TimeDied: " + TheRobotMeta.TimeDied + "\n";
        ret += "TimeStarted: " + TheRobotMeta.TimeStarted + "\n";
        return ret;
    }
}
