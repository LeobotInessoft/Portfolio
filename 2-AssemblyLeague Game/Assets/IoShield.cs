using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class IoShield : MonoBehaviour, FunctionsContainer
{

    public enum EnumAnimatorPackState
    {
        Pack = 0,
        Unpack = 1,
        PackHalfway = 2,
        UnpackHalfway = 3
    }
    public enum EnumAnimatorHitState
    {
        None = 0,
        HitTop = 1,
        HitBottom = 2,
       
    }
    
    public Animator MyAnimator;
    public EnumAnimatorPackState AnimatorPackState;
    public EnumAnimatorHitState AnimatorHitState;
   
    public Vector3 WantedLookDestination;
    public Vector3 ActualLookDestination;

    public IOHandler MyIoHandler;

   
    public float SettingRotationSpeedOutOf100 = 1f;

    public bool IsOn = true;
    public bool IsDead = false;
    public Transform TestTransform;

    // Use this for initialization
    void Start()
    {
        if (MyIoHandler == null)
        {
            MyIoHandler = gameObject.GetComponent<IOHandler>();

        }
        if (MyAnimator == null)
        {
            MyAnimator = gameObject.GetComponent<Animator>();

        }

        MyIoHandler.IoHandler += new IOHandler.DelegateHandleIO(MyIoHandler_IoHandler);
         }
    void MyIoHandler_IoHandler(ref Computer.StandardStack runtimeStack)
    {

        int axVal = int.Parse(runtimeStack.Ax.Val);


        switch (axVal)
        {
            case 1:
                {


                    WantedLookDestination = new Vector3(float.Parse(runtimeStack.Bx.Val), float.Parse(runtimeStack.Cx.Val), float.Parse(runtimeStack.Dx.Val));
                    WantedLookDestination.x *= transform.parent.localScale.x;
      

                    break;
                }
            case 2:
                {
                    //todo
                    //WantedSpeedOutOf100 = int.Parse(runtimeStack.Bx.Val);
                    break;
                }

        }

    }
    // Update is called once per frame
    void Update()
    {
        if (IsOn)
        {
            if (TestTransform != null)
            {
                Vector3 anglePoint = TestTransform.transform.position;



                WantedLookDestination = anglePoint;
                WantedLookDestination.x *= transform.parent.localScale.x;
            }
            ActualLookDestination = Vector3.Lerp(ActualLookDestination, WantedLookDestination, SettingRotationSpeedOutOf100 * 0.01f);

            Vector3 onPlane = ActualLookDestination;
           
            if (Vector3.Distance(onPlane, gameObject.transform.position) >= 2)
            {
                gameObject.transform.right = -1 * (onPlane - transform.position).normalized;
            }
        }
     
        UpdateAnimator();

    }
    private void UpdateAnimator()
    {
        if (MyAnimator != null)
        {
            MyAnimator.SetInteger("HitState", (int)AnimatorHitState);
            MyAnimator.SetInteger("PackState", (int)AnimatorPackState);
            
        }
    }


    bool HasBeenInit = false;
    private List<ComponentType.IoFunctionMeta> SetComponentsFunctionsMeta(ComponentType ct, string actualIO)
    {
        List<ComponentType.IoFunctionMeta> ret = new List<ComponentType.IoFunctionMeta>();
        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Not Set";
            aFunction.FunctionDescription = "Activate this function to fire off the attached device ";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "1";

            aFunction.FunctionExample = "";
            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will show how to shoot directly forward.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample += ct.GenerateExampleCode(aFunction, actualIO);
       
            ct.FunctionsMeta.Add(aFunction);

            print("XXXXX");
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
    
   
}
