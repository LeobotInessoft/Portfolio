using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;
public class PanelRobotMatchPreview : MonoBehaviour
{
    public Vector3 StartPosition;
    public Image ImageScreenshot;
    public Text TextRobotName;
    public Text TextRobotOwnerName;

    public Text TextTotalWeight;
    public Text TextMaxSpeed;
    public Text TextSqrMeterSize;
    public Text TextLinesOfCode;
    public Text TextCodeProcessedPerSecond;

    public Slider SliderTotalWeight;
    public Slider SliderMaxSpeed;
    public Slider SliderSqrMeterSize;
    public Slider SliderLinesOfCode;
    public Slider SliderCodeProcessedPerSecond;


    public Text TextWins;
    public Text TextPlayed;
    public Text TextKills;
    public Text TextDeaths;
    public Text TextPoints;

    public Slider SliderWins;
    public Slider SliderPlayed;
    public Slider SliderKills;
    public Slider SliderDeaths;
    public Slider SliderPoints;


    // Use this for initialization
    Texture2D texture2D;
    void Start()
    {
        if (hasSetStart == false)
        {
            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
            hasSetStart = true;
            StartPosition = gameObject.transform.position;
        }
    }
    bool hasSetStart = false;
    // Update is called once per frame
    void Update()
    {
        if (hasSetStart == false)
        {
            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
            hasSetStart = true;
            StartPosition = gameObject.transform.position;
        }

        float dist = Vector3.Distance(gameObject.transform.position, StartPosition);
        if (dist <= 1)
        {
            gameObject.transform.position = StartPosition;
        }
        else
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, StartPosition, 0.07f);
        }



    
    }
    RobotConstructor.RobotTemplate theRobot;
    public void SetRobot(RobotConstructor.RobotTemplate anXRobot, Vector3 fromPosition)
    {
        theRobot = anXRobot;
        if (hasSetStart == false)
        {
            texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, false);
            hasSetStart = true;
            StartPosition = gameObject.transform.position;
        }

        if (anXRobot != null)
        {
            TextRobotName.text = anXRobot.RobotName;
            TextRobotOwnerName.text = anXRobot.LeagueOwnerName;

            TextTotalWeight.text = anXRobot.CalculateTotalWeight.ToString("f0") + "";
            TextMaxSpeed.text = anXRobot.CalculateMaxSpeed.ToString("f0") + "";
            TextSqrMeterSize.text = anXRobot.CalculateSqrMeterSize.ToString("f0") + "";
            TextLinesOfCode.text = anXRobot.CalculateLinesOfCode.ToString("f0") + "";
            TextCodeProcessedPerSecond.text = anXRobot.CalculateCodeProcessedPerSecond.ToString("f0") + "";

            TextWins.text = anXRobot.TotalWins + "";
            TextPlayed.text = anXRobot.TotalMatches + "";
            TextKills.text = anXRobot.TotalKills + "";
            TextDeaths.text = anXRobot.TotalDeaths + "";
            TextPoints.text = anXRobot.CalculateCodeTotalLeagueScore.ToString("f0") + "";


            if (anXRobot.ScreenShot != null && anXRobot.ScreenShot.Length > 0)
            {
                texture2D.LoadImage(anXRobot.ScreenShot);
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                ImageScreenshot.sprite = sprite;
                ImageScreenshot.gameObject.SetActive(true);

            }
           
        }
        gameObject.transform.position = fromPosition;
        gameObject.SetActive(true);
    }
    public void SetRobotSliders(List<RobotConstructor.RobotTemplate> allRobots)
    {


        if (theRobot != null)
        {
            float minWeight = allRobots.OrderBy(x => x.CalculateTotalWeight).FirstOrDefault().CalculateTotalWeight;
            float maxWeight = allRobots.OrderByDescending(x => x.CalculateTotalWeight).FirstOrDefault().CalculateTotalWeight;
            float myWeight = theRobot.CalculateTotalWeight;

            float minSpeed = allRobots.OrderBy(x => x.CalculateMaxSpeed).FirstOrDefault().CalculateMaxSpeed;
            float maxSpeed = allRobots.OrderByDescending(x => x.CalculateMaxSpeed).FirstOrDefault().CalculateMaxSpeed;
            float mySpeed = theRobot.CalculateMaxSpeed;

            float minSize = allRobots.OrderBy(x => x.CalculateSqrMeterSize).FirstOrDefault().CalculateSqrMeterSize;
            float maxSize = allRobots.OrderByDescending(x => x.CalculateSqrMeterSize).FirstOrDefault().CalculateSqrMeterSize;
            float mySize = theRobot.CalculateSqrMeterSize;

            float minCode = allRobots.OrderBy(x => x.CalculateLinesOfCode).FirstOrDefault().CalculateLinesOfCode;
            float maxCode = allRobots.OrderByDescending(x => x.CalculateLinesOfCode).FirstOrDefault().CalculateLinesOfCode;
            float myCode = theRobot.CalculateLinesOfCode;

            float minCpu = allRobots.OrderBy(x => x.CalculateCodeProcessedPerSecond).FirstOrDefault().CalculateCodeProcessedPerSecond;
            float maxCpu = allRobots.OrderByDescending(x => x.CalculateCodeProcessedPerSecond).FirstOrDefault().CalculateCodeProcessedPerSecond;
            float myCpu = theRobot.CalculateCodeProcessedPerSecond;



            float minWins = allRobots.OrderBy(x => x.TotalWins).FirstOrDefault().TotalWins;
            float maxWins = allRobots.OrderByDescending(x => x.TotalWins).FirstOrDefault().TotalWins;
            float myWins = theRobot.TotalWins;

            float minMatches = allRobots.OrderBy(x => x.TotalMatches).FirstOrDefault().TotalMatches;
            float maxMatches = allRobots.OrderByDescending(x => x.TotalMatches).FirstOrDefault().TotalMatches;
            float myMatches = theRobot.TotalMatches;

            float minKills = allRobots.OrderBy(x => x.TotalKills).FirstOrDefault().TotalKills;
            float maxKills = allRobots.OrderByDescending(x => x.TotalKills).FirstOrDefault().TotalKills;
            float myKills = theRobot.TotalKills;

            float minDeaths = allRobots.OrderBy(x => x.TotalDeaths).FirstOrDefault().TotalDeaths;
            float maxDeaths = allRobots.OrderByDescending(x => x.TotalDeaths).FirstOrDefault().TotalDeaths;
            float myDeaths = theRobot.TotalDeaths;

            float minPoints = allRobots.OrderBy(x => x.CalculateCodeTotalLeagueScore).FirstOrDefault().CalculateCodeTotalLeagueScore;
            float maxPoints = allRobots.OrderByDescending(x => x.CalculateCodeTotalLeagueScore).FirstOrDefault().CalculateCodeTotalLeagueScore;
            float myPoints = theRobot.CalculateCodeTotalLeagueScore;

            {
                float min = minWeight;
                float max = maxWeight;
                float myVal = myWeight;
                SetSlider(SliderTotalWeight, min, max, myVal);
            }
            {
                float min = minSpeed;
                float max = maxSpeed;
                float myVal = mySpeed;
                SetSlider(SliderMaxSpeed, min, max, myVal);
            }
            {
                float min = minSize;
                float max = maxSize;
                float myVal = mySize;
                SetSlider(SliderSqrMeterSize, min, max, myVal);
            }
            {
                float min = minCode;
                float max = maxCode;
                float myVal = myCode;
                SetSlider(SliderLinesOfCode, min, max, myVal);
            }
            {
                float min = minCpu;
                float max = maxCpu;
                float myVal = myCpu;
                SetSlider(SliderCodeProcessedPerSecond, min, max, myVal);
            }

            {
                float min = minWins;
                float max = maxWins;
                float myVal = myWins;
                SetSlider(SliderWins, min, max, myVal);
            }
            {
                float min = minMatches;
                float max = maxMatches;
                float myVal = myMatches;
                SetSlider(SliderPlayed, min, max, myVal);
            }
            {
                float min = minKills;
                float max = maxKills;
                float myVal = myKills;
                SetSlider(SliderKills, min, max, myVal);
            }
            {
                float min = minDeaths;
                float max = maxDeaths;
                float myVal = myDeaths;
                SetSlider(SliderDeaths, min, max, myVal);
            }
            {
                float min = minPoints;
                float max = maxPoints;
                float myVal = myPoints;
                SetSlider(SliderPoints, min, max, myVal);
            }


        }


    }
    private void SetSlider(Slider aSlider, float MinValue, float MaxValue, float value)
    {
        aSlider.minValue = MinValue;
        aSlider.maxValue = MaxValue;
        aSlider.value = value;
    }
}
