using UnityEngine;
using System.Collections;

public class PositionOnParent : MonoBehaviour {
    public int ParentDeviceID;
    public Vector3 Position;
    public Vector3 Rotation;
    public bool IsSet;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (IsSet)
        {
            gameObject.transform.localPosition = Position;
            gameObject.transform.eulerAngles = Rotation;
            IsSet = false;
        }
	}
}
