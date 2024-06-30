using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoginCanvasLogic : MonoBehaviour
{
    public WwwLeagueInterface www;
    public GameObject PanelLogin;
    public GameObject PanelRegister;
    public GameObject PanelLoading;
    public InputField inputLoginEmail;
    public InputField inputLoginPassword;
    public InputField inputRegisterEmail;
    public InputField inputRegisterPassword;
    public InputField inputRegisterPasswordConfirm;
    public InputField inputRegisterDisplayName;

    public Text TextVersion;
    public Text TextLoginResult;
    public Text TextRegisterResult;
    public Text TextServerrResult;

    public Button ButtonRegister;
    // Use this for initialization
    void Start()
    {
        TextVersion.text = "V1."+WwwLeagueInterface.GameVersion + "";
        www.On_GetXData_Received_Login += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_Login);
        www.On_GetXData_Received_Register += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_Register);
        ButtonSwitchToLoading();
        bool success = LoadCredentials();
      
       
        if (WwwLeagueInterface.PlatformForRelease == xActionRequest.EnumGamePlatformType.UWSPC)
        {
            ButtonRegister.gameObject.SetActive(true);
            if (success)
            {
                www.Login();
            }
            else
            {
                ButtonSwitchToLoginClick();
            }
            //  ButtonSwitchToLoginClick();
        }
        else
        {
            ButtonRegister.gameObject.SetActive(false);
            
        }

    }
    public void ButtonExitClick()
    {
        Application.Quit();
    }
    public bool LoadCredentials()
    {
        bool ret = false;
        string fileName = "cred.dat";
        string folder = Application.persistentDataPath + "//";
        if (System.IO.File.Exists(folder + fileName))
        {
            string[] lines = System.IO.File.ReadAllLines(folder + fileName);
            WwwLeagueInterface.LoggedInEmail = lines[0];
            WwwLeagueInterface.LoggedInPassword = lines[1];
            inputLoginEmail.text = lines[0];
            inputLoginPassword.text = lines[1];
            ret = true;
        }

        return ret;

    }
    private void SaveCredentials()
    {

        string fileName = "cred.dat";
        string folder = Application.persistentDataPath + "//";
        string[] lines = new string[] { inputLoginEmail.text, inputLoginPassword.text };
        System.IO.File.WriteAllLines(folder + fileName, lines);
    }
    void www_On_GetXData_Received_Register(XData aData)
    {
        TextRegisterResult.text = aData.Register.ResultText;
        ButtonSwitchToLoginClick();
    }

    void www_On_GetXData_Received_Login(XData aData)
    {
        if (aData.Login.Success == false)
        {
          
            TextLoginResult.text = aData.Login.ResultText;
            ButtonSwitchToLoginClick();
        }
        else
        {
            WwwLeagueInterface.LoggedInUserID = aData.Login.PlayerID;
            SaveCredentials();
            WwwLeagueInterface.LastLoginResult = aData.Login;
            SceneManager.LoadScene("IDEScene");
        }
        //  throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        if (WwwLeagueInterface.PlatformForRelease == xActionRequest.EnumGamePlatformType.SteamPC)
        {
            if (SteamManager.IsSteamError)
            {
                TextServerrResult.text = "Unable to connect to Steam.";
            }
        }

    }
    public void ButtonSwitchToLoading()
    {
        PanelLogin.gameObject.SetActive(false);
        PanelRegister.gameObject.SetActive(false);
        PanelLoading.gameObject.SetActive(true);
    }
    public void ButtonSwitchToRegisterClick()
    {
        PanelLogin.gameObject.SetActive(false);
        PanelRegister.gameObject.SetActive(true);
        PanelLoading.gameObject.SetActive(false);
    }
    public void ButtonSwitchToLoginClick()
    {
        PanelLogin.gameObject.SetActive(true);
        PanelRegister.gameObject.SetActive(false);
        PanelLoading.gameObject.SetActive(false);
    }
    public void ButtonLoginClick()
    {
        TextLoginResult.text = "";
        WwwLeagueInterface.LoggedInEmail = inputLoginEmail.text;
        WwwLeagueInterface.LoggedInPassword = inputLoginPassword.text;
        ButtonSwitchToLoading(); 
        www.Login();
     
    }
    public void ButtonRegisterClick()
    {

        TextRegisterResult.text = "";

        WwwLeagueInterface.LoggedInEmail = inputRegisterEmail.text;
        WwwLeagueInterface.LoggedInPassword = inputRegisterPassword.text;

        inputLoginEmail.text = inputRegisterEmail.text;
        inputLoginPassword.text = inputRegisterPassword.text;

        www.Register(inputRegisterDisplayName.text);
    }
}
