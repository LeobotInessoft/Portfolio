using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using NAudio;

using NAudio.Wave;
using System.IO;

public class WwwTTSLoader : MonoBehaviour
{
    public RobotOwnerLookup TheLookup;
    public AudioSource TheSource;
    public AudioClip TheClip;
    public AudioClip TempClip;

    public IWavePlayer mWaveOutDevice;

    public WaveStream mMainOutputStream;

    public WaveChannel32 mVolumeStream;
    public AudioFileReader TheReader;
    float[] AudioData;
    private bool LoadAudioFromData(byte[] data)
    {
        bool ret = false;
        try
        {

            {
                string tmpName = "tmp.mp3";
                System.IO.File.WriteAllBytes(tmpName, data);

                TheReader = new AudioFileReader(tmpName);

                //Create an empty float to fill with song data
                AudioData = new float[TheReader.Length];
                //Read the file and fill the float
                TheReader.Read(AudioData, 0, AudioData.Length);

                //Create a clip file the size needed to collect the sound data
                TempClip = AudioClip.Create(Path.GetFileNameWithoutExtension(tmpName), AudioData.Length, TheReader.WaveFormat.Channels, TheReader.WaveFormat.SampleRate, false);
                //Fill the file with the sound data
                TempClip.SetData(AudioData, 0);
                //Set the file as the current active sound clip
                TheClip = TempClip;
                TheSource.clip = TheClip;
                TheSource.Play();
            }

            ret = true;

        }

        catch (System.Exception ex)
        {

            Debug.LogWarning("Error! " + ex.Message);

        }



        return ret;

    }
    void OnDestroy()
    {
        print("Script was destroyed");

        if (mMainOutputStream != null)
        {
            mWaveOutDevice.Stop();
            mMainOutputStream.Flush();
            mMainOutputStream.Close();
            mVolumeStream.Flush();
            mVolumeStream.Close();

            mWaveOutDevice = null;
            mMainOutputStream = null;
            mVolumeStream = null;
            System.GC.Collect();
        }
    }
    private IEnumerator LoadAudio(string fileNme)
    {
        ApplySoundOptions();


        WWW www = new WWW(fileNme);
        yield return www;




        byte[] imageData = www.bytes;
        if (imageData != null && imageData.Length > 0)
        {
            LoadAudioFromData(imageData);
        }


    }
    public void ApplySoundOptions()
    {
        RobotOwnerLookup.PlayerOptions op = TheLookup.GetPlayerOptions();
        if (op != null)
        {
            TheSource.volume = ((op.FXVolume + 0.0001f) / 100f) * 0.99f;
        }
    }
    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        TheClip = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadAudioStart(string url)
    {
        StartCoroutine(LoadAudio(url));
    }
    private IEnumerator LoadAudio2(string url)
    {
        WWW loader = new WWW(url);
        yield return loader;

        TheClip = loader.GetAudioClipCompressed(true, AudioType.MPEG);

    }
}
