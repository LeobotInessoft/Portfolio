using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
public class RobotOwnerLookup : MonoBehaviour
{
    public static string Email = "locUser";
    public static string Password = "locPassword";
    string fileName = "local5.txt";
    RobotOwnerLocalList LookupList;

    public static RobotOwnerLookup PublicAccess;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        LoadFromFile();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public PlayerOptions GetPlayerOptions()
    {
        if (LookupList == null) LoadFromFile();

        return LookupList.MyOptions;
    }
    public void SetPlayerOptions(PlayerOptions Options)
    {
        LookupList.MyOptions = Options;

        RobotOwnerLocalList.SaveToFile(LookupList, fileName);

    }

    public PlayerPurchases GetPlayerPurchases()
    {
        if (LookupList == null) LoadFromFile();
        if (LookupList.MyPurchases == null)
        {
            LookupList.MyPurchases = new PlayerPurchases();
            LookupList.MyPurchases.ComponentsPurchasedIDs = new List<string>();
            LookupList.MyPurchases.TotalMoneySpent = 0;
        }
        return LookupList.MyPurchases;
    }
    public void DoInstall(ComponentType aComp)
    {
        print("Installing " + aComp.DeviceName);
        PlayerPurchases aPurch = GetPlayerPurchases();
        int index = aPurch.ComponentsPurchasedIDs.IndexOf(aComp.UniqueDeviceID);
        aPurch.ComponentsPurchasedIDs.RemoveAt(index);
        LookupList.MyPurchases = aPurch;
        RobotOwnerLocalList.SaveToFile(LookupList, fileName);
    }
    public void DoPurchase(ComponentType aComp)
    {
        bool alreadyHas = false;
        PlayerPurchases aPurch = GetPlayerPurchases();
         if (WwwLeagueInterface.LoggedInFundsTotal - LookupList.MyPurchases.TotalMoneySpent > aComp.OfflinePurchaseCost)
        {

            aPurch.ComponentsPurchasedIDs.Add(aComp.UniqueDeviceID);
            LookupList.MyPurchases.TotalMoneySpent += aComp.OfflinePurchaseCost;
        }
        RobotOwnerLocalList.SaveToFile(LookupList, fileName);
    }
    public List<xHint> GetHints()
    {
        if (LookupList == null) LoadFromFile();
        if (LookupList.AllHints == null)
        {
            LookupList.AllHints = new List<xHint>();

        }
        return LookupList.AllHints;
    }
    public void AddHint(xHint aHint)
    {
        print("Adding Hint " + aHint.TipText);
        if (LookupList == null) LoadFromFile();
        if (LookupList.AllHints == null)
        {
            LookupList.AllHints = new List<xHint>();
        }
        xHint existing = LookupList.AllHints.FirstOrDefault(x => x.ID == aHint.ID);
        if (existing == null)
        {
            LookupList.AllHints.Add(aHint);
            RobotOwnerLocalList.SaveToFile(LookupList, fileName);
        }
        else
        {
            existing.TipHeading = aHint.TipHeading;
            existing.TipText = aHint.TipText;
            RobotOwnerLocalList.SaveToFile(LookupList, fileName);
        }

    }
    public List<xNews> GetNews()
    {
        if (LookupList == null) LoadFromFile();
        if (LookupList.AllNews == null)
        {
            LookupList.AllNews = new List<xNews>();

        }
        return LookupList.AllNews;
    }
    public void AddNews(xNews aHint)
    {
        print("Adding NEWS " + aHint.NewsText);
        if (LookupList == null) LoadFromFile();
        if (LookupList.AllNews == null)
        {
            LookupList.AllNews = new List<xNews>();
        }
        xNews existing = LookupList.AllNews.FirstOrDefault(x => x.ID == aHint.ID);
        if (existing == null)
        {
            LookupList.AllNews.Add(aHint);
            RobotOwnerLocalList.SaveToFile(LookupList, fileName);
        }
        else
        {
            existing.DisplayDate = aHint.DisplayDate;
            existing.MainRobotID = aHint.MainRobotID;
            existing.NewsText = aHint.NewsText;
            existing.NewsType = aHint.NewsType;
            RobotOwnerLocalList.SaveToFile(LookupList, fileName);
        }

        if (LookupList.AllNews.Count > 30)
        {
            LookupList.AllNews.RemoveAt(0);
        }

    }
    public decimal GetRateofExchange()
    {
        if (LookupList == null) LoadFromFile();

        return LookupList.RateOfExchange;
    }
    public void SetRoI(decimal rate)
    {
        print("AddSetting RoI " + rate);
        if (LookupList == null) LoadFromFile();
        LookupList.RateOfExchange = rate;
        RobotOwnerLocalList.SaveToFile(LookupList, fileName);
     
    }
    
    
    public List<RobotConstructor.RobotTemplate> GetAllTemplatesForCurrentPlayer()
    {
        List<RobotConstructor.RobotTemplate> ret = new List<RobotConstructor.RobotTemplate>();
        for (int c = 0; c < LookupList.AllPlayerRobots.Count; c++)
        {
            {
                ret.Add(LookupList.AllPlayerRobots[c]);
            }
        }



        return ret;
    }
    public RobotOwner GetRobotOwner(string email, string passwrod)
    {
        RobotOwner ret = null;

        for (int c = 0; c < LookupList.AllLocalOwners.Count; c++)
        {
            if (LookupList.AllLocalOwners[c].Email.Trim().ToLower() == email.Trim().ToLower() && LookupList.AllLocalOwners[c].Password.Trim() == passwrod.Trim())
            {
                ret = LookupList.AllLocalOwners[c];
                break;
            }
        }

        return ret;

    }

    public void SaveRobot(RobotConstructor.RobotTemplate aTemplate)
    {
        int index = -1;
        for (int c = 0; c < LookupList.AllPlayerRobots.Count; c++)
        {
            if (LookupList.AllPlayerRobots[c].RobotID == aTemplate.RobotID)
            {
                index = c;
                break;
            }
        }
        if (index == -1)
        {
            LookupList.AllPlayerRobots.Add(aTemplate);

        }
        else
        {
            LookupList.AllPlayerRobots[index] = aTemplate;
        }
        RobotOwnerLocalList.SaveToFile(LookupList, Application.persistentDataPath + "/" + fileName);
    }
    public void RemoveRobot(RobotConstructor.RobotTemplate aTemplate)
    {
        int index = -1;
        for (int c = 0; c < LookupList.AllPlayerRobots.Count; c++)
        {
            if (LookupList.AllPlayerRobots[c].RobotID == aTemplate.RobotID)
            {
                index = c;
                break;
            }
        }
        if (index == -1)
        {
           
        }
        else
        {
            LookupList.AllPlayerRobots.RemoveAt(index);
        }
        RobotOwnerLocalList.SaveToFile(LookupList, fileName);
    }
    public void LoadFromFile()
    {
        print("PATH: " + Application.persistentDataPath);
        LookupList = RobotOwnerLocalList.LoadFromFile(Application.persistentDataPath + "/" + fileName);
        if (LookupList == null)
        {
            LookupList = new RobotOwnerLocalList();
            LookupList.AllLocalOwners = new List<RobotOwner>();
            LookupList.AllPlayerRobots = new List<RobotConstructor.RobotTemplate>();
            LookupList.AllHints = new List<xHint>();
            LookupList.MyOptions = new PlayerOptions();
            LookupList.MyPurchases = new PlayerPurchases();
            LookupList.AllNews = new List<xNews>();
            LookupList.RateOfExchange = 1000;

            LookupList.MyPurchases.ComponentsPurchasedIDs = new List<string>();
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add("32ca8647-cd1c-4c84-9770-e601df057e52"); // light paladin
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add("9d53083d-7193-4353-a459-ced488b2ab88"); // gunner cockpit
          //  LookupList.MyPurchases.ComponentsPurchasedIDs.Add("32ca8647-cd1c-4c84-9770-e601df057e52"); // shoulders? todo
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add("aedeac92-bb2a-49f6-925f-b2dd8ae5901d"); // laser 1
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add("aedeac92-bb2a-49f6-925f-b2dd8ae5901d"); // laser 1
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add("4f455305-ca5d-4c98-9ec7-63beb57ff7e9"); // machine gun 1
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add("4f455305-ca5d-4c98-9ec7-63beb57ff7e9"); // machine gun 1
            LookupList.MyPurchases.ComponentsPurchasedIDs.Add(" e3819b88-af29-44b7-81fa-579b961a79f9"); // machine gun 1
           
         
            RobotOwnerLocalList.SaveToFile(LookupList, fileName);
        }

    }
    private void RegisterUser(RobotOwner anOwner)
    {
        anOwner.UniqueID = Guid.NewGuid().ToString();
        LookupList.AllLocalOwners.Add(anOwner);
        RobotOwnerLocalList.SaveToFile(LookupList, fileName);
    }
    private void LoginUser(RobotOwner anOwner)
    {
        anOwner.UniqueID = Guid.NewGuid().ToString();
        LookupList.AllLocalOwners.Add(anOwner);
        RobotOwnerLocalList.SaveToFile(LookupList, fileName);
    }
    [System.Serializable]
    public class RobotOwnerLocalList
    {
        public List<RobotOwner> AllLocalOwners;
        public List<RobotConstructor.RobotTemplate> AllPlayerRobots;
        public PlayerOptions MyOptions;
        public PlayerPurchases MyPurchases;
        public List<xHint> AllHints;
        public List<xNews> AllNews;
        public decimal RateOfExchange;
        public static void SaveToFile(RobotOwnerLocalList aRobot, string fileName)
        {

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName,
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, aRobot);
            stream.Close();
        }
        public static RobotOwnerLocalList LoadFromFile(string fileName)
        {
            RobotOwnerLocalList ret = null;
            if (System.IO.File.Exists(fileName))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                ret = (RobotOwnerLocalList)formatter.Deserialize(stream);
                stream.Close();
            }
            return ret;
        }
    }
    [System.Serializable]
    public class RobotOwner
    {
        public string UniqueID;
        public string Password;
        public string Email;
        public string DisplayName;
        public float TotalCredits;
        public float TotalScore;
        public int Rank;
        public string JoinDateUTC;
        public int TotalMatchesPlayed;
        public int TotalMatchesWon;


    }

    [Serializable]
    public class PlayerOptions
    {
        public float MusicVolume = 100;
        public float BattleVolume = 100;
        public float FXVolume = 100;
        public bool RetrieveRobotImages = true;
        public bool RetrieveRobotIntroTTS = true;
        public int GraphicsLevel=3;
    }
    [Serializable]
    public class PlayerPurchases
    {
        public float TotalMoneySpent = 100;
        public List<string> ComponentsPurchasedIDs = new List<string>();

    }
}
