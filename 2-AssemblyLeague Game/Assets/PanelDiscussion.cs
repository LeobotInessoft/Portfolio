using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelDiscussion : MonoBehaviour
{
    public WwwLeagueInterface wwwLeague;
    public Text InputTextToShow;
    public InputField InputToSend;
    public List<string> MessageWaitingToBeSent;
    public ScrollRect TheRect;
    public ScrollRect TheRectPlayers;

    public float TimeBetweenUpdates = 5f;
    float currentTime = 1000;
    System.DateTime LastUpdateDateUTC = System.DateTime.UtcNow;
    public GameObject PanelLoading;
    public Text TextListOfPlayers;
    // Use this for initialization
    void Start()
    {
        LastUpdateDateUTC = System.DateTime.UtcNow.AddDays(-30);

        MessageWaitingToBeSent = new List<string>();
        PanelLoading.gameObject.SetActive(true);
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
        if (MessageWaitingToBeSent.Count > 0)
        {
            msg = MessageWaitingToBeSent[0];
            MessageWaitingToBeSent.RemoveAt(0);

        }

        wwwLeague.SendReceiveDiscussion(msg, LastUpdateDateUTC);
    }
    List<xDiscussion> currentDisscussions = new List<xDiscussion>();
    public void AddDiscussions(List<xDiscussion> discList)
    {
        for (int c = 0; c < discList.Count; c++)
        {
            AddDiscussion(discList[c]);
        }
        PanelLoading.gameObject.SetActive(false);
    }
    private void AddDiscussion(xDiscussion aDiscussion)
    {
        bool HaveNewOnes = false;
        try
        {
            if (currentDisscussions.Count(x => x.ID == aDiscussion.ID) == 0)
            {
                currentDisscussions.Add(aDiscussion);
                LastUpdateDateUTC = aDiscussion.UtcDate;
                HaveNewOnes = true;
            }
            if (HaveNewOnes)
            {
                GenerateDiscussions();

                TheRect.verticalNormalizedPosition = 1.0f;
            }
        }catch
        {

        }
    }
    private void GenerateDiscussions()
    {
        InputTextToShow.text = "";
        for (int c = currentDisscussions.Count - 1; c >= 0; c--)
        {
            InputTextToShow.text += "\n" + currentDisscussions[c].PlayerSenderName + ": " + currentDisscussions[c].Message;

        }
        GenerateListOfPlayers();
    }
    private void GenerateListOfPlayers()
    {
        TextListOfPlayers.text = "";
        List<string> PlayerNames = currentDisscussions.Select(x => x.PlayerSenderName).Distinct().OrderBy(x => x).ToList();
        {
            for (int c = 0; c < PlayerNames.Count; c++)
            {
                TextListOfPlayers.text += currentDisscussions[c].PlayerSenderName + "\n\n" ;

            }
        }
        TheRectPlayers.verticalNormalizedPosition = 1.0f;
      
    }
    public void ButtonSendClick()
    {
        if (InputToSend.text.Trim().Length > 0)
        {
            MessageWaitingToBeSent.Add(InputToSend.text.Trim());
            currentTime = TimeBetweenUpdates + 1;
            InputToSend.text = "";
        }

    }
}
