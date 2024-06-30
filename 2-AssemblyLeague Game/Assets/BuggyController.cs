using UnityEngine;
using System.Collections;

public class BuggyController : MonoBehaviour
{
    public GameObject FrontLeftWheel;
    public GameObject FrontRightWheel;
    public GameObject BackLeftWheel;
    public GameObject BackRightWheel;
    public Vector3 TargetDirection;
    public IoLegsMech MyLegs;
    public float WheelSpinSpeed = 0;
    // Use this for initialization
    void Start()
    {
      }

    // Update is called once per frame
    void Update()
    {
        if (MyLegs == null) MyLegs = gameObject.transform.GetComponentInParent<IoLegsMech>();
        if (MyLegs != null)
        {
            if (MyLegs.IsOn && MyLegs.IsDead == false)
            {
                float spinDir = 1;
                if (WheelSpinSpeed > 0) spinDir = -1;
                if (WheelSpinSpeed < 0) spinDir = 1;
                if (WheelSpinSpeed == 0) spinDir = 0;

                BackLeftWheel.transform.Rotate(Vector3.right * spinDir, WheelSpinSpeed);
                BackRightWheel.transform.Rotate(Vector3.right * spinDir, WheelSpinSpeed);
                  if (FrontLeftWheel != null)
                {
                    float step = 100 * Time.deltaTime * 0.1f;
                    Vector3 newDir = Vector3.RotateTowards(FrontLeftWheel.transform.forward + Vector3.up * 1, TargetDirection, step, 0.0F);
                    //Debug.DrawRay(transform.position, newDir, Color.red);
                    FrontLeftWheel.transform.rotation = Quaternion.LookRotation(newDir);
                }
                if (FrontRightWheel != null)
                {
                    float step = 100 * Time.deltaTime * 0.1f;
                    Vector3 newDir = Vector3.RotateTowards(FrontRightWheel.transform.forward + Vector3.up * 1, TargetDirection, step, 0.0F);
                    //Debug.DrawRay(transform.position, newDir, Color.red);
                    FrontRightWheel.transform.rotation = Quaternion.LookRotation(newDir);
                }
            }
        }
    }
}
