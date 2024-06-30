using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
public class IDECanvasManager : MonoBehaviour
{
    public CameraEffectsManager TheCameraEffectsManager;
    public CamPickRobot TheCamPicTaker;
    public WwwLeagueInterface www;
    public GameObject ButtonDestroyObject;
    public GameObject ButtonNew;
    public RobotOwnerLookup TheLookup;
    public List<RobotConstructor.RobotTemplate> CurrentRobots;
    public string SelectedRobotID;
    public List<PanelRobotRow> AllSpawnedRobotRows;
    public List<PanelDeviceIoRow> AllSpawnedIoRows;
    public Transform ParentToAttachRowTo;
    public Transform ParentToAttachIORowTo;
    public GameObject BaseRobotRow;
    public GameObject BaseIoRow;
    public RobotConstructor TheConstructor;
    public EnumViewMode ViewMode;
    public CodePanel TheCodePanel;
    public PanelIoMap TheIoPanel;
    public PanelConstruction TheConstructionPanel;
    public PanelLeagueLeaderboards TheLeagueLeadersPanel;
    public PanelDiscussion TheDiscussionPanel;
    public PanelShop TheShopPanel;
    public PanelFinances ThePanelFinances;

    public PanelCoomponentManual TheComponentManualPanel;
    public GameObject TheConstructionManual;

    public PanelBasicInfo TheBasicInfoPanel;
    public PanelLeague TheLeaguePanelCat;
    public GameObject TheLeaguePanel;
    public GameObject ThePracticePanel;
    public PanelOptions TheOptionsPanel;
    public GameObject TheConfirmDestroyPanel;
    public GameObject TopRightPanel;
    public GameObject TopRightPanelMainMenu;
    public enum EnumViewMode
    {
        RobotSelect,
        Construct,
        //  IoMap,
        CodeScreen,
        BasicInfo,
        StatsScreen,
        League,
        Practice,
        Options,
        Discussion,
        LeagueLeaderboards,
        Shop,
        Finances,
    }
    public void ButtonFinancialInfoClick()
    {
        SwitchToMode(EnumViewMode.Finances);
        //  TheLookup.DoPurchase();
    }
    public GameObject PanelRobots;
 
    public static bool DefaultViewLeaderboards = true;
    public static string DefaultRobotLoadID = "";
    public static IDECanvasManager PublicAccess;
    void Start()
    {
        PublicAccess = this;
        ApplySoundOptions();
        ApplyCameraOptions();
        MatchWaitLoadingScreen.SetActive(false);
        AudioController.PlayMusicPlaylist("Default");
        Match.PublicAccess = null;
        www.On_GetXData_Received_UploadRobot += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_UploadRobot);
        www.On_GetXData_Received_RetrieveListOfChallengeableRobots += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_RetrieveListOfChallengeableRobots);
        www.On_GetXData_Received_CreateRobotMatch += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_CreateRobotMatch);
        www.On_GetXData_Received_SendReceiveDiscussion += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_SendReceiveDiscussion);
        www.On_GetXData_Received_UpdateLeagueLeaderboard += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_UpdateLeagueLeaderboard);
        www.On_GetXData_RetrieveFinancials += new WwwLeagueInterface.GetXData_Received(www_On_GetXData_Received_RetrieveFinancials);

        Reload();
        if (DefaultViewLeaderboards)
        {
            SwitchToMode(EnumViewMode.RobotSelect);
            SwitchToMode(EnumViewMode.LeagueLeaderboards);

        }
        else
        {
            if (DefaultRobotLoadID == "")
            {
                SwitchToMode(EnumViewMode.RobotSelect);
            }
            else
            {

                SetSelection(DefaultRobotLoadID);
                SwitchToMode(EnumViewMode.League);
                RefreshLeagueListRequest();

            }
        }



    }
    public void ApplySoundOptions()
    {
        RobotOwnerLookup.PlayerOptions op = TheLookup.GetPlayerOptions();
        if (op != null)
        {
            AudioController.SetCategoryVolume("Music", ((op.MusicVolume + 0.0001f) / 100f) * 0.8f);
            AudioController.SetCategoryVolume("FX", ((op.MusicVolume + 0.0001f) / 100f) * 0.99f);
        }
    }


    public void ApplyCameraOptions()
    {
        RobotOwnerLookup.PlayerOptions op = TheLookup.GetPlayerOptions();
        if (op != null)
        {
            TheCameraEffectsManager.SetOptions(op.GraphicsLevel);

        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    void SwitchToMode(EnumViewMode aMode)
    {
        ButtonDestroyObject.gameObject.SetActive(false);
        ButtonNew.gameObject.SetActive(false);
        ClearIORows();
        switch (aMode)
        {
            case EnumViewMode.Finances:
                {


                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                    }
                    SelectedRobotID = "";
                    PanelRobots.SetActive(false);
                    TheCodePanel.gameObject.SetActive(false);
                    TheIoPanel.gameObject.SetActive(false);
                    TheConstructionPanel.gameObject.SetActive(false);
                    TheComponentManualPanel.gameObject.SetActive(false);
                    TheBasicInfoPanel.gameObject.SetActive(false);
                    TheLeaguePanel.gameObject.SetActive(false);
                    ThePracticePanel.gameObject.SetActive(false);
                    TheOptionsPanel.gameObject.SetActive(false);
                    TheLeagueLeadersPanel.gameObject.SetActive(false);
                    TheDiscussionPanel.gameObject.SetActive(false);
                    TheShopPanel.gameObject.SetActive(false);
                    ThePanelFinances.gameObject.SetActive(true);

                    SwitchToMode(EnumViewMode.RobotSelect);
                   
                    break;
                }
            case EnumViewMode.Shop:
                {


                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                    }
                    SelectedRobotID = "";
                    PanelRobots.SetActive(false);
                    TheCodePanel.gameObject.SetActive(false);
                    TheIoPanel.gameObject.SetActive(false);
                    TheConstructionPanel.gameObject.SetActive(false);
                    TheComponentManualPanel.gameObject.SetActive(false);
                    TheBasicInfoPanel.gameObject.SetActive(false);
                    TheLeaguePanel.gameObject.SetActive(false);
                    ThePracticePanel.gameObject.SetActive(false);
                    TheOptionsPanel.gameObject.SetActive(false);
                    TheLeagueLeadersPanel.gameObject.SetActive(false);
                    TheDiscussionPanel.gameObject.SetActive(false);
                    TheShopPanel.gameObject.SetActive(true);
                    ThePanelFinances.gameObject.SetActive(false);

                    SwitchToMode(EnumViewMode.RobotSelect);
                    
                    break;
                }
            case EnumViewMode.LeagueLeaderboards:
                {


                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                    }
                    SelectedRobotID = "";
                    PanelRobots.SetActive(false);
                    TheCodePanel.gameObject.SetActive(false);
                    TheIoPanel.gameObject.SetActive(false);
                    TheConstructionPanel.gameObject.SetActive(false);
                    TheComponentManualPanel.gameObject.SetActive(false);
                    TheBasicInfoPanel.gameObject.SetActive(false);
                    TheLeaguePanel.gameObject.SetActive(false);
                    ThePracticePanel.gameObject.SetActive(false);
                    TheOptionsPanel.gameObject.SetActive(false);
                    TheLeagueLeadersPanel.gameObject.SetActive(true);
                    TheDiscussionPanel.gameObject.SetActive(false);
                    TheShopPanel.gameObject.SetActive(false);
                    ThePanelFinances.gameObject.SetActive(false);
                    SwitchToMode(EnumViewMode.RobotSelect);
                    
                    break;
                }
            case EnumViewMode.Discussion:
                {


                   if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                    }
                    SelectedRobotID = "";
                    PanelRobots.SetActive(false);
                    TheCodePanel.gameObject.SetActive(false);
                    TheIoPanel.gameObject.SetActive(false);
                    TheConstructionPanel.gameObject.SetActive(false);
                    TheComponentManualPanel.gameObject.SetActive(false);
                    TheBasicInfoPanel.gameObject.SetActive(false);
                    TheLeaguePanel.gameObject.SetActive(false);
                    ThePracticePanel.gameObject.SetActive(false);
                    TheOptionsPanel.gameObject.SetActive(false);
                    TheLeagueLeadersPanel.gameObject.SetActive(false);
                    TheDiscussionPanel.gameObject.SetActive(true);
                    TheShopPanel.gameObject.SetActive(false);
                    ThePanelFinances.gameObject.SetActive(false);
                    SwitchToMode(EnumViewMode.RobotSelect);

                   
                    break;
                }
            case EnumViewMode.Options:
                {


                    TopRightPanel.gameObject.SetActive(false);
                    TopRightPanelMainMenu.gameObject.SetActive(false);
                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                    }
                    SelectedRobotID = "";
                    PanelRobots.SetActive(false);
                    TheCodePanel.gameObject.SetActive(false);
                    TheIoPanel.gameObject.SetActive(false);
                    TheConstructionPanel.gameObject.SetActive(false);
                    TheComponentManualPanel.gameObject.SetActive(false);
                    TheBasicInfoPanel.gameObject.SetActive(false);
                    TheLeaguePanel.gameObject.SetActive(false);
                    ThePracticePanel.gameObject.SetActive(false);
                    RobotOwnerLookup.PlayerOptions op = TheLookup.GetPlayerOptions();


                    TheOptionsPanel.SetOptions(op);
                    TheOptionsPanel.gameObject.SetActive(true);

                    TheConstructor.IsInputOn = false;


                    ButtonNew.gameObject.SetActive(false);

                    break;
                }
            case EnumViewMode.RobotSelect:
                {


                    TopRightPanel.gameObject.SetActive(false);
                    TopRightPanelMainMenu.gameObject.SetActive(true);
                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                    }
                    SelectedRobotID = "";
                    PanelRobots.SetActive(true);
                    TheCodePanel.gameObject.SetActive(false);
                    TheIoPanel.gameObject.SetActive(false);
                    TheConstructionPanel.gameObject.SetActive(false);
                    TheComponentManualPanel.gameObject.SetActive(false);
                    TheBasicInfoPanel.gameObject.SetActive(false);
                    TheLeaguePanel.gameObject.SetActive(false);
                    ThePracticePanel.gameObject.SetActive(false);
                    TheOptionsPanel.gameObject.SetActive(false);
                   
                    TheConstructor.IsInputOn = false;

                    if (AllSpawnedRobotRows.Count >= 16)
                    {
                        ButtonNew.gameObject.SetActive(false);
                    }
                    else
                    {
                        ButtonNew.gameObject.SetActive(true);

                    }
                    break;
                }
            case EnumViewMode.Construct:
                {
                    TopRightPanel.gameObject.SetActive(true);
                    TopRightPanelMainMenu.gameObject.SetActive(false);
                    if (SelectedRobotID.Length > 0)
                    {
                        TheConstructor.SwitchToNextPartAcceptor(-1, true);
                        TheConstructor.SwitchToNextPartAcceptor(1, true);

                        PanelRobots.SetActive(false);
                        TheIoPanel.gameObject.SetActive(false);
                        TheCodePanel.gameObject.SetActive(false);
                        TheConstructor.IsInputOn = true;
                        TheComponentManualPanel.gameObject.SetActive(false);
                        TheBasicInfoPanel.gameObject.SetActive(false);
                        TheLeaguePanel.gameObject.SetActive(false);
                        ThePracticePanel.gameObject.SetActive(false);
                        TheOptionsPanel.gameObject.SetActive(false);
                        TheLeagueLeadersPanel.gameObject.SetActive(false);
                        TheDiscussionPanel.gameObject.SetActive(false);
                        TheShopPanel.gameObject.SetActive(false);
                        ThePanelFinances.gameObject.SetActive(false);
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }

                        TheConstructionPanel.gameObject.SetActive(true);

                        TheConstructionPanel.ButtonPreviousAcceptorClick();
                        TheConstructionPanel.ButtonNextAcceptorClick();
                        TheConstructionPanel.ToggleSkip.isOn = true;
                        TheConstructionPanel.RefreshUi();
                    }
                    break;
                }
            case EnumViewMode.CodeScreen:
                {
                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }

                        TopRightPanel.gameObject.SetActive(true);
                        TopRightPanelMainMenu.gameObject.SetActive(false);
                        PanelRobots.SetActive(false);
                        TheComponentManualPanel.gameObject.SetActive(false);
                        TheConstructionPanel.gameObject.SetActive(false);
                        TheIoPanel.gameObject.SetActive(true);
                        TheCodePanel.gameObject.SetActive(true);
                        TheOptionsPanel.gameObject.SetActive(false);
                        TheConstructor.IsInputOn = false;
                        TheBasicInfoPanel.gameObject.SetActive(false);
                        TheLeaguePanel.gameObject.SetActive(false);
                        ThePracticePanel.gameObject.SetActive(false);
                        TheLeagueLeadersPanel.gameObject.SetActive(false);
                        TheDiscussionPanel.gameObject.SetActive(false);
                        TheShopPanel.gameObject.SetActive(false);
                        ThePanelFinances.gameObject.SetActive(false);

                    }

                    break;
                }
            case EnumViewMode.BasicInfo:
                {
                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }
                        TopRightPanel.gameObject.SetActive(true);
                        TopRightPanelMainMenu.gameObject.SetActive(false);

                        PanelRobots.SetActive(false);
                        TheComponentManualPanel.gameObject.SetActive(false);
                        TheConstructionPanel.gameObject.SetActive(false);
                        TheIoPanel.gameObject.SetActive(false);
                        TheCodePanel.gameObject.SetActive(false);
                        TheOptionsPanel.gameObject.SetActive(false);
                        TheConstructor.IsInputOn = false;
                        TheBasicInfoPanel.gameObject.SetActive(true);
                        TheLeaguePanel.gameObject.SetActive(false);
                        ThePracticePanel.gameObject.SetActive(false);
                        TheLeagueLeadersPanel.gameObject.SetActive(false);
                        TheDiscussionPanel.gameObject.SetActive(false);
                        TheShopPanel.gameObject.SetActive(false);
                        ThePanelFinances.gameObject.SetActive(false);

                    }

                    break;
                }
            case EnumViewMode.League:
                {
                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }

                        TopRightPanel.gameObject.SetActive(true);
                        TopRightPanelMainMenu.gameObject.SetActive(false);
                        PanelRobots.SetActive(false);
                        TheComponentManualPanel.gameObject.SetActive(false);
                        TheConstructionPanel.gameObject.SetActive(false);
                        TheIoPanel.gameObject.SetActive(false);
                        TheCodePanel.gameObject.SetActive(false);
                        TheOptionsPanel.gameObject.SetActive(false);
                        TheConstructor.IsInputOn = false;
                        TheBasicInfoPanel.gameObject.SetActive(false);
                        TheLeaguePanel.gameObject.SetActive(true);
                        ThePracticePanel.gameObject.SetActive(false);

                        TheLeagueLeadersPanel.gameObject.SetActive(false);
                        TheDiscussionPanel.gameObject.SetActive(false);
                        TheShopPanel.gameObject.SetActive(false);
                        ThePanelFinances.gameObject.SetActive(false);
                    }

                    break;
                }
            case EnumViewMode.Practice:
                {
                    if (SelectedRobotID.Length > 0)
                    {
                        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
                        if (row != null)
                        {
                            TheCodePanel.TheCode.text = row.TheRobot.MainFunctionCode;
                            SetIoMappingForRobot(row.TheRobot);
                            try
                            {
                                TheBasicInfoPanel.InputRobotColorB.text = row.TheRobot.ColorB.ToString();
                                TheBasicInfoPanel.InputRobotColorR.text = row.TheRobot.ColorR.ToString();
                                TheBasicInfoPanel.InputRobotColorG.text = row.TheRobot.ColorG.ToString();
                                TheBasicInfoPanel.InputRobotName.text = row.TheRobot.RobotName;
                                TheBasicInfoPanel.InputRobotTeamPassword.text = row.TheRobot.TeamPassword;
                            }
                            catch
                            {

                            }
                        }

                        TopRightPanel.gameObject.SetActive(true);
                        TopRightPanelMainMenu.gameObject.SetActive(false);
                        PanelRobots.SetActive(false);
                        TheComponentManualPanel.gameObject.SetActive(false);
                        TheConstructionPanel.gameObject.SetActive(false);
                        TheIoPanel.gameObject.SetActive(false);
                        TheCodePanel.gameObject.SetActive(false);
                        TheOptionsPanel.gameObject.SetActive(false);
                        TheConstructor.IsInputOn = false;
                        TheBasicInfoPanel.gameObject.SetActive(false);
                        TheLeaguePanel.gameObject.SetActive(false);
                        ThePracticePanel.gameObject.SetActive(true);
                        TheLeagueLeadersPanel.gameObject.SetActive(false);
                        TheDiscussionPanel.gameObject.SetActive(false);
                        TheShopPanel.gameObject.SetActive(false);
                        ThePanelFinances.gameObject.SetActive(false);
                    }

                    break;
                }
            
        }
    }
    private void ClearIORows()
    {
        if (AllSpawnedIoRows == null) AllSpawnedIoRows = null;

        for (int c = 0; c < AllSpawnedIoRows.Count; c++)
        {
            Destroy(AllSpawnedIoRows[c].gameObject);
        }
        AllSpawnedIoRows.Clear();
    }
    private void SetIoMappingForRobot(RobotConstructor.RobotTemplate aRobot)
    {
        for (int c = 0; c < aRobot.ModuleList.Count; c++)
        {
            GameObject pref = TheConstructor.GetPrefabByID(aRobot.ModuleList[c].ComponentID);
            IOHandler anIo = pref.GetComponent<IOHandler>();
            if (anIo != null)
            {
                SpawnRow(aRobot.ModuleList[c], anIo);
            }
        }

    }

    public void ButtonCloseOptions()
    {
        RobotOwnerLookup.PlayerOptions op = TheOptionsPanel.GetOptions();
        TheLookup.SetPlayerOptions(op);
       
        ApplyCameraOptions();
        ApplySoundOptions();
        SwitchToMode(EnumViewMode.RobotSelect);
    }
    GameObject curSleRobotObject;
    public void SetSelection(string robotID)
    {
        SelectedRobotID = robotID;
        TheLeaguePanel.gameObject.SetActive(false);
        TheLeaguePanelCat.allTemplatesToShowLEAGUE = new List<xRobot>();
        TheLeaguePanelCat.RefreshXRobot();
        print("Sel " + robotID);
        PanelRobotRow row = GetRowForRobot(robotID);
        if (row != null)
        {
            try
            {
                curSleRobotObject = TheConstructor.LoadRobot(TheConstructor.BuildPlatformTarget, row.TheRobot);
                TheCodePanel.TheComputer = curSleRobotObject.GetComponent<Computer>();
                TheCamPicTaker.TheTarget = curSleRobotObject;
            }
            catch
            {

            }
        }
        SwitchToMode(EnumViewMode.Construct);
    }
    private void Reload()
    {
        CurrentRobots = TheLookup.GetAllTemplatesForCurrentPlayer();
        RefreshUI();



    }
    private void RefreshUI()
    {
        for (int c = 0; c < CurrentRobots.Count; c++)
        {
            CurrentRobots[c].LeagueOwnerID = WwwLeagueInterface.LoggedInUserID; //; RobotOwnerLookup.UserID;

            PanelRobotRow aRow = GetRowForRobot(CurrentRobots[c].RobotID);
            if (aRow == null)
            {
                aRow = SpawnRow(CurrentRobots[c]);
            }

        }
        if (AllSpawnedRobotRows.Count >= 16)
        {
            ButtonNew.gameObject.SetActive(false);
        }
        else
        {
            ButtonNew.gameObject.SetActive(true);

        }

    }
    public PanelRobotRow GetRowForRobot(string id)
    {
        PanelRobotRow ret = null;
        if (AllSpawnedRobotRows == null) AllSpawnedRobotRows = new List<PanelRobotRow>();
        for (int c = 0; c < AllSpawnedRobotRows.Count; c++)
        {
            if (AllSpawnedRobotRows[c].TheRobot.RobotID == id)
            {
                ret = AllSpawnedRobotRows[c];
                break;
            }

        }


        return ret;
    }
    PanelRobotRow SpawnRow(RobotConstructor.RobotTemplate aTemplate)
    {
        PanelRobotRow ret = null;

        GameObject aSpawn = (GameObject)Instantiate(BaseRobotRow, BaseRobotRow.transform.position, BaseRobotRow.transform.rotation);
        aSpawn.transform.SetParent(ParentToAttachRowTo);
        aSpawn.transform.localPosition = Vector3.zero;
        RectTransform aRect = aSpawn.GetComponent<RectTransform>();
        aRect.anchorMin = new Vector2(0, 0);
        aRect.offsetMin = new Vector2(0, 0);
        aRect.offsetMax = new Vector2(0, AllSpawnedRobotRows.Count * -1 * 30);
        ret = aSpawn.GetComponent<PanelRobotRow>();
        ret.TheRobot = aTemplate;
        ret.TheCanvas = this;

        AllSpawnedRobotRows.Add(ret);
        return ret;
    }
    PanelDeviceIoRow SpawnRow(RobotConstructor.DataComponentType aTemplate, IOHandler theIOHandler)
    {
        PanelDeviceIoRow ret = null;
        ComponentType comonentType = theIOHandler.gameObject.GetComponent<ComponentType>();

        FunctionsContainer[] interfaces = comonentType.gameObject.GetInterfaces<FunctionsContainer>();

        for (int c = 0; c < interfaces.Length; c++)
        {
            interfaces[c].SetComponentFunctions(comonentType, theIOHandler.IONumber.ToString());
        }
        GameObject aSpawn = (GameObject)Instantiate(BaseIoRow, BaseIoRow.transform.position, BaseIoRow.transform.rotation);
        aSpawn.transform.SetParent(ParentToAttachIORowTo);
        aSpawn.transform.localPosition = Vector3.zero;
        RectTransform aRect = aSpawn.GetComponent<RectTransform>();
      
        aRect.offsetMin = new Vector2(0, (AllSpawnedIoRows.Count + 2) * -1 * 60);
        aRect.offsetMax = new Vector2(0, (AllSpawnedIoRows.Count + 2) * -1 * 60);

        ret = aSpawn.GetComponent<PanelDeviceIoRow>();
        ret.TheRobotComponent = aTemplate;
        ret.TheCanvas = this;
        ret.PrefebRepresenting = TheConstructor.GetPrefabByID(aTemplate.ComponentID);
        ret.PanelIoMap = TheIoPanel;
        ret.HelpManual = comonentType;
        ret.SetRow();
        AllSpawnedIoRows.Add(ret);
        return ret;
    }
    public void ButtonNewRobot()
    {
        if (AllSpawnedRobotRows == null) AllSpawnedRobotRows = new List<PanelRobotRow>();
        {
            CurrentRobots.Add(new RobotConstructor.RobotTemplate() { LeagueOwnerID = WwwLeagueInterface.LoggedInUserID, RobotName = "", RobotID = System.Guid.NewGuid().ToString(), MainFunctionCode = "", ModuleList = new List<RobotConstructor.DataComponentType>(), FileName = "na", ColorB = 1, ColorG = 1, ColorR = 1 });
        }
        RefreshUI();
    }

    public void ButtonSave()
    {

        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
        if (row != null)
        {
            bool donePic = false;
            if (TheCamPicTaker.TheTarget != null)
            {
                //TheCamPicTaker.TheTarget = TheConstructor.BuildPlatformTarget.gameObject

                currentPic = TheCamPicTaker.TakePicture("Selected");
                donePic = true;

            }
            RobotConstructor.RobotTemplate temp = TheConstructor.GetRobotTemplate();
            row.TheRobot.ModuleList = temp.ModuleList;
            row.TheRobot.MainFunctionCode = TheCodePanel.TheCode.text;
            row.TheRobot.ColorB = temp.ColorB;
            row.TheRobot.ColorG = temp.ColorG;
            row.TheRobot.ColorR = temp.ColorR;
            row.TheRobot.RobotName = TheBasicInfoPanel.InputRobotName.text;
            row.TheRobot.TeamPassword = TheBasicInfoPanel.InputRobotTeamPassword.text;
            row.TheRobot.ScreenShot = currentPic;
            row.TheRobot.CalculateCodeProcessedPerSecond = row.TheRobot.CalculateCodeProcessedPerSecondValue(TheConstructor,temp);
            row.TheRobot.CalculateTotalWeight = row.TheRobot.CalculateTotalWeightValue(TheConstructor, temp);
            row.TheRobot.CalculateLinesOfCode = row.TheRobot.CalculateLinesOfCodeValue(TheConstructor, temp);
            row.TheRobot.CalculateMaxSpeed = row.TheRobot.CalculateMaxSpeedValue(TheConstructor, temp, row.TheRobot.CalculateTotalWeight);
            row.TheRobot.CalculateSqrMeterSize = row.TheRobot.CalculateSqrMeterSizeValue(TheConstructor.BuildPlatformTarget.gameObject);
            row.TheRobot.CalculateTotalArmour = row.TheRobot.CalculateCalculateTotalArmourValue(TheConstructor, temp);
          
         
            Computer pc = TheConstructor.BuildPlatformTarget.GetComponentInChildren<Computer>();
            if (pc != null && row.TheRobot != null)
            {
                pc.CodeText = row.TheRobot.MainFunctionCode;
                for (int c = 0; c < AllSpawnedIoRows.Count; c++)
                {
                    for (int x = 0; x < temp.ModuleList.Count; x++)
                    {
                        if (AllSpawnedIoRows[c].TheRobotComponent.ComponentID == temp.ModuleList[x].ComponentID)
                        {
                            if (AllSpawnedIoRows[c].TheRobotComponent.ParentAcceptorIDs.Count == 0)
                            {

                                temp.ModuleList[x].IoNumberCustomMap = AllSpawnedIoRows[c].InputIoNumber.text;

                            }
                            else
                            {
                                string last1 = AllSpawnedIoRows[c].TheRobotComponent.ParentAcceptorIDs[0];
                                string last2 = temp.ModuleList[x].ParentAcceptorIDs[0];
                                if (last1 == last2)
                                {

                                    temp.ModuleList[x].IoNumberCustomMap = AllSpawnedIoRows[c].InputIoNumber.text;

                                }
                            }
                        }
                    }
                }

                print("Robot ID: " + row.TheRobot.LeagueID);

                TheLookup.SaveRobot(row.TheRobot);
                if (donePic == false)
                {
                    if (TheCamPicTaker.TheTarget == null)
                    {
                        //TheCamPicTaker.TheTarget = TheConstructor.BuildPlatformTarget.gameObject
                        SetSelection(row.TheRobot.RobotID);

                        currentPic = TheCamPicTaker.TakePicture("Selected");
                        row.TheRobot.ScreenShot = currentPic;
                        TheLookup.SaveRobot(row.TheRobot);
                        donePic = true;
                    }

                }
                //  Reload();

            }
        }
    }
    public void ButtonDestroy()
    {

        TheConfirmDestroyPanel.SetActive(true);
    }

    public void ButtonConfirmDestroy()
    {
        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
        if (row != null)
        {
            RobotConstructor.RobotTemplate temp = TheConstructor.GetRobotTemplate();
            row.TheRobot.ModuleList = temp.ModuleList;
            row.TheRobot.MainFunctionCode = TheCodePanel.TheCode.text;
            row.TheRobot.ColorB = temp.ColorB;
            row.TheRobot.ColorG = temp.ColorG;
            row.TheRobot.ColorR = temp.ColorR;
            row.TheRobot.RobotName = TheBasicInfoPanel.InputRobotName.text;
            row.TheRobot.TeamPassword = TheBasicInfoPanel.InputRobotTeamPassword.text;
            TheLookup.RemoveRobot(row.TheRobot);
            
            try
            {
                int index = -1;
                for (int c = 0; c < AllSpawnedRobotRows.Count; c++)
                {
                    if (AllSpawnedRobotRows[c].TheRobot.RobotID == SelectedRobotID)
                    {
                        index = c;
                        break;
                    }
                }
                if (index >= 0)
                {
                    Destroy(AllSpawnedRobotRows[index].gameObject);
                    AllSpawnedRobotRows.RemoveAt(index);
                }
            }
            catch
            {

            }
            try
            {
                Destroy(TheConstructor.BuildPlatformTarget.GetChild(0).gameObject);
            }
            catch
            {

            }
            Reload();
            SwitchToMode(EnumViewMode.RobotSelect);


           

        }

        TheConfirmDestroyPanel.SetActive(false);
    }
    public void ButtonOptionsClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.Options);

    }

    public void ButtonCancelDestroy()
    {
        TheConfirmDestroyPanel.SetActive(false);
    }
    public void ButtonConstructClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.Construct);

    }

    public void ButtonCodeClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.CodeScreen);

    }
    public void ButtonMetaClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.BasicInfo);

    }
    public void ButtonStatsClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.StatsScreen);

    }
    public void ButtonDoneClick()
    {

        ButtonSave();
        SwitchToMode(EnumViewMode.LeagueLeaderboards);

    }
    byte[] currentPic;
    public void ButtonLeagueClick()
    {

        ButtonSave();
        if (currentPic != null)
        {
            print("Pic Dta Length: " + currentPic.Length);
            SwitchToMode(EnumViewMode.League);
        }

    }
    public void ButtonPracticeClick()
    {

        ButtonSave();
        SwitchToMode(EnumViewMode.Practice);

    }
    public void ButtonTestClick()
    {
        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
        if (row != null)
        {
            ButtonSave();
            Match.RobotsInMatch = new List<RobotConstructor.RobotTemplate>();
            Match.RobotsInMatch.Add(row.TheRobot);
            ChaneToMatchScene(null, 0);
        }
    }

    public void ChaneToMatchScene(xArena anARena, float prizeMoney)
    {
        Match.PrizeMoneyLeague = prizeMoney;
        Match.CurrentArena = anARena;
        SceneManager.LoadScene("MatchLoadScene", LoadSceneMode.Single);

    }
    public void ButtonOpenComponentManualFromConstructionPanelClick()
    {
        ComponentType aCom = null;

        if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.transform.childCount != 0)
        {
            aCom = TheConstructor.CurrentAttachTarget.transform.gameObject.GetComponentInChildren<ComponentType>();


        }
        else
        {
            if (TheConstructor.PartInHand != null)
            {
                aCom = TheConstructor.PartInHand.transform.gameObject.GetComponentInChildren<ComponentType>();

            }
        }
        if (aCom != null)
        {
            TheComponentManualPanel.SetComponent(aCom);
            TheComponentManualPanel.gameObject.SetActive(true);
        }
    }
    public void ButtonCloseComponentManualClick()
    {
        TheComponentManualPanel.gameObject.SetActive(false);
    }
    public void ButtonOpenConstructionManualClick()
    {
        TheConstructionManual.gameObject.SetActive(true);
    }
    public void ButtonCloseConstructionManualClick()
    {
        TheConstructionManual.gameObject.SetActive(false);
    }
    public void ButtonUploadRobot()
    {
        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
        if (row != null)
        {
            if (row.TheRobot != null)
            {
               
                if (www.IsBusy == false)
                {
                    TheLeaguePanelCat.SetLoadingScreenOn();

                    www.UploadRobot(row.TheRobot, currentPic);
                }
            }

        }
    }

    public void RefreshLeagueListRequest()
    {
        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
        if (row != null)
        {
            if (row.TheRobot.LeagueID > 0)
            {
                www.RetrieveListOfChallengeableRobots(row.TheRobot.LeagueID);
            }
        }

    }
    public GameObject MatchWaitLoadingScreen;
    public void CreateLeagueMatchRequest(List<int> robotLeagueIDs, bool ViewedFromLeaderboards, int focusOnRobotID, int extraRobot)
    {
        if (robotLeagueIDs.Distinct().Count() >= 1)
        {
            MatchWaitLoadingScreen.SetActive(true);

            IDECanvasManager.DefaultViewLeaderboards = ViewedFromLeaderboards;
            Match.PlayAsRobotID = focusOnRobotID;
            www.CreateRobotMatch(robotLeagueIDs, extraRobot);
        }

    }
    void www_On_GetXData_Received_CreateRobotMatch(XData aData)
    {
        if (aData.CreateRobotMatch.Success)
        {

            Match.RobotsInMatch = new List<RobotConstructor.RobotTemplate>();
            for (int c = 0; c < aData.CreateRobotMatch.RobotsInMatch.Count; c++)
            {
                Match.IsLeagueMatch = true;
                Match.LeagueMatchID = aData.CreateRobotMatch.MatchID;
                Match.RobotsInMatch.Add(aData.CreateRobotMatch.RobotsInMatch[c].RobotTemplate.FullTemplate);
             }

            DefaultRobotLoadID = SelectedRobotID;
            ChaneToMatchScene(aData.CreateRobotMatch.TheArena, aData.CreateRobotMatch.PrizeMoney);
        }
        else
        {
            print("ERR: "+aData.CreateRobotMatch.ResultText);
     
        }

        //  throw new System.NotImplementedException();
    }

    void www_On_GetXData_Received_RetrieveListOfChallengeableRobots(XData aData)
    {


        TheLeaguePanelCat.allTemplatesToShowLEAGUE = aData.RetrieveListOfChallengeableRobots.RobotsInMatch;
        TheLeaguePanelCat.RefreshXRobot();
        PanelRobotRow row = GetRowForRobot(SelectedRobotID);
        if (row != null)
        {
            xRobot aRob = aData.RetrieveListOfChallengeableRobots.RobotsInMatch.FirstOrDefault(x => x.ID == row.TheRobot.LeagueID);
            int index = aData.RetrieveListOfChallengeableRobots.RobotsInMatch.IndexOf(aRob);
            if (index > 0)
            {
                int page = (int)((float)index / (float)5);
                TheLeaguePanelCat.currentPage = page;
                TheLeaguePanelCat.RefreshXRobot();
            }

        }
        TheLeaguePanel.gameObject.SetActive(true);

       }

    void www_On_GetXData_Received_UploadRobot(XData aData)
    {
        if (aData.UploadRobot.Success)
        {
            PanelRobotRow row = GetRowForRobot(SelectedRobotID);
            if (row != null)
            {
                row.TheRobot.LeagueID = aData.UploadRobot.RobotID;
                print("Robot ID: " + row.TheRobot.LeagueID);
                ButtonSave();
                RefreshLeagueListRequest();
            }
        }
     }

    void www_On_GetXData_Received_SendReceiveDiscussion(XData aData)
    {
        if (aData.SendReceiveDiscussion.Success)
        {

            TheDiscussionPanel.AddDiscussions(aData.SendReceiveDiscussion.AllDiscussions);


        }else
        {


        }
    }
    void www_On_GetXData_Received_UpdateLeagueLeaderboard(XData aData)
    {
        if (aData.UpdateLeagueLeaderboard.Success)
        {

            TheLeagueLeadersPanel.SetInfo(aData.UpdateLeagueLeaderboard);


        }
    }


    void www_On_GetXData_Received_RetrieveFinancials(XData aData)
    {
        if (aData.RetrieveFinancials.Success)
        {
            ThePanelFinances.SetInfo(aData.RetrieveFinancials);


        }
    }

    public void ButtonExit()
    {
        Application.Quit();
    }


    public void ButtonLeagueLeaderboardClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.LeagueLeaderboards);

    }
    public void ButtonDiscussionClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.Discussion);

    }
    public void ButtonShopClick()
    {
        ButtonSave();
        SwitchToMode(EnumViewMode.Shop);

    }
}
