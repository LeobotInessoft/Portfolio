using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsTester : MonoBehaviour {
    public List<IoWeapon> allLegs = new List<IoWeapon>();
    public bool DoShootTest = false;
    public bool DoLookAtTest = false;
    public bool DoSelfDestructTest = false;
    public Transform LookAtCube;
  
	// Use this for initialization
	void Start () {
        allLegs = new List<IoWeapon>();
        allLegs.AddRange(transform.GetComponentsInChildren<IoWeapon>());
        for (int c = 0; c < allLegs.Count; c++)
        {
            RobotMeta aMeta = allLegs[c].gameObject.AddComponent<RobotMeta>();
            aMeta.Health = 100;
            allLegs[c].TheRobotMeta = aMeta;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int c = 0; c < allLegs.Count; c++)
        {
            if (allLegs[c] != null && allLegs[c].gameObject != null)
            {
                RobotMeta aMeta = allLegs[c].gameObject.GetComponent<RobotMeta>();
                aMeta.Health = 100;
                allLegs[c].TheRobotMeta = aMeta;

                if (DoShootTest)
                {
                    Computer.StandardStack aStack = new Computer.StandardStack();
                    aStack.Ax.Val = "1";
                    allLegs[c].MyIoHandler_IoHandler(ref aStack);
                }
                if (DoLookAtTest)
                {
                    Computer.StandardStack aStack = new Computer.StandardStack();
                    aStack.Ax.Val = "8";
                    aStack.Bx.Val = LookAtCube.transform.position.x + "";
                    aStack.Cx.Val = LookAtCube.transform.position.y + "";
                    aStack.Dx.Val = LookAtCube.transform.position.z + "";

                    allLegs[c].MyIoHandler_IoHandler(ref aStack);
                }
                if (DoSelfDestructTest)
                {
                    print("SELF D");
                     allLegs[c].myComponentType.MustSelfDestruct = true;
                }
                
            }
        }
	}
}
