using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCodeGenConditionAdd : MonoBehaviour
{
    public PanelCodeGenerate PanelCodeGenerate;
    public Dropdown DropdownConditionType;
    public Dropdown DropdownAttribute;
    public Dropdown DropdownTarget;
    public Dropdown DropdownConditionCheck;
    public InputField SingleValue;
    public Button ButtonAdd;
    public Button ButtonCancel;
    public InputField ThreeValue1;
    public InputField ThreeValue2;
    public InputField ThreeValue3;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    List<Computer.CodeGenCondition.EnumTarget> possibleTargets = new List<Computer.CodeGenCondition.EnumTarget>();
    public void Reset()
    {
        SingleValue.text = "";
        ThreeValue1.text = "";
        ThreeValue2.text = "";
        ThreeValue3.text = "";

        SingleValue.gameObject.SetActive(true);
        ThreeValue1.gameObject.SetActive(false);
        ThreeValue2.gameObject.SetActive(false);
        ThreeValue3.gameObject.SetActive(false);


        DropdownConditionType.ClearOptions();
        DropdownAttribute.ClearOptions();
        DropdownTarget.ClearOptions();
        DropdownConditionCheck.ClearOptions();
        SingleValue.text = "";

        {
            var values = Computer.CodeGenCondition.EnumConditionType.GetValues(typeof(Computer.CodeGenCondition.EnumConditionType));
            List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();
            foreach (Computer.CodeGenCondition.EnumConditionType con in values)
            {
                Dropdown.OptionData anOp = new Dropdown.OptionData(Computer.CodeGenCondition.GetDisplayText(con));
                ops.Add(anOp);
            }
            DropdownConditionType.AddOptions(ops);
        }
        {
            var values = Computer.CodeGenCondition.EnumAttribute.GetValues(typeof(Computer.CodeGenCondition.EnumAttribute));
            List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();
            foreach (Computer.CodeGenCondition.EnumAttribute con in values)
            {
                Dropdown.OptionData anOp = new Dropdown.OptionData(Computer.CodeGenCondition.GetDisplayText(con));
                ops.Add(anOp);
            }
            DropdownAttribute.AddOptions(ops);
        }
        {
            var values = Computer.CodeGenCondition.EnumTarget.GetValues(typeof(Computer.CodeGenCondition.EnumTarget));
            possibleTargets.Clear();
            List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();
            foreach (Computer.CodeGenCondition.EnumTarget con in values)
            {
                possibleTargets.Add(con);
                Dropdown.OptionData anOp = new Dropdown.OptionData(Computer.CodeGenCondition.GetDisplayText(con));
                ops.Add(anOp);
            }
            DropdownTarget.AddOptions(ops);
        }
        {
            var values = Computer.CodeGenCondition.EnumConditionCheck.GetValues(typeof(Computer.CodeGenCondition.EnumConditionCheck));
            List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();
            foreach (Computer.CodeGenCondition.EnumConditionCheck con in values)
            {
                Dropdown.OptionData anOp = new Dropdown.OptionData(Computer.CodeGenCondition.GetDisplayText(con));
                ops.Add(anOp);
            }
            DropdownConditionCheck.AddOptions(ops);
        }
    }
    public void ButtonAddClick()
    {
        if (SingleValue.text.Length > 0 || ThreeValue1.text.Length > 0 || ThreeValue2.text.Length > 0 || ThreeValue3.text.Length > 0)
        {
            Computer.CodeGenCondition aCond = new Computer.CodeGenCondition();
            aCond.Attribute = (Computer.CodeGenCondition.EnumAttribute)DropdownAttribute.value;
            aCond.ConditionCheck = (Computer.CodeGenCondition.EnumConditionCheck)DropdownConditionCheck.value;
            aCond.ConditionType = (Computer.CodeGenCondition.EnumConditionType)DropdownConditionType.value;

            switch (possibleTargets[DropdownTarget.value])
            {
                case Computer.CodeGenCondition.EnumTarget.I_153_ScanSpecificPosition:
                    {
                        aCond.NumberOfValues = 3;
                        aCond.SingleValue = ThreeValue1.text;
                        aCond.SingleValue2 = ThreeValue2.text;
                        aCond.SingleValue3 = ThreeValue3.text;


                        break;
                    }
                default:
                    {
                        aCond.NumberOfValues = 1;
                        aCond.SingleValue = SingleValue.text;

                        break;

                    }
            }

            aCond.Target = (Computer.CodeGenCondition.EnumTarget)DropdownTarget.value;
            PanelCodeGenerate.AddCondition(aCond);

            gameObject.SetActive(false);
        }
    }
    public void ButtonCancelClick()
    {
        gameObject.SetActive(false);
    }
    Computer.CodeGenAction.EnumActionTarget selTarget;

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
                    break;
                }
            case Computer.CodeGenAction.EnumActionTarget.SpecificAngle:
                {
                    ThreeValue1.gameObject.SetActive(true);
                    ThreeValue2.gameObject.SetActive(true);
                    ThreeValue3.gameObject.SetActive(false);
                    SingleValue.gameObject.SetActive(false);
                    break;
                }
            case Computer.CodeGenAction.EnumActionTarget.SpecificValue:
                {
                    ThreeValue1.gameObject.SetActive(false);
                    ThreeValue2.gameObject.SetActive(false);
                    ThreeValue3.gameObject.SetActive(false);
                    SingleValue.gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    ThreeValue1.gameObject.SetActive(false);
                    ThreeValue2.gameObject.SetActive(false);
                    ThreeValue3.gameObject.SetActive(false);
                    SingleValue.gameObject.SetActive(false);
                    break;
                }
        }

    }

    public void DropdownTargetValueChanged(int index)
    {

        SingleValue.gameObject.SetActive(false);
        ThreeValue1.gameObject.SetActive(false);
        ThreeValue2.gameObject.SetActive(false);
        ThreeValue3.gameObject.SetActive(false);


        List<Dropdown.OptionData> ops = new List<Dropdown.OptionData>();

        switch (possibleTargets[index])
        {
            case Computer.CodeGenCondition.EnumTarget.I_153_ScanSpecificPosition:
                {
                    SingleValue.gameObject.SetActive(false);
                    ThreeValue1.gameObject.SetActive(true);
                    ThreeValue2.gameObject.SetActive(true);
                    ThreeValue3.gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    SingleValue.gameObject.SetActive(true);
                    ThreeValue1.gameObject.SetActive(false);
                    ThreeValue2.gameObject.SetActive(false);
                    ThreeValue3.gameObject.SetActive(false);
                    break;

                }
        }



    }



}
