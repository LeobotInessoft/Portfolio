using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoBackPack : MonoBehaviour, FunctionsContainer
{
    public bool IsOn = true;
    public ComponentType myComponentType;
    public RobotMeta TheRobotMeta;
    public IOHandler MyIoHandler;
    public enum EnumBackPackType
    {
        NotSet,
        ExtraPowerSpeedHeatArmour,
        JetPack,
        MineLayer,
        MineSweeper,
        Shield,
        PositionScramble,
        ExtraSelfDestruct,
        TwoWayRadio,
        Cooler,
        EnergyShield,
        ExtraMountSpace,
        Booster,

    }
    public EnumBackPackType BackPackType;
    // Use this for initialization
    void Start()
    {
        myComponentType = GetComponent<ComponentType>();
        if (TheRobotMeta == null)
        {
            TheRobotMeta = FindRobotMetaParent();
        }
        if (MyIoHandler == null)
        {
            MyIoHandler = gameObject.GetComponent<IOHandler>();

        }


        MyIoHandler.IoHandler += new IOHandler.DelegateHandleIO(MyIoHandler_IoHandler);
        ComponentType ct = (ComponentType)gameObject.GetComponent<ComponentType>();
        SetComponentFunctions(ct, "0");
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool HasBeenInit = false;
    public void ResetInit()
    {
        HasBeenInit = false;
    }
    public void MyIoHandler_IoHandler(ref Computer.StandardStack runtimeStack)
    {

        int axVal = int.Parse(runtimeStack.Ax.Val);


        switch (axVal)
        {

            case 1:
                {

                    float angleY = float.Parse(runtimeStack.Bx.Val);

                    break;
                }


        }

    }
    private List<ComponentType.IoFunctionMeta> SetComponentsFunctionsMeta(ComponentType ct, string actualIO)
    {
        List<ComponentType.IoFunctionMeta> ret = new List<ComponentType.IoFunctionMeta>();


        return ret;
    }
    public void SetComponentFunctions(ComponentType ct, string actualIO)
    {
        if (HasBeenInit == false)
        {
            actualIO = "[[IO]]";

            List<ComponentType.IoFunctionMeta> functionsMeta = SetComponentsFunctionsMeta(ct, actualIO);
            for (int c = 0; c < functionsMeta.Count; c++)
            {
                {
                    string functionText = "";
                    functionText += "<b>" + functionsMeta[c].FunctionName + "</b>";
                    functionText += "\n";
                    functionText += "<b>Description:</b>";
                    functionText += "\n";
                    functionText += functionsMeta[c].FunctionDescription;
                    functionText += "\n";
                    functionText += "<b>Inputs:</b>";
                    functionText += "\n";

                    functionText += functionsMeta[c].InputVariables.InputVariableMain.Register + ": " + functionsMeta[c].InputVariables.InputVariableMainValue + " ";
                    functionText += "\n";
                    for (int x = 0; x < functionsMeta[c].InputVariables.InputVariables.Count; x++)
                    {
                        functionText += functionsMeta[c].InputVariables.InputVariables[x].Register + ": " + functionsMeta[c].InputVariables.InputVariables[x].RegisterDescription + " ";
                        functionText += "\n";

                    }
                    functionText += "<b>Outputs:</b>";
                    functionText += "\n";
                    if (functionsMeta[c].OutputVariables.OutputVariables.Count == 0)
                    {
                        functionText += "NONE";
                        functionText += "\n";
                    }
                    else
                    {
                        for (int x = 0; x < functionsMeta[c].OutputVariables.OutputVariables.Count; x++)
                        {
                            functionText += functionsMeta[c].OutputVariables.OutputVariables[x].Register + ": " + functionsMeta[c].OutputVariables.OutputVariables[x].RegisterDescription + " ";
                            functionText += "\n";

                        }
                    }



                    functionText += "<b>Examples:</b>";
                    functionText += "\n";
                    functionText += functionsMeta[c].FunctionExample;
                    functionText += "\n";
                    ct.Functions2.Add(functionText);
                }

            }


        }
        HasBeenInit = true;

    }


    RobotMeta FindRobotMetaParent()
    {
        RobotMeta ret = null;
        bool mustContinue = true;
        Transform currentTans = gameObject.transform;

        while (mustContinue && currentTans != null)
        {
            ret = currentTans.GetComponent<RobotMeta>();
            if (ret != null)
            {
                mustContinue = false;
            }
            currentTans = currentTans.transform.parent;

        }
        return ret;
    }

}
