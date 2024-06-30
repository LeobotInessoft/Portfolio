using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.paypal.soap.api;
using com.paypal.sdk.profiles;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
namespace RobotServ
{
    public partial class PaypalConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string result = "";
            try
            {
              
                {
                    string apiUsername = "[Removed]";
                    string apiPass = "[Removed]";
                    string apiSignature = "[Removed]";

                   
                    if (Request.QueryString.Get("token") != null)
                    {

                       

                        IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
                        profile.APIUsername = apiUsername;
                        profile.APIPassword = apiPass;
                        profile.Environment = "live";
                         profile.APISignature = apiSignature;
                       
                        CallerServices caller = new CallerServices();
                        caller.APIProfile = profile;


                        GetExpressCheckoutDetailsResponseType GetECDetailsRes = new GetExpressCheckoutDetailsResponseType();
                        GetExpressCheckoutDetailsRequestType GetECDetailsReqType = new GetExpressCheckoutDetailsRequestType();
                        GetExpressCheckoutDetailsReq GetECDetailsReq = new GetExpressCheckoutDetailsReq();

                        GetECDetailsReqType.Token = Request.QueryString.Get("token");
                        GetECDetailsReq.GetExpressCheckoutDetailsRequest = GetECDetailsReqType;

                        PayPalAPIAASoapBinding PPInterface = new PayPalAPIAASoapBinding();
                        
                         PPInterface.RequesterCredentials = new CustomSecurityHeaderType();
                         try
                        {

                            GetECDetailsRes = (GetExpressCheckoutDetailsResponseType)caller.Call("GetExpressCheckoutDetails", GetECDetailsReqType);
                               if (GetECDetailsRes.Ack == AckCodeType.Success)
                            {
                                DoExpressCheckoutPaymentRequestDetailsType DoECPmtReqDetails = new DoExpressCheckoutPaymentRequestDetailsType();
                                DoExpressCheckoutPaymentRequestType DoECReqType = new DoExpressCheckoutPaymentRequestType();
                                DoExpressCheckoutPaymentReq DoECPmtReq = new DoExpressCheckoutPaymentReq();
                                DoExpressCheckoutPaymentResponseType DoECPmtRes = new DoExpressCheckoutPaymentResponseType();

                                //    DoECReqType.Version = Globals.Version;
                                DoECPmtReqDetails.Token = GetECDetailsRes.GetExpressCheckoutDetailsResponseDetails.Token;
                                DoECPmtReqDetails.PayerID = GetECDetailsRes.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;
                                DoECPmtReqDetails.PaymentAction = PaymentActionCodeType.Sale;
                                DoECPmtReqDetails.PaymentDetails = new PaymentDetailsType[1];

                                DoECPmtReqDetails.PaymentDetails[0] = new PaymentDetailsType();
                                DoECPmtReqDetails.PaymentDetails[0].OrderTotal = new BasicAmountType();
                                DoECPmtReqDetails.PaymentDetails[0].OrderTotal.currencyID = CurrencyCodeType.USD;
                                DoECPmtReqDetails.PaymentDetails[0].OrderTotal.Value = Session["OrderTotal"].ToString();
                                DoECReqType.DoExpressCheckoutPaymentRequestDetails = DoECPmtReqDetails;
                                DoECPmtReq.DoExpressCheckoutPaymentRequest = DoECReqType;

                                PPInterface = new PayPalAPIAASoapBinding();
                                PPInterface.RequesterCredentials = new CustomSecurityHeaderType();
                                DoECPmtRes = (DoExpressCheckoutPaymentResponseType)caller.Call("DoExpressCheckoutPayment", DoECReqType);

                                if (DoECPmtRes.Ack == AckCodeType.Success)
                                {
                                    long orderID = (long)(Session["orderID"]);
                                    DataClassesInterfaceDataContext currentContext = new DataClassesInterfaceDataContext();

                                   

                                    GameFundingRecord aRecord = currentContext.GameFundingRecords.FirstOrDefault(x => x.ID == orderID);
                                    //   Deposit theInvoice = currentContext.Deposits.FirstOrDefault(x => x.ID == orderID);

                                    if (aRecord != null && aRecord.IsCompleted == false)
                                    {
                                        aRecord.IsCompleted = true;
                                        Player aPlayer = currentContext.Players.FirstOrDefault(x => x.ID == aRecord.ID_Player);
                                        aPlayer.Money += aRecord.GameFundsAmount;

                                        GameFinancialRow aFinanceStat = new GameFinancialRow();
                                        aFinanceStat.EventDate = System.DateTime.UtcNow;
                                        aFinanceStat.ID_Match = 0;
                                        aFinanceStat.ID_Player = aPlayer.ID;
                                        aFinanceStat.ID_PlayerInitiater = aPlayer.ID;
                                        aFinanceStat.ID_Robot = 0;
                                        aFinanceStat.MoneyAmount = aRecord.GameFundsAmount;
                                        string reason = "";
                                        reason = "Funded your account. Thank you for your support! You rock!";
                                        aFinanceStat.Reason = reason;
                                        currentContext.GameFinancialRows.InsertOnSubmit(aFinanceStat);


                                        currentContext.SubmitChanges();
                                        result = reason+" You may now close this window and will find your newly added funds ready to be used in game.";
                                        //     Dashboard.SendEmail("GameFund Received: Record #" + aRecord.ID, "Game ID  " + aRecord.IDGame + " completed.", "leon@inessoft.com");
                                    }
                                    else
                                    {
                                        result = "Funding Already Applied";

                                    }
                                   
                                }
                                else
                                {
                                    result = "Error: " + DoECPmtRes.Errors[0].LongMessage;

                                }
                            }
                            else
                            {
                                result = "Error: " + GetECDetailsRes.Errors[0].LongMessage;

                            }
                        }
                        catch (Exception ex)
                        {
                            result = "Error: " + ex.ToString();

                        }
                    }
                    else
                    {

                        result = "To add funds to your account you need to do so via the game shop interface.";
                    }

                }
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.ToString();

            }

            Label1.Text = result;
        }

    }
}