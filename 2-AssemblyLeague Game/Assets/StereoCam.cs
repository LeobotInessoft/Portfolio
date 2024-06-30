using UnityEngine;
using System.Collections;

public class StereoCam : MonoBehaviour {
    public Transform Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.LookAt(Target);
	}
}
