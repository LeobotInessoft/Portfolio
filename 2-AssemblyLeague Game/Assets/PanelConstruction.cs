using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class PanelConstruction : MonoBehaviour
{
    public PanelComponentDescription PanelDescription;
    public PanelComponentStats TheStats;
    public RobotConstructor TheConstructor;
    public UnityEngine.UI.Text TextConstructorLoction;
    public UnityEngine.UI.Text TextConstructorPart;
    public UnityEngine.UI.Text TextDescription;
    public UnityEngine.UI.Toggle ToggleSkip;

    public UnityEngine.UI.Button ButtonInstall;
    public UnityEngine.UI.Button ButtonRemove;
    public UnityEngine.UI.Button ButtonComponentManaual;

    // Use this for initialization
    void Start()
    {

    }
    public void SetButtonsInstallActive(bool val)
    {
        ButtonComponentManaual.gameObject.SetActive(true);
        ButtonInstall.gameObject.SetActive(val);
        ButtonRemove.gameObject.SetActive(!val);
       

    }
    public void SetButtonsBothActive(bool val)
    {
        ButtonInstall.gameObject.SetActive(val);
        ButtonRemove.gameObject.SetActive(val);

        ButtonComponentManaual.gameObject.SetActive(val);


    }

    // Update is called once per frame
    void Update()
    {
        TextConstructorLoction.text = TheConstructor.currentPartAcceptorIndex + "";
        
    }
    public string GenerateDescription()
    {
        string ret = "";
        RobotConstructor.RobotTemplate aTemplate = TheConstructor.GetRobotTemplate();
        ret += aTemplate.ModuleList.Count + " modules\n";
        List<ComponentType> allComps = new List<ComponentType>();
        for (int c = 0; c < aTemplate.ModuleList.Count; c++)
        {
            allComps.Add(TheConstructor.GetPrefabByID(aTemplate.ModuleList[c].ComponentID).GetComponent<ComponentType>());
        }

        aTemplate.CalculateTotalWeight = aTemplate.CalculateTotalWeightValue(TheConstructor, aTemplate);
        aTemplate.CalculateCodeProcessedPerSecond = aTemplate.CalculateCodeProcessedPerSecondValue(TheConstructor, aTemplate);
        aTemplate.CalculateLinesOfCode = aTemplate.CalculateLinesOfCodeValue(TheConstructor, aTemplate);
        aTemplate.CalculateMaxSpeed = aTemplate.CalculateMaxSpeedValue(TheConstructor, aTemplate, aTemplate.CalculateTotalWeight);
        aTemplate.CalculateSqrMeterSize = aTemplate.CalculateSqrMeterSizeValue(TheConstructor.BuildPlatformTarget.gameObject);
        
        float totalAcel = allComps.Sum(x => x.Legs_AccelerationAdd);
        float totalPower = allComps.Sum(x => x.PowerProvidedPerFrame);
        float totalPowerUsage = allComps.Sum(x => x.PowerUsedPerFrame);
        float totalArmour = allComps.Sum(x => x.Armour);

        ret += "Profile is " + aTemplate.CalculateSqrMeterSize.ToString("f0") + " Meters² and an effective weight of " + aTemplate.CalculateTotalWeight.ToString("f0") + "KG which can accelerate at " + totalAcel.ToString("f0") + "KM/S to reach a maximum speed of " + aTemplate.CalculateMaxSpeed.ToString("f0") + "KM/H.\n";
        ret += "Total power capacity is " + totalPower.ToString("f0") + "KW/S while maximum possible power consumption is " + totalPowerUsage.ToString("f0") + "KW/S if all devices were turned on simultaneously.\n";
        if (totalPowerUsage > totalPower) ret += "This robot will overheat easily.\n";
        ret += "Total instructions that can be processed per second is " + aTemplate.CalculateCodeProcessedPerSecond.ToString("f0") + "Lines of code.\n";
        ret += "An average armor plating of  " + totalArmour.ToString("f0") + "CM protects this robot.\n";
       
        return ret;

    }
  
    public void RefreshUi()
    {
        List<ModuleAcceptor> allAcceptors = new List<ModuleAcceptor>();
        allAcceptors.AddRange(TheConstructor.BuildPlatformTarget.GetComponentsInChildren<ModuleAcceptor>());
        bool IsAllUsed = true;

        for (int c = 0; c < allAcceptors.Count; c++)
        {
            if (allAcceptors[c].HasAttachment == false)
            {
                IsAllUsed = false;
                break;
            }
        }
        IsSkipMode = !IsAllUsed;
       
        TextDescription.text = GenerateDescription();
        TextConstructorLoction.text = TheConstructor.currentPartAcceptorIndex + "";
        TextConstructorPart.text = "";
        if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.transform.GetComponentsInChildren<ComponentType>().Length != 0)
        {
            SetButtonsInstallActive(false);
            ComponentType aCom = TheConstructor.CurrentAttachTarget.transform.gameObject.GetComponentInChildren<ComponentType>();
            if (aCom != null)
            {
                PanelDescription.ShowComponent(aCom);
                TheStats.SetStats(aCom, TheConstructor);
                TextConstructorPart.text = aCom.DeviceName + "";
            }
            {
                 ComponentType aCom2 = TheConstructor.CurrentAttachTarget.GetComponentInChildren<ComponentType>();

                if (aCom2 != null)
                {
                    TheStats.SetStats(aCom2, TheConstructor);
                    PanelDescription.ShowComponent(aCom2);
                    TextConstructorPart.text = aCom2.DeviceName;
                }
                else
                {
                    if (TheConstructor.CurrentAttachTarget != null)
                    {
                        PanelDescription.ShowAcceptor(TheConstructor.CurrentAttachTarget);
             
                    }
                    else
                    {
                        PanelDescription.ShowComponent(null);
                    }
                    TheStats.SetStats(null, TheConstructor);
                    TextConstructorPart.text = "";


                }
            
            }
        }
        else
        {
            if (TheConstructor.PartInHand != null)
            {
                SetButtonsInstallActive(true); 
                ComponentType aCom = TheConstructor.PartInHand.transform.gameObject.GetComponentInChildren<ComponentType>();
                if (aCom != null)
                {

                    PanelDescription.ShowComponent(aCom);
                    TheStats.SetStats(aCom, TheConstructor);
                    TextConstructorPart.text = "[EMPTY SLOT] " + aCom.DeviceName ;
                }
                else
                {
                    if (TheConstructor.CurrentAttachTarget != null)
                    {
                        PanelDescription.ShowAcceptor(TheConstructor.CurrentAttachTarget);

                    }
                    else
                    {
                        PanelDescription.ShowComponent(null);
                    }
                    TheStats.SetStats(null, TheConstructor);
                    TextConstructorPart.text = "NO COM (" + TheConstructor.PartInHand.name + ")";

                }
            }
            else
            {
                SetButtonsBothActive(false);
                if (TheConstructor.CurrentAttachTarget != null)
                {
                    ComponentType aCom = TheConstructor.CurrentAttachTarget.GetComponentInChildren<ComponentType>();

                    if (aCom != null)
                    {
                        PanelDescription.ShowComponent(aCom);
                        TheStats.SetStats(aCom, TheConstructor);

                        TextConstructorPart.text = aCom.DeviceName;
                    }
                    else
                    {
                        if (TheConstructor.CurrentAttachTarget != null)
                        {
                            PanelDescription.ShowAcceptor(TheConstructor.CurrentAttachTarget);

                        }
                        else
                        {
                            PanelDescription.ShowComponent(null);
                        }
                        TheStats.SetStats(null, TheConstructor);
                        TextConstructorPart.text = "";
             
                    }
             
                }
                else
                {
                    if (TheConstructor.CurrentAttachTarget != null)
                    {
                        PanelDescription.ShowAcceptor(TheConstructor.CurrentAttachTarget);

                    }
                    else
                    {
                        PanelDescription.ShowComponent(null);
                    }
                    TheStats.SetStats(null, TheConstructor);
                    TextConstructorPart.text = "";
                }
            }
        }
    }
    bool IsSkipMode = true;
    public void ButtonPreviousAcceptorClick()
    {
        ModuleAcceptor indexBefore = TheConstructor.CurrentAttachTarget;
        IsSkipMode = ToggleSkip.isOn;
        //if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.transform.childCount == 0)
        {
            if (TheConstructor.PartInHand != null)
            {
                Destroy(TheConstructor.PartInHand.gameObject);
                TheConstructor.PartInHand = null;
            }
            TheConstructor.SwitchToNextPartAcceptor(-1, IsSkipMode);
            if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.GetComponentsInChildren<ComponentType>().Length == 0 && TheConstructor.PartInHand != null || TheConstructor.CurrentAttachTarget == null)
            {
                if (ToggleSkip.isOn)
                {
                    if (TheConstructor.CurrentAttachTarget == indexBefore)
                    {
                        ToggleSkip.isOn = false;
                        IsSkipMode = ToggleSkip.isOn;
                        TheConstructor.SwitchToNextPartAcceptor(1, IsSkipMode);
                        TheConstructor.SwitchToNextPartAcceptor(-1, IsSkipMode);
                    }
        
                }
            }

        }
        RefreshUi();
    }

    public void ButtonNextAcceptorClick()
    {
        IsSkipMode = ToggleSkip.isOn;

        ModuleAcceptor indexBefore = TheConstructor.CurrentAttachTarget;
        {
            if (TheConstructor.PartInHand != null)
            {
                Destroy(TheConstructor.PartInHand.gameObject);
                TheConstructor.PartInHand = null;

            }
            TheConstructor.SwitchToNextPartAcceptor(1, IsSkipMode);
            if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.GetComponentsInChildren<ComponentType>().Length == 0 && TheConstructor.PartInHand != null || TheConstructor.CurrentAttachTarget== null)
            {
                if (ToggleSkip.isOn)
                {
                    {
                        ToggleSkip.isOn = false;
                        IsSkipMode = ToggleSkip.isOn;
                        TheConstructor.SwitchToNextPartAcceptor(-1, IsSkipMode);
                        TheConstructor.SwitchToNextPartAcceptor(1, IsSkipMode);
                    }

                }
            }
        }
        RefreshUi();
    }
    public void ButtonPreviousComponentClick()
    {
        print("Next Part ");
        IsSkipMode = ToggleSkip.isOn;



        if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.transform.GetComponentsInChildren<ComponentType>().Length == 0 )
        {
            print("Next Part 2");

            TheConstructor.SwitchToNextPartInList(1, TheConstructor.currentPartAcceptorIndex, TheConstructor.TheOwnerLookupFilter);
        }
        RefreshUi();

    }

    public void ButtonNextComponentClick()
    {
        IsSkipMode = ToggleSkip.isOn;
        if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.transform.GetComponentsInChildren<ComponentType>().Length == 0)
        {
            TheConstructor.SwitchToNextPartInList(-1, TheConstructor.currentPartAcceptorIndex,  TheConstructor.TheOwnerLookupFilter);
        }
        RefreshUi();
    }

    public void ButtonApplyComponent()
    {
        IsSkipMode = ToggleSkip.isOn;
        
        if (TheConstructor.CurrentAttachTarget != null && TheConstructor.CurrentAttachTarget.transform.childCount == 0)
        {
            TheConstructor.ApplyPart();
            IDECanvasManager.PublicAccess.ButtonSave(); 
            TheConstructor.SwitchToNextPartAcceptor(1, IsSkipMode);
        }
         RefreshUi();


       
    }
    public void ButtonRemoveComponent()
    {
        IsSkipMode = ToggleSkip.isOn;
        if (TheConstructor.CurrentAttachTarget != null)
        {
            for (int c = 0; c <= TheConstructor.CurrentAttachTarget.transform.childCount; c++)
            {
                Transform aChild = TheConstructor.CurrentAttachTarget.transform.GetChild(c);
                aChild.transform.parent = null;
                Destroy(aChild.gameObject, 0.1f);

            }
        }
         RefreshUi();
    }
}
