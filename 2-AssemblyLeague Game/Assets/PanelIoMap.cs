using UnityEngine;
using System.Collections;

public class PanelIoMap : MonoBehaviour {

    public ComponentHelpManual ComponentManual;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ShowManual(ComponentType HelpManual, RobotConstructor.DataComponentType TheRobotComponent, string currentIO)
    {

        ComponentManual.SetComponent(HelpManual,TheRobotComponent, currentIO);
        ComponentManual.gameObject.SetActive(true);
    }
    public void CloseManual()
    {
        ComponentManual.gameObject.SetActive(false);
    }
}
