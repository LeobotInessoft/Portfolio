using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxAudio : MonoBehaviour
{

    public Transform CameraPos;
    public AudioController TheController;
    public static AuxAudio PublicAccess;
    float volume = 1f;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (PublicAccess == null)
        {
            PublicAccess = this;

        }
    }

    public void ApplySoundOptions()
    {
        RobotOwnerLookup.PlayerOptions op = RobotOwnerLookup.PublicAccess.GetPlayerOptions();
        if (op != null)
        {
            volume = op.FXVolume;

          }
    }
    public void PlayInMatchAnnouncement(string audioID)
    {
        ApplySoundOptions();
       
        // TheController.UnloadAllAudioClips();
        AudioItem anItem = TheController._GetAudioItem(audioID);
        if (anItem != null)
        {
           
            TheController.PlayAudioItem(anItem, volume, CameraPos.position, CameraPos.gameObject.transform);
        }
    }
    public void PlayInMatchAnnouncement(string audioID, float delay)
    {
        ApplySoundOptions();        
        AudioItem anItem = TheController._GetAudioItem(audioID);
        if (anItem != null)
        {
            
            TheController.PlayAudioItem(anItem, volume, CameraPos.position, CameraPos.gameObject.transform, delay);
        }
    }
    public void PlayComponentName(ComponentType aType)
    {
        ApplySoundOptions();
        AudioItem anItem = TheController._GetAudioItem(aType.UniqueDeviceID);
        if (anItem != null)
        {
             TheController.PlayAudioItem(anItem, volume, CameraPos.position, aType.gameObject.transform);
        }
    }
    public void PlayComponentName(ComponentType aType, Transform asParent)
    {
        ApplySoundOptions();
         // TheController.UnloadAllAudioClips();
        AudioItem anItem = TheController._GetAudioItem(aType.UniqueDeviceID);
        if (anItem != null)
        {
             TheController.PlayAudioItem(anItem, volume, CameraPos.position, asParent);
        }
    }


    public void PlayComponentDescription(ComponentType aType)
    {
        ApplySoundOptions();
        AudioItem anItem = TheController._GetAudioItem(aType.UniqueDeviceID+"desc");
        if (anItem != null)
        {
             TheController.PlayAudioItem(anItem, volume, CameraPos.position, aType.gameObject.transform, 2f, 0);

        }
    }
    public void PlayComponentDescription(ComponentType aType, Transform asParent)
    {
        ApplySoundOptions();
        AudioItem anItem = TheController._GetAudioItem(aType.UniqueDeviceID + "desc");
        if (anItem != null)
        {
             TheController.PlayAudioItem(anItem, volume, CameraPos.position, asParent, 2f, 0);
            
        }
    }
}
