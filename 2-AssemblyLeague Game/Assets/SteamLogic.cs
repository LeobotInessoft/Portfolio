using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamLogic : MonoBehaviour
{
    public string PersonName;
    public CSteamID ID;
    public AccountID_t acc;
    public byte[] data;
    public HAuthTicket aTicket;
    public string TicketString;
    public bool IsInit = false;
    public static SteamLogic PublicAccess;
    public WwwLeagueInterface TheLeague;
    public Callback<MicroTxnAuthorizationResponse_t> xx;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        IsInit = false;
        if (SteamManager.Initialized)
        {
            PersonName = SteamFriends.GetPersonaName();

            ID = SteamUser.GetSteamID();
            acc = ID.GetAccountID();
            data = new byte[1024];


            uint actualLenght = 0;

            aTicket = SteamUser.GetAuthSessionTicket(data, 1024, out actualLenght);
            TicketString = System.BitConverter.ToString(data, 0, data.Length).Replace("-", string.Empty);

            Debug.Log(ID.m_SteamID + " " + TicketString + " " + acc.m_AccountID + " " + name + " " + aTicket.m_HAuthTicket);

            IsInit = true;


            xx = Callback<MicroTxnAuthorizationResponse_t>.Create(funct);
            xx.Register(funct);
            if (TheLeague != null)
                TheLeague.SteamIsReady();
        }
    }
    MicroTxnAuthorizationResponse_t lastTx = new MicroTxnAuthorizationResponse_t();
    public bool MustSendTx = false;
    void funct(MicroTxnAuthorizationResponse_t tx)
    {
        lastTx = tx;
        MustSendTx = true;

        return;
    }
    // Update is called once per frame
    void Update()
    {

        if (SteamManager.Initialized)
        {
            SteamAPI.RunCallbacks();
        }
        if (MustSendTx)
        {
            MustSendTx = false;

            {
               

                try
                {
                    print("Sending Confirm");

                    TheLeague.SteamWalletPayConfirm(lastTx.m_ulOrderID);
                    // TheLeague.SteamWalletPayConfirm(0);
                    print("Done");
                }
                catch (System.Exception e)
                {
                    // System.IO.File.WriteAllText("c:\\db\\log.txt", e.ToString()+":::::" + e.InnerException);
                    Application.Quit();
                }
            }

        }
    }
}
