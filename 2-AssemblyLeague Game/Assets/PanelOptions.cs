using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelOptions : MonoBehaviour {

    public Slider SliderMusic;
    public Slider SliderBattleMusic;
    public Slider SliderFX;
    public Toggle ToggleRetriveImages;
    public Toggle ToggleRetrieveIntroTTS;
    public Toggle ToggleGraphicsLevelLow;
    public Toggle ToggleGraphicsLevelMedium;
    public Toggle ToggleGraphicsLevelHigh;
    public Toggle ToggleGraphicsLevelFull;


    public PanelCredits ThePanelCredits;
	// Use this for initialization
	void Start () {
        ThePanelCredits.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ButtonCreditsClick()
    {
        AudioController.PlayMusicPlaylist("CreditsPlay");
        ThePanelCredits.gameObject.SetActive(true);
    }
    public void ButtonCloseCreditsClick()
    {
        ThePanelCredits.gameObject.SetActive(false);
   
    }
    public void SetOptions(RobotOwnerLookup.PlayerOptions opt)
    {
        SliderMusic.minValue = 0;
        SliderMusic.maxValue = 100;
        SliderMusic.value = opt.MusicVolume;

        SliderBattleMusic.minValue = 0;
        SliderBattleMusic.maxValue = 100;
        SliderBattleMusic.value = opt.BattleVolume;
        
        
        SliderFX.minValue = 0;
        SliderFX.maxValue = 100;
        SliderFX.value = opt.FXVolume;


        ToggleRetriveImages.isOn = opt.RetrieveRobotImages;
        ToggleRetrieveIntroTTS.isOn = opt.RetrieveRobotIntroTTS;

        if (opt.GraphicsLevel == 0) ToggleGraphicsLevelLow.isOn = true;
        if (opt.GraphicsLevel == 1) ToggleGraphicsLevelMedium.isOn = true;
        if (opt.GraphicsLevel == 2) ToggleGraphicsLevelHigh.isOn = true;
        if (opt.GraphicsLevel == 3) ToggleGraphicsLevelFull.isOn = true;



    }
    public RobotOwnerLookup.PlayerOptions GetOptions()
    {
        RobotOwnerLookup.PlayerOptions ret = new RobotOwnerLookup.PlayerOptions();

        ret.FXVolume = SliderFX.value;
        ret.MusicVolume = SliderMusic.value;
        ret.BattleVolume = SliderBattleMusic.value;
        ret.RetrieveRobotImages = ToggleRetriveImages.isOn;
        ret.RetrieveRobotIntroTTS = ToggleRetrieveIntroTTS.isOn;
        if (ToggleGraphicsLevelLow.isOn) ret.GraphicsLevel = 0;
        if (ToggleGraphicsLevelMedium.isOn) ret.GraphicsLevel = 1;
        if (ToggleGraphicsLevelHigh.isOn) ret.GraphicsLevel = 2;
        if (ToggleGraphicsLevelFull.isOn) ret.GraphicsLevel = 3;
 
        return ret;
    }
}
