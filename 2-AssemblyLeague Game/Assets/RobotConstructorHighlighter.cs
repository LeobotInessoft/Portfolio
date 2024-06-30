using UnityEngine;
using System.Collections;

public class RobotConstructorHighlighter : MonoBehaviour {
    public RobotConstructor MyRobotConsrtructor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (MyRobotConsrtructor.CurrentAttachTarget != null)
        {
            gameObject.transform.position = MyRobotConsrtructor.CurrentAttachTarget.gameObject.transform.position;
        }
    }
}
