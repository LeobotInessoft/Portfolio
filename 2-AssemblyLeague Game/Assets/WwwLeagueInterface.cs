using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
public class WwwLeagueInterface : MonoBehaviour
{

    #region Config
    /// <summary>
    /// Config
    /// </summary>
    
    public static string BaseUrl = "http://assemblyleague.com/";
   

  
    public static xActionRequest.EnumGamePlatformType PlatformForRelease = xActionRequest.EnumGamePlatformType.SteamPC;
    public static int GameVersion = 2;
    public static int LoggedInUserID = 0;
    public static string LoggedInEmail = "1";
    public static string LoggedInPassword = "2";
    public static float LoggedInFundsTotal = 0;
    public bool IsBusy = false;
    public static LoginResult LastLoginResult;
    public delegate void GetXData_Received(XData aData);
    #endregion
    public event GetXData_Received On_GetXData_Received;
    public event GetXData_Received On_GetXData_Received_Register;
    public event GetXData_Received On_GetXData_Received_Login;
    public event GetXData_Received On_GetXData_Received_UploadRobot;
    public event GetXData_Received On_GetXData_Received_UpdateUser;
    public event GetXData_Received On_GetXData_Received_RetrieveListOfChallengeableRobots;
    public event GetXData_Received On_GetXData_Received_CreateRobotMatch;
    public event GetXData_Received On_GetXData_Received_SubmitRobotMatch;
    public event GetXData_Received On_GetXData_Received_SubmitVideo;
    public event GetXData_Received On_GetXData_Received_SendReceiveDiscussion;
    public event GetXData_Received On_GetXData_Received_UpdateLeagueLeaderboard;
    public event GetXData_Received On_GetXData_FundsUpdate;
    public event GetXData_Received On_GetXData_RetrieveFinancials;

    // Use this for initialization
    public static WwwLeagueInterface PublicAccess;
    public float Progess;

    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        IsBusy = false;

    }
    bool isSteamReady = false;
    public  void SteamIsReady()
    {
        isSteamReady = true;
     
    }
    public   void HandleDis2(MicroTxnAuthorizationResponse_t tx)
    {
        if(tx.m_bAuthorized==1)
        {
            SteamWalletPayConfirm(tx.m_ulOrderID);
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        if (www != null)
        {
            Progess = www.progress;
        }
        if (LastLoginResult == null)
        {
            LastLoginResult = new LoginResult();
        }
      
    }
    public void SteamLoginForce()
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.LoginAccount;
        act.Login = new LoginRequest();
        act.Login.Email = "";
        act.Login.Passowrd = "";
        act.Login.GameVersion = GameVersion;

        act.Register = new RegisterRequest();
        act.Register.Email = "";
        act.Register.Passowrd = "";
        act.Register.Displayname = SteamLogic.PublicAccess.PersonName;
        StartCoroutine(GetAllData(act));
    }
    public void Login()
    {
        if (PlatformForRelease == xActionRequest.EnumGamePlatformType.SteamPC)
        {

            SteamLoginForce();
        }
        else
        {
            xActionRequest act = new xActionRequest();
            act.ActionType = xActionRequest.xAction.LoginAccount;
            act.Login = new LoginRequest();
            act.Login.Email = LoggedInEmail;
            act.Login.Passowrd = LoggedInPassword;
            act.Login.GameVersion = GameVersion;
            StartCoroutine(GetAllData(act));
        }
    }
    public void Register(string DisplayName)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.RegisterAccount;
        act.Register = new RegisterRequest();
        act.Register.Email = LoggedInEmail;
        act.Register.Passowrd = LoggedInPassword;
        act.Register.Displayname = DisplayName;
        if (WwwLeagueInterface.PlatformForRelease == xActionRequest.EnumGamePlatformType.SteamPC)
        {
            act.Register.Displayname = SteamLogic.PublicAccess.PersonName;
        }
        StartCoroutine(GetAllData(act));

    }
    public void RetrieveListOfChallengeableRobots(int asRobotID)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.RetrieveListOfRobots;
        act.RetrieveListOfChallengeableRobots = new RetrieveListOfChallengeableRobotsRequest();
        act.RetrieveListOfChallengeableRobots.Email = LoggedInEmail;
        act.RetrieveListOfChallengeableRobots.Passowrd = LoggedInPassword;
        act.RetrieveListOfChallengeableRobots.AsRobotID = asRobotID;
        act.RetrieveListOfChallengeableRobots.IncludeScreensot = RobotOwnerLookup.PublicAccess.GetPlayerOptions().RetrieveRobotImages;

        StartCoroutine(GetAllData(act));
    }
    public void SubmitRobotMatch(long matchID, List<xRobotMatchResult> robots)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.SubmitRobotMatch;
        act.SubmitRobotMatch = new SubmitRobotMatchRequest();
        act.SubmitRobotMatch.Email = LoggedInEmail;
        act.SubmitRobotMatch.Passowrd = LoggedInPassword;
        act.SubmitRobotMatch.MatchID = matchID;
        act.SubmitRobotMatch.RobotsInMatch = robots;
        StartCoroutine(GetAllData(act));
    }
    public void CreateRobotMatch(List<int> robots, int extraRobot)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.CreateRobotMatch;
        act.CreateRobotMatch = new CreateRobotMatchRequest();
        act.CreateRobotMatch.Email = LoggedInEmail;
        act.CreateRobotMatch.Passowrd = LoggedInPassword;
        act.CreateRobotMatch.RobotIDs = robots;
        act.CreateRobotMatch.ExtraRobotToInclude = extraRobot;
        StartCoroutine(GetAllData(act));
    }
    public void UpdateUser(byte[] DisplayImage, string DisplayImageExtension, string Displayname, string NewPassowrd)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.UpdateAccount;
        act.UpdateUser = new UpdateUserRequest();
        act.UpdateUser.Email = LoggedInEmail;
        act.UpdateUser.Passowrd = LoggedInPassword;
        act.UpdateUser.DisplayImage = DisplayImage;
        act.UpdateUser.DisplayImageExtension = DisplayImageExtension;
        act.UpdateUser.Displayname = Displayname;
        act.UpdateUser.NewPassowrd = NewPassowrd;


        StartCoroutine(GetAllData(act));
    }
    public void UploadRobot(RobotConstructor.RobotTemplate aTemplate, byte[] screenshotOfRobot)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.UploadRobot;
        act.UploadRobot = new UploadRobotRequest();
        act.UploadRobot.Email = LoggedInEmail;
        act.UploadRobot.Passowrd = LoggedInPassword;
        act.UploadRobot.RobotID = aTemplate.LeagueID;
        act.UploadRobot.TheTemplate = aTemplate;
        act.UploadRobot.ScreenShot = screenshotOfRobot;
        act.UploadRobot.RobotName = aTemplate.RobotName;
        //  act.UploadRobot.RobotTemplate= aTemplate.
        StartCoroutine(GetAllData(act));
    }
    public void UploadComponent(List<ComponentType> components)
    {
         xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.UploadComponents;
        act.UploadComponent = new UploadComponentRequest();
        act.UploadComponent.Email = LoggedInEmail;
        act.UploadComponent.Passowrd = LoggedInPassword;
        act.UploadComponent.AllComponents = new List<xComponent>();
        for (int c = 0; c < components.Count; c++)
        {
            xComponent aCom = new xComponent();
            aCom.Description = components[c].ShortDescription + "\n";
            if (components[c].SpeechDescription.Length > 0)
            {
                aCom.Description = components[c].SpeechDescription + "\n";
            }
            aCom.Name = components[c].DeviceName;
            aCom.ID = components[c].UniqueDeviceID;
            aCom.ScreenshotImage = new byte[0];
            aCom.TTSName = new byte[0];
            act.UploadComponent.AllComponents.Add(aCom);
        }
        //  act.UploadRobot.RobotTemplate= aTemplate.
        StartCoroutine(GetAllData(act));
    }
    public void UploadMatchVideo(long matchID, byte[] video)
    {
         xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.SubmitVideo;
        act.SubmitVideo = new SubmitVideohRequest();
        act.SubmitVideo.Email = LoggedInEmail;
        act.SubmitVideo.Passowrd = LoggedInPassword;
        act.SubmitVideo.MatchID = matchID;
        act.SubmitVideo.VideoData = video;
        //  act.UploadRobot.RobotTemplate= aTemplate.
        StartCoroutine(GetAllData(act));
    }
    public void SendReceiveDiscussion(string messageToSend, System.DateTime lastReceiveDate)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.SendReceiveDiscussion;
        act.SendReceiveDiscussion = new SendReceiveDiscussionRequest();
        act.SendReceiveDiscussion.Email = LoggedInEmail;
        act.SendReceiveDiscussion.Passowrd = LoggedInPassword;
        act.SendReceiveDiscussion.SendMessage = messageToSend;
        act.SendReceiveDiscussion.LastUtcUpdateDate = lastReceiveDate;
        //  act.UploadRobot.RobotTemplate= aTemplate.
        StartCoroutine(GetAllData(act));
    }
    public void UpdateLeagueLeaderboard()
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.UpdateLeagueLeaderboard;
        act.UpdateLeagueLeaderboard = new UpdateLeagueLeaderboardRequest();
        act.UpdateLeagueLeaderboard.Email = LoggedInEmail;
        act.UpdateLeagueLeaderboard.Passowrd = LoggedInPassword;
        print("OPPPPP " + RobotOwnerLookup.PublicAccess.GetPlayerOptions());
        act.UpdateLeagueLeaderboard.IncludeScreensot = RobotOwnerLookup.PublicAccess.GetPlayerOptions().RetrieveRobotImages;
        //  act.UploadRobot.RobotTemplate= aTemplate.
        StartCoroutine(GetAllData(act));
    }
    public void RetrieveFinancials()
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.RetrieveFinancials;
        act.RetrieveFinancials = new RetrieveFinancialsRequest();
        act.RetrieveFinancials.Email = LoggedInEmail;
        act.RetrieveFinancials.Passowrd = LoggedInPassword;
        //  act.UploadRobot.RobotTemplate= aTemplate.
        StartCoroutine(GetAllData(act));
    }
    public void SteamWalletPayInit(decimal USDAmounbt)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.AddFundsSteamWallet;
        act.AddFundsSteamWallet = new AddFundsSteamWalletRequest();
        act.AddFundsSteamWallet.Email = LoggedInEmail;
        act.AddFundsSteamWallet.Passowrd = LoggedInPassword;
        act.AddFundsSteamWallet.UsdAmount = USDAmounbt;

        StartCoroutine(GetAllData(act));
        Steamworks.MicroTxnAuthorizationResponse_t xx = new Steamworks.MicroTxnAuthorizationResponse_t();
      
    }
    public  void SteamWalletPayConfirm(ulong orderID)
    {
        xActionRequest act = new xActionRequest();
        act.ActionType = xActionRequest.xAction.AddFundsSteamWalletChoice;
        act.AddFundsSteamWalletChoice = new AddFundsSteamWalletChoiceRequest();
        act.AddFundsSteamWalletChoice.Email = LoggedInEmail;
        act.AddFundsSteamWalletChoice.Passowrd = LoggedInPassword;
        act.AddFundsSteamWalletChoice.OrderID = orderID;
        StartCoroutine(GetAllData(act));
       
    }
    public WWW www;
    public IEnumerator GetAllData(xActionRequest action)
    {
        if (IsBusy == false)
        {
            IsBusy = true;
            System.DateTime lastNewsDate = System.DateTime.UtcNow.AddDays(-25);
            if (RobotOwnerLookup.PublicAccess != null)
            {
                List<xNews> allNews = RobotOwnerLookup.PublicAccess.GetNews();
                if (allNews.Count > 0)
                {
                    allNews = allNews.Where(x => x.DisplayDate <= System.DateTime.UtcNow).ToList();
                    if (allNews.Count > 0)
                    {
                        lastNewsDate = allNews.OrderByDescending(x => x.DisplayDate).FirstOrDefault().DisplayDate;
                    }
                }

            }
            action.LastNewsUpdateDate = lastNewsDate;
            action.PlatformType = PlatformForRelease;
            if (action.PlatformType == xActionRequest.EnumGamePlatformType.SteamPC)
            {
                action.SteamTicket = SteamLogic.PublicAccess.TicketString;
                action.SteamUserID = SteamLogic.PublicAccess.ID.m_SteamID.ToString();
            }
            
            Application.backgroundLoadingPriority = ThreadPriority.High;
            string url = BaseUrl+"GameInterface.aspx";
            WWWForm aForm = new WWWForm();

            try
            {
                aForm.AddBinaryData("xActionRequest", GetBytes(xActionRequest.SerializeToText(action)));
            }
            catch
            {
                IsBusy = false;
                print("ERRRRRRRRRRRRROR");
            }
            www = new WWW(url, aForm);
            www.threadPriority = ThreadPriority.High;

        
            yield return www;
            IsBusy = false;
             XData aData = XData.DeserializeText(www.text);
           
            if (On_GetXData_Received != null)
            {
                On_GetXData_Received(aData);
            }

            if (aData != null)
            {
                if (aData.CreateRobotMatch != null && On_GetXData_Received_CreateRobotMatch != null)
                {
                    On_GetXData_Received_CreateRobotMatch(aData);
                }
                if (aData.Login != null && On_GetXData_Received_Login != null)
                {
                    On_GetXData_Received_Login(aData);
                }
                if (aData.Register != null && On_GetXData_Received_Register != null)
                {
                    On_GetXData_Received_Register(aData);
                }
                if (aData.RetrieveListOfChallengeableRobots != null && On_GetXData_Received_RetrieveListOfChallengeableRobots != null)
                {
                    On_GetXData_Received_RetrieveListOfChallengeableRobots(aData);
                }
                if (aData.SubmitRobotMatch != null && On_GetXData_Received_SubmitRobotMatch != null)
                {
                    On_GetXData_Received_SubmitRobotMatch(aData);
                }
                if (aData.UpdateUser != null && On_GetXData_Received_UpdateUser != null)
                {
                    On_GetXData_Received_UpdateUser(aData);
                }
                if (aData.UploadRobot != null && On_GetXData_Received_UploadRobot != null)
                {
                    On_GetXData_Received_UploadRobot(aData);
                }
                if (aData.SubmitVideo != null && On_GetXData_Received_SubmitVideo != null)
                {
                    On_GetXData_Received_SubmitVideo(aData);
                }
                if (aData.SendReceiveDiscussion != null && On_GetXData_Received_SendReceiveDiscussion != null)
                {
                    On_GetXData_Received_SendReceiveDiscussion(aData);
                }

                if (aData.UpdateLeagueLeaderboard != null && On_GetXData_Received_UpdateLeagueLeaderboard != null)
                {
                    On_GetXData_Received_UpdateLeagueLeaderboard(aData);
                }
                if (aData.RetrieveFinancials != null && On_GetXData_RetrieveFinancials != null)
                {
                    On_GetXData_RetrieveFinancials(aData);
                }
                LoggedInFundsTotal = aData.CurrentPlayerFunds;
                if (On_GetXData_FundsUpdate != null)
                {
                    On_GetXData_FundsUpdate(aData);
                }
                if (aData.RandomHint != null)
                {
                    if (RobotOwnerLookup.PublicAccess != null)
                    {
                        RobotOwnerLookup.PublicAccess.AddHint(aData.RandomHint);
                    }
                }

                if (aData.LatestNews != null)
                {
                    if (RobotOwnerLookup.PublicAccess != null)
                    {
                        for (int c = 0; c < aData.LatestNews.Count; c++)
                        {
                            RobotOwnerLookup.PublicAccess.AddNews(aData.LatestNews[c]);
                        }
                    }
                }
                if (RobotOwnerLookup.PublicAccess != null)
                {
                    RobotOwnerLookup.PublicAccess.SetRoI(aData.RoI);
                }

            }
        }
        else
        {
            yield return 0;
           
        }
    }

    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
    public static string ReplaceTextFormat(string input)
    {
        string ret = "";
        print("Text to Fix: " + input);

        bool IsInTag = false;
        string tempInput = input.Replace("<br>", "\n").Replace("</br>", "\n").Replace("<br/>", "\n");
        for (int c = 0; c < tempInput.Length; c++)
        {
            if (tempInput[c].ToString().ToLower() == "<")
            {
                IsInTag = true;
            }


            if (IsInTag == false)
            {
                ret += tempInput[c].ToString();
            }
            if (tempInput[c].ToString().ToLower() == ">")
            {
                IsInTag = false;
            }

        }


        print("Result Text: " + input);
        return ret;
    }


}
public class xActionRequest
{
    public EnumGamePlatformType PlatformType;
    public string SteamUserID;
    public string SteamTicket;

    public xAction ActionType;
    public RegisterRequest Register;
    public LoginRequest Login;
    public UpdateUserRequest UpdateUser;
    public UploadRobotRequest UploadRobot;
    public RetrieveListOfChallengeableRobotsRequest RetrieveListOfChallengeableRobots;
    public CreateRobotMatchRequest CreateRobotMatch;
    public SubmitRobotMatchRequest SubmitRobotMatch;
    public SubmitVideohRequest SubmitVideo;
    public SendReceiveDiscussionRequest SendReceiveDiscussion;
    public UpdateLeagueLeaderboardRequest UpdateLeagueLeaderboard;
    public RetrieveFinancialsRequest RetrieveFinancials;
    public System.DateTime LastNewsUpdateDate;
    public UploadComponentRequest UploadComponent;
    public AddFundsSteamWalletRequest AddFundsSteamWallet;
    public AddFundsSteamWalletChoiceRequest AddFundsSteamWalletChoice;


    public enum xAction
    {
        None,
        RegisterAccount,
        LoginAccount,
        UpdateAccount,
        UploadRobot,
        RetrieveListOfRobots,
        CreateRobotMatch,
        SubmitRobotMatch,
        SubmitVideo,
        SendReceiveDiscussion,
        UpdateLeagueLeaderboard,
        RetrieveFinancials,
        UploadComponents,
        AddFundsSteamWallet,
        AddFundsSteamWalletChoice,





    }
    public enum EnumGamePlatformType
    {
        SteamPC = 0,
        UWSPC = 1,
    }
    public static string SerializeToText(xActionRequest aProject)
    {
        string FileContent;
        XmlSerializer xsSubmit = new XmlSerializer(typeof(xActionRequest));
        StringWriter sww = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sww);
        xsSubmit.Serialize(writer, aProject);
        var xml = sww.ToString();
        return sww.ToString();
    }
    public static xActionRequest DeserializeText(string text)
    {
        try
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(xActionRequest));
            StringReader sww = new StringReader(text);
            XmlReader writer = XmlReader.Create(sww);
            xActionRequest theProject = (xActionRequest)xsSubmit.Deserialize(writer);
            return theProject;
        }
        catch
        {
            return null;
        }
    }

}
public class XData
{
    public float CurrentPlayerFunds;
    public RegisterResult Register;
    public LoginResult Login;
    public UpdateUserResult UpdateUser;
    public UploadRobotResult UploadRobot;
    public RetrieveListOfChallengeableRobotsResult RetrieveListOfChallengeableRobots;
    public CreateRobotMatchResult CreateRobotMatch;
    public SubmitRobotMatchResult SubmitRobotMatch;
    public SubmitVideohResult SubmitVideo;
    public SendReceiveDiscussionResult SendReceiveDiscussion;
    public UpdateLeagueLeaderboardResult UpdateLeagueLeaderboard;
    public RetrieveFinancialsResult RetrieveFinancials;
    public AddFundsSteamWalletResult AddFundsSteamWallet;
    public xHint RandomHint;
    public List<xNews> LatestNews;
    public decimal RoI;
    
    public static string SerializeToText(XData aProject)
    {
        string FileContent;
        XmlSerializer xsSubmit = new XmlSerializer(typeof(XData));
        StringWriter sww = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sww);
        xsSubmit.Serialize(writer, aProject);
        var xml = sww.ToString();
        return sww.ToString();
    }
    public static XData DeserializeText(string text)
    {
        try
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(XData));
            StringReader sww = new StringReader(text);
            XmlReader writer = XmlReader.Create(sww);
            XData theProject = (XData)xsSubmit.Deserialize(writer);
            return theProject;
        }
        catch
        {
            return null;
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
public class AddFundsSteamWalletRequest
{
    public string Email;
    public string Passowrd;
    public decimal UsdAmount;


}
public class xComponent
{
    public string ID;
    public string Name;
    public string Description;
    public byte[] ScreenshotImage;
    public byte[] TTSName;


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
    public System.DateTime LastUtcUpdateDate;
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
    public System.DateTime UtcDate;
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
   

}
public class xFinanceRow
{
    public long ID;
    public int PlayerID;
    public int RobotID;
    public System.DateTime EventDate;
    public long MatchID;
    public float MoneyAmount;
    public string Reason;
    public int PlayerInitiatorID;


}

