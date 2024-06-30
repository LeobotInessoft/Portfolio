using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHudRowUpdater : MonoBehaviour {
    public RobotLeaderboardRow TheRow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObjectFollower.PublicAccess.GameObjectToFollow == null)
        {
            TheRow.gameObject.SetActive(false);
        }
        else
        {
            TheRow.SetRow(GameObjectFollower.PublicAccess.GameObjectToFollow.GetComponent<RobotMeta>());
            TheRow.gameObject.SetActive(true);
       
        }
	}
}
