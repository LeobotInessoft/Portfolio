using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CodePanel : MonoBehaviour
{

    public InputField TheCode;
   public Computer TheComputer;
    public GameObject ManualPanel;
    public static CodePanel PublicAccess;
    public bool ShowCodeForRobot = true;
    public bool IsAllowedToChangeCode = true;
    public GameObject PanelDebugger;
    public GameObject PanelCodeGenerator;
    RobotMeta theMeta;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        SwitchPanel(EnumSidePanelType.CodeGenerator);

    }
    void OnEnable()
    {
        SwitchPanel(EnumSidePanelType.CodeGenerator);

    }
    public void ButtonDebuggerClick()
    {
        IDECanvasManager.PublicAccess.ButtonSave();
        SwitchPanel(EnumSidePanelType.Debugger);
    }
    public void ButtonCodeGenClick()
    {
        IDECanvasManager.PublicAccess.ButtonSave();
        SwitchPanel(EnumSidePanelType.CodeGenerator);
    }
    public enum EnumSidePanelType
    {
        Debugger,
        CodeGenerator
    }
    private void SwitchPanel(EnumSidePanelType aType)
    {
        if (PanelDebugger != null && PanelCodeGenerator != null)
        {
            switch (aType)
            {
                case EnumSidePanelType.Debugger:
                    {
                        PanelDebugger.gameObject.SetActive(true);
                        PanelCodeGenerator.gameObject.SetActive(false);
                        break;
                    }
                case EnumSidePanelType.CodeGenerator:
                    {
                        PanelDebugger.gameObject.SetActive(false);
                        PanelCodeGenerator.gameObject.SetActive(true);
                        break;
                    }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (TheCode.gameObject.activeSelf != ShowCodeForRobot)
        {
            TheCode.gameObject.SetActive(ShowCodeForRobot);
        }
        if (TheComputer != null)
        {
            List<RuntimeVariableText> allTexts = new List<RuntimeVariableText>();
            allTexts.AddRange(gameObject.GetComponentsInChildren<RuntimeVariableText>());
            for (int c = 0; c < allTexts.Count; c++)
            {
                Computer.Variable aVar = TheComputer.runtimeStack.FindVariableFromName(allTexts[c].VariableName);
                allTexts[c].RefreshVariable(aVar.Val);

            }


            if (theMeta == null) theMeta = TheComputer.gameObject.transform.GetComponent<RobotMeta>();
            
            SetCodePanelState();
        }
       

    }

    public void SetCodePanelState()
    {
        if (GameObjectFollower.PublicAccess != null && MatchCanvasManager.PublicAccess!= null)
        {
            {
                 if (Match.IsLeagueMatch)
                {

                    if (MatchCanvasManager.PublicAccess.TheLogPanel.TheRobotMeta != null)
                    {
                        if (WwwLeagueInterface.LoggedInUserID == MatchCanvasManager.PublicAccess.TheLogPanel.TheRobotMeta.RuntimeRobotOwnderID)
                        {
                            MatchCanvasManager.PublicAccess.TheCodePanel.ShowCodeForRobot = true;
                        }
                        else
                        {
                            MatchCanvasManager.PublicAccess.TheCodePanel.ShowCodeForRobot = false;

                        }
                    }
                }
                else
                {
                    MatchCanvasManager.PublicAccess.TheCodePanel.ShowCodeForRobot = true;

                }

                if (Match.IsLeagueMatch)
                {
                    MatchCanvasManager.PublicAccess.TheCodePanel.IsAllowedToChangeCode = false;
                }
                else
                {
                    MatchCanvasManager.PublicAccess.TheCodePanel.IsAllowedToChangeCode = true;

                }
                if (MatchCanvasManager.PublicAccess.TheCodePanel.IsAllowedToChangeCode)
                {


                    if (MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.isFocused == false)
                    {
                        MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.text = MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.CodeText;
                    }
                }
                else
                {
                    MatchCanvasManager.PublicAccess.TheCodePanel.TheCode.text = MatchCanvasManager.PublicAccess.TheCodePanel.TheComputer.CodeText;

                }
            }
        }
    }
  
   

    public void OnEndEdit(string inp)
    {
        if (IsAllowedToChangeCode)
        {
            if (TheComputer != null)
            {
                TheComputer.CodeText = TheCode.text;
            }
            print("OnEndEdit: ");
        }
    }
    public void ButtonStepCode()
    {
        if (IsAllowedToChangeCode)
        {
            TheComputer.CodeText = TheCode.text;
        }
        TheComputer.StepExecute();
    }
    public void ButtonSwitchToManual()
    {
        ManualPanel.SetActive(true);
    }
    public void ButtonCloseManual()
    {
        ManualPanel.SetActive(false);
    }
}
