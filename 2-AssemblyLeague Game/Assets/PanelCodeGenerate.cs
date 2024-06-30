using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCodeGenerate : MonoBehaviour
{
    public CodePanel TheCodePanel;
    public Computer.CodeGeneration TheCodeGeneration;
    public PanelCodeGenConditionAdd PanelAddCondition;
    public PanelCodeGenActionAdd PanelAddAction;
    public Text TextConditions;
    public Text TextActions;
    public InputField InputCodeName;
    // Use this for initialization
    void Start()
    {
        TheCodeGeneration = new Computer.CodeGeneration();
        TheCodeGeneration.Coditions = new List<Computer.CodeGenCondition>();
        TheCodeGeneration.Actions = new List<Computer.CodeGenAction>();
    }
    void OnEnable()
    {
        TheCodeGeneration = new Computer.CodeGeneration();
        TheCodeGeneration.Coditions = new List<Computer.CodeGenCondition>();
        TheCodeGeneration.Actions = new List<Computer.CodeGenAction>();
        TextActions.text = "";
        TextConditions.text = "";
        PanelAddCondition.gameObject.SetActive(false);
        PanelAddAction.gameObject.SetActive(false);
        InputCodeName.text = GenerateCodeName();

    }
    private string GenerateCodeName()
    {
        string ret = "";
        ret += System.DateTime.Now.ToString("MMMddHHmmss");// +Random.Range(0, 0).ToString();
        return ret;
    }





    // Update is called once per frame
    void Update()
    {

    }
    private void RefreshUi()
    {
        TextConditions.text = "";
        for (int c = 0; c < TheCodeGeneration.Coditions.Count; c++)
        {
            string aLine = TheCodeGeneration.Coditions[c].GetHumanText();
            TextConditions.text += "\n"+aLine;
        }

        TextActions.text = "";
        for (int c = 0; c < TheCodeGeneration.Actions.Count; c++)
        {
            string aLine = TheCodeGeneration.Actions[c].GetHumanText();
            TextActions.text += "\n" + aLine;
        }

    }
    public void ButtonAddConditionClick()
    {
        IDECanvasManager.PublicAccess.ButtonSave();
        PanelAddCondition.Reset();
        PanelAddCondition.gameObject.SetActive(true);
    }
    public void AddCondition(Computer.CodeGenCondition aCondition)
    {
        TheCodeGeneration.Coditions.Add(aCondition);
        RefreshUi();
    }
    public void AddAction(Computer.CodeGenAction aCondition)
    {
        TheCodeGeneration.Actions.Add(aCondition);
        RefreshUi();
    }

    public void ButtonAddActionClick()
    {
        IDECanvasManager.PublicAccess.ButtonSave();
        PanelAddAction.Reset();
        PanelAddAction.gameObject.SetActive(true);
    }
    public void ButtonClearClick()
    {
        TheCodeGeneration.Coditions = new List<Computer.CodeGenCondition>();
        TheCodeGeneration.Actions = new List<Computer.CodeGenAction>();
        TextActions.text = "";
        TextConditions.text = "";
        PanelAddCondition.gameObject.SetActive(false);
        PanelAddAction.gameObject.SetActive(false);
        InputCodeName.text = GenerateCodeName();
    }
    public void ButtonGenerateClick()
    {
        InputCodeName.text = InputCodeName.text.Replace(" ","_").Trim();
        TheCodePanel.TheCode.text += "\n" + TheCodeGeneration.GenerateCode(InputCodeName.text);

        InputCodeName.text = GenerateCodeName();
 
    }
}
