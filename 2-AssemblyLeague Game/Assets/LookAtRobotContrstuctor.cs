using UnityEngine;
using System.Collections;

public class LookAtRobotContrstuctor : MonoBehaviour {
    public RobotConstructor MyRobotConstructor;
	// Use this for initialization
	void Start () {
	
	}
    float xDir = 1;
    float zDir = -1;
	// Update is called once per frame
	void Update () {

        Vector3 newPos = transform.position;
        newPos.x += 0.05f * xDir;
        newPos.z += 0.05f * zDir;

        if (newPos.x > 10)
        {
            xDir = -1;
         }
        if (newPos.z > 10)
        {
             zDir = -1;
        }

        if (newPos.x < -10 )
        {
            xDir = 1;
        }
        if (newPos.z < -10)
        {
             zDir = 1;
        }
    
        transform.position = newPos;
        if (MyRobotConstructor.CurrentAttachTarget != null)
        {
            transform.LookAt(MyRobotConstructor.CurrentAttachTarget.transform);
        }
        if (transform.position.y <= MyRobotConstructor.CurrentAttachTarget.transform.position.y)
        {
            transform.Translate(Vector3.up * 10f*Time.deltaTime, Space.World);
        }

        if (transform.position.y > MyRobotConstructor.CurrentAttachTarget.transform.position.y)
        {
            transform.Translate(Vector3.up * -1 * 10f * Time.deltaTime, Space.World);
        }

	}

    private float ReturnCirclePos(float x, float radius)
    {
        float y = 0;
        y = Mathf.Sqrt(radius - x * x);
        return y;
    }
}
