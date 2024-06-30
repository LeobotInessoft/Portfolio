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

            // ClassTTS.SayIt(Server.MapPath("TTS"), "This match will be between Sneaky Creep, Cheap Dog, Funny Man, and John. The prize is $300");
            Response.Clear();
            try
            {
                xActionRequest anAction = new xActionRequest();
                //string userID = Request.Form["clientID"];
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
                                // theData.AddFundsSteamWallet = robotDB.AddSteamWalletFunds(currentContext, anAction.AddFundsSteamWallet, anAction.SteamUserID);
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
                                        // Player aPlayer = currentContext.Players.FirstOrDefault(x => x.ID == aRecord.ID_Player);
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
                                        //aPlayer.Money=
                                    }

                                }
                                //  SteamPay(currentContext, robotDB.GetPlayerFromDB(currentContext, anAction.AddFundsSteamWallet.Email, anAction.AddFundsSteamWallet.Passowrd, anAction.SteamUserID), anAction.PlatformType, anAction.SteamTicket, anAction.SteamUserID, anAction.AddFundsSteamWallet.UsdAmount);
                                // theData.AddFundsSteamWallet = robotDB.AddSteamWalletFunds(currentContext, anAction.AddFundsSteamWallet, anAction.SteamUserID);
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


                    //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    //Response.Headers.Add("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
                    //Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                    //Response.Headers.Add("Access-Control-Allow-Origin", "*");


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
            //  if (result == 1)
            {
                using (WebClient wc = new WebClient())
                {

                    //var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=14000000BA8E5C0C96BD396425937F03010010018E00E5591800000001000000020000004C3858C52001A8C04F2B08010A000000B2000000320000000400000025937F030100100182130B004C3858C52001A8C000000000D7C1E4595771005A0100EA3903000000000033639BF8CD5218905BE524EB74BDD1D821DCEAF6E0791DD70145E7AD4A4560171ED256DBC687A08728B8D949EBCD3433198B8031E4E973178910565FF95839D43B2AF21FACEACC112E3629B0821A931ACED8B6F6DF0E1FAACADD15D21ADC47F8A219D5760EE5642293A0924DCEBE83B8C27A51DFE667BC9040E3D151F49B3B015C84DE01242262B8748F043EB0FD329DC870757ECACDF28B166C514C2BB5A8CCB85384C90365397BFA2F7E9B4BCA2841762E435BB6BFC206102B055A0B2B12EE9366FB1797A54693630D6C0FD6A6AA29AFC0ABB53006DEBFA24A50C8102EBB8AB450E0D91A1221F6B131BC0DC940CCB86EC973654FECCAA7BBDA7C3541B8C977A4BFFD8FE5ACA57C165E62280D8B7C95F745A7811D9DE061C07FCFD37218F74BDF8AF8C99DCC83F6B96BA7EC7DD97D180A3729C4D955F50BB8CBFD31F6AAA92B86187E30AF112A380AB9107DECD265E9DA984402241F5D8C62997D06C4B2018BCA13DE75EFA9737291CEA649573A070C024D45F7E74E68861987EDCB6C0A07DC163C8C3AC92B36825ED95C1F326EE4A20E10CFD2CC0D297360ACD12CEA48BE8DCC0160B172EDD00CC4A3F1F1F582DDF5CFDCFE95B4102F2039C8AE40C86B43E23969BC7F06136377256C860D45FB56BEAA61356DEF09F412DB5999008D3A15FB33DE6EEAA1917C37071131E9C64324C9294771E287799274E6ED659D084C208551C212640862AD7E6A2FCC04C3A177B35AD88906D5B885A0476CFE9EEA747274E92943FC4B911CE5F72C247224E559C2387BF11755E52A536BA89C88974CF184D48EA2E9340796CDBFF1B8D5DA7F48B475E21CC94C273A5601920522786327C2B7C1F7A1320E31E122F78E418939AC3CD4D84486CAB485DE4061FD456C146D8CD8273B1D38547A41FDE1D107B9C284DC8CA08244C3A72CF97909D4B1A72E904927A2F20E4120D246F816E66AFB85D0BC723B6617F15BA88B13EBE17BE976D69FAEAFD5F7EB898DE05C8CE710C8847A0C361C39EB0B7338CE37B9CAFCFA4181C930E9855D44B2B9042B626B4E1CDAD8F68E5E17443DF31D7ECABA960C976F42E8FB1C4D80B6635FA4CE0514E01A9233C8B12B4E9A799509C6943E5543F4F0E2282DFAC0658DAE3CB2BAA7342CA9C468CB7B736D0624BB83FBC647A1D1FD4D6040A3A6ABA4148FC6CEAD263DA4DC765FB384D567A2396777227B3CDD204E2626477F754C67BA2BE4AD4473CD90CF9A1CB29754650000000000000000000000000000000000000000000000000000000000000000000000&appid=725890");
                    //var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=" + steamTicket + "&appid=725890");
                    //var json = wc.DownloadString("https://api.steampowered.com/ISteamMicroTxn/FinalizeTxn/v2/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=" + steamTicket + "&appid=725890&ordierid=" + orderID);
                    //var json = wc.DownloadString("https://partner.steam-api.com/ISteamMicroTxn/FinalizeTxn/v2/key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=" + steamTicket + "&appid=725890&ordierid=" + orderID);



                    //json = json.Replace("params", "params1");
                    //json = json.Replace("\t", " ");
                    //json = json.Replace("  ", " ");

                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("key", "4AAE3948E90F96C3EEEC40E904FD97BB");
                    reqparm.Add("orderid", orderID.ToString());
                    // reqparm.Add("steamid", userID);
                    //  reqparm.Add("itemcount", "1");
                    reqparm.Add("appid", "725890");
                    //   reqparm.Add("language", "en");
                    //   reqparm.Add("currency", "USD");
                    // reqparm.Add("itemid[0]", "4");
                    //   reqparm.Add("qty[0]", USDAmount.ToString("f0"));
                    //   reqparm.Add("amount[0]", (USDAmount * 100).ToString("f0"));
                    //    reqparm.Add("description[0]", "Adding $" + gameCredits.ToString("f2") + " In-Game Funds");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param2", "escaping is already handled");
                    byte[] responsebytes = wc.UploadValues("https://partner.steam-api.com/ISteamMicroTxn/FinalizeTxn/v2/", "POST", reqparm);

                 string json=   System.Text.Encoding.UTF8.GetString(responsebytes);

                    //  Newtonsoft.Json.JsonConvert.
                    // response account = JsonConvert.DeserializeObject<response>(json);

                    isOK = json.ToLower().Replace(" ", "").Contains("\"result\":\"ok\"");
                    //if (isOK)
                    //{
                    //    if (json.ToLower().Contains(userID))
                    //    {
                    //        isOK = true;
                    //    }
                    //    else
                    //    {
                    //        isOK = false;
                    //    }
                    //}
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
                    reqparm.Add("key", "4AAE3948E90F96C3EEEC40E904FD97BB");
                    reqparm.Add("orderid", aRecord.ID.ToString());
                    reqparm.Add("steamid", userID);
                    reqparm.Add("itemcount", "1");
                    reqparm.Add("appid", "725890");
                    reqparm.Add("language", "en");
                    reqparm.Add("currency", "USD");
                    reqparm.Add("itemid[0]", "4");
                    reqparm.Add("qty[0]", USDAmount.ToString("f0"));
                    reqparm.Add("amount[0]", (USDAmount * 100).ToString("f0"));
                    reqparm.Add("description[0]", "Adding $" + gameCredits.ToString("f2") + " In-Game Funds");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param1", "<any> kinds & of = ? strings");
                    //reqparm.Add("param2", "escaping is already handled");
                    byte[] responsebytes = wc.UploadValues("https://partner.steam-api.com/ISteamMicroTxn/InitTxn/v3/", "POST", reqparm);
                    string responsebody = System.Text.Encoding.UTF8.GetString(responsebytes);
                    int xxx = 0;
                    // https://partner.steam-api.com/ISteamMicroTxn/InitTxn/v3/
                    //var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=14000000BA8E5C0C96BD396425937F03010010018E00E5591800000001000000020000004C3858C52001A8C04F2B08010A000000B2000000320000000400000025937F030100100182130B004C3858C52001A8C000000000D7C1E4595771005A0100EA3903000000000033639BF8CD5218905BE524EB74BDD1D821DCEAF6E0791DD70145E7AD4A4560171ED256DBC687A08728B8D949EBCD3433198B8031E4E973178910565FF95839D43B2AF21FACEACC112E3629B0821A931ACED8B6F6DF0E1FAACADD15D21ADC47F8A219D5760EE5642293A0924DCEBE83B8C27A51DFE667BC9040E3D151F49B3B015C84DE01242262B8748F043EB0FD329DC870757ECACDF28B166C514C2BB5A8CCB85384C90365397BFA2F7E9B4BCA2841762E435BB6BFC206102B055A0B2B12EE9366FB1797A54693630D6C0FD6A6AA29AFC0ABB53006DEBFA24A50C8102EBB8AB450E0D91A1221F6B131BC0DC940CCB86EC973654FECCAA7BBDA7C3541B8C977A4BFFD8FE5ACA57C165E62280D8B7C95F745A7811D9DE061C07FCFD37218F74BDF8AF8C99DCC83F6B96BA7EC7DD97D180A3729C4D955F50BB8CBFD31F6AAA92B86187E30AF112A380AB9107DECD265E9DA984402241F5D8C62997D06C4B2018BCA13DE75EFA9737291CEA649573A070C024D45F7E74E68861987EDCB6C0A07DC163C8C3AC92B36825ED95C1F326EE4A20E10CFD2CC0D297360ACD12CEA48BE8DCC0160B172EDD00CC4A3F1F1F582DDF5CFDCFE95B4102F2039C8AE40C86B43E23969BC7F06136377256C860D45FB56BEAA61356DEF09F412DB5999008D3A15FB33DE6EEAA1917C37071131E9C64324C9294771E287799274E6ED659D084C208551C212640862AD7E6A2FCC04C3A177B35AD88906D5B885A0476CFE9EEA747274E92943FC4B911CE5F72C247224E559C2387BF11755E52A536BA89C88974CF184D48EA2E9340796CDBFF1B8D5DA7F48B475E21CC94C273A5601920522786327C2B7C1F7A1320E31E122F78E418939AC3CD4D84486CAB485DE4061FD456C146D8CD8273B1D38547A41FDE1D107B9C284DC8CA08244C3A72CF97909D4B1A72E904927A2F20E4120D246F816E66AFB85D0BC723B6617F15BA88B13EBE17BE976D69FAEAFD5F7EB898DE05C8CE710C8847A0C361C39EB0B7338CE37B9CAFCFA4181C930E9855D44B2B9042B626B4E1CDAD8F68E5E17443DF31D7ECABA960C976F42E8FB1C4D80B6635FA4CE0514E01A9233C8B12B4E9A799509C6943E5543F4F0E2282DFAC0658DAE3CB2BAA7342CA9C468CB7B736D0624BB83FBC647A1D1FD4D6040A3A6ABA4148FC6CEAD263DA4DC765FB384D567A2396777227B3CDD204E2626477F754C67BA2BE4AD4473CD90CF9A1CB29754650000000000000000000000000000000000000000000000000000000000000000000000&appid=725890");



                    //var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=" + steamTicket + "&appid=725890");
                    //json = json.Replace("params", "params1");
                    //json = json.Replace("\t", " ");
                    //json = json.Replace("  ", " ");

                    ////  Newtonsoft.Json.JsonConvert.
                    //// response account = JsonConvert.DeserializeObject<response>(json);

                    //IsOK2 = json.ToLower().Replace(" ", "").Contains("\"result\":\"ok\"");
                    //if (IsOK2)
                    //{
                    //    if (json.ToLower().Contains(userID))
                    //    {
                    //        IsOK2 = true;
                    //    }
                    //    else
                    //    {
                    //        IsOK2 = false;
                    //    }
                    //}
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

                    //var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=14000000BA8E5C0C96BD396425937F03010010018E00E5591800000001000000020000004C3858C52001A8C04F2B08010A000000B2000000320000000400000025937F030100100182130B004C3858C52001A8C000000000D7C1E4595771005A0100EA3903000000000033639BF8CD5218905BE524EB74BDD1D821DCEAF6E0791DD70145E7AD4A4560171ED256DBC687A08728B8D949EBCD3433198B8031E4E973178910565FF95839D43B2AF21FACEACC112E3629B0821A931ACED8B6F6DF0E1FAACADD15D21ADC47F8A219D5760EE5642293A0924DCEBE83B8C27A51DFE667BC9040E3D151F49B3B015C84DE01242262B8748F043EB0FD329DC870757ECACDF28B166C514C2BB5A8CCB85384C90365397BFA2F7E9B4BCA2841762E435BB6BFC206102B055A0B2B12EE9366FB1797A54693630D6C0FD6A6AA29AFC0ABB53006DEBFA24A50C8102EBB8AB450E0D91A1221F6B131BC0DC940CCB86EC973654FECCAA7BBDA7C3541B8C977A4BFFD8FE5ACA57C165E62280D8B7C95F745A7811D9DE061C07FCFD37218F74BDF8AF8C99DCC83F6B96BA7EC7DD97D180A3729C4D955F50BB8CBFD31F6AAA92B86187E30AF112A380AB9107DECD265E9DA984402241F5D8C62997D06C4B2018BCA13DE75EFA9737291CEA649573A070C024D45F7E74E68861987EDCB6C0A07DC163C8C3AC92B36825ED95C1F326EE4A20E10CFD2CC0D297360ACD12CEA48BE8DCC0160B172EDD00CC4A3F1F1F582DDF5CFDCFE95B4102F2039C8AE40C86B43E23969BC7F06136377256C860D45FB56BEAA61356DEF09F412DB5999008D3A15FB33DE6EEAA1917C37071131E9C64324C9294771E287799274E6ED659D084C208551C212640862AD7E6A2FCC04C3A177B35AD88906D5B885A0476CFE9EEA747274E92943FC4B911CE5F72C247224E559C2387BF11755E52A536BA89C88974CF184D48EA2E9340796CDBFF1B8D5DA7F48B475E21CC94C273A5601920522786327C2B7C1F7A1320E31E122F78E418939AC3CD4D84486CAB485DE4061FD456C146D8CD8273B1D38547A41FDE1D107B9C284DC8CA08244C3A72CF97909D4B1A72E904927A2F20E4120D246F816E66AFB85D0BC723B6617F15BA88B13EBE17BE976D69FAEAFD5F7EB898DE05C8CE710C8847A0C361C39EB0B7338CE37B9CAFCFA4181C930E9855D44B2B9042B626B4E1CDAD8F68E5E17443DF31D7ECABA960C976F42E8FB1C4D80B6635FA4CE0514E01A9233C8B12B4E9A799509C6943E5543F4F0E2282DFAC0658DAE3CB2BAA7342CA9C468CB7B736D0624BB83FBC647A1D1FD4D6040A3A6ABA4148FC6CEAD263DA4DC765FB384D567A2396777227B3CDD204E2626477F754C67BA2BE4AD4473CD90CF9A1CB29754650000000000000000000000000000000000000000000000000000000000000000000000&appid=725890");
                    var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=4AAE3948E90F96C3EEEC40E904FD97BB&ticket=" + steamTicket + "&appid=725890");
                    json = json.Replace("params", "params1");
                    json = json.Replace("\t", " ");
                    json = json.Replace("  ", " ");

                    //  Newtonsoft.Json.JsonConvert.
                    // response account = JsonConvert.DeserializeObject<response>(json);

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
        //public xCommerceClient TheClient;
        //public List<xCategory> AllCatergories;
        //public List<xProduct> AllProducts;
        //public List<xArticle> AllArticles;       

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