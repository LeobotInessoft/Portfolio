using UnityEngine;
using System.Collections;

public class TracksController : MonoBehaviour {
    public Vector3 TargetDirection;

    public float WheelSpinSpeed = 0;
    public int RollState = 0;
    //public bool IsDriving;
     Animator TheAnim;
	// Use this for initialization
	void Start () {
        TheAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        TheAnim.SetInteger("RollState", RollState);
	}
}
