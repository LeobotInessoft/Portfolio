using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; 

public class ArenaLookup : MonoBehaviour {
     // Use this for initialization
    public LoadScenes BundleSceneLoader;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadArena(xArena anArena)
    {
     
        if (anArena.ExternalURLocation.Length == 0)
        {
            SceneManager.LoadScene(anArena.BuiltInLevelID, LoadSceneMode.Single);
      
        }
        else
        {
            BundleSceneLoader.server = anArena.ExternalURLocation;
            BundleSceneLoader.sceneAssetBundle = anArena.AssetBundleName + "";
            BundleSceneLoader.sceneName= anArena.BuiltInLevelID;
            BundleSceneLoader.enabled = true;
            //todo load asset bundle!
        }
    }
}
