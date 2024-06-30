using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
namespace RobotServ
{
    public class ClassRobotDB
    {
        
        static int ServerGameVersion = 2;
        public RegisterResult RegisterUser(DataClassesInterfaceDataContext cc, RegisterRequest req, string steamID)
        {
            RegisterResult res = new RegisterResult();
            res.Success = false;
            res.ResultText = "";
            Player aPlayer = null;
            bool AlreadExists = false;
            if (steamID == null) steamID = "";
              if (steamID!= null && steamID.Length > 1)
            {
                aPlayer = cc.Players.FirstOrDefault(x => x.SteamID == steamID);

            }
            else
            {
                aPlayer = cc.Players.FirstOrDefault(x => x.Email.Trim().ToLower() == req.Email.Trim().ToLower());
            }
            
            if (aPlayer == null)
            {
                
            }
            else
            {
                res.ResultText = "Email is already registered.";
            }



            if (aPlayer == null)
            {
                aPlayer = new Player();
                cc.Players.InsertOnSubmit(aPlayer);

                aPlayer.DisplayName = req.Displayname;
                aPlayer.Email = req.Email;
                aPlayer.Password = req.Passowrd;
                aPlayer.SteamID = steamID;
                aPlayer.MoneyWon = 0;
                aPlayer.Money = 10000;
                aPlayer.ResetCode = "";
                aPlayer.ImageExt = "";
                aPlayer.IsPlayer = true;
                cc.SubmitChanges();
                res.Success = true;
                res.ResultText = "Account Registered Successfully";
            }
            return res;
        }
        public LoginResult LoginUser(DataClassesInterfaceDataContext cc, LoginRequest req, string steamID)
        {
            LoginResult res = new LoginResult();
            res.Success = false;
            res.ResultText = "";
            if (steamID == null) steamID = "";
            if (ServerGameVersion == req.GameVersion)
            {
                Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

                if (aPlayer != null)
                {

                }
                else
                {
                    res.ResultText = "Invalid username/password.";
                }


                if (aPlayer != null)
                {
                    res.PlayerID = aPlayer.ID;
                    res.Success = true;
                }
            }
            else
            {
                res.ResultText = "A new version of the game is available for you to download. Please download the latest version to login.";
            }
            return res;
        }
        public UpdateUserResult UpdateUser(DataClassesInterfaceDataContext cc, UpdateUserRequest req, HttpServerUtility server, string steamID)
        {
            UpdateUserResult res = new UpdateUserResult();
            res.Success = false;
            res.ResultText = "";
            if (steamID == null) steamID = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer == null)
            {
                aPlayer.DisplayName = req.Displayname;
                if (req.NewPassowrd.Length > 1)
                {
                    aPlayer.Password = req.NewPassowrd;
                }
                cc.SubmitChanges();
                res.Success = true;
                ClassTTS.GeneratePlayerName(cc, aPlayer, server);
            }
            return res;
        }

        public UploadRobotResult UploadRobot(DataClassesInterfaceDataContext cc, UploadRobotRequest req, string mappedPathToScnreeshotsImages, HttpServerUtility server, string steamID)
        {
            UploadRobotResult res = new UploadRobotResult();
            res.Success = false;
            res.ResultText = "";
            if (steamID == null) steamID = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer != null)
            {
                PlayerRobot myRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == req.TheTemplate.LeagueID && x.ID_PlayerOwner == aPlayer.ID);

                if (myRobot == null)
                {
                    myRobot = new PlayerRobot();
                    myRobot.ID_PlayerOwner = aPlayer.ID;
                    myRobot.GamesPlayed = 0;
                    myRobot.MoneyWon = 0;
                    myRobot.RobotName = "";
                    myRobot.TotalDeaths = 0;
                    myRobot.TotalKills = 0;
                    myRobot.TotalPoints = 0;
                    myRobot.LinesOfCode = 0;
                    cc.PlayerRobots.InsertOnSubmit(myRobot);
                }
                if (req.TheTemplate.RobotName != null)
                {
                    myRobot.RobotName = req.TheTemplate.RobotName;
                    myRobot.LinesOfCode = req.TheTemplate.MainFunctionCode.Count(x => x == '\n');
                }
                myRobot.RobotTemplateImage = RobotConstructor.RobotTemplate.ConvertToBytes(req.TheTemplate);
                cc.SubmitChanges();
                res.RobotID = myRobot.ID;

                if (req.ScreenShot != null && req.ScreenShot.Length > 0)
                {
                    SaveScreenshot(myRobot, mappedPathToScnreeshotsImages, req.ScreenShot);
                }
                ClassTTS.GenerateRobotName(cc, myRobot, server);
                res.Success = true;
            }
            return res;
        }
        private void SaveScreenshot(PlayerRobot aRobot, string mappedPathToScnreeshotsImages, byte[] pngData)
        {
            try
            {
                string locationToSave = mappedPathToScnreeshotsImages + "\\" + aRobot.ID + ".png";
                System.IO.File.WriteAllBytes(locationToSave, pngData);

            }
            catch
            {

            }

        }
        public RetrieveListOfChallengeableRobotsResult RetrieveListOfChallengableRobots(DataClassesInterfaceDataContext cc, RetrieveListOfChallengeableRobotsRequest req, string steamID)
        {
            RetrieveListOfChallengeableRobotsResult res = new RetrieveListOfChallengeableRobotsResult();
            res.Success = false;
            res.ResultText = "";
            res.RobotsInMatch = new List<xRobot>();
            if (steamID == null) steamID = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer != null)
            {


                //PlayerRobot myRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == robotID && x.ID_PlayerOwner == aPlayer.ID);

                //if (myRobot == null)
                //{
                //    myRobot = new PlayerRobot();
                //    myRobot.ID_PlayerOwner = aPlayer.ID;

                //    cc.PlayerRobots.InsertOnSubmit(myRobot);
                //}

                //myRobot.RobotTemplate = robotTemplate;
                //cc.SubmitChanges();

                List<PlayerRobot> betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();
                //betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();
                if (req.AsRobotID > 0)
                {
                    PlayerRobot myRob = betterRobots.FirstOrDefault(x => x.ID == req.AsRobotID);
                    int indexOfMyRobot = betterRobots.IndexOf(myRob);
                    int page = (int)((float)indexOfMyRobot / 5f);
                    int pageStart = page - 1;
                    int pageEnd = page + 2;

                    if (pageStart < 0) pageStart = 0;

                    int curIndex = pageStart * 5;

                    List<PlayerRobot> tmp = new List<PlayerRobot>();
                    for (int c = curIndex; c < pageEnd * 5; c++)
                    {
                        if (c < betterRobots.Count)
                        {
                            tmp.Add(betterRobots[c]);
                        }
                    }
                    betterRobots = tmp;
                }
                for (int c = 0; c < betterRobots.Count; c++)
                {
                    xRobot xRob = ConvertToXRobot(cc, betterRobots[c], false, req.IncludeScreensot);
                    res.RobotsInMatch.Add(xRob);
                }
                res.Success = true;
            }
            return res;
        }
        private RetrieveListOfChallengeableRobotsResult RetrieveListOfChallengableRobotsInDivisionOnly(DataClassesInterfaceDataContext cc, RetrieveListOfChallengeableRobotsRequest req, string steamID)
        {
            RetrieveListOfChallengeableRobotsResult res = new RetrieveListOfChallengeableRobotsResult();
            res.Success = false;
            res.ResultText = "";
            res.RobotsInMatch = new List<xRobot>();
            if (steamID == null) steamID = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer != null)
            {


                //PlayerRobot myRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == robotID && x.ID_PlayerOwner == aPlayer.ID);

                //if (myRobot == null)
                //{
                //    myRobot = new PlayerRobot();
                //    myRobot.ID_PlayerOwner = aPlayer.ID;

                //    cc.PlayerRobots.InsertOnSubmit(myRobot);
                //}

                //myRobot.RobotTemplate = robotTemplate;
                //cc.SubmitChanges();

                List<PlayerRobot> betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();
                //betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();
                if (req.AsRobotID > 0)
                {
                    PlayerRobot myRob = betterRobots.FirstOrDefault(x => x.ID == req.AsRobotID);
                    int indexOfMyRobot = betterRobots.IndexOf(myRob);
                    int page = (int)((float)indexOfMyRobot *0.2f);
                    int pageStart = page - 0;
                    int pageEnd = page + 1;

                    if (pageStart < 0) pageStart = 0;

                    int curIndex = pageStart * 5;

                    List<PlayerRobot> tmp = new List<PlayerRobot>();
                    for (int c = curIndex; c < pageEnd * 5; c++)
                    {
                        if (c < betterRobots.Count)
                        {
                            tmp.Add(betterRobots[c]);
                        }
                    }
                    betterRobots = tmp;
                }
                for (int c = 0; c < betterRobots.Count; c++)
                {
                    xRobot xRob = ConvertToXRobot(cc, betterRobots[c], false, req.IncludeScreensot);
                    res.RobotsInMatch.Add(xRob);
                }
                res.Success = true;
            }
            return res;
        }
        public CreateRobotMatchResult CreateRobotMatch(DataClassesInterfaceDataContext cc, CreateRobotMatchRequest req, HttpServerUtility server, string steamID)
        {
            if (steamID == null) steamID = "";
            CreateRobotMatchResult res = new CreateRobotMatchResult();
            res.Success = false;
            res.ResultText = "";
            res.PrizeMoney = 1;
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);
            req.RobotIDs = req.RobotIDs.Distinct().ToList();
            if (aPlayer == null)
            {

            }
            else
            {
                }


            //if (aPlayer != null)
            {
                //PlayerRobot myRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == robotID && x.ID_PlayerOwner == aPlayer.ID);

                //if (myRobot == null)
                //{
                //    myRobot = new PlayerRobot();
                //    myRobot.ID_PlayerOwner = aPlayer.ID;

                //    cc.PlayerRobots.InsertOnSubmit(myRobot);
                //}

                //myRobot.RobotTemplate = robotTemplate;
                //cc.SubmitChanges();
                if (req.RobotIDs.Count > 1)
                {
                    if (req.ExtraRobotToInclude > 0) req.RobotIDs.Add(req.ExtraRobotToInclude);
                    req.RobotIDs = req.RobotIDs.Distinct().ToList();
                    List<PlayerRobot> betterRobots = cc.PlayerRobots.Where(x => req.RobotIDs.Contains(x.ID)).OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();

                    Match aMatch = new Match();
                    aMatch.GameEndDateUTC = DateTime.UtcNow;
                    aMatch.GameStartDateUTC = aMatch.GameEndDateUTC;
                    aMatch.HasFinished = false;
                    aMatch.ID_BotWinner = 0;
                    aMatch.YouTubeID = "";
                    aMatch.ID_Arena = 0;
                    aMatch.PrizeMoney = 300;//todo update
                    
                    res.PrizeMoney = (float)aMatch.PrizeMoney;
                    MatchArena anArena = cc.MatchArenas.FirstOrDefault(x => x.IsEnabled == true);//todo!
                    if (anArena != null)
                    {
                        aMatch.ID_Arena = anArena.ID;
                    }
                    res.TheArena = ConvertToXArena(cc, anArena);
                    res.RobotsInMatch = new List<xRobot>();
                    cc.Matches.InsertOnSubmit(aMatch);
                    cc.SubmitChanges();
                    res.MatchID = aMatch.ID;
                    try
                    {
                        for (int c = 0; c < betterRobots.Count; c++)
                        {
                            RobotMatchResult aResult = new RobotMatchResult();
                            aResult.BulletsFired = 0;
                            aResult.BulletsHit = 0;
                            aResult.Health = 0;
                            aResult.ID_Match = aMatch.ID;
                            aResult.ID_Robot = betterRobots[c].ID;
                            aResult.IsSubmitted = false;
                            aResult.MinutesSurvived = 0;
                            aResult.RamDamage = 0;
                            aResult.DamageReceived = 0;

                            cc.RobotMatchResults.InsertOnSubmit(aResult);
                            cc.SubmitChanges();
                            xRobot xRob = ConvertToXRobot(cc, betterRobots[c], true, true);
                            res.RobotsInMatch.Add(xRob);

                        }

                        ClassTTS.GenerateMatchIntro(cc, aMatch, server);
                        res.Success = true;
                    }
                    catch(Exception er)
                    {
                        res.ResultText = er.ToString();
                    }
                }
                else
                {
                    List<int> idsOfAllRobotsInDivision = new List<int>();
                    RetrieveListOfChallengeableRobotsResult resList = RetrieveListOfChallengableRobotsInDivisionOnly(cc, new RetrieveListOfChallengeableRobotsRequest() { Email = req.Email, Passowrd = req.Passowrd, AsRobotID = req.RobotIDs[0] }, steamID);

                    req.RobotIDs = resList.RobotsInMatch.Select(x => x.ID).ToList();
                    // if (req.ExtraRobotToInclude > 0) req.RobotIDs.Add(req.ExtraRobotToInclude);
                    req.RobotIDs = req.RobotIDs.Distinct().ToList();

                    res = CreateRobotMatch(cc, req, server, steamID);
                }
            }
            return res;
        }


        private int DetermineRobotDivision(DataClassesInterfaceDataContext cc, PlayerRobot aRobot)
        {
            int division = 0;
            List<int> idsOfAllRobotsInDivision = new List<int>();
            //RetrieveListOfChallengeableRobotsResult resList = RetrieveListOfChallengableRobotsInDivisionOnly(cc, new RetrieveListOfChallengeableRobotsRequest() { Email = req.Email, Passowrd = req.Passowrd, AsRobotID = req.RobotIDs[0] });

            List<PlayerRobot> betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();
            //betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();
            //if (aRobot. > 0)
            {
                PlayerRobot myRob = betterRobots.FirstOrDefault(x => x.ID == aRobot.ID);
                int indexOfMyRobot = betterRobots.IndexOf(myRob);
                int page = (int)((float)indexOfMyRobot * 0.2f);
                int pageStart = page - 0;
                int pageEnd = page + 1;

                if (pageStart < 0) pageStart = 0;

                int curIndex = pageStart *1;
                division = curIndex;

                //List<PlayerRobot> tmp = new List<PlayerRobot>();
                //for (int c = curIndex; c < pageEnd * 5; c++)
                //{
                //    if (c < betterRobots.Count)
                //    {
                //        tmp.Add(betterRobots[c]);
                //    }
                //}
                //betterRobots = tmp;
            }
            return division;

        }

        public SubmitRobotMatchResult SubmitRobotMatch(DataClassesInterfaceDataContext cc, SubmitRobotMatchRequest req,   string steamID)
        {
            if (steamID == null) steamID = "";
            SubmitRobotMatchResult res = new SubmitRobotMatchResult();
            res.Success = true;
            res.ResultText = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd,  steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer != null && req.MatchID > 0)
            {

                Match aMatch = cc.Matches.FirstOrDefault(x => x.ID == req.MatchID);
                if (aMatch.HasFinished == false)
                {
                    aMatch.HasFinished = true;

                    // List<PlayerRobot> betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();
                    //betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();
                    try
                    {
                        for (int c = 0; c < req.RobotsInMatch.Count; c++)
                        {
                            PlayerRobot aRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == req.RobotsInMatch[c].RobotID);
                            int divisionBefore = DetermineRobotDivision(cc, aRobot);
                            RobotMatchResult aResult = cc.RobotMatchResults.FirstOrDefault(x => x.ID_Match == aMatch.ID && x.ID_Robot == aRobot.ID);
                            Player owner= cc.Players.FirstOrDefault(x=>x.ID==aRobot.ID_PlayerOwner);
                            aResult.IsSubmitted = true;
                            aResult.BulletsFired = req.RobotsInMatch[c].BulletsFired;
                            aResult.BulletsHit = req.RobotsInMatch[c].BulletsHit;
                            aResult.Health = req.RobotsInMatch[c].Health;
                            aResult.RamDamage = req.RobotsInMatch[c].RamDamage;
                            aResult.MinutesSurvived = req.RobotsInMatch[c].MinutesSurvived;
                            aResult.TotalPoints = req.RobotsInMatch[c].TotalPoints;
                            aResult.IsDead = req.RobotsInMatch[c].IsDead;
                            aResult.TotalKills = req.RobotsInMatch[c].TotalKills;

                            //aResult.kil
                            aRobot.GamesPlayed++;
                            if (req.RobotsInMatch[c].IsDead)
                            {

                                aRobot.TotalDeaths++;
                            }
                            aRobot.TotalKills += req.RobotsInMatch[c].TotalKills;
                            float kdRatio = ((float)aRobot.TotalKills + 1f) / ((float)aRobot.TotalDeaths + 1f);
                            aRobot.TotalPoints = (int)(aRobot.TotalPoints + 2 * ((double)req.RobotsInMatch[c].TotalPoints - (double)Math.Max(95, (double)Math.Min((double)190, (double)kdRatio * 50))));


                            int myRankForMatch = req.RobotsInMatch.OrderByDescending(x => x.TotalPoints).ToList().IndexOf(req.RobotsInMatch[c]);
                            decimal moneyPerfect = 0;//(myRankForMatch + 1f) / (float)req.RobotsInMatch.Count;
                            string rankText = "";
                            string fullWinNewsText = "";
                            if (myRankForMatch == 0)
                            {
                                moneyPerfect = aMatch.PrizeMoney;
                                rankText = " won with " + req.RobotsInMatch[c].TotalPoints.ToString("f0") + " points ";
                                //fullWinNewsText = aRobot.RobotName + " by " + owner.DisplayName + " has won $" + moneyPerfect.ToString("f0");
                                
                            }
                            if (req.RobotsInMatch.Count > 2)
                            {
                                if (myRankForMatch == 1)
                                {
                                    moneyPerfect = (decimal)0.4 * aMatch.PrizeMoney;
                                    rankText = " came 2nd ";
                                }
                            }
                            if (req.RobotsInMatch.Count > 3)
                            {
                                if (myRankForMatch == 2)
                                {
                                    moneyPerfect = (decimal)0.2 * aMatch.PrizeMoney;
                                    rankText = " came 3rd ";

                                }
                            }

                         
                            aRobot.MoneyWon += moneyPerfect;
                            if (owner != null)
                            {
                                owner.Money += moneyPerfect;
                            }
                            cc.SubmitChanges();
                            if (moneyPerfect > 0)
                            {
                                GameFinancialRow aFinanceStat = new GameFinancialRow();
                                aFinanceStat.EventDate = System.DateTime.UtcNow;
                                aFinanceStat.ID_Match = aMatch.ID;
                                if (owner != null)
                                {
                                    aFinanceStat.ID_Player = owner.ID;
                                }
                                else
                                {
                                    aFinanceStat.ID_Player = -1;
                                }



                                aFinanceStat.ID_PlayerInitiater = aPlayer.ID;
                                aFinanceStat.ID_Robot = aRobot.ID;
                                aFinanceStat.MoneyAmount = moneyPerfect;

                                string names = "";
                                List<int> otherRobotIDs = req.RobotsInMatch.Where(x => x.RobotID != aRobot.ID).OrderByDescending(x => x.TotalPoints).Select(x => x.RobotID).ToList();
                                List<string> robotNames = cc.PlayerRobots.Where(x => x.ID!= aRobot.ID && otherRobotIDs.Contains(x.ID)).OrderBy(x => x.RobotName).Select(x => x.RobotName).ToList();
                                for (int x = 0; x < robotNames.Count; x++)
                                {
                                    if (x > 0) names += ", ";
                                    names += robotNames[x];
                                }

                                string matchInit = "";
                                if (owner != null && aPlayer.ID != owner.ID)
                                {
                                    matchInit = "Match played by " + aPlayer.DisplayName + ".";
                                }
                                aFinanceStat.Reason = aRobot.RobotName + rankText + "against " + names + ". " + matchInit;
                                cc.GameFinancialRows.InsertOnSubmit(aFinanceStat);
                                cc.SubmitChanges();
                            }
                            DbUpdateRobotStats(cc, aRobot, aResult, req.RobotsInMatch.Count, myRankForMatch);

                            int divisionAfter = DetermineRobotDivision(cc, aRobot);

                            if (myRankForMatch == 0)
                            {  
                                if (divisionAfter < divisionBefore)
                                {
                                    AddNews(cc, aRobot.RobotName + " has moved up to Division " + divisionAfter);
                                }
                            }
                            //if (divisionAfter > divisionBefore)
                            //{
                            //    AddNews(cc, aRobot.RobotName + " has moved down to Division " + divisionAfter);
                            //}
                            if (fullWinNewsText.Length > 0)
                            {
                                AddNews(cc, fullWinNewsText);
                          

                            }
                            // aRobot.TotalKills += req.RobotsInMatch[c].TotalKills;

                        }

                        cc.SubmitChanges();

                        res.Success = true;
                    }
                    catch (Exception ec)
                    {
                        res.ResultText = "ERROR104! " + ec.ToString() + " " + ec.StackTrace;
                        res.Success = false;

                    }

                }
            }
            return res;
        }
        private void AddNews(DataClassesInterfaceDataContext cc, string news)
        {
            NewReelEvent aReel = new NewReelEvent();
            cc.NewReelEvents.InsertOnSubmit(aReel);
            aReel.DisplayDate = System.DateTime.UtcNow;
            aReel.ID_MainRobot = 0;
            aReel.NewsText = news;
            aReel.NewsType = 0;
          
            cc.SubmitChanges();

        }
        private void DbUpdateRobotStats(DataClassesInterfaceDataContext cc, PlayerRobot aRobot, RobotMatchResult aResult, int numberOfRobots, int rankInMatch)
        {

            RobotConstructor.RobotTemplate FullTemplate = RobotConstructor.RobotTemplate.ConvertFromBytes(aRobot.RobotTemplateImage.ToArray());

            RobotStat statRobot = cc.RobotStats.FirstOrDefault(x => x.ID_Robot == aRobot.ID && x.ID_Player == aRobot.ID_PlayerOwner);
            RobotStat statPlayer = cc.RobotStats.FirstOrDefault(x => x.ID_Robot == 0 && x.ID_Player == aRobot.ID_PlayerOwner);
            if (statRobot == null)
            {
                statRobot = new RobotStat();
                statRobot.ID_Player = aRobot.ID_PlayerOwner;
                statRobot.ID_Robot = aRobot.ID;
                statRobot.CalcRank = 0;
                statRobot.RatioCodeToScore = 0;
                statRobot.RatioHitsToMiss = 0;
                statRobot.RatioKillToDeath = 0;
                statRobot.RatioMultiRobotMatchWinsToLoss = 0;
                statRobot.RatioWinToLoss = 0;
                statRobot.TotalBulletsFired = 0;
                statRobot.TotalBulletsHit = 0;
                statRobot.TotalDamageReceived = 0;
                statRobot.TotalDeaths = 0;
                statRobot.TotalKills = 0;
                statRobot.TotalLinesOfCode = 0;
                statRobot.TotalMatches = 0;
                statRobot.TotalMatchesWon = 0;
                statRobot.TotalMultiRobotMatchesPlayed = 0;
                statRobot.TotalMultiRobotMatchesWon = 0;
                statRobot.TotalPoints = 0;
                cc.RobotStats.InsertOnSubmit(statRobot);
            }
            if (statPlayer == null)
            {
                statPlayer = new RobotStat();
                statPlayer.ID_Player = aRobot.ID_PlayerOwner;
                statPlayer.ID_Robot = 0;
                statPlayer.CalcRank = 0;
                statPlayer.RatioCodeToScore = 0;
                statPlayer.RatioHitsToMiss = 0;
                statPlayer.RatioKillToDeath = 0;
                statPlayer.RatioMultiRobotMatchWinsToLoss = 0;
                statPlayer.RatioWinToLoss = 0;
                statPlayer.TotalBulletsFired = 0;
                statPlayer.TotalBulletsHit = 0;
                statPlayer.TotalDamageReceived = 0;
                statPlayer.TotalDeaths = 0;
                statPlayer.TotalKills = 0;
                statPlayer.TotalLinesOfCode = 0;
                statPlayer.TotalMatches = 0;
                statPlayer.TotalMatchesWon = 0;
                statPlayer.TotalMultiRobotMatchesPlayed = 0;
                statPlayer.TotalMultiRobotMatchesWon = 0;
                statPlayer.TotalPoints = 0;
                cc.RobotStats.InsertOnSubmit(statPlayer);
            }
            bool didIWin = rankInMatch == 0 ? true : false;
            bool isMultiRobotMatch = numberOfRobots > 2 ? true : false;
            {
                statRobot.CalcRank = cc.RobotStats.Count(x => x.ID_Player != 0 && x.TotalPoints >= aRobot.TotalPoints);
                statRobot.TotalBulletsFired += aResult.BulletsFired;
                statRobot.TotalBulletsHit += aResult.BulletsHit;
                statRobot.TotalDamageReceived += (long)aResult.DamageReceived;
                statRobot.TotalDeaths += aResult.Health <= 0 ? 1 : 0;
                statRobot.TotalKills += aResult.TotalKills;
                statRobot.TotalLinesOfCode = aRobot.LinesOfCode;
                statRobot.TotalMatches += 1;
                statRobot.TotalMatchesWon += didIWin ? 1 : 0;
                statRobot.TotalMultiRobotMatchesPlayed += isMultiRobotMatch ? 1 : 0;
                statRobot.TotalMultiRobotMatchesWon = didIWin ? 1 : 0;
                statRobot.TotalPoints += (long)aResult.TotalPoints;

                statRobot.RatioCodeToScore = DetermineRatio(statRobot.TotalPoints+1f, statRobot.TotalLinesOfCode+1f);
                statRobot.RatioHitsToMiss = DetermineRatio((float)statRobot.TotalBulletsHit, (float)statRobot.TotalBulletsFired);
                statRobot.RatioKillToDeath = DetermineRatio(statRobot.TotalKills+1f, statRobot.TotalDeaths+1f);
                statRobot.RatioMultiRobotMatchWinsToLoss = DetermineRatio(statRobot.TotalMultiRobotMatchesWon+1f, statRobot.TotalMultiRobotMatchesPlayed+1f);
                statRobot.RatioWinToLoss = DetermineRatio(statRobot.TotalMatchesWon + 1f, statRobot.TotalMatches + 1f);

            }
            {
                statPlayer.CalcRank = cc.RobotStats.Count(x => x.ID_Player == 0 && x.TotalPoints >= aRobot.TotalPoints);
                statPlayer.TotalBulletsFired += aResult.BulletsFired;
                statPlayer.TotalBulletsHit += aResult.BulletsHit;
                statPlayer.TotalDamageReceived += (long)aResult.DamageReceived;
                statRobot.TotalDeaths += aResult.Health <= 0 ? 1 : 0;
                statPlayer.TotalKills += aResult.TotalKills;
                statPlayer.TotalLinesOfCode = aRobot.LinesOfCode;
                statPlayer.TotalMatches += 1;
                statPlayer.TotalMatchesWon += didIWin ? 1 : 0;
                statPlayer.TotalMultiRobotMatchesPlayed += isMultiRobotMatch ? 1 : 0;
                statPlayer.TotalMultiRobotMatchesWon = didIWin ? 1 : 0;
                statPlayer.TotalPoints += (long)aResult.TotalPoints;

                statPlayer.RatioCodeToScore = DetermineRatio(statPlayer.TotalPoints + 1f, statPlayer.TotalLinesOfCode + 1f);
                statPlayer.RatioHitsToMiss = DetermineRatio((float)statPlayer.TotalBulletsHit, (float)statPlayer.TotalBulletsFired);
                statPlayer.RatioKillToDeath = DetermineRatio(statPlayer.TotalKills + 1f, statPlayer.TotalDeaths + 1f);
                statPlayer.RatioMultiRobotMatchWinsToLoss = DetermineRatio(statPlayer.TotalMultiRobotMatchesWon + 1f, statPlayer.TotalMultiRobotMatchesPlayed + 1f);
                statPlayer.RatioWinToLoss = DetermineRatio(statPlayer.TotalMatchesWon + 1f, statPlayer.TotalMatches + 1f);

            }

            cc.SubmitChanges();
        }
        private float DetermineRatio(float val1, float val2)
        {
            float ret = 0;
            if (val1 == 0 || val2 == 0)
            {
                ret = (val1 + 0.001f) / (val2 + 0.001f);
            }
            else
            {
                ret = (val1) / (val2);

            }

            return ret;
        }
        public SubmitVideohResult SubmitVideo(DataClassesInterfaceDataContext cc, SubmitVideohRequest req, string steamID)
        {
            if (steamID == null) steamID = "";
            string videoFolder = @"C:\AssemblyVideosQueue\";
            SubmitVideohResult res = new SubmitVideohResult();
            res.Success = false;
            res.ResultText = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer != null && req.MatchID > 0)
            {

                Match aMatch = cc.Matches.FirstOrDefault(x => x.ID == req.MatchID);
                if (aMatch.HasFinished == true && aMatch.YouTubeID.Length <= 2)
                {


                    try
                    {
                        string fileName = videoFolder + aMatch.ID + ".mp4";
                        System.IO.File.WriteAllBytes(fileName, req.VideoData);
                        cc.SubmitChanges();

                        res.Success = true;
                    }
                    catch
                    {
                        res.ResultText = "ERROR104!!!";
                        res.Success = false;

                    }

                }
            }
            return res;
        }


        public SendReceiveDiscussionResult SendReceiveDiscussion(DataClassesInterfaceDataContext cc, SendReceiveDiscussionRequest req, string steamID)
        {
            if (steamID == null) steamID = "";
            //string videoFolder = @"C:\AssemblyVideosQueue\";
            SendReceiveDiscussionResult res = new SendReceiveDiscussionResult();
            res.Success = false;
            res.ResultText = "";

            if (req.SendMessage.Trim().Length > 0)
            {
                Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd,  steamID);


                InGameDiscussion aDisc = new InGameDiscussion();
                aDisc.ID_Player = aPlayer.ID;
                aDisc.MessageText = req.SendMessage.Trim();
                aDisc.UtcDate = System.DateTime.UtcNow;
                cc.InGameDiscussions.InsertOnSubmit(aDisc);
                cc.SubmitChanges();

            }

            List<InGameDiscussion> aDiscList = cc.InGameDiscussions.Where(x => x.UtcDate >= req.LastUtcUpdateDate).OrderByDescending(x => x.UtcDate).Take(50).ToList();
            aDiscList = aDiscList.OrderBy(x => x.UtcDate).ToList();
            res.AllDiscussions = new List<xDiscussion>();
            for (int c = 0; c < aDiscList.Count; c++)
            {
                xDiscussion aDisc = new xDiscussion();
                aDisc.ID = aDiscList[c].ID;
                aDisc.PlayerSenderID = aDiscList[c].ID_Player;
                aDisc.UtcDate = aDiscList[c].UtcDate;
                aDisc.Message = aDiscList[c].MessageText;
                Player otherPlayer = cc.Players.FirstOrDefault(x => x.ID == aDiscList[c].ID_Player);
                if (otherPlayer != null)
                {
                    aDisc.PlayerSenderName = otherPlayer.DisplayName;
                }
                res.AllDiscussions.Add(aDisc);
            }
            res.Success = true;
            return res;
        }
        public UpdateLeagueLeaderboardResult UpdateLeagueLeaderboards(DataClassesInterfaceDataContext cc, UpdateLeagueLeaderboardRequest req, string steamID)
        {
            if (steamID == null) steamID = "";
            //string videoFolder = @"C:\AssemblyVideosQueue\";
            UpdateLeagueLeaderboardResult res = new UpdateLeagueLeaderboardResult();
            res.Success = false;
            res.ResultText = "";
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd,  steamID);
            if (aPlayer != null) res.ViewingAsPlayerID = aPlayer.ID;

            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot == 0).OrderByDescending(x => x.TotalPoints).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot == 0).OrderByDescending(x => x.TotalPoints).FirstOrDefault();
                    if (myStat != null)
                    {
                        //PlayerRobot topRobot = cc.PlayerRobots.Where(x => x.ID_PlayerOwner == myStat.ID_Player).OrderByDescending(x => x.TotalPoints).FirstOrDefault();
                        //if (topRobot != null)
                        //{
                        //    myStat.ID_Robot = topRobot.ID;

                        //}
                        top.Add(myStat);
                    }
                }
                res.TopAssemblers = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                //List<PlayerRobot> topRobots = new List<PlayerRobot>();
                // topRobots.AddRange(cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).Take(5).ToList());
                //   List<int> ids = topRobots.Select(x => x.ID).ToList();
                List<RobotStat> top = new List<RobotStat>();
                //  top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderBy(x => x.CalcRank).ToList().Take(5).ToList());
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.TotalPoints).Take(5).ToList());
                // top.AddRange(cc.RobotStats.Where(x => ids.Contains(x.ID_Robot)).ToList());
                if (aPlayer != null)
                {
                    //PlayerRobot myTop = cc.PlayerRobots.Where(x => x.ID_PlayerOwner == aPlayer.ID).OrderByDescending(x => x.TotalPoints).FirstOrDefault();
                    //if (myTop != null)
                    {
                        RobotStat myStat = cc.RobotStats.OrderByDescending(x => x.TotalPoints).FirstOrDefault(x => x.ID_Player == aPlayer.ID);
                        if (myStat != null)
                        {
                            top.Add(myStat);
                        }
                    }
                }
                res.TopRobots = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.RatioCodeToScore).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot != 0).OrderByDescending(x => x.RatioCodeToScore).FirstOrDefault();
                    if (myStat != null)
                    {
                        top.Add(myStat);
                    }
                }
                res.TopRobotCodeToScoreRatio = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.RatioKillToDeath).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot != 0).OrderByDescending(x => x.RatioKillToDeath).FirstOrDefault();
                    if (myStat != null)
                    {
                        top.Add(myStat);
                    }
                }
                res.TopRobotKillsToDeathRatio = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.RatioHitsToMiss).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot != 0).OrderByDescending(x => x.RatioHitsToMiss).FirstOrDefault();
                    if (myStat != null)
                    {
                        top.Add(myStat);
                    }
                }
                res.TopRobotMostAccurateHitToMissRatio = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.TotalKills).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot != 0).OrderByDescending(x => x.TotalKills).FirstOrDefault();
                    if (myStat != null)
                    {
                        top.Add(myStat);
                    }
                }
                res.TopRobotMostKills = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.RatioMultiRobotMatchWinsToLoss).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot != 0).OrderByDescending(x => x.RatioMultiRobotMatchWinsToLoss).FirstOrDefault();
                    if (myStat != null)
                    {
                        top.Add(myStat);
                    }
                }
                res.TopRobotMultiMatchWinRatio = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }
            {
                List<RobotStat> top = new List<RobotStat>();
                top.AddRange(cc.RobotStats.Where(x => x.ID_Robot != 0).OrderByDescending(x => x.RatioWinToLoss).Take(5).ToList());
                if (aPlayer != null)
                {
                    RobotStat myStat = cc.RobotStats.Where(x => x.ID_Player == aPlayer.ID && x.ID_Robot != 0).OrderByDescending(x => x.RatioWinToLoss).FirstOrDefault();
                    if (myStat != null)
                    {
                        top.Add(myStat);
                    }
                }
                res.TopRobotWinPercentRatio = ConvertStatToLeaderboardList(cc, top, req.IncludeScreensot);
            }

            res.Success = true;
            //res.AllDiscussions


            //if (req.SendMessage.Trim().Length > 0)
            //{
            //    Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd);


            //    InGameDiscussion aDisc = new InGameDiscussion();
            //    aDisc.ID_Player = aPlayer.ID;
            //    aDisc.MessageText = req.SendMessage.Trim();
            //    aDisc.UtcDate = System.DateTime.UtcNow;
            //    cc.InGameDiscussions.InsertOnSubmit(aDisc);
            //    cc.SubmitChanges();

            //}

            //List<InGameDiscussion> aDiscList = cc.InGameDiscussions.Where(x => x.UtcDate >= req.LastUtcUpdateDate).OrderByDescending(x => x.UtcDate).Take(50).ToList();
            //aDiscList = aDiscList.OrderBy(x => x.UtcDate).ToList();
            //res.AllDiscussions = new List<xDiscussion>();
            //for (int c = 0; c < aDiscList.Count; c++)
            //{
            //    xDiscussion aDisc = new xDiscussion();
            //    aDisc.ID = aDiscList[c].ID;
            //    aDisc.PlayerSenderID = aDiscList[c].ID_Player;
            //    aDisc.UtcDate = aDiscList[c].UtcDate;
            //    aDisc.Message = aDiscList[c].MessageText;
            //    Player otherPlayer = cc.Players.FirstOrDefault(x => x.ID == aDiscList[c].ID_Player);
            //    if (otherPlayer != null)
            //    {
            //        aDisc.PlayerSenderName = otherPlayer.DisplayName;
            //    }
            //    res.AllDiscussions.Add(aDisc);
            //}



            res.Success = true;
            return res;
        }
        private List<xLeagueLeaderboard> ConvertStatToLeaderboardList(DataClassesInterfaceDataContext cc, List<RobotStat> stats, bool IncludeScreensot)
        {
            List<xLeagueLeaderboard> ret = new List<xLeagueLeaderboard>();
            for (int c = 0; c < stats.Count; c++)
            {
                ret.Add(CovertStatToLeaderBoard(cc, stats[c], IncludeScreensot));
            }
            return ret;
        }
        private xLeagueLeaderboard CovertStatToLeaderBoard(DataClassesInterfaceDataContext cc, RobotStat aStat, bool IncludeScreensot)
        {
            //PlayerRobot aRobt = cc.PlayerRobots.FirstOrDefault(x => x.ID == aStat.ID_Robot);
            xLeagueLeaderboard ret = new xLeagueLeaderboard();
            ret.ID = aStat.ID;
            ret.PlayerID = aStat.ID_Player;
            ret.RobotID = aStat.ID_Robot;
            //ret.Rank = aStat.CalcRank;// cc.PlayerRobots.Count(x => x.TotalPoints >= aRobt.TotalPoints) + 1;

            if (ret.RobotID == 0)
            {
                ret.Rank = cc.RobotStats.Count(x => x.ID_Robot == 0 && x.TotalPoints >= aStat.TotalPoints);

            }
            else
            {
                ret.Rank = cc.RobotStats.Count(x => x.ID_Robot != 0 && x.TotalPoints >= aStat.TotalPoints);
            }
            //if (aRobt != null)
            //{
            //    ret.Rank = cc.PlayerRobots.Count(x => x.TotalPoints >= aRobt.TotalPoints) + 1;
            //}
            //else
            //{
            //    //Player aPlayer = cc.PlayerRobots.FirstOrDefault(x => x.ID == aStat.ID_Robot);

            //}
            ret.RatioCodeToScore = (float)aStat.RatioCodeToScore;
            ret.RatioHitsToMiss = (float)aStat.RatioHitsToMiss;
            ret.RatioKillToDeath = (float)aStat.RatioKillToDeath;
            ret.RatioMultiRobotMatchWinsToLoss = (float)aStat.RatioMultiRobotMatchWinsToLoss;
            ret.RatioWinToLoss = (float)aStat.RatioWinToLoss;
            ret.TotalKills = aStat.TotalKills;
            ret.TotalPoints = aStat.TotalPoints;

            Player aplayer = cc.Players.FirstOrDefault(x => x.ID == aStat.ID_Player);
            if (aplayer != null)
            {
                ret.PlayerName = aplayer.DisplayName;

            }
            PlayerRobot aRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == aStat.ID_Robot);
            if (aRobot != null)
            {
                ret.RobotName = aRobot.RobotName;
                if (IncludeScreensot)
                {
                    RobotConstructor.RobotTemplate tmp = RobotConstructor.RobotTemplate.ConvertFromBytes(aRobot.RobotTemplateImage.ToArray());
                    ret.ScreenshotData = tmp.ScreenShot;
                }

            }
            else
            {
                if (aplayer != null)
                {
                    PlayerRobot topRobot = cc.PlayerRobots.Where(x => x.ID_PlayerOwner == aplayer.ID).OrderByDescending(x => x.TotalPoints).FirstOrDefault();
                    if (topRobot != null)
                    {
                        ret.RobotID = topRobot.ID;
                        aRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == ret.RobotID);
                        if (aRobot != null)
                        {
                            ret.RobotName = aRobot.RobotName;
                            if (IncludeScreensot)
                            {
                                RobotConstructor.RobotTemplate tmp = RobotConstructor.RobotTemplate.ConvertFromBytes(aRobot.RobotTemplateImage.ToArray());
                                ret.ScreenshotData = tmp.ScreenShot;
                            }
                        }
                    }
                }
            }
            return ret;
        }
        public float GetPlayerFunds(DataClassesInterfaceDataContext cc, string email, string pass, string steamID)
        {
            if (steamID == null) steamID = "";
            float ret = 0;
            Player aPlayer = GetPlayerFromDB(cc, email, pass,  steamID);
            //aPlayer.Money += 1;
            //cc.SubmitChanges();
            if (aPlayer != null)
            {
                ret = (float)aPlayer.Money;
            }
            return ret;
        }

        public xRobot ConvertToXRobot(DataClassesInterfaceDataContext cc, PlayerRobot aRobot, bool IncludeTemplate, bool IncludeScreensot)
        {
            Player owner = cc.Players.FirstOrDefault(x => x.ID == aRobot.ID_PlayerOwner);
            xRobot ret = new xRobot();
            ret.ID = aRobot.ID;
            ret.GamesPlayed = aRobot.GamesPlayed;
            ret.MoneyWon = aRobot.MoneyWon;
            ret.OwnerID = aRobot.ID_PlayerOwner;
            ret.RobotName = aRobot.RobotName;
            if (owner != null)
            {
                ret.OwnerName = owner.DisplayName;
            }
            else
            {
                ret.OwnerName = "[BROKE FREE]";
           
               
            }
            if (IncludeTemplate)
            {
                ret.RobotTemplate = new xRobotTemplate();
                ret.RobotTemplate.FullTemplate = RobotConstructor.RobotTemplate.ConvertFromBytes(aRobot.RobotTemplateImage.ToArray());
                ret.RobotTemplate.FullTemplate.LeagueID = aRobot.ID;
                if (owner != null)
                {
                    ret.RobotTemplate.FullTemplate.LeagueOwnerID = owner.ID;
                    ret.RobotTemplate.FullTemplate.LeagueOwnerName = owner.DisplayName;
                }
                else
                {
                    ret.RobotTemplate.FullTemplate.LeagueOwnerID =-1;
                    ret.RobotTemplate.FullTemplate.LeagueOwnerName = "[BROKE FREE]";
           
                }
                RobotStat statRobot = cc.RobotStats.FirstOrDefault(x => x.ID_Robot == aRobot.ID && x.ID_Player == aRobot.ID_PlayerOwner);
                if (statRobot != null)
                {
                    //   ret.RobotTemplate.FullTemplate.CalculateCodeProcessedPerSecond = 0;
                    ret.RobotTemplate.FullTemplate.CalculateCodeTotalLeagueScore = aRobot.TotalPoints;
                    ret.RobotTemplate.FullTemplate.CalculateLinesOfCode = aRobot.LinesOfCode;
                    //   ret.RobotTemplate.FullTemplate.CalculateMaxSpeed = 0;
                    //   ret.RobotTemplate.FullTemplate.CalculateSqrMeterSize = 0;
                    //  ret.RobotTemplate.FullTemplate.CalculateTotalWeight = 0;
                    ret.RobotTemplate.FullTemplate.TotalDeaths = statRobot.TotalDeaths;
                    ret.RobotTemplate.FullTemplate.TotalKills = statRobot.TotalKills;
                    ret.RobotTemplate.FullTemplate.TotalMatches = statRobot.TotalMatches;
                    ret.RobotTemplate.FullTemplate.TotalWins = statRobot.TotalMatchesWon;

                }
            }
            if (IncludeScreensot)
            {
                RobotConstructor.RobotTemplate tmp = RobotConstructor.RobotTemplate.ConvertFromBytes(aRobot.RobotTemplateImage.ToArray());
                ret.ScreenShot = tmp.ScreenShot;
            }
            int myRankForMatch = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList().IndexOf(aRobot);
            ret.WorldRank = myRankForMatch + 1;
            ret.TotalDeaths = aRobot.TotalDeaths;
            ret.TotalPoints = aRobot.TotalPoints;
            ret.TotalKills = aRobot.TotalKills;

            return ret;
        }
        public Player GetPlayerFromDB(DataClassesInterfaceDataContext cc, string email, string passowrd, string steamID)
        {
            if (steamID == null) steamID = "";
            Player aPlayer = null;
            if (steamID!= null && steamID.Length > 1)
            {
                aPlayer = cc.Players.FirstOrDefault(x => x.SteamID ==steamID);
            }
            else
            {
                aPlayer = cc.Players.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == passowrd);
         
            }
          //  aPlayer = cc.Players.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());
            return aPlayer;
        }

        public xArena ConvertToXArena(DataClassesInterfaceDataContext cc, MatchArena anArena)
        {
            // Player owner = cc.Players.FirstOrDefault(x => x.ID == aRobot.ID_PlayerOwner);
            xArena ret = new xArena();
            if (anArena != null)
            {
                ret.ID = anArena.ID;
                ret.ArenaDescription = anArena.ArenaDescription;
                ret.ArenaName = anArena.ArenaName;
                ret.BuiltInLevelID = anArena.BuiltInLevelID;
                ret.ExternalURLocation = anArena.ExternalURLLocation;
                ret.MatchedPlayedHere = anArena.MatchesPlayedHere;
                ret.AssetBundleName = anArena.AssetBundleName;
            }
            else
            {
                ret.ID = 0;
                ret.ArenaDescription = "Sand Arena";
                ret.ArenaName = "Test Arena";
                ret.BuiltInLevelID = "FreeForAllScene";
                ret.ExternalURLocation = "";
                ret.MatchedPlayedHere = 0;
                ret.AssetBundleName = "";
            }


            return ret;
        }

        public RetrieveFinancialsResult RetrieveFinancials(DataClassesInterfaceDataContext cc, RetrieveFinancialsRequest req, string steamID)
        {
            if (steamID == null) steamID = "";
            RetrieveFinancialsResult res = new RetrieveFinancialsResult();
            res.Success = false;
            res.ResultText = "";
            res.LastFinances = new List<xFinanceRow>();
            Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd,  steamID);

            if (aPlayer == null)
            {

            }
            else
            {
                res.ResultText = "Invalid username/password.";
            }


            if (aPlayer != null)
            {
                List<GameFinancialRow> myLastFinances = cc.GameFinancialRows.Where(x => x.ID_Player == aPlayer.ID).OrderByDescending(x => x.EventDate).Take(50).ToList();


                //Random aRand = new Random();
                //int tot = aRand.Next(0, 15);
                for (int c = 0; c < myLastFinances.Count; c++)
                {
                    xFinanceRow aRow = new xFinanceRow();
                    aRow.EventDate = myLastFinances[c].EventDate;
                    aRow.ID = myLastFinances[c].ID;
                    aRow.MatchID = myLastFinances[c].ID_Match;
                    aRow.MoneyAmount = (float)myLastFinances[c].MoneyAmount;
                    aRow.PlayerID = myLastFinances[c].ID_Player;
                    aRow.PlayerInitiatorID = myLastFinances[c].ID_PlayerInitiater;
                    aRow.Reason = myLastFinances[c].Reason;
                    aRow.RobotID = myLastFinances[c].ID_Robot;
                    aRow.EventDate = myLastFinances[c].EventDate;
                    res.LastFinances.Add(aRow);
                    //    res.LastFinances.Add(new xFinanceRow() { EventDate = System.DateTime.Now, Reason = "Test" + c, MoneyAmount = 100 });
                    //    res.LastFinances.Add(new xFinanceRow() { EventDate = System.DateTime.Now, Reason = "Test" + aRand.Next(0, 100), MoneyAmount = 100 });
                    //    res.LastFinances.Add(new xFinanceRow() { EventDate = System.DateTime.Now, Reason = "Test", MoneyAmount = 100 });
                }

                ////PlayerRobot myRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == robotID && x.ID_PlayerOwner == aPlayer.ID);

                ////if (myRobot == null)
                ////{
                ////    myRobot = new PlayerRobot();
                ////    myRobot.ID_PlayerOwner = aPlayer.ID;

                ////    cc.PlayerRobots.InsertOnSubmit(myRobot);
                ////}

                ////myRobot.RobotTemplate = robotTemplate;
                ////cc.SubmitChanges();

                //List<PlayerRobot> betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();
                ////betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();
                //if (req.AsRobotID > 0)
                //{
                //    PlayerRobot myRob = betterRobots.FirstOrDefault(x => x.ID == req.AsRobotID);
                //    int indexOfMyRobot = betterRobots.IndexOf(myRob);
                //    int page = (int)((float)indexOfMyRobot / 5f);
                //    int pageStart = page - 1;
                //    int pageEnd = page + 2;

                //    if (pageStart < 0) pageStart = 0;

                //    int curIndex = pageStart * 5;

                //    List<PlayerRobot> tmp = new List<PlayerRobot>();
                //    for (int c = curIndex; c < pageEnd * 5; c++)
                //    {
                //        if (c < betterRobots.Count)
                //        {
                //            tmp.Add(betterRobots[c]);
                //        }
                //    }
                //    betterRobots = tmp;
                //}


                //for (int c = 0; c < betterRobots.Count; c++)
                //{
                //    xRobot xRob = ConvertToXRobot(cc, betterRobots[c], false, req.IncludeScreensot);
                //    res.RobotsInMatch.Add(xRob);
                //}
                res.Success = true;
            }
            return res;
        }
        public AddFundsSteamWalletResult AddSteamWalletFunds(DataClassesInterfaceDataContext cc, AddFundsSteamWalletRequest req, string steamID)
        {
            if (steamID == null) steamID = "";
            AddFundsSteamWalletResult res = new AddFundsSteamWalletResult();
            res.Success = false;
            res.ResultText = "";

            //res.LastFinances = new List<xFinanceRow>();
            //Player aPlayer = GetPlayerFromDB(cc, req.Email, req.Passowrd, steamID);

            //if (aPlayer == null)
            //{

            //}
            //else
            //{
            //    res.ResultText = "Invalid username/password.";
            //}


            //if (aPlayer != null)
            //{
            //    List<GameFinancialRow> myLastFinances = cc.GameFinancialRows.Where(x => x.ID_Player == aPlayer.ID).OrderByDescending(x => x.EventDate).Take(50).ToList();


            //    //Random aRand = new Random();
            //    //int tot = aRand.Next(0, 15);
            //    for (int c = 0; c < myLastFinances.Count; c++)
            //    {
            //        xFinanceRow aRow = new xFinanceRow();
            //        aRow.EventDate = myLastFinances[c].EventDate;
            //        aRow.ID = myLastFinances[c].ID;
            //        aRow.MatchID = myLastFinances[c].ID_Match;
            //        aRow.MoneyAmount = (float)myLastFinances[c].MoneyAmount;
            //        aRow.PlayerID = myLastFinances[c].ID_Player;
            //        aRow.PlayerInitiatorID = myLastFinances[c].ID_PlayerInitiater;
            //        aRow.Reason = myLastFinances[c].Reason;
            //        aRow.RobotID = myLastFinances[c].ID_Robot;
            //        aRow.EventDate = myLastFinances[c].EventDate;
            //        res.LastFinances.Add(aRow);
            //        //    res.LastFinances.Add(new xFinanceRow() { EventDate = System.DateTime.Now, Reason = "Test" + c, MoneyAmount = 100 });
            //        //    res.LastFinances.Add(new xFinanceRow() { EventDate = System.DateTime.Now, Reason = "Test" + aRand.Next(0, 100), MoneyAmount = 100 });
            //        //    res.LastFinances.Add(new xFinanceRow() { EventDate = System.DateTime.Now, Reason = "Test", MoneyAmount = 100 });
            //    }

            //    ////PlayerRobot myRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == robotID && x.ID_PlayerOwner == aPlayer.ID);

            //    ////if (myRobot == null)
            //    ////{
            //    ////    myRobot = new PlayerRobot();
            //    ////    myRobot.ID_PlayerOwner = aPlayer.ID;

            //    ////    cc.PlayerRobots.InsertOnSubmit(myRobot);
            //    ////}

            //    ////myRobot.RobotTemplate = robotTemplate;
            //    ////cc.SubmitChanges();

            //    //List<PlayerRobot> betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();// cc.PlayerRobots.Where(x => x.TotalPoints >= myRobot.TotalPoints).ToList();
            //    ////betterRobots = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).ToList();
            //    //if (req.AsRobotID > 0)
            //    //{
            //    //    PlayerRobot myRob = betterRobots.FirstOrDefault(x => x.ID == req.AsRobotID);
            //    //    int indexOfMyRobot = betterRobots.IndexOf(myRob);
            //    //    int page = (int)((float)indexOfMyRobot / 5f);
            //    //    int pageStart = page - 1;
            //    //    int pageEnd = page + 2;

            //    //    if (pageStart < 0) pageStart = 0;

            //    //    int curIndex = pageStart * 5;

            //    //    List<PlayerRobot> tmp = new List<PlayerRobot>();
            //    //    for (int c = curIndex; c < pageEnd * 5; c++)
            //    //    {
            //    //        if (c < betterRobots.Count)
            //    //        {
            //    //            tmp.Add(betterRobots[c]);
            //    //        }
            //    //    }
            //    //    betterRobots = tmp;
            //    //}


            //    //for (int c = 0; c < betterRobots.Count; c++)
            //    //{
            //    //    xRobot xRob = ConvertToXRobot(cc, betterRobots[c], false, req.IncludeScreensot);
            //    //    res.RobotsInMatch.Add(xRob);
            //    //}
            //    res.Success = true;
            //}
            return res;
        }

        public xHint GenerateRandomHint(DataClassesInterfaceDataContext cc)
        {
            xHint ret = new xHint();
            ret.ID = 0;
            ret.TipText = "Tip";
            ret.TipHeading = "Tip ";

            Random aRand = new Random();
            int hintNumber = aRand.Next(0, cc.GameTips.Count());
            List<GameTip> tips = cc.GameTips.Take(hintNumber + 1).ToList();
            if (tips.Count > 0)
            {
                GameTip aTip = tips.OrderByDescending(x => x.ID).FirstOrDefault(); ;
                ret.ID = aTip.ID;
                ret.TipHeading = aTip.TipHeading;
                ret.TipText = aTip.TipText;
            }

            return ret;
        }

        public List<xNews> GenerateNews(DataClassesInterfaceDataContext cc, DateTime lastDate)
        {
            List<xNews> ret = new List<xNews>();
            DateTime actualDate = System.DateTime.UtcNow.AddDays(-30);
            if (lastDate > actualDate)
            {

                if (lastDate <= System.DateTime.UtcNow)
                {
                    actualDate = lastDate;
                }
            }
            List<NewReelEvent> news = cc.NewReelEvents.Where(x => x.DisplayDate > actualDate).OrderBy(x => x.DisplayDate).Take(5).ToList();

            for (int c = 0; c < news.Count; c++)
            {

                xNews anX = new xNews();
                anX.DisplayDate = news[c].DisplayDate;
                anX.ID = news[c].ID;
                anX.MainRobotID = news[c].ID_MainRobot;
                anX.NewsText = news[c].NewsText;
                anX.NewsType = news[c].NewsType;
                ret.Add(anX);
            }

            return ret;
        }

        public void UploadComonentsDEVONLY(DataClassesInterfaceDataContext cc, UploadComponentRequest req, HttpServerUtility server)
        {
            for (int c = 0; c < req.AllComponents.Count; c++)
            {
                if (req.AllComponents[c].Description.Length == 0) req.AllComponents[c].Description = req.AllComponents[c].Name + " is a module for you to use as a component";
                string actual = req.AllComponents[c].Description.Trim();
                if (actual.EndsWith(".") == false) actual += ".";

                // ClassTTS.GenerateComponentName(req.AllComponents[c].ID, req.AllComponents[c].Name, req.AllComponents[c].Description, server);
                ClassTTS.GenerateComponentDescription(req.AllComponents[c].ID, req.AllComponents[c].Name, req.AllComponents[c].Description, server);
            }



        }

    }
    public class RegisterRequest
    {
        public string Displayname;
        public string Email;
        public string Passowrd;

    }
    public class RegisterResult
    {
        public bool Success;
        public string ResultText;
    }
    public class LoginRequest
    {
        public int GameVersion;
        public string Email;
        public string Passowrd;

    }
    public class LoginResult
    {
        public int PlayerID;
        public bool Success;
        public string ResultText;
    }
    public class UpdateUserRequest
    {
        public string Email;
        public string Passowrd;
        public string Displayname;
        public string NewPassowrd;
        public byte[] DisplayImage;
        public string DisplayImageExtension;

    }
    public class UpdateUserResult
    {
        public bool Success;
        public string ResultText;
    }
    public class UploadRobotRequest
    {
        public string Email;
        public string Passowrd;
        public int RobotID;
        public string RobotTemplate;
        public string RobotName;
        public RobotConstructor.RobotTemplate TheTemplate;
        public byte[] ScreenShot;
    }
    public class UploadRobotResult
    {
        public bool Success;
        public string ResultText;
        public int RobotID;

    }
    public class RetrieveListOfChallengeableRobotsRequest
    {
        public string Email;
        public string Passowrd;
        public int AsRobotID;
        public bool IncludeScreensot;

    }
    public class RetrieveListOfChallengeableRobotsResult
    {
        public bool Success;
        public string ResultText;
        public List<xRobot> RobotsInMatch;

    }
    public class xRobot
    {
        public int ID;
        public int OwnerID;
        public string RobotName;
        public string OwnerName;
        public int GamesPlayed;
        public int TotalDeaths;
        public int TotalKills;
        public int WorldRank;
        public long TotalPoints;
        public decimal MoneyWon;
        public byte[] ScreenShot;
        public xRobotTemplate RobotTemplate;


    }
    public class xArena
    {
        public int ID;
        public string ArenaName;
        public string ArenaDescription;
        public string BuiltInLevelID;
        public string ExternalURLocation;
        public long MatchedPlayedHere;
        public string AssetBundleName;


    }
    public class UploadComponentRequest
    {
        public string Email;
        public string Passowrd;
        public List<xComponent> AllComponents;

    }
    public class xComponent
    {
        public string ID;
        public string Name;
        public string Description;
        public byte[] ScreenshotImage;
        public byte[] TTSName;


    }
    public class AddFundsSteamWalletRequest
    {
        public string Email;
        public string Passowrd;
        public decimal UsdAmount;
        

    }

    public class AddFundsSteamWalletChoiceRequest
    {
        public string Email;
        public string Passowrd;
        public ulong OrderID;
        public ulong ConfirmationCode;

    }
    [System.Serializable]
    public class xHint
    {
        public int ID;
        public string TipText;
        public string TipHeading;


    }
    [System.Serializable]
    public class xNews
    {
        public long ID;
        public System.DateTime DisplayDate;
        public int MainRobotID;
        public int NewsType;
        public string NewsText;


    }
    public class xRobotTemplate
    {
        public RobotConstructor.RobotTemplate FullTemplate;

    }
    public class xRobotMatchResult
    {
        public int RobotID;
        public int BulletsFired;
        public int BulletsHit;
        public float RamDamage;
        public float Health;
        public float TotalPoints;
        public int TotalKills;
        public float MinutesSurvived;
        public bool IsDead;

        public float DamageReceived;
    }
    public class CreateRobotMatchRequest
    {
        public string Email;
        public string Passowrd;
        public List<int> RobotIDs;
        public int ExtraRobotToInclude;

    }
    public class CreateRobotMatchResult
    {
        public bool Success;
        public string ResultText;
        public List<xRobot> RobotsInMatch;
        public long MatchID;
        public xArena TheArena;
        public float PrizeMoney;
    }

    public class SubmitRobotMatchRequest
    {
        public string Email;
        public string Passowrd;
        public long MatchID;
        public int ArenaID;
        public List<xRobotMatchResult> RobotsInMatch;

    }
    public class SubmitRobotMatchResult
    {
        public bool Success;
        public string ResultText;

    }
    public class SubmitVideohRequest
    {
        public string Email;
        public string Passowrd;
        public long MatchID;
        public byte[] VideoData;

    }
    public class SubmitVideohResult
    {
        public bool Success;
        public string ResultText;

    }
    public class SendReceiveDiscussionRequest
    {
        public string Email;
        public string Passowrd;
        public DateTime LastUtcUpdateDate;
        public string SendMessage;

    }
    public class SendReceiveDiscussionResult
    {
        public bool Success;
        public string ResultText;
        public List<xDiscussion> AllDiscussions;
    }
    public class xDiscussion
    {
        public long ID;
        public int PlayerSenderID;
        public string PlayerSenderName;
        public DateTime UtcDate;
        public string Message;

    }

    public class UpdateLeagueLeaderboardRequest
    {
        public string Email;
        public string Passowrd;
        public bool IncludeScreensot;

    }
    public class UpdateLeagueLeaderboardResult
    {
        public bool Success;
        public string ResultText;
        public int ViewingAsPlayerID;
        public List<xLeagueLeaderboard> TopAssemblers;
        public List<xLeagueLeaderboard> TopRobots;
        public List<xLeagueLeaderboard> TopRobotMostKills;
        public List<xLeagueLeaderboard> TopRobotKillsToDeathRatio;
        public List<xLeagueLeaderboard> TopRobotCodeToScoreRatio;
        public List<xLeagueLeaderboard> TopRobotWinPercentRatio;
        public List<xLeagueLeaderboard> TopRobotMostAccurateHitToMissRatio;
        public List<xLeagueLeaderboard> TopRobotMultiMatchWinRatio;

    }
    public class xLeagueLeaderboard
    {
        public long ID;
        public int PlayerID;
        public int RobotID;
        public string RobotName;
        public string PlayerName;
        public long Rank;
        public long TotalPoints;
        public long TotalKills;
        public float RatioKillToDeath;
        public float RatioCodeToScore;
        public float RatioWinToLoss;
        public float RatioHitsToMiss;
        public float RatioMultiRobotMatchWinsToLoss;
        public byte[] ScreenshotData;


    }


    public class RetrieveFinancialsRequest
    {
        public string Email;
        public string Passowrd;

    }
    public class RetrieveFinancialsResult
    {
        public bool Success;
        public string ResultText;
        public int ViewingAsPlayerID;
        public List<xFinanceRow> LastFinances;


    }
    public class AddFundsSteamWalletResult
    {
        public bool Success;
        public string ResultText;
       // public int ViewingAsPlayerID;
       // public List<xFinanceRow> LastFinances;


    }
    public class xFinanceRow
    {
        public long ID;
        public int PlayerID;
        public int RobotID;
        public DateTime EventDate;
        public long MatchID;
        public float MoneyAmount;
        public string Reason;
        public int PlayerInitiatorID;


    }


    public class RobotConstructor
    {
        [Serializable]
        public class RobotTemplate
        {
            //  public string ChasisID;
            public List<DataComponentType> ModuleList;
            public int LeagueID;
            public string LeagueOwnerName;
            public string OwnerID;
            public int LeagueOwnerID;
            public string RobotID;
            public string FileName;
            public string RobotName;
            public string TeamPassword;

            public string MainFunctionCode;
            public float ColorR;
            public float ColorG;
            public float ColorB;

            public float TotalWins;
            public float TotalMatches;
            public float TotalKills;
            public float TotalDeaths;
            public byte[] ScreenShot;
            public float CalculateTotalWeight;
            public float CalculateMaxSpeed;
            public float CalculateSqrMeterSize;
            public float CalculateLinesOfCode;
            public float CalculateCodeProcessedPerSecond;
            public float CalculateCodeTotalLeagueScore;


            public float CalculateTotalWeightValue()
            {
                float TotalKG = 9999;

                return TotalKG;

            }
            public float CalculateMaxSpeedValue()
            {
                float TotalKmpH = 9999;

                return TotalKmpH;

            }
            public float CalculateSqrMeterSizeValue()
            {
                float TotalKmpH = 9999;

                return TotalKmpH;

            }
            public float CalculateLinesOfCodeValue()
            {
                float TotalKmpH = 0f;// UnityEngine.Random.Range(1, 200);

                return TotalKmpH;

            }
            public float CalculateCodeProcessedPerSecondValue()
            {
                float TotalKmpH = 0f;// UnityEngine.Random.Range(1, 200);

                return TotalKmpH;

            }
            public float CalculateCodeTotalLeagueScoreValue()
            {
                float TotalKmpH = 0f;// UnityEngine.Random.Range(1, 200);

                return TotalKmpH;

            }
            public static void SaveToFile(RobotTemplate aRobot, string fileName)
            {

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, aRobot);
                stream.Close();
            }
            public static RobotTemplate LoadFromFile(string fileName)
            {
                RobotTemplate ret = null;

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                ret = (RobotTemplate)formatter.Deserialize(stream);
                stream.Close();
                return ret;
            }
            public static byte[] ConvertToBytes(RobotTemplate aRobot)
            {

                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, aRobot);
                //byte[] allBytes = new byte[(int)stream.Length];
                //stream.Read(allBytes, 0, (int)stream.Length);
                //stream.Close();
                byte[] allBytes = stream.GetBuffer();
                return allBytes;
            }

            public static RobotTemplate ConvertFromBytes(byte[] allData)
            {
                RobotTemplate ret = null;

                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(allData);

                //Stream stream = new FileStream(fileName,
                //                          FileMode.Open,
                //                          FileAccess.Read,
                //                          FileShare.Read);
                ret = (RobotTemplate)formatter.Deserialize(stream);

                stream.Close();
                return ret;
            }
        }
        [Serializable]
        public class DataComponentType
        {
            public string ComponentID;
            public DataIoHandler MyIoMap;
            public List<string> ParentAcceptorIDs;
            public string IoNumberCustomMap;


        }
        [Serializable]
        public class DataModuleAcceptor
        {
            public string AcceptorID;
            public string MapToComponentID;
        }
        [Serializable]
        public class DataIoHandler
        {
            public string IoDeviceID;
            public string MapToInterrupNumber;

        }

    }
}