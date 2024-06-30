using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IoLegsMech : MonoBehaviour, FunctionsContainer
{
    public ComponentType myComponentType;
    public bool IsLegsTouchingSomething = false;
    private Vector3 curNormal = Vector3.up; // smoothed terrain normal
    private Quaternion iniRot; // initial rotation

    public Rigidbody MyRigidbody;
    

    public enum EnumAnimatorTransformationState
    {
        Legs = 0,
        Roller = 1
    }
    public enum EnumAnimatorHealthState
    {
        Idle = 0,
        IdleHandsDown = 1,
        DeactivateFaint = 2,
        Deactivate = 3,
        Dead = 4
    }
    public enum EnumAnimatorTurnState
    {
        NoTurn = 0,
        StandingTurn = 1,
        StafeLeft = 2,
        StrafeRight = 3
    }
    public enum EnumLegType
    {
        Walker = 0,
        Wheeled,
        Track,
        Aircraft,
        Spider,
    }


    public EnumLegType LegsType;
    public Animator MyAnimator;
    public EnumAnimatorHealthState AnimatorHealthState;
    public EnumAnimatorTurnState AnimatorTurnState;
    public EnumAnimatorTransformationState AnimatorTransformState;

    public int WantedSpeedOutOf100 = 0;
    public float ActualSpeedOutOf100 = 0;
    public Vector3 WantedLookDestination;
    public Vector3 ActualLookDestination;
    public EnumAnimatorTransformationState WantedAnimatorTransformState;

    public IOHandler MyIoHandler;

    public float SettingMaxSpeed = 100;
  
    public float SettingMaxSpeedStrafe = 50;
    public float SettingAccelerationStrafe = 1f;


    public float SettingRotationSpeedOutOf100 = 1f;

    public bool IsOn = true;
    public bool IsDead = false;
    public Transform TestTransform;
    public Vector3 eulerAngleVelocity;
    public Vector3 targetDir;
    public float CurrentVelocity;

    public float MaxVelocitySTRENGHT = 100;
    
    BuggyController buggyController;
    public MeshCollider myMeshCollider;

    void Start()
    {
        WantedSpeedOutOf100 = 0;
        myComponentType = GetComponent<ComponentType>();
        currentTouching = new List<Transform>();
        myMeshCollider = transform.GetComponent<MeshCollider>();
        iniRot = transform.rotation;
        buggyController = GetComponent<BuggyController>();
        if (MyIoHandler == null)
        {
            MyIoHandler = gameObject.GetComponent<IOHandler>();

        }
        if (MyAnimator == null)
        {
            MyAnimator = gameObject.GetComponent<Animator>();

        }
        if (MyRigidbody == null)
        {
            MyRigidbody = gameObject.GetComponent<Rigidbody>();

        }
        if (myMeshCollider == null)
        {
            myMeshCollider = transform.GetComponentInChildren<MeshCollider>();

        }
        ComponentType aComp = gameObject.GetComponent<ComponentType>();
        SettingMaxSpeed = aComp.Legs_MaxForwardSpeed;
        SettingRotationSpeedOutOf100 = aComp.Legs_MaxRotateSpeed;
        MyIoHandler.IoHandler += new IOHandler.DelegateHandleIO(MyIoHandler_IoHandler);
        ComponentType ct = (ComponentType)gameObject.GetComponent<ComponentType>();
        SetComponentFunctions(ct, "0");
        SetMeshCollider();
    }

    private void SetMeshCollider()
    {
        if (myMeshCollider != null)
        {
            LegsCollider aCol = myMeshCollider.GetComponent<LegsCollider>();
            if (aCol == null)
            {
                aCol = myMeshCollider.gameObject.AddComponent<LegsCollider>();
            }
            aCol.MyLegs = this;
        }
    }
    bool HasBeenInit = false;
    private List<ComponentType.IoFunctionMeta> SetComponentsFunctionsMeta(ComponentType ct, string actualIO)
    {
        List<ComponentType.IoFunctionMeta> ret = new List<ComponentType.IoFunctionMeta>();
       
        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.WorldPositionBxCxDx;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Look at Position";
            aFunction.FunctionDescription = "Set the 3D-world coordinates to look at.";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "1";
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Bx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.WorldPosX;
                aVar.RegisterDescription = "The X-coordinate in the arena to look at.";
                aVar.ExampleValue = "500";
                aFunction.InputVariables.InputVariables.Add(aVar);
            }
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Cx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.WorldPosY;
                aVar.RegisterDescription = "The Y-coordinate in the arena to look at.";
                aFunction.InputVariables.InputVariables.Add(aVar);
                aVar.ExampleValue = "0";
            }
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Dx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.WorldPosZ;
                aVar.RegisterDescription = "The Z-coordinate in the arena to look at.";
                aFunction.InputVariables.InputVariables.Add(aVar);
                aVar.ExampleValue = "500";
            }

            aFunction.FunctionExample = "";

            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will rotate the robot to face the location.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample += ct.GenerateExampleCode(aFunction, actualIO);

            ct.FunctionsMeta.Add(aFunction);
            ret.Add(aFunction);
        }

        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NumberBx;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Set Desired Speed";
            aFunction.FunctionDescription = "Pass a value between -100% and 100% in Bx to indicate the desired speed you want to move forward or backward.";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "2";
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Bx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.SpeedPercent;
                aVar.RegisterDescription = "The percentage out of 100% of your maximum speed to move at";
                aVar.ExampleValue = "100";
                aFunction.InputVariables.InputVariables.Add(aVar);
            }


            aFunction.FunctionExample = "";

            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will move the robot forward at full speed.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample += ct.GenerateExampleCode(aFunction, actualIO);

            ct.FunctionsMeta.Add(aFunction);
            ret.Add(aFunction);
        }
        if (LegsType == EnumLegType.Spider)
        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.DropDownBx;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Transform";
            aFunction.FunctionDescription = "Set the angle of rotation for this weapon";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "3";
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Bx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.TransformState;
                aVar.RegisterDescription = "Transform between 0 (legs) and 1 (wheels).";
                aVar.ExampleValue = "1";
                aVar.DropDownOptions = new List<ComponentType.IoVariableDropDownOption>();
                {
                    ComponentType.IoVariableDropDownOption op = new ComponentType.IoVariableDropDownOption();
                    op.Value = "0" ;
                    op.DisplayText = "Legs";
                    aVar.DropDownOptions.Add(op);
                }
                {
                    ComponentType.IoVariableDropDownOption op = new ComponentType.IoVariableDropDownOption();
                    op.Value = "1";
                    op.DisplayText = "Wheels";
                    aVar.DropDownOptions.Add(op);
                }
                aFunction.InputVariables.InputVariables.Add(aVar);
            }



            aFunction.FunctionExample = "";

            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will transform the robot to use wheels instead of legs.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample += ct.GenerateExampleCode(aFunction, actualIO);

            ct.FunctionsMeta.Add(aFunction);
            ret.Add(aFunction);
        }
        if (LegsType == EnumLegType.Spider)
        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.DropDownBx;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Strafe Left/Right";
            aFunction.FunctionDescription = "Set your robot to strafe (move sideways) in a direction";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "4";
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Bx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.StrafeState;
                aVar.RegisterDescription = "Pass of either '0', '1' or '2' to change between no strafing, strafing left and strafing right.";
                aVar.ExampleValue = "1";
                aVar.DropDownOptions = new List<ComponentType.IoVariableDropDownOption>();
                {
                    ComponentType.IoVariableDropDownOption op = new ComponentType.IoVariableDropDownOption();
                    op.Value = "0";
                    op.DisplayText = "Move Forward / No Strafing";
                    aVar.DropDownOptions.Add(op);
                }
                {
                    ComponentType.IoVariableDropDownOption op = new ComponentType.IoVariableDropDownOption();
                    op.Value = "1";
                    op.DisplayText = "Strafe Left";
                    aVar.DropDownOptions.Add(op);
                }
                {
                    ComponentType.IoVariableDropDownOption op = new ComponentType.IoVariableDropDownOption();
                    op.Value = "2";
                    op.DisplayText = "Strafe Right";
                    aVar.DropDownOptions.Add(op);
                }
                aFunction.InputVariables.InputVariables.Add(aVar);
            }
          


            aFunction.FunctionExample = "";

            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will rotate the weapon.";
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

   

    void MyIoHandler_IoHandler(ref Computer.StandardStack runtimeStack)
    {

        int axVal = int.Parse(runtimeStack.Ax.Val);

        switch (axVal)
        {
            case 1:
                {

                  

                    WantedLookDestination = new Vector3(float.Parse(runtimeStack.Bx.Val), float.Parse(runtimeStack.Cx.Val), float.Parse(runtimeStack.Dx.Val));
                    myMeta.AddEventLog("Now Looking at " + WantedLookDestination);
                    if (Vector3.Distance(WantedLookDestination, gameObject.transform.position) >= 0.1f)
                    {
                        switch (LegsType)
                        {

                            case EnumLegType.Aircraft:
                                {
                                    targetDir = WantedLookDestination - transform.position;
                                    break;
                                }
                            case EnumLegType.Spider:
                                {
                                    WantedLookDestination.y = gameObject.transform.position.y;
                                    targetDir = WantedLookDestination - transform.position;
                                    break;
                                }
                            case EnumLegType.Track:
                                {
                                    WantedLookDestination.y = gameObject.transform.position.y;
                                    targetDir = WantedLookDestination - transform.position;
                                    break;
                                }
                            case EnumLegType.Walker:
                                {
                                    WantedLookDestination.y = gameObject.transform.position.y;
                                    targetDir = WantedLookDestination - transform.position;
                                    break;
                                }
                            case EnumLegType.Wheeled:
                                {
                                    WantedLookDestination.y = gameObject.transform.position.y;// Arena.PublicAccess.Therrain.SampleHeight(WantedLookDestination) + Arena.PublicAccess.gameObject.transform.position.y;
                                    targetDir = WantedLookDestination - transform.position;
                                    if (buggyController != null)
                                        buggyController.TargetDirection = targetDir;
                                    break;
                                }

                        }

                    }
                    break;
                }
            case 2:
                {
                    WantedSpeedOutOf100 = int.Parse(runtimeStack.Bx.Val);
                    myMeta.AddEventLog("Moving at " + WantedSpeedOutOf100 + " speed");

                    break;
                }
            case 3:
                {
                    myMeta.AddEventLog("Transforming");
                    int val = int.Parse(runtimeStack.Bx.Val);
                    if (val == 0)
                        WantedAnimatorTransformState = EnumAnimatorTransformationState.Legs;
                    if (val == 1)
                        WantedAnimatorTransformState = EnumAnimatorTransformationState.Roller;
                    break;
                }
            case 4:
                {
                    myMeta.AddEventLog("Strafing");
                    int val = int.Parse(runtimeStack.Bx.Val);
                    if (val == 0)
                        AnimatorTurnState = EnumAnimatorTurnState.NoTurn;
                    if (val == 1)
                        AnimatorTurnState = EnumAnimatorTurnState.StafeLeft;
                    if (val == 2)
                        AnimatorTurnState = EnumAnimatorTurnState.StrafeRight;


                    break;
                }
        }

    }
    float speed = 1;
    float bias = 300;
    public float bitUp = 0.000f;
    RobotMeta myMeta;
    float tranformWaitTime = 0f;
    RaycastHit groumdHit;

    public bool IsOnGround = false;
    private void StickToGroundRotation()
    {
        MyAnimator.applyRootMotion = false;
        if (IsLegsTouchingSomething)
        {
            IsOnGround = true;
        }
        else
        {
            if (Physics.Raycast(transform.position + Vector3.up * 1f, -Vector3.up, out groumdHit, 3f))
            {
                IsOnGround = true;
            }
        }
        
        var rot = Quaternion.FromToRotation(Vector3.up, curNormal);
        transform.rotation = rot * iniRot;
       
    }
    private void StickToGroundRotation2()
    {
        MyAnimator.applyRootMotion = false;
        if (IsLegsTouchingSomething)
        {
            IsOnGround = true;
        }
        else
        {
            if (Physics.Raycast(transform.position + Vector3.up * 1f, -Vector3.up, out groumdHit, 3f))
            {
                IsOnGround = true;
            }
        }
        
        var rot = Quaternion.FromToRotation(Vector3.up, curNormal);
        transform.rotation = rot * iniRot;


    }
    
    
    private void DoHeatUpdate()
    {
        float heatUsage = (Mathf.Abs(ActualSpeedOutOf100) + 1f) / 100f;
        myComponentType.AddUsageHeat(heatUsage);

    }
    float accelBias = 50000;

    float currentAccelAdd = 0;
    float acelBiasInc = 50f;
    void FixedUpdate()
    {
        if (myComponentType.IsOverheated == false)
        {
            DoHeatUpdate();
            StickToGroundRotation();
            if (myMeta == null) myMeta = gameObject.GetComponent<RobotMeta>();
            if (myMeta != null)
            {
                if (myMeta.Health <= 0)
                {
                    IsDead = true;
                }
                else
                {
                    IsDead = false;
                }
            }
            if (IsDead == false)
            {
                if (
                AnimatorHealthState == EnumAnimatorHealthState.Dead)
                {

                    if (Match.PublicAccess != null) Match.PublicAccess.TurnRobotComputerOnOff(gameObject, true);
                    AnimatorHealthState = EnumAnimatorHealthState.Idle;
                }
                CurrentVelocity = MyRigidbody.velocity.magnitude;
                if (TestTransform != null)
                {
                    Vector3 anglePoint = TestTransform.transform.position;



                    WantedLookDestination = anglePoint;
                }


                ActualLookDestination = Vector3.Lerp(ActualLookDestination, WantedLookDestination, SettingRotationSpeedOutOf100 * 0.01f);

                Vector3 onPlane = ActualLookDestination;
                onPlane.y = gameObject.transform.position.y;

               

                if (WantedSpeedOutOf100 > 100) WantedSpeedOutOf100 = 100;
                if (WantedSpeedOutOf100 < -100) WantedSpeedOutOf100 = -100;

                if (WantedSpeedOutOf100 > ActualSpeedOutOf100)
                {
                    ActualSpeedOutOf100 += 1f;
                }
                if (WantedSpeedOutOf100 < ActualSpeedOutOf100)
                {
                    ActualSpeedOutOf100 -= 1f;
                }

                float actualMaxSpeed = SettingMaxSpeed;
                float actualMaxAccelletatro = SettingMaxSpeed;
                if (AnimatorTransformState == EnumAnimatorTransformationState.Legs)
                {
                    if (AnimatorTurnState == EnumAnimatorTurnState.StafeLeft || AnimatorTurnState == EnumAnimatorTurnState.StrafeRight)
                    {

                        actualMaxSpeed = SettingMaxSpeedStrafe;
                         }
                }
                 speed = ActualSpeedOutOf100 / 100.0f;
                float weightRatio = 1;
                if (myComponentType.WeightInKG > 0)
                {
                    weightRatio = myComponentType.WeightInKG / 1000f;
                }

                if (currentAccelAdd < 0)
                {
                    currentAccelAdd = 0;
                }

                if (IsOn)
                {
                    switch (LegsType)
                    {

                        case EnumLegType.Aircraft:
                            {
                                
                                float acceleration = 0f;

                                if (ActualSpeedOutOf100 > 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = currentAccelAdd;// myComponentType.Legs_AccelerationAdd;

                                }
                                if (ActualSpeedOutOf100 < 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = -currentAccelAdd;
                                }
                                currentAccelAdd -= Time.deltaTime * acelBiasInc * 0.3f;
                                if (currentAccelAdd < 0)
                                {
                                    currentAccelAdd = 0;
                                }
                                 {

                                    if (Mathf.Abs(CurrentVelocity) < Mathf.Abs(speed * myComponentType.Legs_MaxForwardSpeed))
                                        MyRigidbody.AddForce(gameObject.transform.forward * acceleration * accelBias * weightRatio, ForceMode.Force);

                                }

                                float step = SettingRotationSpeedOutOf100 * Time.deltaTime * 0.1f;
                                Vector3 newDir = Vector3.RotateTowards(transform.forward + transform.up * bitUp, targetDir, step, 0.0F);
                                iniRot = Quaternion.LookRotation(newDir);
                               
                                break;
                            }
                        case EnumLegType.Spider:
                            {
                                if (WantedAnimatorTransformState == AnimatorTransformState && tranformWaitTime <= 0)
                                {
                                    if (IsOnGround)
                                    {
                                        {
                                            Vector3 forw = gameObject.transform.forward;
                                            if (AnimatorTransformState == EnumAnimatorTransformationState.Legs)
                                            {
                                                if (AnimatorTurnState == EnumAnimatorTurnState.StafeLeft)
                                                {
                                                    forw = gameObject.transform.right * -1;
                                                }
                                                if (AnimatorTurnState == EnumAnimatorTurnState.StrafeRight)
                                                {
                                                    forw = gameObject.transform.right * 1;
                                                }
                                            }
                                            float acceleration = 0f;

                                            if (ActualSpeedOutOf100 > 0)
                                            {
                                                currentAccelAdd += Time.deltaTime * acelBiasInc;
                                                if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                                {
                                                    currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                                }
                                                acceleration = currentAccelAdd;// myComponentType.Legs_AccelerationAdd;

                                            }
                                            if (ActualSpeedOutOf100 < 0)
                                            {
                                                currentAccelAdd += Time.deltaTime * acelBiasInc;
                                                if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                                {
                                                    currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                                }
                                                acceleration = -currentAccelAdd;// myComponentType.Legs_AccelerationAdd;
                                            }
                                            currentAccelAdd -= Time.deltaTime * acelBiasInc * 0.3f;
                                            if (currentAccelAdd < 0)
                                            {
                                                currentAccelAdd = 0;
                                            }

                                            {

                                                if (Mathf.Abs(CurrentVelocity) < Mathf.Abs(speed * myComponentType.Legs_MaxForwardSpeed))
                                                    MyRigidbody.AddForce(gameObject.transform.forward * acceleration * accelBias * weightRatio, ForceMode.Force);

                                            }
                                        }

                                    }

                                    float step = SettingRotationSpeedOutOf100 * Time.deltaTime * 0.1f;
                                    Vector3 newDir = Vector3.RotateTowards(transform.forward + transform.up * bitUp, targetDir, step, 0.0F);
                                    iniRot = Quaternion.LookRotation(newDir);
                                }
                                else
                                {
                                    if (WantedAnimatorTransformState != AnimatorTransformState)
                                    {

                                        tranformWaitTime = 2f;
                                        AnimatorTransformState = WantedAnimatorTransformState;
                                    }
                                    tranformWaitTime -= Time.deltaTime;
                                }
                                break;
                            }
                        case EnumLegType.Track:
                            {
                                float acceleration = 0f;

                                if (ActualSpeedOutOf100 > 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = currentAccelAdd;

                                }
                                if (ActualSpeedOutOf100 < 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = -currentAccelAdd;
                                }
                                currentAccelAdd -= Time.deltaTime * acelBiasInc * 0.3f;
                                if (currentAccelAdd < 0)
                                {
                                    currentAccelAdd = 0;
                                }
                                if (IsOnGround)
                                {

                                    if (Mathf.Abs(CurrentVelocity) < Mathf.Abs(speed * myComponentType.Legs_MaxForwardSpeed))
                                        MyRigidbody.AddForce(gameObject.transform.forward * acceleration * accelBias * weightRatio, ForceMode.Force);

                                }

                                float step = SettingRotationSpeedOutOf100 * Time.deltaTime * 0.1f;
                                Vector3 newDir = Vector3.RotateTowards(transform.forward + transform.up * bitUp, targetDir, step, 0.0F);
                                iniRot = Quaternion.LookRotation(newDir);
                                break;
                            }
                        case EnumLegType.Walker:
                            {

                                float acceleration = 0f;

                                if (ActualSpeedOutOf100 > 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = currentAccelAdd;

                                }
                                if (ActualSpeedOutOf100 < 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = -currentAccelAdd;
                                }
                                currentAccelAdd -= Time.deltaTime * acelBiasInc * 0.3f;
                                if (currentAccelAdd < 0)
                                {
                                    currentAccelAdd = 0;
                                }
                                if (IsOnGround)
                                {

                                    if (Mathf.Abs(CurrentVelocity) < Mathf.Abs(speed * myComponentType.Legs_MaxForwardSpeed))
                                        MyRigidbody.AddForce(gameObject.transform.forward * acceleration * accelBias * weightRatio, ForceMode.Force);

                                }



                                {

                                    float step = SettingRotationSpeedOutOf100 * Time.deltaTime * 0.1f;
                                    Vector3 newDir = Vector3.RotateTowards(transform.forward + transform.up * bitUp, targetDir, step, 0.0F);
                                    iniRot = Quaternion.LookRotation(newDir);
                                }
                                break;
                            }
                        case EnumLegType.Wheeled:
                            {
                                float acceleration = 0f;

                                if (ActualSpeedOutOf100 > 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = currentAccelAdd;

                                }
                                if (ActualSpeedOutOf100 < 0)
                                {
                                    currentAccelAdd += Time.deltaTime * acelBiasInc;
                                    if (currentAccelAdd > myComponentType.Legs_AccelerationAdd)
                                    {
                                        currentAccelAdd = myComponentType.Legs_AccelerationAdd;
                                    }
                                    acceleration = -currentAccelAdd;
                                }
                                currentAccelAdd -= Time.deltaTime * acelBiasInc * 0.3f;
                                if (currentAccelAdd < 0)
                                {
                                    currentAccelAdd = 0;
                                }
                                if (IsOnGround)
                                {

                                    if (Mathf.Abs(CurrentVelocity) < Mathf.Abs(speed * myComponentType.Legs_MaxForwardSpeed))
                                        MyRigidbody.AddForce(gameObject.transform.forward * acceleration * accelBias * weightRatio, ForceMode.Force);

                                }

                                if (buggyController != null)
                                    buggyController.WheelSpinSpeed = ActualSpeedOutOf100;

                                float step = SettingRotationSpeedOutOf100 * Time.deltaTime * 0.1f;
                                Vector3 newDir = Vector3.RotateTowards(transform.forward + transform.up * bitUp, targetDir, step, 0.0F);
                                iniRot = Quaternion.LookRotation(newDir);
                                break;
                            }
                    }


                }
            }
            else
            {
                if (AnimatorHealthState != EnumAnimatorHealthState.Dead)
                {
                    if (Match.PublicAccess != null)
                        Match.PublicAccess.TurnRobotComputerOnOff(gameObject, false);

                }

                AnimatorHealthState = EnumAnimatorHealthState.Dead;
            }
        }
        else
        {
            ActualSpeedOutOf100 = 0;
            AnimatorHealthState = EnumAnimatorHealthState.Dead;

        }
        UpdateAnimator();
    }
   
   
    private void AdjustScales()
    {
        Transform[] allChilds = gameObject.transform.GetComponentsInChildren<Transform>();
        for (int c = 0; c < allChilds.Length; c++)
        {
            if (allChilds[c].localScale.x == -1)
            {
                Transform[] allChilds2 = allChilds[c].transform.GetComponentsInChildren<Transform>();
                for (int c2 = 0; c2 < allChilds2.Length; c2++)
                {
                    allChilds2[c2].localScale = new Vector3(-1 * allChilds2[c2].localScale.x, 1, 1);
                }
            }
        }

    }
    private void UpdateAnimator()
    {
        if (MyAnimator != null)
        {

            if (IsOn && myComponentType.IsOverheated == false)
            {
                AnimatorHealthState = EnumAnimatorHealthState.Idle;
            }
            else
            {
                AnimatorHealthState = EnumAnimatorHealthState.Deactivate;
            }

            if (IsDead)
            {
                AnimatorHealthState = EnumAnimatorHealthState.Dead;
            }

            float walkSpeedAnim = 0;

            if (ActualSpeedOutOf100 != 0)
            {
                walkSpeedAnim = ActualSpeedOutOf100 / 100;
            }
            
            switch (LegsType)
            {

                case EnumLegType.Aircraft:
                    {
                        //MyAnimator.SetInteger("WalkSpeed", (int)AnimatorWalkSpeed);
                        //MyAnimator.SetInteger("HealthState", (int)AnimatorHealthState);
                        //MyAnimator.SetInteger("TurnState", (int)AnimatorTurnState);
                        break;
                    }
                case EnumLegType.Spider:
                    {
                        float fakeAnimSpeed = walkSpeedAnim;
                        int fakeTurnState = (int)AnimatorTurnState;
                        if (fakeAnimSpeed < 0)
                        {
                            if (fakeTurnState == 2)
                            {
                                fakeAnimSpeed = Mathf.Abs(fakeAnimSpeed);
                                fakeTurnState = 3;
                            }
                            else
                            {
                                if (fakeTurnState == 3)
                                {
                                    fakeAnimSpeed = Mathf.Abs(fakeAnimSpeed);
                                    fakeTurnState = 2;
                                }
                            }
                        }

                        MyAnimator.SetFloat("WalkBlendSpeed", fakeAnimSpeed);
                        MyAnimator.SetInteger("HealthState", (int)AnimatorHealthState);
                          MyAnimator.SetInteger("TurnState", fakeTurnState);
                        MyAnimator.SetInteger("TransformState", (int)AnimatorTransformState);
                        break;
                    }
                case EnumLegType.Track:
                    {
                        MyAnimator.SetFloat("WalkBlendSpeed", walkSpeedAnim);
                        MyAnimator.SetInteger("HealthState", (int)AnimatorHealthState);
                        MyAnimator.SetInteger("TurnState", (int)AnimatorTurnState);
                        break;
                    }
                case EnumLegType.Walker:
                    {
                        MyAnimator.SetFloat("WalkBlendSpeed", walkSpeedAnim);
                        MyAnimator.SetInteger("HealthState", (int)AnimatorHealthState);
                        MyAnimator.SetInteger("TurnState", (int)AnimatorTurnState);
                        break;
                    }
                case EnumLegType.Wheeled:
                    {
                        MyAnimator.SetFloat("WalkBlendSpeed", walkSpeedAnim);
                        MyAnimator.SetInteger("HealthState", (int)AnimatorHealthState);
                        MyAnimator.SetInteger("TurnState", (int)AnimatorTurnState);
                          break;
                    }
            }
        }
    }



    void OnCollisionEnter(Collision collision)
    {
        HandleTouchEnter(collision.gameObject.transform);
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        HandleTouchEnter(collisionInfo.gameObject.transform);
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        HandleTouchExit(collisionInfo.gameObject.transform);
    }

   
    public List<Transform> currentTouching = new List<Transform>();

    private void HandleTouchEnter(Transform aTrans)
    {
        if (currentTouching.Contains(aTrans) == false) currentTouching.Add(aTrans);

        if (currentTouching.Count > 0)
        {
            IsLegsTouchingSomething = true;
        }

    }
    private void HandleTouchExit(Transform aTrans)
    {
        if (currentTouching.Contains(aTrans)) currentTouching.Remove(aTrans);

        if (currentTouching.Count == 0)
        {
            IsLegsTouchingSomething = false;
        }

    }
}

public interface FunctionsContainer
{
    void SetComponentFunctions(ComponentType aType, string actualIO);

}

