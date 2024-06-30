using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentSelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.parent != null) transform.parent = null;
		
	}
	
	// Update is called once per frame
	void Update () {
  }
}
