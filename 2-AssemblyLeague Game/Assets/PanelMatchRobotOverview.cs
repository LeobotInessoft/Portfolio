using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelMatchRobotOverview : MonoBehaviour {
    public InputField TheCode;
    public RobotMeta TheRobotMeta;
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (TheRobotMeta != null)
        {
            TheCode.text = TheRobotMeta.Template.RobotName;
        }
	}
}
