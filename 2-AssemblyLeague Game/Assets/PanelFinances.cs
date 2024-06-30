using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFinances : MonoBehaviour {

    public WwwLeagueInterface wwwLeague;
    public GameObject PanelLoading;
    public float TimeBetweenUpdates = 5f;
    float currentTime = 0;
    System.DateTime LastUpdateDateUTC = System.DateTime.UtcNow;
    public PanelFinancialsContent TheContent;
    void Start()
    {
        LastUpdateDateUTC = System.DateTime.UtcNow.AddDays(-30);

        PanelLoading.gameObject.SetActive(true);
        currentTime = TimeBetweenUpdates + 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > TimeBetweenUpdates)
        {
            if (wwwLeague.IsBusy == false)
            {
                currentTime = 0;
                DoUpdateRequest();
            }

        }

    }
    private void DoUpdateRequest()
    {
        string msg = "";
        

        wwwLeague.RetrieveFinancials();
    }

    public void SetInfo(RetrieveFinancialsResult res)
    {
        TheContent.SetCurrentDataSet(res.LastFinances);
        PanelLoading.gameObject.SetActive(false);
    }
}
