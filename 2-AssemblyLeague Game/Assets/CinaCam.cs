using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinaCam : MonoBehaviour {
    public CinaCamTarget CinaTarget;
    public GameObjectFollower Follower;
    public GameObject Target;
	// Use this for initialization
	
    public class CinamaPrediction
    {
        public long FrameNumberWhenPredictionWasCreated;
        public long FrameNumberWhenPredicionWillComeTrue;
        public enum EnumPredictionType
        {
            BulletWillImpactRobot,
            RobotBeDestroyed,


        }



    }
    
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        switch (Follower.FollowType)
        {
            case GameObjectFollower.EnumFollowType.FreeMouse:
                {
                    float speed = 10f;
                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed, Space.Self);
 
                  
                    break;
                }
        }
	}
}
