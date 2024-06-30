using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelComponentDescription : MonoBehaviour
{
    public Image TheImage;
    public Text TextDescription;
    public string ComponentToShow = "e4438c52-4c62-4e72-aad2-14a47eec38c0";
    public string AcceptorToShow = "empty";

    // Use this for initialization
    void Start()
    {
        // ShowComponent(ComponentToShow);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowComponent(ComponentType aType)
    {
        Transform asParent = null;
        if (CameraEffectsManager.PublicAccess != null)
        {
            asParent = CameraEffectsManager.PublicAccess.gameObject.transform;
        }
        if (aType != null)
        {
            ComponentToShow = aType.UniqueDeviceID;
            TheImage.sprite = Resources.Load<Sprite>("Component Images/" + aType.UniqueDeviceID + "");
            TextDescription.text = aType.ShortDescription;
            AuxAudio.PublicAccess.PlayComponentName(aType, asParent);

        }
        else
        {
            TextDescription.text = "";
            ComponentToShow = "";
            TheImage.sprite = Resources.Load<Sprite>("Component Images/EMPTY");
        }
    }
    public void ShowAcceptor(ModuleAcceptor aType)
    {
        print("Show Acceptor");
        if (aType != null)
        {
            AcceptorToShow = aType.UniqueDeviceID;
            TheImage.sprite = Resources.Load<Sprite>("Acceptor Images/" + aType.UniqueDeviceID + "");

            if (TheImage.sprite != null)
            {
                TheImage.sprite = Resources.Load<Sprite>("Acceptor Images/" + aType.UniqueDeviceID + "");

                string acceptList = "";
                if (aType.AllowedTypes == null || aType.AllowedTypes.Count == 0)
                {
                    acceptList = "NONE";
                }
                else
                {
                    for (int c = 0; c < aType.AllowedTypes.Count; c++)
                    {
                        if (c > 0) acceptList += ", ";
                        acceptList += ComponentType.GetDisplayString(aType.AllowedTypes[c]);
                    }
                }
                TextDescription.text = "Module Plug Accepts:" + acceptList;
                print("Show Acceptor set " + aType.UniqueDeviceID);
            }
            else
            {
                print("Show Acceptor not found " + aType.UniqueDeviceID);
                TextDescription.text = "";
                AcceptorToShow = "";
                TheImage.sprite = Resources.Load<Sprite>("Acceptor Images/EMPTY");

            }
        }
        else
        {
            TextDescription.text = "";
            AcceptorToShow = "";
            TheImage.sprite = Resources.Load<Sprite>("Acceptor Images/EMPTY");
        }
    }
}
