using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelRobotRow : MonoBehaviour
{

    public RobotConstructor.RobotTemplate TheRobot;
    public Text TextRobotName;
    public IDECanvasManager TheCanvas;
    public Text TextQuickStat;
    public Image ImageOfRobot;
    Texture2D texture2D;
    // Use this for initialization
    void Start()
    {
         texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
             
    }
    bool ImageSet = false;
    // Update is called once per frame
    void Update()
    {
        if (TheRobot != null)
        {
            TextQuickStat.text = TheRobot.TotalWins.ToString("f0") + " Wins / " + TheRobot.TotalMatches.ToString("f0") + " Matches " + TheRobot.TotalKills.ToString("f0") + " Kills / " + TheRobot.TotalDeaths.ToString("f0") + " Deaths";
            if (TheRobot.RobotName != null && TheRobot.RobotName.Length >= 1)
            {
                TextRobotName.text = TheRobot.RobotName;
           
            }
            else
            {
                TextRobotName.text = "UNNAMED ROBOT";// TheRobot.RobotID;
            }
            if (ImageSet == false)
            {
                if (TheRobot.ScreenShot != null && TheRobot.ScreenShot.Length > 0)
                {
                    texture2D.LoadImage(TheRobot.ScreenShot);
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                   
                    ImageOfRobot.sprite = sprite;
                    ImageSet = true;
                }
            }
        }
    }

    public void ButtonClick()
    {
        if (TheCanvas != null)
        {
            TheCanvas.SetSelection(TheRobot.RobotID);
        }
    }

}
