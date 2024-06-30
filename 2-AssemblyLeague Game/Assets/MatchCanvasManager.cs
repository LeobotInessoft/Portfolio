using UnityEngine;
using System.Collections;

public class MatchCanvasManager : MonoBehaviour
{
    public CameraEffectsManager TheCameraEffectsManager;
    public RobotOwnerLookup TheLookup;
    public GameObject PanelGeneral;
    public GameObject PanelLeaderboard;
    public Match TheMatch;
    public static MatchCanvasManager PublicAccess;
    public GameObject ButtonExit;
    public CodePanel TheCodePanel;
    public PanelMatchRobotLog TheLogPanel;
    public PanelMatchRobotOverview TheOverviewPanel;
    public PanelMatchRobotStats TheStatsPanel;
    // Use this for initialization
    void Start()
    {
        PanelGeneral.SetActive(true);
        PanelLeaderboard.SetActive(false);
        PublicAccess = this;
        ApplySoundOptions();
        ApplyCameraOptions();
    }
    public void ApplySoundOptions()
    {
        RobotOwnerLookup.PlayerOptions op = TheLookup.GetPlayerOptions();
        if (op != null)
        {
            AudioController.SetCategoryVolume("Music", ((op.MusicVolume + 0.0001f) / 100f) * 0.8f);
            AudioController.SetCategoryVolume("FX", ((op.MusicVolume + 0.0001f) / 100f) * 0.99f);
        }
    }


    public void ApplyCameraOptions()
    {
        RobotOwnerLookup.PlayerOptions op = TheLookup.GetPlayerOptions();
        if (op != null)
        {
            TheCameraEffectsManager.SetOptions(op.GraphicsLevel);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Match.IsLeagueMatch)
        {
            if (Match.LeagueMatchID != 0)
            {
                ButtonExit.SetActive(false);
            }
            else
            {
                if (WwwLeagueInterface.PublicAccess.IsBusy)
                {
                    ButtonExit.SetActive(false);
                }
                else
                {
                    ButtonExit.SetActive(true);

                }
            }

        }
        else
        {
            ButtonExit.SetActive(true);
        }
        if (closeASAP)
        {
            if (WwwLeagueInterface.PublicAccess.IsBusy == false)
            {
                closeASAP = false;
                ButtonCloseLeaderboardClick();

            }
        }
    }
    public void ButtonLeaderboardClick()
    {
        PanelGeneral.SetActive(false);
        PanelLeaderboard.SetActive(true);

    }
    bool closeASAP = false;
    public void ButtonCloseLeaderboardClick()
    {
        {
            PanelGeneral.SetActive(true);
            PanelLeaderboard.SetActive(false);
            if (TheMatch.HasMatchEnded)
            {
                if (Match.IsLeagueMatch)
                {

                    ButtonExitClick();
                }
            }
        }

    }
    public void ButtonPublishCloseLeaderboardClick()
    {
        if (TheMatch.IsWaitingOnUpload == false && TheMatch.MustDoUploadMatch == false)
        {
            TheMatch.MustDoUploadMatch = true;

        }
        else
        {
            closeASAP = true;

        }
    }
    public void ButtonExitClick()
    {
        if (Match.IsLeagueMatch == false)
        {
            TheMatch.TheRecorder.FinishRecord();
            TheMatch.ExitArena();
        }
        else
        {
            if (TheMatch.HasMatchEnded)
            {
                if (WwwLeagueInterface.PublicAccess.IsBusy == false)
                {
                    TheMatch.TheRecorder.FinishRecord();
                    TheMatch.ExitArena();
                }
            }
        }
    }
    public void ShowLeaderboard(bool showIt)
    {
        PanelGeneral.SetActive(!showIt);
        PanelLeaderboard.SetActive(showIt);

    }
}
