using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Match : MonoBehaviour
{
    public bool DoSlowSpawn = false;
    bool isWaitingForSlowSpawnToFinish = false;
    public LeagueRecorder TheRecorder;
    public CodePanel TheCodePanel;
    public PanelLeaderBoard TheLeaderBoardPanel;
    public WwwLeagueInterface www;
    public static int PlayAsRobotID = 4;
    public static bool IsLeagueMatch = false;
    public static long LeagueMatchID = 0;
    public static xArena CurrentArena;
    public static float PrizeMoneyLeague = 0;
    public System.DateTime MatchStartTime;
    public System.DateTime MatchIntroductionEnd;
    public System.DateTime MatchEndTime;

    public bool HasMatchStarted;

    public bool HasMatchEnded;
    public bool IsInIntroductionMode;
    public List<string> RobotsInMat2ch;
    public static List<RobotConstructor.RobotTemplate> RobotsInMatch;
    public List<GameObject> AllSpawnedRobots;
    public RobotConstructor TheRobotConstructor;

    public Arena TheArena;

    public MatchReferee TheReferee;
    public static EnumExitType ExitType;

    public static Match PublicAccess;
    public RobotOwnerLookup BackupLookup;

    float MatchSecondsLength = 180;
    float IntroductionExtraLength = 5;
    public float IntroductionSecondsLength = 5;
    public bool IsWaitingOnUpload = false;


    bool hasAnnounced10SecMark = false;
    float framesToInit = 10;

    public enum EnumExitType
    {
        IDE

    }
    public List<RobotMeta> GetAllRobotMetas()
    {
        List<RobotMeta> ret = new List<RobotMeta>();
        for (int c = 0; c < Match.PublicAccess.AllSpawnedRobots.Count; c++)
        {
            RobotMeta aPart = Match.PublicAccess.AllSpawnedRobots[c].GetComponent<RobotMeta>();

            ret.Add(aPart);
        }
        return ret;
    }
    public Transform MyCamera;
    public GameObject PrefabHoverCanvas;
    void Start()
    {
        HasMatchStarted = false;
        HasMatchEnded = false;
        AudioController.musicParent = MyCamera.transform;
        AudioController.PlayMusicPlaylist("Battle");

        VideoID = 0;
        PublicAccess = this;
        CodePanel.PublicAccess = TheCodePanel;
        PanelLeaderBoard.PublicAccess = TheLeaderBoardPanel;
        www.On_GetXData_Received_SubmitRobotMatch += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_SubmitRobotMatch);
        www.On_GetXData_Received_SubmitVideo += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_SubmitVideo);
        IsForcedGameOver = false;
        IsInIntroductionMode = true;
        HasBeenInit = false;
    }

    bool HasBeenInit = false;

    public static long VideoID = 0;
    public bool IsForcedGameOver = false;
    bool IsGameOver = false;
    System.DateTime timeToStopShowingWinners;
    public void KillBot(RobotPart aBot)
    {
        int index = -1;

        for (int c = 0; c < AllSpawnedRobots.Count; c++)
        {
            if (AllSpawnedRobots[c].gameObject.transform == aBot.gameObject.transform)
            {
                index = c;
                break;
            }
        }
        if (index >= 0)
        {
            TurnRobotOnOff(AllSpawnedRobots[index], false);
            Destroy(AllSpawnedRobots[index], 5);
            AllSpawnedRobots.RemoveAt(index);


        }
    }
    private void ScheduleNewMatch()
    {
        hasAnnounced10SecMark = false; //if (ForceStartMatch)
        {
            MatchStartTime = System.DateTime.Now.AddSeconds(IntroductionExtraLength);
            MatchEndTime = MatchStartTime.AddSeconds(MatchSecondsLength + IntroductionSecondsLength);
            HasMatchStarted = false;
            HasMatchEnded = false;
        }


    }

    void Update()
    {
        if (isWaitingForSlowSpawnToFinish == false)
        {
            if (HasBeenInit == false)
            {
                if (framesToInit <= 0)
                {
                    ScheduleNewMatch();
                    HasBeenInit = true;
                }
                else
                {
                    framesToInit -= Time.deltaTime;
                }
            }
            else
            {
                if (NeedToUploadScore)
                {
                    SubmitScoresToSeverServ();
                }
                else
                {
                    if (LeagueMatchID > 0)
                    {
                        VideoID = LeagueMatchID;
                    }
                    if (MustDoUploadMatch)
                    {
                        if (TheRecorder.IsFinished && TheRecorder.IsConfiguredEnabled)
                        {
                            MustDoUploadMatch = false;
                            byte[] video = TheRecorder.GetVideo();
                            IsWaitingOnUpload = true;
                            www.UploadMatchVideo(VideoID, video);

                        }
                    }
                    else
                    {

                        if (IsWaitingOnUpload == false)
                        {
                            if (IsLeagueMatch == false || (IsLeagueMatch == true && LeagueMatchID > 0))
                            {
                                if (IsForcedGameOver)
                                {
                                    print("FORCE OVER match");
                                    IsForcedGameOver = false;
                                    MatchEndTime = System.DateTime.Now.AddSeconds(-1);
                                }
                                {

                                    if (System.DateTime.Now > MatchIntroductionEnd && IsInIntroductionMode)
                                    {
                                        IsInIntroductionMode = false;
                                        TurnAllRobotsOn();
                                        if (HasMatchStarted)
                                        {
                                            MatchCommentator.PublicAccess.DoAnnouncement(MatchCommentator.EnumAnouncement.LetTheBattleBegin);
                                        }
                                    }
                                }

                                if (HasMatchStarted && System.DateTime.Now.AddSeconds(10) >= MatchEndTime && HasMatchEnded == false)
                                {
                                    if (hasAnnounced10SecMark == false)
                                    {
                                        if (MatchCommentator.PublicAccess != null)
                                        {
                                            MatchCommentator.PublicAccess.DoAnnouncement(MatchCommentator.EnumAnouncement.TenSceondsRemain);
                                        }
                                        hasAnnounced10SecMark = true;
                                    }
                                }

                                if (System.DateTime.Now <= MatchStartTime)
                                {

                                    if (HasMatchStarted == false)
                                    {
                                        MatchCanvasManager.PublicAccess.ShowLeaderboard(false);
                                        IsGameOver = false;
                                        print("start match");
                                        HasMatchStarted = true;
                                        HasMatchEnded = false;
                                        ClearScorecard();
                                        SpawnAllRobotsAtLocation();
                                        MatchIntroductionEnd = System.DateTime.Now.AddSeconds(IntroductionSecondsLength);
                                        IsInIntroductionMode = true;
                                        if (IsLeagueMatch)
                                        {
                                            TheRecorder.BegingRecord();
                                        }

                                    }
                                }

                                if (System.DateTime.Now >= MatchEndTime || AllSpawnedRobots.Count <= 1)
                                {
                                    if (HasMatchStarted)
                                    {
                                        IsGameOver = true;
                                        HasMatchStarted = false;
                                        {
                                            print("end match");

                                            if (MatchCommentator.PublicAccess != null)
                                            {
                                                MatchCommentator.PublicAccess.DoAnnouncement(MatchCommentator.EnumAnouncement.BattleOver);
                                            }
                                            HasMatchStarted = false;
                                            HasMatchEnded = true;
                                            TurnAllRobotsOff();
                                            if (IsLeagueMatch)
                                            {
                                                NeedToUploadScore = true;
                                            }
                                            timeToStopShowingWinners = System.DateTime.Now.AddSeconds(30);
                                            MatchCanvasManager.PublicAccess.ShowLeaderboard(true);


                                        }
                                    }
                                }
                                if (NeedToUploadScore == false)
                                {
                                    if (IsGameOver)
                                    {
                                        //show end of game stuff
                                        if (timeToStopShowingWinners <= System.DateTime.Now)
                                        {
                                            print("SCHEDULE match");
                                            HasMatchStarted = false;
                                            HasMatchEnded = false;
                                            IsGameOver = false; ;
                                            ScheduleNewMatch();
                                        }
                                    }
                                }

                                TheLeaderBoardPanel.UpdateLeaderboard(AllSpawnedRobots);

                            }
                        }
                    }


                }
            }
        }
    }
    private void ClearScorecard()
    {
        print("Clear Scorecard");
        try
        {
            TheLeaderBoardPanel.ClearLeaderboard();
        }
        catch
        {

        }
    }
    public bool NeedToUploadScore = false;
    private void SubmitScoresToSeverServ()
    {
        if (www.IsBusy == false)
        {
            if (IsLeagueMatch)
            {
                List<xRobotMatchResult> results = new List<xRobotMatchResult>();
                for (int c = 0; c < AllSpawnedRobots.Count; c++)
                {
                    RobotMeta aMeta = AllSpawnedRobots[c].GetComponent<RobotMeta>();
                    xRobotMatchResult aResult = new xRobotMatchResult();
                    aResult.RobotID = aMeta.RuntimeRobotLeagueID;
                    aResult.BulletsFired = aMeta.ShotsFired;
                    aResult.BulletsHit = aMeta.ShotsHit;
                    aResult.Health = aMeta.Health;
                    aResult.IsDead = aMeta.IsDead;
                    aResult.MinutesSurvived = aMeta.MinutesSurvived;
                    aResult.RamDamage = aMeta.DamageGiven;
                    aResult.DamageReceived = aMeta.DamageReceived;
                    aResult.TotalKills = aMeta.Kills;
                    aResult.TotalPoints = aMeta.RuntimeScore;


                    results.Add(aResult);
                    Destroy(AllSpawnedRobots[c]);
                }
                www.SubmitRobotMatch(LeagueMatchID, results);
                if (IsLeagueMatch)
                {
                    TheRecorder.FinishRecord();
                }
                NeedToUploadScore = false;
            }
        }
    }
    public bool MustDoUploadMatch = false;
    void www_On_GetXData_Received_SubmitRobotMatch(XData aData)
    {
        if (aData.SubmitRobotMatch.Success)
        {
            if (TheRecorder.IsConfiguredEnabled)
            {
                //if (TheRecorder.IsFinished)
                //{
                //    MustDoUploadMatch = true;
                //  //  byte[] video = TheRecorder.GetVideo();
                //  //  www.UploadMatchVideo(LeagueMatchID, video);
                //}
                //else
                //{
                //    MustDoUploadMatch = true;
                //}
            }
            else
            {
                NeedToUploadScore = false;

                LeagueMatchID = 0;

            }

        }
    }
    void www_On_GetXData_Received_SubmitVideo(XData aData)
    {
        if (aData.SubmitVideo.Success)
        {
            LeagueMatchID = 0;

            print("Done Uploading Video");

            IsWaitingOnUpload = false;
        }
    }

    private void SpawnAllRobotsAtLocation()
    {
        if (DoSlowSpawn)
        {
            isWaitingForSlowSpawnToFinish = true;
        }
        ClearAllRobots();
        print("Spawning");
        if (AllSpawnedRobots == null) AllSpawnedRobots = new List<GameObject>();
        if (RobotsInMatch == null)
        {

            RobotsInMatch = new List<RobotConstructor.RobotTemplate>();

            RobotsInMatch = BackupLookup.GetAllTemplatesForCurrentPlayer();
            if (RobotsInMatch.Count > 7)
            {
                RobotsInMatch = RobotsInMatch.Take(7).ToList();
            }

        }
        List<RobotMeta> allMetas = new List<RobotMeta>();
        for (int c = 0; c < RobotsInMatch.Count; c++)
        {
            GameObject aBot = SpawnARobot(RobotsInMatch[c]);
            if (aBot != null)
            {
                aBot.transform.position = TheArena.FindASpawnPosition(c, RobotsInMatch.Count);
                RobotMeta aRobotMeta = aBot.GetComponent<RobotMeta>();
                aRobotMeta.RuntimeRank = 0;
                aRobotMeta.RuntimeRobotID = c;
                aRobotMeta.RuntimeRobotOwnderID = RobotsInMatch[c].LeagueOwnerID;
                aRobotMeta.RuntimeRobotLeagueID = RobotsInMatch[c].LeagueID;
                aRobotMeta.RuntimeRobotOwnerName = RobotsInMatch[c].LeagueOwnerName;
                aRobotMeta.RuntimeRank = 0;
                aRobotMeta.CurrentMatch = this;
                aRobotMeta.Health = 250;
                RobotPart aPart = aBot.GetComponent<RobotPart>();
                aPart.aPartMeta = aRobotMeta;
                if (aRobotMeta.RuntimeRobotLeagueID == PlayAsRobotID || aRobotMeta.RuntimeRobotID == PlayAsRobotID)
                {
                    GameObjectFollower.PublicAccess.GameObjectToFollow = aBot;
                }
                AllSpawnedRobots.Add(aBot);
                allMetas.Add(aRobotMeta);
            }

        }


        TheLeaderBoardPanel.SetLeaderboard(AllSpawnedRobots);
        CameraSwitcherPanel.PublicAccess.SetUpDropdownList(allMetas);
    }

    public GameObject SpawnHover2(RobotMeta aMeta)
    {
        GameObject theHover = Instantiate(PrefabHoverCanvas, aMeta.gameObject.transform.position + Vector3.up * 7, aMeta.gameObject.transform.rotation, aMeta.gameObject.transform);
        CanvasHover aHover = theHover.GetComponent<CanvasHover>();
        aHover.TheMeta = aMeta;
        return theHover;
    }
    private GameObject SpawnARobot(string filename)
    {
        // GameObject ret = null;
        GameObject ret = TheRobotConstructor.LoadRobot(TheRobotConstructor.BuildPlatformTarget);
        ComponentType aType = ret.GetComponent<ComponentType>();

        aType.TurnPartOnOff(false);
        for (int c = 0; c < TheRobotConstructor.BuildPlatformTarget.childCount; c++)
        {
            Transform child = (Transform)TheRobotConstructor.BuildPlatformTarget.GetChild(c);
            child.transform.parent = null;
        }

        return ret;


    }
    private GameObject SpawnARobot(RobotConstructor.RobotTemplate filename)
    {
        GameObject ret = TheRobotConstructor.LoadRobot(TheRobotConstructor.BuildPlatformTarget, filename);
        try
        {
            ComponentType aType = ret.GetComponent<ComponentType>();

            aType.TurnPartOnOff(false);
            for (int c = 0; c < TheRobotConstructor.BuildPlatformTarget.childCount; c++)
            {
                Transform child = (Transform)TheRobotConstructor.BuildPlatformTarget.GetChild(c);

                child.transform.parent = null;
            }
        }
        catch
        {
            //ret=null
        }

        return ret;


    }


    private void ClearAllRobots()
    {
        try
        {
            PanelLeaderBoard.PublicAccess.ClearLeaderboard();
        }

        catch
        {

        }
        print("Clearing");
        if (AllSpawnedRobots == null) AllSpawnedRobots = new List<GameObject>();

        for (int c = 0; c < AllSpawnedRobots.Count; c++)
        {
            Destroy(AllSpawnedRobots[c]);
        }
        AllSpawnedRobots.Clear();
    }

    private void TurnAllRobotsOn()
    {
        print("Turning All On");

        if (AllSpawnedRobots != null)
        {
            for (int c = 0; c < AllSpawnedRobots.Count; c++)
            {
                print("ROBOT " + AllSpawnedRobots[c].name);
                ComponentType aComponent = AllSpawnedRobots[c].GetComponent<ComponentType>();
                if (aComponent != null)
                {
                    aComponent.TurnPartOnOff(true);
                }
                else
                {
                    print("ROBOT " + AllSpawnedRobots[c].name + " NOT FOUND!");

                }
            }
        }
        print("Turning All On DONE");

    }

    private void TurnAllRobotsOff()
    {
        print("Turning All Off");
        if (AllSpawnedRobots != null)
        {
            for (int c = 0; c < AllSpawnedRobots.Count; c++)
            {
                ComponentType aComponent = AllSpawnedRobots[c].GetComponent<ComponentType>();
                if (aComponent != null)
                {
                    aComponent.TurnPartOnOff(false);
                }
            }
        }

    }
    public void TurnRobotOnOff(GameObject aRobot, bool OnOff)
    {
        print("Turning All Off");
        if (aRobot != null)
        {
            {
                ComponentType aComponent = aRobot.GetComponent<ComponentType>();
                if (aComponent != null)
                {
                    aComponent.TurnPartOnOff(OnOff);
                }
            }
        }

    }

    public void TurnRobotComputerOnOff(GameObject aRobot, bool OnOff)
    {
        print("Turning All Off");
        if (aRobot != null)
        {
            {
                Computer aComponent = aRobot.GetComponent<Computer>();
                if (aComponent != null)
                {
                    aComponent.IsEnabled = OnOff;
                }
            }
        }

    }
    public RobotMeta GetRobotOfRuntimeID(int id)
    {
        RobotMeta ret = null;
        for (int c = 0; c < AllSpawnedRobots.Count; c++)
        {
            RobotMeta aMeta = AllSpawnedRobots[c].GetComponent<RobotMeta>();
            if (aMeta.RuntimeRobotID == id)
            {
                ret = aMeta;
                break;
            }
        }
        return ret;
    }

    public void ExitArena()
    {
        if (ExitType == EnumExitType.IDE)
        {
            Application.LoadLevel("IDEScene");
        }

    }
}
