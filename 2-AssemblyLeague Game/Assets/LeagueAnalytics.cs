using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
public class LeagueAnalytics : MonoBehaviour
{
    public static LeagueAnalytics PublicAccess;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        AddStat_OpenScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddStat_OpenScene(string sceneName)
    {
        
        Analytics.CustomEvent("OpenScene", new Dictionary<string, object>
  {
    { "sceneName", sceneName }

  });
        print("Done Adding Stat");
    }

   
}
