﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfBelowGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.position.y <= -2000)
        {
            Destroy(gameObject);

        }

	}
}
