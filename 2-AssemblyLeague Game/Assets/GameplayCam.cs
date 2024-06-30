using UnityEngine;
using System.Collections;

public class GameplayCam : MonoBehaviour
{
    public bool IsAutoSwitchOn = false;
    public Match CurrentMatch;
    System.DateTime nextSwitchTime;
    public int currentPlayerIndex;
    public GameObjectFollower TheFollower;
    public bool StayOnObject = false;
    // Use this for initialization
    void Start()
    {
        nextSwitchTime = System.DateTime.Now;
    }
    float timeHealthStayed0 = 0;
    GameObject awaitingToSwitchTo = null;
    float waitiTimeDeath = 0;
    // Update is called once per frame
    void Update()
    {
        if (awaitingToSwitchTo != null)
        {
            waitiTimeDeath += Time.deltaTime;
            if (waitiTimeDeath >= 3f)
            {
                TheFollower.GameObjectToFollow = awaitingToSwitchTo;
                awaitingToSwitchTo = null;
                waitiTimeDeath = 0;
            }
        }
        GameObject currentFollow = GameObjectFollower.PublicAccess.GameObjectToFollow;
        if (currentFollow != null)
        {
            RobotMeta aMeta = currentFollow.transform.GetComponent<RobotMeta>();
            if (aMeta != null)
            {
                if (aMeta.IsDead)
                {
                    if (aMeta.LastRobotThatHitMe != null)
                    {
                        if (aMeta.LastRobotThatHitMe.IsDead == false)
                        {
                            awaitingToSwitchTo = aMeta.LastRobotThatHitMe.gameObject;
                        }
                    }
                }
            }
        }



    }
}
