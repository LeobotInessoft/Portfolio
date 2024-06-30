using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PanelCodeGenActionAdd : MonoBehaviour
{
    public PanelCodeGenerate PanelCodeGenerate;
    public Dropdown DropdownComponent;
    public Dropdown DropdownAction;
    public Dropdown DropdownTarget;
    public Dropdown DropdownValue;
    public InputField SingleValue;
    public InputField ThreeValue1;
    public InputField ThreeValue2;
    public InputField ThreeValue3;
    public Button ButtonAdd;
    public Button ButtonCancel;
    List<ComponentType> currentComponents;
    List<RobotConstructor.DataComponentType> currentDataComponents;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Reset()
    {
        currentComponents = new List<ComponentType>();
        PanelRobotRow aRow = IDECanvasManager.PublicAccess.GetRowForRobot(IDECanvasManager.PublicAccess.SelectedRobotID);
        DropdownComponent.value = 0;
        DropdownAction.value = 0;
        DropdownTarget.value = 0;
        DropdownValue.value = 0;
        DropdownComponent.value = 0;
        DropdownComponent.ClearOptions();
        DropdownAction.ClearOptions();
        DropdownTarget.ClearOptions();
        DropdownValue.ClearOptions();
       

          SingleValue.text = "";
        ThreeValue1.text = "";
        ThreeValue2.text = "";
        ThreeValue3.text = "";
        DropdownAction.gameObject.SetActive(false);
        DropdownTarget.gameObject.SetActive(false);
        DropdownValue.gameObject.SetActive(false);
        SingleValue.gameObject.SetActive(false);
        ThreeValue1.gameObject.SetActive(false);
        ThreeValue2.gameObject.SetActive(false);
        ThreeValue3.gameObject.SetActive(false);
        List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();

        currentDataComponents = aRow.TheRobot.ModuleList;
        for (int c = 0; c < aRow.TheRobot.ModuleList.Count; c++)
        {
            GameObject aComp = RobotConstructor.PublicAccess.GetPrefabByID(aRow.TheRobot.ModuleList[c].ComponentID);
            ComponentType aType = aComp.GetComponent<ComponentType>();
            aType.InitAllFunctionsIO(aRow.TheRobot.ModuleList[c].IoNumberCustomMap);
            currentComponents.Add(aType);
            //  string disp = aRow.TheRobot.ModuleList[c].IoNumberCustomMap + " - " + aRow.TheRobot.ModuleList[c].MyIoMap.MapToInterrupNumber + " - " + aComp.name;
            string disp = aRow.TheRobot.ModuleList[c].IoNumberCustomMap + " - " + aType.DeviceName;
            Dropdown.OptionData anOp = new Dropdown.OptionData(disp);//Computer.CodeGenCondition.GetDisplayText());
            ops.Add(anOp);
        }

        DropdownComponent.AddOptions(ops);
        DropdownComponent.value = 0;
        DropdownComponentValueChanged(0);
        
    }
    public void ButtonAddClick()
    {
    

        Computer.CodeGenAction anAct = new Computer.CodeGenAction();
        anAct.SingleValue = SingleValue.text;
        anAct.ThreeValue1 = ThreeValue1.text;
        anAct.ThreeValue2 = ThreeValue2.text;
        anAct.ThreeValue3 = ThreeValue3.text;
        anAct.DropdownValue = DropdownValue.value;
        anAct.TheIONumber = selIONumber;
        anAct.Target = selTarget;
        anAct.TheComponent = selComp;
        anAct.TheFunction = selFunction;
        

        PanelCodeGenerate.AddAction(anAct);

        gameObject.SetActive(false);
    }
    public void ButtonCancelClick()
    {
        gameObject.SetActive(false);
    }

    ComponentType selComp;
    ComponentType.IoFunctionMeta selFunction;
    Computer.CodeGenAction.EnumActionTarget selTarget;

    string selIONumber;
    public void DropdownComponentValueChanged(int index)
    {
        DropdownAction.ClearOptions();
        selComp = currentComponents[index];
        DropdownAction.gameObject.SetActive(true);
        DropdownTarget.gameObject.SetActive(false);
        DropdownValue.gameObject.SetActive(false);
        SingleValue.gameObject.SetActive(false);
        ThreeValue1.gameObject.SetActive(false);
        ThreeValue2.gameObject.SetActive(false);
        ThreeValue3.gameObject.SetActive(false);
        selComp.InitAllFunctionsIO(currentDataComponents[index].IoNumberCustomMap);
        selIONumber = currentDataComponents[index].IoNumberCustomMap;
        
        if (selComp.FunctionsMeta.Count == 0)
        {
            DropdownAction.gameObject.SetActive(false);
            ButtonAdd.gameObject.SetActive(false);
        }else
        {
            ButtonAdd.gameObject.SetActive(true);
            List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();

            for (int c = 0; c < selComp.FunctionsMeta.Count; c++)
            {
                Dropdown.OptionData anOp = new Dropdown.OptionData(selComp.FunctionsMeta[c].FunctionName);
                ops.Add(anOp);
            }
           
            DropdownAction.AddOptions(ops);
            DropdownAction.value = 0;
            DropdownActionValueChanged(0);

        }
    }
    List<Computer.CodeGenAction.EnumActionTarget> possibleTargets;
    public void DropdownActionValueChanged(int index)
    {

        DropdownTarget.ClearOptions();

        DropdownAction.gameObject.SetActive(true);

        DropdownTarget.gameObject.SetActive(true);
        DropdownValue.gameObject.SetActive(false);
        SingleValue.gameObject.SetActive(false);
        ThreeValue1.gameObject.SetActive(false);
        ThreeValue2.gameObject.SetActive(false);
        ThreeValue3.gameObject.SetActive(false);


        List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();
        if (selComp.FunctionsMeta.Count == 0)
        {
            DropdownTarget.gameObject.SetActive(false);

        }
        else
        {
            selFunction = selComp.FunctionsMeta[index];
            possibleTargets = selComp.FunctionsMeta[index].GetPossibleTargets();
            if (possibleTargets.Count == 0 || (possibleTargets.Count == 1 && possibleTargets[0] == Computer.CodeGenAction.EnumActionTarget.None))
            {
                DropdownTarget.gameObject.SetActive(false);
                DropdownValue.value = 0;
                DropdownTargetValueChanged(0);
            }
            else
            {
                for (int c = 0; c < possibleTargets.Count; c++)
                {
                    Dropdown.OptionData anOp = new Dropdown.OptionData(Computer.CodeGenAction.GetDisplayText(possibleTargets[c]));
                    ops.Add(anOp);
                }
                DropdownTarget.AddOptions(ops);

                DropdownValue.value = 0;
                DropdownTargetValueChanged(0);
            }
        }

    }
    private void SetValueFieldsActive(Computer.CodeGenAction.EnumActionTarget target)
    {
        selTarget = target;
        
        switch (target)
        {
            case Computer.CodeGenAction.EnumActionTarget.SpecificPosition:
                {
                    ThreeValue1.gameObject.SetActive(true);
                    ThreeValue2.gameObject.SetActive(true);
                    ThreeValue3.gameObject.SetActive(true);
                    SingleValue.gameObject.SetActive(false);
                    DropdownValue.gameObject.SetActive(false);
                    break;
                }
            case Computer.CodeGenAction.EnumActionTarget.SpecificAngle:
                {
                    ThreeValue1.gameObject.SetActive(true);
                    ThreeValue2.gameObject.SetActive(true);
                    ThreeValue3.gameObject.SetActive(false);
                    SingleValue.gameObject.SetActive(false);
                    DropdownValue.gameObject.SetActive(false);
                    break;
                }
            case Computer.CodeGenAction.EnumActionTarget.SpecificValue:
                {
                    ThreeValue1.gameObject.SetActive(false);
                    ThreeValue2.gameObject.SetActive(false);
                    ThreeValue3.gameObject.SetActive(false);
                    SingleValue.gameObject.SetActive(true);
                    DropdownValue.gameObject.SetActive(false);
                    break;
                }
            default:
                {
                    ThreeValue1.gameObject.SetActive(false);
                    ThreeValue2.gameObject.SetActive(false);
                    ThreeValue3.gameObject.SetActive(false);
                    SingleValue.gameObject.SetActive(false);
                    DropdownValue.gameObject.SetActive(false);
                    break;
                }
        }

    }

    public void DropdownTargetValueChanged(int index)
    {

        DropdownValue.ClearOptions();
     
        SetValueFieldsActive(possibleTargets[index]);
        if (selFunction.InputVariables.DataSetType == ComponentType.IoFunctionMeta.EnumIODataSetType.DropDownBx)
        {
            print("IS a DROPDOWN");
            List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();
            for (int c = 0; c < selFunction.InputVariables.InputVariables[0].DropDownOptions.Count; c++)
            {
                Dropdown.OptionData anOp = new Dropdown.OptionData(selFunction.InputVariables.InputVariables[0].DropDownOptions[c].DisplayText);
                ops.Add(anOp);
            }
            DropdownValue.AddOptions(ops);
            DropdownValue.gameObject.SetActive(true);
        }
      
    }

}
