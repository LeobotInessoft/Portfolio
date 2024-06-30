using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScrollZoom : MonoBehaviour
{

    public Camera CameraToModify;
    public Transform Mover;
    public float defaultFieldOfView = 60;

    // Use this for initialization
    void Start()
    {

    }
    public void RestoreFoV()
    {
        CameraToModify.fieldOfView = defaultFieldOfView;
    }
    public Vector3 locPos;
    // Update is called once per frame
    void Update()
    {
        locPos = Mover.localPosition;
        locPos.z += Input.mouseScrollDelta.y;
        if (locPos.z < -100)
        {
            locPos.z = -100;
        }
        if (locPos.z > 1)
        {
            CameraToModify.fieldOfView -= Input.mouseScrollDelta.y;
            if (CameraToModify.fieldOfView < 15) CameraToModify.fieldOfView = 15;

            locPos.z = 1;
        }
        else
        {
            if (CameraToModify.fieldOfView < 60)
            {
                CameraToModify.fieldOfView -= Input.mouseScrollDelta.y;
            }
            else
            {
                CameraToModify.fieldOfView = 60;
                Mover.localPosition = locPos;
            }
        }
    }

}
