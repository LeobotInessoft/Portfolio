using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockVR.Video;
public class LeagueRecorder : MonoBehaviour
{
    public bool IsConfiguredEnabled = true;
    public bool IsFinished = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsConfiguredEnabled)
        {
            if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.FINISH)
            {
                IsFinished = true;
            }
        }
    }

    public void BegingRecord()
    {
        if (IsConfiguredEnabled)
        {
            if (System.IO.Directory.Exists(PathConfig.saveFolder) == false) System.IO.Directory.CreateDirectory(PathConfig.saveFolder);
            print("START Video " + PathConfig.saveFolder);

            IsFinished = false;

            VideoCaptureCtrl.instance.StartCapture();
        }
    }
    public void FinishRecord()
    {
        if (IsConfiguredEnabled)
        {
            print("FINISH Video");
            VideoCaptureCtrl.instance.StopCapture();
        }
    }
    public byte[] GetVideo()
    {
        byte[] ret = new byte[0];
        if (IsConfiguredEnabled)
        {
            string fileName = PathConfig.saveFolder + Match.VideoID + ".mp4";
            if (System.IO.File.Exists(fileName))
            {
                ret = System.IO.File.ReadAllBytes(fileName);
            }
            string[] files = System.IO.Directory.GetFiles(PathConfig.saveFolder, "*.mp4");

        }

        return ret;
    }
}
