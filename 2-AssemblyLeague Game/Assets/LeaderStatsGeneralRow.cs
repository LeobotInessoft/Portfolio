using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderStatsGeneralRow : MonoBehaviour
{
    EnumScoreType curScoreType;
 public   xLeagueLeaderboard cureLeader;
    public Button ButtonPlay;
    public Text TextBig;
    public Text TexSmall;
    public Text Score;
    public Image ImageScreenshot;
    Texture2D texture2D;
    public enum EnumScoreType
    {
        TopAssemblers,
        TopRobots,
        TopRobotMostKills,
        TopRobotKillsToDeathRatio,
        TopRobotCodeToScoreRatio,
        TopRobotWinPercentRatio,
        TopRobotMostAccurateHitToMissRatio,
        TopRobotMultiMatchWinRatio,

    }
    // Use this for initialization
    void Start()
    {
        if (texture2D == null)
        {

            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
        }
  
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
         List<int> ids = new List<int>();
      
         ids.Add(cureLeader.RobotID);
         int extraId = PanelLeagueLeaderboards.PublicAccess.GetPersonalRobotIDInStat(curScoreType);
         IDECanvasManager.PublicAccess.CreateLeagueMatchRequest(ids, true, cureLeader.RobotID, extraId);
    }
    public void SetInfo(xLeagueLeaderboard aLeaderBoard, EnumScoreType scoreType)
    {
        if (texture2D == null)
        {

            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
        }
        if (aLeaderBoard.ScreenshotData != null && aLeaderBoard.ScreenshotData.Length > 0)
        {
            texture2D.LoadImage(aLeaderBoard.ScreenshotData);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            ImageScreenshot.sprite = sprite;
            ImageScreenshot.gameObject.SetActive(true);

        }
        else
        {
            ImageScreenshot.gameObject.SetActive(false);

        }
        cureLeader = aLeaderBoard;
        curScoreType = scoreType;
        if (aLeaderBoard != null)
        {
            TextBig.text = aLeaderBoard.RobotName;
            TexSmall.text = aLeaderBoard.PlayerName;

            switch (scoreType)
            {
                case EnumScoreType.TopAssemblers:
                    {
                        Score.text = aLeaderBoard.Rank.ToString("f0");
                        break;
                    }
                case EnumScoreType.TopRobotCodeToScoreRatio:
                    {
                        Score.text = aLeaderBoard.RatioCodeToScore.ToString("f2");
                        break;
                    }
                case EnumScoreType.TopRobotKillsToDeathRatio:
                    {
                        Score.text = aLeaderBoard.RatioKillToDeath.ToString("f2");
                        break;
                    }
                case EnumScoreType.TopRobotMostAccurateHitToMissRatio:
                    {
                        Score.text = aLeaderBoard.RatioHitsToMiss.ToString("f2");
                        break;
                    }
                case EnumScoreType.TopRobotMostKills:
                    {
                        Score.text = aLeaderBoard.TotalKills.ToString("f0");
                        break;
                    }
                case EnumScoreType.TopRobotMultiMatchWinRatio:
                    {
                        Score.text = aLeaderBoard.RatioMultiRobotMatchWinsToLoss.ToString("f2");
                        break;
                    }
                case EnumScoreType.TopRobots:
                    {
                        Score.text = aLeaderBoard.Rank.ToString("f0");
                        break;
                    }
                case EnumScoreType.TopRobotWinPercentRatio:
                    {
                        Score.text = aLeaderBoard.RatioWinToLoss.ToString("f2");
                        break;
                    }

                default:
                    {
                        Score.text = "-";
                        break;
                    }

            }
        }
    }
}
