using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using com.paypal.soap.api;
using com.paypal.sdk.profiles;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;


namespace RobotServ
{
    public partial class AddFunds : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = "";
            bool hasAllQuery = true;
            if (Request.QueryString["uid"] == null || Request.QueryString["uid"].Length == 0)
            {
                hasAllQuery = false;
            }

            if (Request.QueryString["am"] == null || Request.QueryString["am"].Length == 0)
            {
                hasAllQuery = false;
            }
            if (hasAllQuery)
            {
                int UserID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                decimal amountUSD = Convert.ToDecimal(Request.QueryString["am"].ToString());

                if (amountUSD >= (decimal)0.1)
                {
                    DataClassesInterfaceDataContext currentContext = new DataClassesInterfaceDataContext();
                    GeneralSettingRow roi = currentContext.GeneralSettingRows.FirstOrDefault(x => x.SettingName.ToLower() == "roi");
                    decimal roiValue = Convert.ToDecimal(roi.SettingValue);
                    Player aPlayer = currentContext.Players.FirstOrDefault(x => x.ID == UserID);
                    if (aPlayer != null)
                    {
                        GameFundingRecord aRecord = new GameFundingRecord();
                        aRecord.DateCreated = DateTime.Now;
                        aRecord.ID_Player = aPlayer.ID;
                        aRecord.IsCompleted = false;
                        aRecord.PaypalCode = "";
                        aRecord.UsdAmount = (decimal)amountUSD;
                        aRecord.ProcessDate = aRecord.DateCreated;
                        aRecord.HasBeenProcessed = false;
                        aRecord.GameFundsAmount = aRecord.UsdAmount * roiValue;

                        currentContext.GameFundingRecords.InsertOnSubmit(aRecord);
                        currentContext.SubmitChanges();

                        if (aRecord != null)
                        {
                            result = "Thank you for your support. You will now be redirected to the secure PayPal gateway.";
                            Label1.Text = result;
                            DoExpress((decimal)aRecord.UsdAmount, aRecord.ID);
                            Response.Redirect(Request.Url.ToString());
                        }
                    }
                    else
                    {
                        result = "Invalid Account";
                    }

                }
                else
                {
                    result = "Invalid Amount";
                }

            }
            else
            {
                result = "To add funds to your account please do so via the Shop in game.";

            }
            Label1.Text = result;

        }

        string apiUsername = "[removed]";
        string apiPass = "[removed]";
        string apiSignature = "[removed]";

        private void DoExpress(decimal dollaramount, long invoiceID)
        {



            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
            profile.APIUsername = apiUsername;
            profile.APIPassword = apiPass;
            profile.Environment = "live";
            profile.APISignature = apiSignature;


            CallerServices caller = new CallerServices();
            caller.APIProfile = profile;
            SetExpressCheckoutRequestType SetECReqType = new SetExpressCheckoutRequestType();
            SetExpressCheckoutRequestDetailsType SetECReqTypeDetails = new SetExpressCheckoutRequestDetailsType();
            SetExpressCheckoutReq SetECReq = new SetExpressCheckoutReq();

            //  SetECReqType.Version = Globals.Version;
            SetECReqTypeDetails.OrderTotal = new BasicAmountType();
            SetECReqTypeDetails.OrderTotal.currencyID = CurrencyCodeType.USD;
            SetECReqTypeDetails.OrderTotal.Value = dollaramount.ToString();
            SetECReqTypeDetails.OrderDescription = "Assembly League ($" + dollaramount.ToString() + ")";

            string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;
            if (basePath.EndsWith("/"))
            {
                SetECReqTypeDetails.ReturnURL = basePath + "paypalconfirm.aspx";
            }
            else
            {
                SetECReqTypeDetails.ReturnURL = basePath + "/paypalconfirm.aspx";
            }
            SetECReqTypeDetails.CancelURL = Request.Url.ToString();// basePath + "default.aspx";// Request.RawUrl;// basePath + "default.aspx";
            SetECReqType.SetExpressCheckoutRequestDetails = SetECReqTypeDetails;
            SetECReq.SetExpressCheckoutRequest = SetECReqType;

            UserIdPasswordType user = new UserIdPasswordType();
            user.Username = apiUsername;
            user.Password = apiPass;

            user.Signature = apiSignature;// Globals.Signature;

            PayPalAPIAASoapBinding PPInterface = new PayPalAPIAASoapBinding();

            PPInterface.RequesterCredentials = new CustomSecurityHeaderType();
            PPInterface.RequesterCredentials.Credentials = user;
            try
            {
                SetECReqType.SetExpressCheckoutRequestDetails.NoShipping = "1";
                SetExpressCheckoutResponseType response = (SetExpressCheckoutResponseType)caller.Call("SetExpressCheckout", SetECReqType);

                if (response.Ack == AckCodeType.Success)
                {
                    Session["orderID"] = invoiceID;
                    Session["OrderTotal"] = SetECReqTypeDetails.OrderTotal.Value;
                    String SBredirectURL = "https://www.paypal.com/cgi-bin/" + "webscr?cmd=_express-checkout&token=";
                    Response.Redirect(SBredirectURL + response.Token);
                }
                else
                {
                    //  messageLabel.Text = SetECRes.Errors[0].LongMessage;
                }
            }
            catch (Exception ex)
            {
                // messageLabel.Text = ex.ToString();
            }
        }
        private bool ConfirmSteamTicket(string steamTicket, string userID)
        {
            string steamKey = "[Removed]";
            string steamAppID = "[Removed]";
            bool IsOK2 = false;
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key=" + steamKey + "&ticket=" + steamTicket + "&appid=" + steamAppID);
                json = json.Replace("params", "params1");
                json = json.Replace("\t", " ");
                json = json.Replace("  ", " ");


                IsOK2 = json.ToLower().Contains("\"result\": \"ok\"");
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
            return IsOK2;
        }


    }
}