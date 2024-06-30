using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosChanger : MonoBehaviour {
    public Transform Target;
    public List<Transform> PossibleTargets;
    public Transform ObjectToLookAt;
    public RobotConstructor TheConstructor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (TheConstructor.CurrentAttachTarget != null)
        {
            ObjectToLookAt = TheConstructor.CurrentAttachTarget.gameObject.transform;
        }
        else
        {
            ObjectToLookAt = TheConstructor.BuildPlatformTarget;
        
        }
        if (Target != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, Target.position);
            if (distance <= 0.05f)
            {
                gameObject.transform.position = Target.position;
                gameObject.transform.rotation = Target.rotation;


            }
            else
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Target.position, Time.deltaTime*5f);
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Target.rotation, Time.deltaTime*5f);
             
            }
        }
        if (ObjectToLookAt != null)
        {

            Transform bestTarget = Target;
            if (Target == null)
            {
                bestTarget = PossibleTargets[0];

            }
            float bestDistance = Vector3.Distance(bestTarget.position, ObjectToLookAt.position);
            if (PossibleTargets.Count > 0)
            {
                for (int c = 0; c < PossibleTargets.Count; c++)
                {
                    float aDistance = Vector3.Distance(PossibleTargets[c].position, ObjectToLookAt.position);
                    if (aDistance < bestDistance)
                    {
                        bestDistance = aDistance;
                        bestTarget = PossibleTargets[c];
                    }
                }
                Target = bestTarget;
            }

        }
	
	}
}
