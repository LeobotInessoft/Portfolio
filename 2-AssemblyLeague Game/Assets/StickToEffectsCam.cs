using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToEffectsCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    bool hasSetup = false;
	// Update is called once per frame
	void Update () {
        if (hasSetup == false)
        {
            if (CameraEffectsManager.PublicAccess != null)
            {
                gameObject.transform.parent = CameraEffectsManager.PublicAccess.gameObject.transform;
                gameObject.transform.localPosition = Vector3.zero;
                hasSetup = true;
            }
        }
	}
}
