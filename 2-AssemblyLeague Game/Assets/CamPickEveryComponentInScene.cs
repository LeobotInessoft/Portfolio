using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPickEveryComponentInScene : MonoBehaviour {
    public CamPicTaker ThePicTaker;
    public bool MustTakePics = false;
    bool isBusyTakingPics = false;
    int currentIndex = 0;
    List<ComponentType> allTypes;
    public Transform BaseParent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (MustTakePics)
        {
            allTypes = new List<ComponentType>();
            allTypes.AddRange(BaseParent.transform.GetComponentsInChildren<ComponentType>());

            MustTakePics = false;
            isBusyTakingPics = true;
        }
        if (isBusyTakingPics)
        {
            TakePic(allTypes[currentIndex]);
            currentIndex++;

            if (currentIndex >= allTypes.Count)
            {
                currentIndex = 0;
                isBusyTakingPics = false;
            }
            
        }
	}
    private void TakePic(ComponentType aComponent)
    {
        print("Taking Pic Of " + aComponent.DeviceName);
        ThePicTaker.Target = aComponent.gameObject;
        ThePicTaker.picName = aComponent.UniqueDeviceID;//3 +" " + aComponent.DeviceName;
        ThePicTaker.MustTakePic = true;
    }
}
