using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelCurrentLeader : MonoBehaviour {
    public Text TextLeadingBotName;
    public Text TextLeadingOwnerName;

    public Match TheMatch;
    public MatchReferee TheReferee;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RobotMeta currentLeader = TheMatch.GetRobotOfRuntimeID(TheReferee.LastLeaderRuntimeID);

        if (currentLeader != null)
        {
            TextLeadingBotName.text = currentLeader.RuntimeRobotID+"";
        }
	}
}
