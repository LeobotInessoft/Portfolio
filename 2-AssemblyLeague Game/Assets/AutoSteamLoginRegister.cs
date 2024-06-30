using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSteamLoginRegister : MonoBehaviour {
    public SteamLogic Steam;
    public WwwLeagueInterface WWW;
	// Use this for initialization
	void Start () {
		
	}
    bool Started = false;
	// Update is called once per frame
	void Update () {
        if (WwwLeagueInterface.PlatformForRelease == xActionRequest.EnumGamePlatformType.SteamPC)
        {
            if (Started == false)
            {
                if (Steam.IsInit)
                {
                    //if (WWW.IsBusy == false)
                    {
                        WWW.SteamLoginForce();
                        Started = true;
                    }
                }
            }
        }
	}

    
}
