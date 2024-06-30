using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelLeague : MonoBehaviour
{
    //  public IDECanvasManager TheCanvas;
    public IDECanvasManager TheIDE;
    public List<PanelRobotChallengeRow> ChallengeRows;
    public List<xRobot> allTemplatesToShowLEAGUE;
    public bool IsOnline = false;
    public int currentPage = 0;
    public GameObject PanelWithOptions;
    public GameObject PanelLoading;

    // Use this for initialization
    void Start()
    {
        if (IsOnline)
        {
            PanelLoading.gameObject.SetActive(true);
            PanelWithOptions.SetActive(false);
            RefreshXRobot();
        }
        else
        {
            Refresh();
        }
        nextRefresh = System.DateTime.Now;

    }
    public void SetLoadingScreenOn()
    {
        PanelLoading.gameObject.SetActive(true);

    }
    System.DateTime nextRefresh;
    // Update is called once per frame
    void Update()
    {

        if (IsOnline == false && nextRefresh <= System.DateTime.Now)
        {
            if (IsOnline)
            {
                RefreshXRobot();
            }
            else
            {
                Refresh();
            }
            nextRefresh = System.DateTime.Now.AddSeconds(5);
        }

    }
    public void Refresh()
    {
        List<RobotConstructor.RobotTemplate> allTemplatesToShow = TheIDE.TheLookup.GetAllTemplatesForCurrentPlayer();
        int rowCount = 0;
        if (currentPage * ChallengeRows.Count >= allTemplatesToShow.Count)
        {
            currentPage = 0;
        }
        if (currentPage < 0)
        {
            currentPage = allTemplatesToShow.Count - ChallengeRows.Count;
        }
        if (currentPage < 0)
        {
            currentPage = 0;
        }
        for (int x = 0; x < ChallengeRows.Count; x++)
        {

            if (allTemplatesToShow.Count > currentPage * ChallengeRows.Count + x)
            {
                ChallengeRows[rowCount].Setup(allTemplatesToShow[currentPage * ChallengeRows.Count + x]);
            }
            else
            {
                ChallengeRows[rowCount].Clear();

            }

            rowCount++;

        }

    }
    public void RefreshXRobot()
    {
        PanelLoading.gameObject.SetActive(false);

        int rowCount = 0;
        if (allTemplatesToShowLEAGUE == null) allTemplatesToShowLEAGUE = new List<xRobot>();
        if (currentPage * ChallengeRows.Count >= allTemplatesToShowLEAGUE.Count)
        {
            currentPage = 0;
        }
        if (currentPage < 0)
        {
            currentPage = allTemplatesToShowLEAGUE.Count / ChallengeRows.Count - 1;
        }
        if (currentPage < 0)
        {
            currentPage = 0;
        }
        if (currentPage * ChallengeRows.Count >= allTemplatesToShowLEAGUE.Count)
        {
            currentPage = 0;
        }
        if (allTemplatesToShowLEAGUE.Count > 0)
        {
            PanelWithOptions.SetActive(true);
        }
        else
        {
            PanelWithOptions.SetActive(false);

        }
        for (int x = 0; x < ChallengeRows.Count; x++)
        {

            if (allTemplatesToShowLEAGUE.Count > currentPage * ChallengeRows.Count + x)
            {

                ChallengeRows[rowCount].Setup(allTemplatesToShowLEAGUE[currentPage * ChallengeRows.Count + x]);
            }
            else
            {
                ChallengeRows[rowCount].Clear();

            }

            rowCount++;

        }

    }
    public void ButtonNextPage()
    {
        currentPage++;
        if (IsOnline)
        {
            RefreshXRobot();
        }
        else
        {
            Refresh();
        }
    }
    public void ButtonPreviousPage()
    {
        currentPage--;
        if (currentPage < 0)
        {

        }
        if (IsOnline)
        {
            RefreshXRobot();
        }
        else
        {
            Refresh();
        }

    }
    public void ButtonChallengeAllClick()
    {
        PanelRobotRow row = TheIDE.GetRowForRobot(TheIDE.SelectedRobotID);
        if (row != null)
        {
            if (IsOnline == false)
            {
                IDECanvasManager.DefaultViewLeaderboards = false;
                // ButtonSave();
                Match.RobotsInMatch = new List<RobotConstructor.RobotTemplate>();
                Match.RobotsInMatch.Add(row.TheRobot);
                for (int c = 0; c < ChallengeRows.Count; c++)
                {
                    if (ChallengeRows[c].TheRobot != null)
                    {
                        if (ChallengeRows[c].TheRobot.RobotID != row.TheRobot.RobotID)
                        {
                            Match.RobotsInMatch.Add(ChallengeRows[c].TheRobot);
                        }
                    }
                }
                Match.IsLeagueMatch = false;

                TheIDE.ChaneToMatchScene(null, 0);
            }
            else
            {
                List<int> ids = new List<int>();
                ids.Add(row.TheRobot.LeagueID);
                for (int c = 0; c < ChallengeRows.Count; c++)
                {
                    if (ChallengeRows[c].TheXRobot != null)
                    {
                        if (ChallengeRows[c].TheXRobot.ID != row.TheRobot.LeagueID)
                        {
                            ids.Add(ChallengeRows[c].TheXRobot.ID);
                        }
                    }
                }
                TheIDE.CreateLeagueMatchRequest(ids, false, row.TheRobot.LeagueID, 0);

            }
        }
    }
}
