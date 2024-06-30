using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoCockpit : MonoBehaviour, FunctionsContainer
{
    public bool IsOn = true;
    public ComponentType myComponentType;
    public RobotMeta TheRobotMeta;
    public IOHandler MyIoHandler;
    public float RotationDegreesYWanted = 0;
    public float RotationDegreesZWanted = 0;
    public float RotationDegreesYActual = 0;
    public float RotationDegreesZActual = 0;
    public float Setting_RotationSpeed = 1f;
    public bool CanRotateAroundY=true;
    public bool CanRotateAroundZ = false;
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
        if (myComponentType.IsComponenentUsable)
        {

            if (myComponentType.IsOverheated == false)
            {
                if (Mathf.Abs(RotationDegreesYActual - RotationDegreesYWanted) > 0.1f)
                {
                    if (RotationDegreesYActual < RotationDegreesYWanted)
                    {
                        RotationDegreesYActual += Setting_RotationSpeed;
                    }
                    if (RotationDegreesYActual > RotationDegreesYWanted)
                    {
                        RotationDegreesYActual -= Setting_RotationSpeed;
                    }
                }

                if (Mathf.Abs(RotationDegreesZActual - RotationDegreesZWanted) > 0.1f)
                {
                    if (RotationDegreesZActual < RotationDegreesZWanted)
                    {
                        RotationDegreesZActual += Setting_RotationSpeed;
                    }
                    if (RotationDegreesZActual > RotationDegreesZWanted)
                    {
                        RotationDegreesZActual -= Setting_RotationSpeed;
                    }
                }
                if (TheRobotMeta != null && TheRobotMeta.Health > 0)
                {
                    if (IsOn)
                    {
                        Quaternion originalRot = gameObject.transform.localRotation;
                        gameObject.transform.localRotation = Quaternion.AngleAxis(RotationDegreesZActual, -1 * Vector3.right) * Quaternion.AngleAxis(RotationDegreesYActual, -1 * Vector3.up);
                    }
                }

            }
        }
    }

    public void MyIoHandler_IoHandler(ref Computer.StandardStack runtimeStack)
    {

        int axVal = int.Parse(runtimeStack.Ax.Val);


        switch (axVal)
        {

            case 1:
                {
                  
                    float angleY = float.Parse(runtimeStack.Bx.Val);
               
                    RotationDegreesYWanted = angleY;
                   
                    break;
                }


        }

    }
    bool HasBeenInit = false;
    private List<ComponentType.IoFunctionMeta> SetComponentsFunctionsMeta(ComponentType ct, string actualIO)
    {
        List<ComponentType.IoFunctionMeta> ret = new List<ComponentType.IoFunctionMeta>();
        
        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NumberBx;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Set Y Angle";
            aFunction.FunctionDescription = "Set the angle of the cockpit.";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "1";
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Bx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.SpeedPercent;
                aVar.RegisterDescription = "The angle to rotate around the Y-Axis";
                aVar.ExampleValue = "15";
                aFunction.InputVariables.InputVariables.Add(aVar);
            }
            


            aFunction.FunctionExample = "";

            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will rotate the hull around the Y Axis.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample += ct.GenerateExampleCode(aFunction, actualIO);

            ct.FunctionsMeta.Add(aFunction);
            ret.Add(aFunction);
        }
        return ret;
    }
    public void ResetInit()
    {
        HasBeenInit = false;
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
