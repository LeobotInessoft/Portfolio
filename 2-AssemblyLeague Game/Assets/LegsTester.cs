using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsTester : MonoBehaviour
{
    public int MovementSpeed = 100;
    public List<IoLegsMech> allLegs = new List<IoLegsMech>();
    public bool DoForwardMoveTest = false;
    public bool DoLookAtTest = false;
    public Transform LookAtCube;
    public bool DoSelfDestructTest = false;
 
    void Start()
    {
        allLegs = new List<IoLegsMech>();
        allLegs.AddRange(transform.GetComponentsInChildren<IoLegsMech>());
        for (int c = 0; c < allLegs.Count; c++)
        {
            RobotMeta aMeta = allLegs[c].gameObject.transform.GetComponent<RobotMeta>();
            aMeta.Health = 100;
            if (DoForwardMoveTest)
            {
                allLegs[c].WantedSpeedOutOf100 = MovementSpeed;
            }
            else
            {
                allLegs[c].WantedSpeedOutOf100 = 0;

            }
            if (DoLookAtTest)
            {
                allLegs[c].WantedLookDestination = LookAtCube.transform.position;

                allLegs[c].WantedLookDestination.y = allLegs[c].transform.position.y;// Arena.PublicAccess.Therrain.SampleHeight(WantedLookDestination) + Arena.PublicAccess.gameObject.transform.position.y;
                allLegs[c].targetDir = allLegs[c].WantedLookDestination - transform.position;
            }
          
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int c = 0; c < allLegs.Count; c++)
        {
            if (DoForwardMoveTest)
            {
                allLegs[c].WantedSpeedOutOf100 = MovementSpeed;
            }
            else
            {
                //  allLegs[c].WantedSpeedOutOf100 = 0;

            }
            if (DoLookAtTest)
            {
                allLegs[c].WantedLookDestination = LookAtCube.transform.position;

                allLegs[c].WantedLookDestination.y = allLegs[c].transform.position.y;// Arena.PublicAccess.Therrain.SampleHeight(WantedLookDestination) + Arena.PublicAccess.gameObject.transform.position.y;
                allLegs[c].targetDir = allLegs[c].WantedLookDestination - allLegs[c].transform.position;
            }
            if (DoSelfDestructTest)
            {
                allLegs[c].myComponentType.MustSelfDestruct = true;

            }
           
        }
    }
}
