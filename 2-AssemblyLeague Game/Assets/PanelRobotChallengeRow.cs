using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PanelRobotChallengeRow : MonoBehaviour
{
    public IDECanvasManager TheCanvas;
    public RobotConstructor.RobotTemplate TheRobot;
    public xRobot TheXRobot;
    public Text TextRobotName;
    public Text TextRobotOwner;
    public Text TextRoboRank;
    public Text TextRobotPoints;
    public Text TextRobotKDHeading;
    public Text TextRobotKDValue;
    public Color DefaultColor;
    public Image ImageRobot;
    Texture2D texture2D;
    // Use this for initialization
    void Start()
    {
        DefaultColor = TextRobotOwner.color;
        texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonChallengeClick()
    {
        PanelRobotRow row = TheCanvas.GetRowForRobot(TheCanvas.SelectedRobotID);
        if (row != null)
        {
            if (TheRobot != null)
            {
                Match.RobotsInMatch = new List<RobotConstructor.RobotTemplate>();
                Match.PlayAsRobotID = row.TheRobot.LeagueID;
                Match.RobotsInMatch.Add(row.TheRobot);
                Match.RobotsInMatch.Add(TheRobot);
                Match.IsLeagueMatch = false;
                IDECanvasManager.PublicAccess.ChaneToMatchScene(null,0);
            }
            else
            {
                if (TheXRobot != null)
                {
                   

                    List<int> ids = new List<int>();
                   
                    ids.Add(row.TheRobot.LeagueID);
                    ids.Add(TheXRobot.ID);
                    TheCanvas.CreateLeagueMatchRequest(ids, false, row.TheRobot.LeagueID, 0);
                    

                }
            }
        }
    }

    public void Setup(RobotConstructor.RobotTemplate aRobot)
    {
        TheXRobot = null;
        TheRobot = aRobot;
        TextRobotName.text = TheRobot.RobotName;
        TextRobotOwner.gameObject.SetActive(false);
        TextRoboRank.gameObject.SetActive(false);
        TextRobotKDValue.gameObject.SetActive(false);
        TextRobotKDHeading.gameObject.SetActive(false);
        TextRobotPoints.gameObject.SetActive(false);
        if (texture2D == null)
        {

            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
        }
        if (aRobot.ScreenShot != null && aRobot.ScreenShot.Length > 0)
        {
            texture2D.LoadImage(aRobot.ScreenShot);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            ImageRobot.sprite = sprite;
            ImageRobot.gameObject.SetActive(true);

        }
        else
        {
            ImageRobot.gameObject.SetActive(false);

        }
        gameObject.SetActive(true);
    }
    public void Setup(xRobot aRobot)
    {

        if (texture2D == null)
        {

            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
        }

        TheRobot = null;
        TheXRobot = aRobot;
        TextRobotName.text = aRobot.RobotName;
        TextRobotOwner.text = aRobot.OwnerName;
        TextRoboRank.text = aRobot.WorldRank + "";
        TextRobotKDValue.text = aRobot.TotalKills + "/" + aRobot.TotalDeaths;
        TextRobotPoints.text = aRobot.TotalPoints + "";
        if (aRobot.ScreenShot != null && aRobot.ScreenShot.Length > 0)
        {
            texture2D.LoadImage(aRobot.ScreenShot);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            ImageRobot.sprite = sprite;
            ImageRobot.gameObject.SetActive(true);

        }
        else
        {
            ImageRobot.gameObject.SetActive(false);

        }
        TextRobotOwner.gameObject.SetActive(true);
        TextRoboRank.gameObject.SetActive(true);
        TextRobotKDValue.gameObject.SetActive(true);
        TextRobotKDHeading.gameObject.SetActive(true);
        TextRobotPoints.gameObject.SetActive(true);


        if (aRobot.OwnerID == WwwLeagueInterface.LoggedInUserID)
        {
            Color col = DefaultColor;
        }
        else
        {
            Color col = DefaultColor;
            col.a = 0.5f;

        }
        gameObject.SetActive(true);
    }
    public void Clear()
    {
        TheXRobot = null;
        TheRobot = null;
        TextRobotName.text = "";
        gameObject.SetActive(false);
    }
}
