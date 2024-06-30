using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PanelMatchRobotLog : MonoBehaviour
{
    public InputField TheCode;
  public   RobotMeta TheRobotMeta;
    RobotMeta prevRob = null;
    public bool TurnLoggingOffForUnFocussed = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TheRobotMeta = GameObjectFollower.PublicAccess.GameObjectToFollow.transform.GetComponent<RobotMeta>();
        if (TheRobotMeta != null)
        {
            TheRobotMeta.IsEventLoggingOn = true;
            TheCode.text = GenerateStats();

            if (TurnLoggingOffForUnFocussed)
            {
                if (prevRob != null)
                {
                    if (prevRob.transform != TheRobotMeta.transform)
                    {
                        prevRob.IsEventLoggingOn = false;
                    }
                }
            }
            prevRob = TheRobotMeta;
        }
        else
        {
            print("DEAD");
             }

    }
    private string GenerateStats()
    {
        string ret = "";
        for (int c = 0; c < TheRobotMeta.EventLog.Count; c++)
        {
            ret += c + ": " + TheRobotMeta.EventLog[c] + "\n";

        }
        return ret;
    }
}
