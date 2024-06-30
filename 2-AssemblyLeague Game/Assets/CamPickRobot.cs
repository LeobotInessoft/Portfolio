using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPickRobot : MonoBehaviour
{

    public CamPicTaker ThePicTaker;
    public GameObject TheTarget= null;
    //public bool MustTakePic;
    // Use this for initialization
    void Start()
    {

    }
    
    public byte[] TakePicture(string named)
    {

        ThePicTaker.picName = named;
          
     if (TheTarget != null)
        {
            ComponentType aComp = TheTarget.GetComponent<ComponentType>();
            return TakePic(aComp);
        }
        else
        {
            return new byte[0]; 
        }

    }
    
    private byte[] TakePic(ComponentType aComponent)
    {
        byte[] picData = new byte[0];
        if (aComponent != null)
        {
            print("Taking Pic Of ROBOT " + aComponent.DeviceName);
            ThePicTaker.Target = aComponent.gameObject;
             ThePicTaker.MustTakePic = true;
            ThePicTaker.DoTakePicLogic();
            picData = ThePicTaker.GetLastPicBytes();
        }
        return picData;

    }	// Update is called once per frame
    void Update()
    {
       
    }
}
