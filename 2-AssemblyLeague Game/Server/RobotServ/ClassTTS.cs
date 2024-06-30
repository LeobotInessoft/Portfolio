using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Amazon.Polly;
using Amazon.Polly.Model;
using System.IO;
using Amazon.Runtime;
namespace RobotServ
{
    public class ClassTTS
    {
       static string amazonID = "[Removed]";
      static string amazonKey = "[Removed]";

        public static void SayIt(string mappedPath, string text)
        {
           
            for (int c = 0; c < 8; c++)
            {
                AmazonPollyConfig aCon = new AmazonPollyConfig();
                aCon.RegionEndpoint = Amazon.RegionEndpoint.EUCentral1;
                AmazonPollyClient pc = new AmazonPollyClient(amazonID, amazonKey, aCon);
                Amazon.Polly.Model.DescribeVoicesResponse voices = pc.DescribeVoices(new Amazon.Polly.Model.DescribeVoicesRequest() { LanguageCode = Amazon.Polly.LanguageCode.EnUS });

                SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest();
                sreq.Text = text;
                sreq.OutputFormat = OutputFormat.Mp3;
                sreq.VoiceId = voices.Voices[c].Id;
                SynthesizeSpeechResponse sres = pc.SynthesizeSpeech(sreq);
                Random aRand = new Random();
                string aRandTxt = aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "";
                string fileName = mappedPath + "\\" + aRandTxt + ".mp3";
                using (var fileStream = File.Create(fileName))
                {
                    sres.AudioStream.CopyTo(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                }
            //    System.Diagnostics.Process.Start(fileName);
            }
        }

        public static byte[] GenerateMatchIntro(DataClassesInterfaceDataContext cc, Match aMatch, HttpServerUtility server)
        {
            byte[] ret = new byte[0];
            string robotNamesFolder = "TTS\\MatchIntro";

            string fileName = server.MapPath(robotNamesFolder) + "\\" + aMatch.ID + ".mp3";

            string matchText = "";
            string arenaTest = "at an Unknown Location";
            MatchArena aN = cc.MatchArenas.First(x => x.ID == aMatch.ID_Arena);
            if (aN != null)
            {
                arenaTest = "at the "+aN.ArenaName+" arena ";
            }
            matchText += "Match " + aMatch.ID + " will take place " + arenaTest + " and will be between: ";
            List<RobotMatchResult> rows = cc.RobotMatchResults.Where(x => x.ID_Match == aMatch.ID).ToList();
            List<PlayerRobot> robotsInMatch = new List<PlayerRobot>();
            List<Player> playersInMatch = new List<Player>();
            float totalPoints = 0;
            for (int c = 0; c < rows.Count; c++)
            {
                PlayerRobot aRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == rows[c].ID_Robot);
                if (aRobot != null)
                {
                    totalPoints += aRobot.TotalPoints;
                    Player aPlayer = cc.Players.FirstOrDefault(x => x.ID == aRobot.ID_PlayerOwner);
                    if (aPlayer != null)
                    {
                        if (playersInMatch.Contains(aPlayer) == false)
                        {
                            playersInMatch.Add(aPlayer);
                        }
                    }
                    robotsInMatch.Add(aRobot);

                    if (c > 0)
                    {
                        if (c == rows.Count - 1)
                        {
                            matchText += " and ";

                        }
                        else
                        {
                            matchText += ", ";
                        }
                    }


                     matchText += aRobot.RobotName + "";
                }
            }
            totalPoints += 1;
            matchText += ".";
            matchText += " The Total prize winnings for this match is $" + aMatch.PrizeMoney.ToString("f0") + ".";
            PlayerRobot mostLikelyWinner = robotsInMatch.OrderByDescending(x => ((x.TotalKills + 1) - (x.TotalDeaths + 1)+0.1f) / (x.GamesPlayed + 1)).FirstOrDefault();
            PlayerRobot leastLikelyWinner = robotsInMatch.OrderBy(x => ((x.TotalKills + 1) - (x.TotalDeaths + 1) + 0.1f) / (x.GamesPlayed + 1)).FirstOrDefault();
            if (playersInMatch.Count > 0)
            {
                playersInMatch = playersInMatch.OrderBy(x => x.DisplayName).ToList();
            }
            matchText += " The expected winner for this match is " + mostLikelyWinner.RobotName + " while the underdog is " + leastLikelyWinner.RobotName + ".";
            ret = SayItToFileAndBytes(fileName, matchText, 4);

            return ret;
        }
        public static byte[] GenerateComponentName(string id, string compName, string compDescription, HttpServerUtility server)
        {
            byte[] ret = new byte[0];
            string robotNamesFolder = "TTS\\ComponentNames";

            string fileName = server.MapPath(robotNamesFolder) + "\\" + id + ".mp3";
            if (compName.Length > 0)
            {
                ret = SayItToFileAndBytes(fileName, compName);
            }
            return ret;
        }
        public static byte[] GenerateComponentDescription(string id, string compName, string compDescription, HttpServerUtility server)
        {
            byte[] ret = new byte[0];
            string robotNamesFolder = "TTS\\ComponentDescriptions";

            string fileName = server.MapPath(robotNamesFolder) + "\\" + id + "desc.mp3";
            if (compDescription.Length > 0)
            {


                ret = SayItToFileAndBytes(fileName, compDescription);
            }

            return ret;
        }
        public static byte[] GenerateRobotName(DataClassesInterfaceDataContext cc, PlayerRobot aRobot, HttpServerUtility server)
        {
            byte[] ret = new byte[0];
            string robotNamesFolder = "TTS\\RobotNames";

            string fileName = server.MapPath(robotNamesFolder) + "\\" + aRobot.ID + ".mp3";
            string fileNameOwned = server.MapPath(robotNamesFolder) + "\\" + aRobot.ID + "Owned.mp3";
            Player aPlayer = cc.Players.FirstOrDefault(x => x.ID == aRobot.ID_PlayerOwner);
            ret = SayItToFileAndBytes(fileNameOwned, aRobot.RobotName + " owned by " + aPlayer.DisplayName);
            ret = SayItToFileAndBytes(fileName, aRobot.RobotName);

            return ret;
        }
        public static byte[] GeneratePlayerName(DataClassesInterfaceDataContext cc, Player aPlayer, HttpServerUtility server)
        {
            byte[] ret = new byte[0];
            string robotNamesFolder = "TTS\\PlayerNames";

            string fileName = server.MapPath(robotNamesFolder) + "\\" + aPlayer.ID + ".mp3";
            ret = SayItToFileAndBytes(fileName, aPlayer.DisplayName);

            return ret;
        }
        //   public byte
        public static void SayItToFile(string fileName, string text)
        {
            AmazonPollyConfig aCon = new AmazonPollyConfig();
            aCon.RegionEndpoint = Amazon.RegionEndpoint.EUCentral1;
            AmazonPollyClient pc = new AmazonPollyClient(amazonID, amazonKey, aCon);
            Amazon.Polly.Model.DescribeVoicesResponse voices = pc.DescribeVoices(new Amazon.Polly.Model.DescribeVoicesRequest() { LanguageCode = Amazon.Polly.LanguageCode.EnUS });

            SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest();
            sreq.Text = text;
            sreq.OutputFormat = OutputFormat.Mp3;
            sreq.VoiceId = voices.Voices[3].Id;
            SynthesizeSpeechResponse sres = pc.SynthesizeSpeech(sreq);
            Random aRand = new Random();
            //string aRandTxt = aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "";
            //    string fileName = mappedPath + "\\" + aRandTxt + ".mp3";
            using (var fileStream = File.Create(fileName))
            {
                sres.AudioStream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
            }

        }
        static AmazonPollyClient pc;
        static Amazon.Polly.Model.DescribeVoicesResponse voices;
        public static byte[] SayItToFileAndBytes(string fileName, string text)
        {
            byte[] retBytes = new byte[0];
            if (text.Length > 0)
            {
                if (pc == null)
                {
                    AmazonPollyConfig aCon = new AmazonPollyConfig();
                    aCon.RegionEndpoint = Amazon.RegionEndpoint.EUCentral1;
                    pc = new AmazonPollyClient(amazonID, amazonKey, aCon);
                    voices = pc.DescribeVoices(new Amazon.Polly.Model.DescribeVoicesRequest() { LanguageCode = Amazon.Polly.LanguageCode.EnUS });

                }
                SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest();
                sreq.Text = text;
                sreq.OutputFormat = OutputFormat.Mp3;
                sreq.VoiceId = voices.Voices[3].Id;
                SynthesizeSpeechResponse sres = pc.SynthesizeSpeech(sreq);
                Random aRand = new Random();
                // string aRandTxt = aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "";
                //    string fileName = mappedPath + "\\" + aRandTxt + ".mp3";

                using (var fileStream = File.Create(fileName))
                {
                    sres.AudioStream.CopyTo(fileStream);
                    retBytes = new byte[fileStream.Length];

                    fileStream.Write(retBytes, 0, retBytes.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            return retBytes;
        }
        public static byte[] SayItToFileAndBytes(string fileName, string text, int voiceID)
        {
            byte[] retBytes = new byte[0];
            if (text.Length > 0)
            {
                if (pc == null)
                {
                    AmazonPollyConfig aCon = new AmazonPollyConfig();
                    aCon.RegionEndpoint = Amazon.RegionEndpoint.EUCentral1;
                    pc = new AmazonPollyClient(amazonID, amazonKey, aCon);
                    voices = pc.DescribeVoices(new Amazon.Polly.Model.DescribeVoicesRequest() { LanguageCode = Amazon.Polly.LanguageCode.EnUS });

                }
                SynthesizeSpeechRequest sreq = new SynthesizeSpeechRequest();
                sreq.Text = text;
                sreq.OutputFormat = OutputFormat.Mp3;
                sreq.VoiceId = voices.Voices[voiceID].Id;

                SynthesizeSpeechResponse sres = pc.SynthesizeSpeech(sreq);
                Random aRand = new Random();
                // string aRandTxt = aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "" + aRand.Next(0, 10) + "";
                //    string fileName = mappedPath + "\\" + aRandTxt + ".mp3";

                using (var fileStream = File.Create(fileName))
                {
                    sres.AudioStream.CopyTo(fileStream);
                    retBytes = new byte[fileStream.Length];

                    fileStream.Write(retBytes, 0, retBytes.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            return retBytes;
        }

    }
}