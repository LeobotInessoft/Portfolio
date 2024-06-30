using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPickEveryAcceptorInScen : MonoBehaviour
{
    public CamPicTaker ThePicTaker;
    public bool MustTakePics = false;
    bool isBusyTakingPics = false;
    int currentIndex = 0;
    List<ModuleAcceptor> allTypes;
    public Transform BaseParent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MustTakePics)
        {
            allTypes = new List<ModuleAcceptor>();
            allTypes.AddRange(BaseParent.transform.GetComponentsInChildren<ModuleAcceptor>());

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
    private void TakePic(ModuleAcceptor aComponent)
    {
        ComponentType parComp = aComponent.gameObject.transform.GetComponentInParent<ComponentType>();
        int defLayer = 0;

        List<Transform> allTrans = new List<Transform>();
        allTrans.Add(parComp.transform);
        allTrans.AddRange(parComp.transform.GetComponentsInChildren<Transform>());
        for (int c = 0; c < allTrans.Count; c++)
        {
            GameObject theObj = allTrans[c].gameObject;
            if (theObj != null)
            {
                theObj.layer = CamPicTaker.PicTakLayer;

            }
        }

        print("Taking Pic Of ACCEPTOR " + aComponent.UniqueDeviceID);
        ThePicTaker.Target = aComponent.gameObject;
        ThePicTaker.picName = aComponent.UniqueDeviceID;//3 +" " + aComponent.DeviceName;
        ThePicTaker.MustTakePic = true;
        ThePicTaker.DoTakePicLogic();
        for (int c = 0; c < allTrans.Count; c++)
        {
            GameObject theObj = allTrans[c].gameObject;
            if (theObj != null)
            {
                theObj.layer = 0;

            }
        }
    }
}