using UnityEngine;
using System.Collections;

public class FaceItTest : MonoBehaviour {

    public Transform Target;
	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 WantedLookDestination = Target.transform.position;
        Vector3 targetDir = WantedLookDestination - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward + Vector3.up * 1, targetDir,1f, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        Quaternion look = Quaternion.LookRotation(newDir);

        Vector3 eular = look.eulerAngles;
        if (transform.lossyScale.x < 0 || transform.lossyScale.z < 0)
        {
          //  eular.y = eular.y * -1f;
           // eular.z = eular.z * -1f;
        }
        print("L"+transform.lossyScale);
        print("E"+eular);
        LeanTween.rotate(this.gameObject, eular, 0);

        //   gameObject.transform.LookAt(Target);
	}

    public void LookAtTrue(Vector3 target, float time)
    {
        Vector3 WantedLookDestination = Target.transform.position;
        Vector3 targetDir = WantedLookDestination - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward + Vector3.up * 1, targetDir, 1f, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        Quaternion look = Quaternion.LookRotation(newDir);

        Vector3 eular = look.eulerAngles;
        if (transform.lossyScale.x < 0 || transform.lossyScale.z < 0)
        {
            eular.y = eular.y * -1f;
            // eular.z = eular.z * -1f;
        }
        print("L" + transform.lossyScale);
        print("E" + eular);
        LeanTween.rotateLocal(this.gameObject, eular, time);
    }
}
