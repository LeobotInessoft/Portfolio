using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTest : MonoBehaviour {
  //  UnityEngine.FrameRecorder.RecorderComponent aRecComp;
    UnityEngine.FrameRecorder.Recorder aRec;
    UnityEngine.FrameRecorder.RecordingSession aSess;
	// Use this for initialization
	void Start () {
       
        aRec.BeginRecording(aSess);
        
	}
	
	// Update is called once per frame
	void Update () {
   
	}
}
