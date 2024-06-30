using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;
namespace RobotServ
{
    public partial class GameInterface : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XData theData = new XData();

             Response.Clear();
            try
            {
                xActionRequest anAction = new xActionRequest();
                 try
                {
                    byte[] byteData = new byte[Request.Files["xActionRequest"].ContentLength];
                    Request.Files["xActionRequest"].InputStream.Read(byteData, 0, byteData.Length);
                    anAction = xActionRequest.DeserializeText(GetString(byteData));

                }
                catch
                {
                    Response.Write("err");
                }

                bool isSteamValid = ConfirmSteamTicket(anAction.PlatformType, anAction.SteamTicket, anAction.SteamUserID);
                if (isSteamValid)
                {

                    DataClassesInterfaceDataContext currentContext = new DataClassesInterfaceDataContext();
                    ClassRobotDB robotDB = new ClassRobotDB();

                    switch (anAction.ActionType)
                    {
                        case xActionRequest.xAction.UploadComponents:
                            {
                                robotDB.UploadComonentsDEVONLY(currentContext, anAction.UploadComponent, Server);
                                break;
                            }
                        case xActionRequest.xAction.CreateRobotMatch:
                            {
                                theData.CreateRobotMatch = robotDB.CreateRobotMatch(currentContext, anAction.CreateRobotMatch, Server, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.CreateRobotMatch.Email, anAction.CreateRobotMatch.Passowrd, anAction.SteamUserID);
                                break;
                            }
                        case xActionRequest.xAction.LoginAccount:
                            {
                                theData.Login = robotDB.LoginUser(currentContext, anAction.Login, anAction.SteamUserID);
                                if (theData.Login.Success == false)
                                {
                                    if (anAction.Register != null)
                                    {
                                        robotDB.RegisterUser(currentContext, anAction.Register, anAction.SteamUserID);
                                        theData.Login = robotDB.LoginUser(currentContext, anAction.Login, anAction.SteamUserID);
                                        theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.Login.Email, anAction.Login.Passowrd, anAction.SteamUserID);

                                    }
                                }
                                else
                                {
                                    theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.Login.Email, anAction.Login.Passowrd, anAction.SteamUserID);
                                }
                                break;
                            }
                        case xActionRequest.xAction.RegisterAccount:
                            {
                                theData.Register = robotDB.RegisterUser(currentContext, anAction.Register, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.Register.Email, anAction.Register.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.RetrieveListOfRobots:
                            {
                                theData.RetrieveListOfChallengeableRobots = robotDB.RetrieveListOfChallengableRobots(currentContext, anAction.RetrieveListOfChallengeableRobots, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.RetrieveListOfChallengeableRobots.Email, anAction.RetrieveListOfChallengeableRobots.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.UpdateAccount:
                            {
                                theData.UpdateUser = robotDB.UpdateUser(currentContext, anAction.UpdateUser, Server, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.UpdateUser.Email, anAction.UpdateUser.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.UploadRobot:
                            {
                                theData.UploadRobot = robotDB.UploadRobot(currentContext, anAction.UploadRobot, Server.MapPath("Robots"), Server, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.UploadRobot.Email, anAction.UploadRobot.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.SubmitRobotMatch:
                            {
                                theData.SubmitRobotMatch = robotDB.SubmitRobotMatch(currentContext, anAction.SubmitRobotMatch, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.SubmitRobotMatch.Email, anAction.SubmitRobotMatch.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.SubmitVideo:
                            {
                                theData.SubmitVideo = robotDB.SubmitVideo(currentContext, anAction.SubmitVideo, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.SubmitVideo.Email, anAction.SubmitVideo.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.SendReceiveDiscussion:
                            {
                                theData.SendReceiveDiscussion = robotDB.SendReceiveDiscussion(currentContext, anAction.SendReceiveDiscussion, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.SendReceiveDiscussion.Email, anAction.SendReceiveDiscussion.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.UpdateLeagueLeaderboard:
                            {
                                theData.UpdateLeagueLeaderboard = robotDB.UpdateLeagueLeaderboards(currentContext, anAction.UpdateLeagueLeaderboard, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.UpdateLeagueLeaderboard.Email, anAction.UpdateLeagueLeaderboard.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.RetrieveFinancials:
                            {
                                theData.RetrieveFinancials = robotDB.RetrieveFinancials(currentContext, anAction.RetrieveFinancials, anAction.SteamUserID);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.RetrieveFinancials.Email, anAction.RetrieveFinancials.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.AddFundsSteamWallet:
                            {
                                SteamPay(currentContext, robotDB.GetPlayerFromDB(currentContext, anAction.AddFundsSteamWallet.Email, anAction.AddFundsSteamWallet.Passowrd, anAction.SteamUserID), anAction.PlatformType, anAction.SteamTicket, anAction.SteamUserID, anAction.AddFundsSteamWallet.UsdAmount);
                                theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.AddFundsSteamWallet.Email, anAction.AddFundsSteamWallet.Passowrd, anAction.SteamUserID);

                                break;
                            }
                        case xActionRequest.xAction.AddFundsSteamWalletChoice:
                            {
                                bool isAllowed = SteamPayUserConfirmation(anAction.SteamTicket, anAction.SteamUserID, anAction.AddFundsSteamWalletChoice.OrderID, anAction.AddFundsSteamWalletChoice.ConfirmationCode);
                                if (isAllowed)
                                {
                                    Player aPlayer = robotDB.GetPlayerFromDB(currentContext, anAction.AddFundsSteamWalletChoice.Email, anAction.AddFundsSteamWalletChoice.Passowrd, anAction.SteamUserID);
                                    GeneralSettingRow roi2 = currentContext.GeneralSettingRows.FirstOrDefault(x => x.SettingName.ToLower() == "roi");
                                    decimal roiValue2 = Convert.ToDecimal(roi2.SettingValue);

                                    SteamWalletPaymentRecord aRecord = currentContext.SteamWalletPaymentRecords.FirstOrDefault(x => x.ID_Player == aPlayer.ID && x.ID == (long)anAction.AddFundsSteamWalletChoice.OrderID && x.IsComplete == false);
                                    if (aRecord != null)
                                    {
                                        aRecord.WasSuccess = true;
                                        aRecord.IsComplete = true;
                                         aPlayer.Money += aRecord.USDAmount * roiValue2;

                                        GameFinancialRow aFinanceStat = new GameFinancialRow();
                                        aFinanceStat.EventDate = System.DateTime.UtcNow;
                                        aFinanceStat.ID_Match = 0;
                                        aFinanceStat.ID_Player = aPlayer.ID;
                                        aFinanceStat.ID_PlayerInitiater = aPlayer.ID;
                                        aFinanceStat.ID_Robot = 0;
                                        aFinanceStat.MoneyAmount = aRecord.USDAmount * roiValue2;
                                        string reason = "";
                                        reason = "Funded your account. Thank you for your support! You rock!";
                                        aFinanceStat.Reason = reason;
                                        currentContext.GameFinancialRows.InsertOnSubmit(aFinanceStat);


                                        currentContext.SubmitChanges();
                                      }

                                }
                                 theData.CurrentPlayerFunds = robotDB.GetPlayerFunds(currentContext, anAction.AddFundsSteamWalletChoice.Email, anAction.AddFundsSteamWalletChoice.Passowrd, anAction.SteamUserID);

                                break;
                            }
                    }


                    theData.RandomHint = robotDB.GenerateRandomHint(currentContext);
                    theData.LatestNews = robotDB.GenerateNews(currentContext, anAction.LastNewsUpdateDate);
                    GeneralSettingRow roi = currentContext.GeneralSettingRows.FirstOrDefault(x => x.SettingName.ToLower() == "roi");
                    decimal roiValue = Convert.ToDecimal(roi.SettingValue);
                    theData.RoI = roiValue;

                    string ret = XData.SerializeToText(theData);

                    Response.Write(ret);
                }
            }
            catch (Exception exc)
            {
                try
                {
                    string ret = XData.SerializeToText(theData);
                    Response.Write(exc.ToString());
                }
                catch
                {
                    Response.Write(exc.ToString());


                }


                //  Response.Write(ret);


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
        private bool SteamPayUserConfirmation(string userID, string steamTicket, ulong orderID, ulong result)
        {
            bool isOK = false;
              {
                using (WebClient wc = new WebClient())
                {

                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("key", "[removed]");
                    reqparm.Add("orderid", orderID.ToString());
                      reqparm.Add("appid", "[removed]");
                   
                    byte[] responsebytes = wc.UploadValues("https://partner.steam-api.com/ISteamMicroTxn/FinalizeTxn/v2/", "POST", reqparm);

                 string json=   System.Text.Encoding.UTF8.GetString(responsebytes);

                    
                    isOK = json.ToLower().Replace(" ", "").Contains("\"result\":\"ok\"");
                   
                }
            }
            return isOK;
        }

        private bool SteamPay(DataClassesInterfaceDataContext cc, Player aPlayer, xActionRequest.EnumGamePlatformType platform, string steamTicket, string userID, decimal USDAmount)
        {


            bool IsOK2 = false;

            if (platform == xActionRequest.EnumGamePlatformType.SteamPC)
            {

                using (WebClient wc = new WebClient())
                {
                    //                    key	string	✔	Steamworks Web API publisher authentication key.
                    //orderid	uint64	✔	Unique 64-bit ID for order
                    //steamid	uint64	✔	Steam ID of user making purchase.
                    //appid	uint32	✔	App ID of game this transaction is for.
                    //itemcount	uint32	✔	Number of items in cart.
                    //language	string	✔	ISO 639-1 language code of the item descriptions.
                    //currency	string	✔	ISO 4217 currency code. See Supported Currencies for proper format of each currency.
                    //usersession	string		Session where user will authorize the transaction. Valid options are "client" or "web". If this parameter is not supplied, the interface will be assumed to be through a currently logged in Steam client session.
                    //ipaddress	string		IP address of user in string format (xxx.xxx.xxx.xxx). Only required if [param]usersession[/param] is set to web.
                    //itemid[0]	uint32	✔	3rd party ID for item.
                    //qty[0]	uint32	✔	Quantity of this item.
                    //amount[0]	int32	✔	Total cost (in cents) of item(s). See Supported Currencies for proper format of each amount. Note that the amount you pass needs to be in the format that matches the "currency" code you pass.
                    //description[0]	string	✔	Description of item.
                    //category[0]	string		Optional category grouping for item.
                    //associated_bundle[0]	uint32		Optional bundleid of associated bundle.
                    //billingtype[0]	string		Optional recurring billing type.
                    //startdate[0]	string		Optional start date for recurring billing.
                    //enddate[0]	string		Optional end date for recurring billing.
                    //period[0]	string		Optional period for recurring billing.
                    //frequency[0]	uint32		Optional frequency for recurring billing.
                    //recurringamt[0]	string		Optional recurring billing amount.
                    //bundlecount	uint32		Number of bundles in cart.
                    //bundleid[0]	uint32		3rd party ID of the bundle. This shares the same ID space as 3rd party items.
                    //bundle_qty[0]	uint32		Quantity of this bundle.
                    //bundle_desc[0]	string		Description of bundle.
                    //bundle_category[0]	string		Optional category grouping for bundle.
                    GeneralSettingRow roi = cc.GeneralSettingRows.FirstOrDefault(x => x.SettingName.ToLower() == "roi");
                    decimal roiValue = Convert.ToDecimal(roi.SettingValue);
                    // theData.RoI = roiValue;

                    decimal gameCredits = USDAmount * roiValue;
                    SteamWalletPaymentRecord aRecord = new SteamWalletPaymentRecord();
                    aRecord.DateStarted = System.DateTime.UtcNow;
                    aRecord.DateFinished = aRecord.DateStarted;
                    aRecord.GameCredits = gameCredits;
                    aRecord.ID_Player = aPlayer.ID;
                    aRecord.IsComplete = false;
                    aRecord.ResultText = "";
                    aRecord.USDAmount = USDAmount;
                    cc.SteamWalletPaymentRecords.InsertOnSubmit(aRecord);
                    cc.SubmitChanges();
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("key", "[removed]");
                    reqparm.Add("orderid", aRecord.ID.ToString());
                    reqparm.Add("steamid", userID);
                    reqparm.Add("itemcount", "1");
                    reqparm.Add("appid", "[removed]");
                    reqparm.Add("language", "en");
                    reqparm.Add("currency", "USD");
                    reqparm.Add("itemid[0]", "4");
                    reqparm.Add("qty[0]", USDAmount.ToString("f0"));
                    reqparm.Add("amount[0]", (USDAmount * 100).ToString("f0"));
                    reqparm.Add("description[0]", "Adding $" + gameCredits.ToString("f2") + " In-Game Funds");
                    byte[] responsebytes = wc.UploadValues("https://partner.steam-api.com/ISteamMicroTxn/InitTxn/v3/", "POST", reqparm);
                    string responsebody = System.Text.Encoding.UTF8.GetString(responsebytes);
                    int xxx = 0;
                   


                    
                }
            }
            if (platform == xActionRequest.EnumGamePlatformType.UWSPC)
            {
                IsOK2 = true;
            }
            return IsOK2;
        }

        private bool ConfirmSteamTicket(xActionRequest.EnumGamePlatformType platform, string steamTicket, string userID)
        {


            bool IsOK2 = false;

            if (platform == xActionRequest.EnumGamePlatformType.SteamPC)
            {

                using (WebClient wc = new WebClient())
                {

                    var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key="+"[removed]" + "&ticket=" + steamTicket + "&appid=725890");
                    json = json.Replace("params", "params1");
                    json = json.Replace("\t", " ");
                    json = json.Replace("  ", " ");

               
                    IsOK2 = json.ToLower().Replace(" ", "").Contains("\"result\":\"ok\"");
                    if (IsOK2)
                    {
                        if (json.ToLower().Contains(userID))
                        {
                            IsOK2 = true;
                        }
                        else
                        {
                            IsOK2 = false;
                        }
                    }
                }
            }
            if (platform == xActionRequest.EnumGamePlatformType.UWSPC)
            {
                IsOK2 = true;
            }
            return IsOK2;
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
        public DateTime LastNewsUpdateDate;
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

}