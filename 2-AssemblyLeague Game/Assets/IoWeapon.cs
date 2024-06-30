using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IoWeapon : MonoBehaviour, FunctionsContainer
{
    public ComponentType myComponentType;
    public float SettingMaxYRotation = 45;
    public float SettingMaxZRotation = 45;
    public bool IsWeaponOneAnimShot = false;
  
    public GameObject BulletPrefab;
    public enum WeaponShootAnimation
    {
        NoSet = 0,
        Gadget_Minigun,
        GrenadeLauncher,
        Cannon,
        DoubleGun_Single,
        DoubleGun_Round,
        Flak,
        Sniper,
        MachineGun,
        Minigun1,
        Minigun2,
        Minigun3,
        Minigun4,
        Minigun5,




    }
    public WeaponShootAnimation ShootAnimation;
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

    
    public IOHandler MyIoHandler;

    public float SettingRotationSpeedOutOf100 = 1f;

    public bool IsOn = true;
    public bool IsDead = false;
    public Transform TestTransform;

    public Bullet.BulletType TheBulletType;
    public List<Transform> BulletExitPoints;
    public List<Transform> BulletsToHide;
    public List<Transform> BulletsToShow;

    public RobotMeta TheRobotMeta;

    // Use this for initialization
    void Start()
    {
        myComponentType = GetComponent<ComponentType>();
        if (TheRobotMeta == null)
        {
            TheRobotMeta = FindRobotMetaParent();
        }
        shootStartTime = System.DateTime.Now;
        shootEndTime = System.DateTime.Now;
        if (MyIoHandler == null)
        {
            MyIoHandler = gameObject.GetComponent<IOHandler>();

        }
        if (MyAnimator == null)
        {
            MyAnimator = gameObject.GetComponent<Animator>();

        }

        MyIoHandler.IoHandler += new IOHandler.DelegateHandleIO(MyIoHandler_IoHandler);
        ComponentType ct = (ComponentType)gameObject.GetComponent<ComponentType>();
        SetComponentFunctions(ct, "0");
        if (MyAnimator != null)
        {
            MyAnimator.SetInteger("WeaponID", (int)ShootAnimation);

        }
        DoShootEffectsEnd();
        SpawnAllMuzzles();
        StopParticles();
    }
    private void SpawnAllMuzzles()
    {
        if (RobotConstructor.PublicAccess != null)
        {
            for (int c = 0; c < BulletExitPoints.Count; c++)
            {
                {
                    GameObject muzzleFab = RobotConstructor.PublicAccess.GetMuzzleFirePrefab();
                    GameObject muzzle = Instantiate(muzzleFab, BulletExitPoints[c]);
                    muzzle.transform.parent = BulletExitPoints[c];
                }
                {
                    GameObject muzzleFab = RobotConstructor.PublicAccess.GetExpendCasingPrefab();
                    GameObject muzzle = Instantiate(muzzleFab, BulletExitPoints[c]);
                    muzzle.transform.parent = BulletExitPoints[c];
                }
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
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Fire Weapon";
            aFunction.FunctionDescription = "Activate this function to fire off the attached device ";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "1";

            aFunction.FunctionExample = "";
            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will show how to shoot directly forward.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample += ct.GenerateExampleCode(aFunction, actualIO);
       
            ct.FunctionsMeta.Add(aFunction);
            ret.Add(aFunction);
        }
        {
            ComponentType.IoFunctionMeta aFunction = new ComponentType.IoFunctionMeta();
            aFunction.InputVariables = new ComponentType.IoInput();
            aFunction.InputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.AngleBx;
            aFunction.InputVariables.InputVariableMain = new ComponentType.IoVariable();
            aFunction.InputVariables.InputVariables = new List<ComponentType.IoVariable>();


            aFunction.OutputVariables = new ComponentType.IoOutput();
            aFunction.OutputVariables.DataSetType = ComponentType.IoFunctionMeta.EnumIODataSetType.NoExtra;
            aFunction.OutputVariables.OutputVariables = new List<ComponentType.IoVariable>();

            aFunction.FunctionName = "Set Angle";
            aFunction.FunctionDescription = "Set the angle of rotation for this weapon";
            aFunction.InputVariables.InputVariableMain.Register = ComponentType.IoVariable.EnumRegisters.Ax;
            aFunction.InputVariables.InputVariableMain.RegisterDescription = "Function Select";
            aFunction.InputVariables.InputVariableMainValue = "2";
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Bx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.AngleAroundY;
                aVar.RegisterDescription = "The angle to rotate around the Y-Axis";
                aVar.ExampleValue = "15";
                aFunction.InputVariables.InputVariables.Add(aVar);
            }
            {
                ComponentType.IoVariable aVar = new ComponentType.IoVariable();
                aVar.Register = ComponentType.IoVariable.EnumRegisters.Cx;
                aVar.RegisterType = ComponentType.IoVariable.EnumRegisterDataType.AngleAroundZ;
                aVar.RegisterDescription = "The angle to rotate around the Z-Axis";
                aFunction.InputVariables.InputVariables.Add(aVar);
                aVar.ExampleValue = "10";
            }


            aFunction.FunctionExample = "";

            aFunction.FunctionExample += "Mapped to IO " + actualIO + ", this example will rotate the weapon.";
            aFunction.FunctionExample += "\n";
            aFunction.FunctionExample +=ct. GenerateExampleCode(aFunction, actualIO);
        
            ct.FunctionsMeta.Add(aFunction);
            ret.Add(aFunction);
        }
        return ret;
    }
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
                    if (shootEndTime <= System.DateTime.Now)
                    {
                        TheRobotMeta.AddEventLog("Firing Weapon");
                        MustShoot = true;
                    }
                    break;
                }
            case 2:
                {
               
                    TheRobotMeta.AddEventLog("Changing Weapon Angle");
                    float angleY = float.Parse(runtimeStack.Bx.Val);
                    float angleZ = float.Parse(runtimeStack.Cx.Val);

                    if (angleY < -1 * SettingMaxYRotation) angleY = -1 * SettingMaxYRotation;
                    if (angleY > 1 * SettingMaxYRotation) angleY = 1 * SettingMaxYRotation;
                    if (angleZ < -1 * SettingMaxZRotation) angleZ = -1 * SettingMaxZRotation;
                    if (angleZ > 1 * SettingMaxZRotation) angleZ = 1 * SettingMaxZRotation;

                    RotationDegreesYWanted = angleY;
                    RotationDegreesZWanted = angleZ;                
                    break;
                }


        }

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

                    functionText += functionsMeta[c].InputVariables.InputVariableMain + ": " + functionsMeta[c].InputVariables.InputVariableMainValue + " ";
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
  
    public RobotPart GetRobotPart()
    {
        RobotPart ret = null;

        bool MustEnd = false;
        Transform curTrans = gameObject.transform;
        while (MustEnd == false)
        {
            ret = curTrans.GetComponent<RobotPart>();
            if (ret != null)
            {
                MustEnd = true;
            }

        }


        return ret;
    }
    public float GetXScaleOLD()
    {
        float ret = 1;

        bool MustEnd = false;
        Transform curTrans = gameObject.transform;
        while (MustEnd == false)
        {
            if (curTrans.localScale.x == -1)
            {
                ret = curTrans.localScale.x;
                MustEnd = true;
            }
            if (curTrans.parent == null)
            {
                MustEnd = true;
            }
            else
            {
                curTrans = curTrans.parent;
            }
        }

        ret = transform.parent.localScale.x;
        return ret;
    }
    public float SetXScale()
    {
        float ret = 1;

        bool MustEnd = false;
        Transform curTrans = gameObject.transform;
        while (MustEnd == false)
        {
            if (curTrans.localScale.x == -1)
            {
                ret = curTrans.localScale.x;

                Transform[] allChilds = gameObject.transform.parent.GetComponentsInChildren<Transform>();
                for (int c = 0; c < allChilds.Length; c++)
                {
                    allChilds[c].localScale = new Vector3(-1, 1, 1);
                }

                MustEnd = true;
            }
            if (curTrans.parent == null)
            {
                MustEnd = true;
            }
            else
            {
                curTrans = curTrans.parent;
            }
        }

        ret = transform.parent.localScale.x;
        return ret;
    }
    
    System.DateTime shootStartTime;
    System.DateTime shootEndTime;
    public bool MustShoot;
    public float WantedAngle;
    public float AngleInPlane(Transform from, Vector3 to, Vector3 planeNormal)
    {
        Vector3 dir = to - from.position;
        dir.Normalize();
        Vector3 p1 = Project(dir, planeNormal);
        Vector3 p2 = Project(from.forward, planeNormal);

        return Vector3.Angle(p1, p2);
    }
    public float AngleInPlane2(Transform from, Vector3 to, Vector3 planeNormal)
    {
        Vector3 dir = to - from.position;

        Vector3 p1 = Project(dir, planeNormal);
        Vector3 p2 = Project(from.forward, planeNormal);

        return Vector3.Angle(p1, p2);
    }
    public Vector3 Project(Vector3 v, Vector3 onto)
    {
        return v - (Vector3.Dot(v, onto) / Vector3.Dot(onto, onto)) * onto;
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
    bool lastFrameStillShooting = false;
    // Update is called once per frame
    private void DoShoot()
    {

        MustShoot = false;
        if (shootEndTime <= System.DateTime.Now)
        {

            hasShot = true;
            if (BulletExitPoints == null || BulletExitPoints.Count == 0)
            {
                DoShootEffectsStart();
                Bullet newBullet = BulletCache.PublicAccess.GetBulletToShoot(TheBulletType);
                newBullet.gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * 2.5f;
                newBullet.gameObject.transform.rotation = gameObject.transform.rotation;

                newBullet.Fire(TheRobotMeta, this);
                frameCountDown = 10;
                myComponentType.AddUsageHeat();
            }
            else
            {
                myComponentType.AddUsageHeat();
                frameCountDown = 10;
                DoShootEffectsStart();
                for (int c = 0; c < BulletExitPoints.Count; c++)
                {
                    if (TheBulletType != Bullet.BulletType.NoBullet)
                    {
                       
                        Bullet newBullet = BulletCache.PublicAccess.GetBulletToShoot(TheBulletType);
                        Vector3 pos = BulletExitPoints[c].transform.position + BulletExitPoints[c].transform.forward * 2.5f;
                        newBullet.gameObject.transform.position = pos;
                        newBullet.gameObject.transform.rotation = BulletExitPoints[c].transform.rotation;
                        newBullet.Fire(TheRobotMeta, this);
                    }
                    ParticleSystem[] muzzleFlash = BulletExitPoints[c].GetComponentsInChildren<ParticleSystem>(true);

                    for (int z = 0; z < muzzleFlash.Length; z++)
                    {
                          //print("MUZZLE PLAY");
                        muzzleFlash[z].Play();
                    }

                }

            }
            shootEndTime = System.DateTime.Now.AddSeconds(myComponentType.Weapon_Shoot_DelayTime*0.1f);
        }
        else
        {

        }


    }
    int frameCountDown = 10;
    public void ExtraBullet()
    {
        for (int c = 0; c < BulletExitPoints.Count; c++)
        {
            {
                if (TheBulletType != Bullet.BulletType.NoBullet)
                {
                    Bullet newBullet = BulletCache.PublicAccess.GetBulletToShoot(TheBulletType);
                  
                    newBullet.gameObject.transform.position = BulletExitPoints[c].transform.position + BulletExitPoints[c].transform.forward * 2.5f;
                    newBullet.gameObject.transform.rotation = BulletExitPoints[c].transform.rotation;
                    newBullet.Fire(TheRobotMeta, this);
                }
                ParticleSystem[] muzzleFlash = BulletExitPoints[c].GetComponentsInChildren<ParticleSystem>(true);

                for (int z = 0; z < muzzleFlash.Length; z++)
                {
                    //  print("MUZZLE PLAY");
                    muzzleFlash[z].Play();
                }
            }
        }
    }
    private void DoShootEffectsStart()
    {

        IsFireDamageActive = true;
        if (MyAnimator != null)
        {
            MyAnimator.SetInteger("ActionID", 1);

        }
        lastFrameStillShooting = true;
        if (BulletsToHide != null && BulletsToHide.Count > 0)
        {
            for (int c = 0; c < BulletsToHide.Count; c++)
            {
                BulletsToHide[c].gameObject.SetActive(false);
            }
        }

       
    }
    private void DoShootEffectsEnd()
    {

        IsFireDamageActive = false;
         if (MyAnimator != null)
        {
            MyAnimator.SetInteger("ActionID", 0);

        }
        lastFrameStillShooting = false;
        if (BulletsToHide != null && BulletsToHide.Count > 0)
        {
            for (int c = 0; c < BulletsToHide.Count; c++)
            {
                if (BulletsToHide[c] != null)
                {
                    BulletsToHide[c].gameObject.SetActive(true);
                }
            }
        }
       
    }
    public bool hasShot = false;
    float shotCountDown = 1f;
    public float RotationDegreesYWanted = 0;
    public float RotationDegreesZWanted = 0;
    public float RotationDegreesYActual = 0;
    public float RotationDegreesZActual = 0;
    public float Setting_RotationSpeed = 1f;
    int frameCount = 0;
    private void StopParticles()
    {
         for (int c = 0; c < BulletExitPoints.Count; c++)
        {
            ParticleSystem[] muzzleFlash = BulletExitPoints[c].GetComponentsInChildren<ParticleSystem>(true);

            for (int z = 0; z < muzzleFlash.Length; z++)
            {
                 muzzleFlash[z].Stop();
            }
        }

    }

    public bool IsFireDamageActive = false;
    float fireActiveTime = 0f;
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

                {
                    if (hasShot)
                    {
                        shotCountDown -= Time.deltaTime;
                        if (shotCountDown <= 0)
                        {
                            if (MyAnimator != null)
                            {
                                MyAnimator.SetInteger("ActionID", 0);

                            }
                            StopParticles();
                            hasShot = false;
                            shotCountDown = 2f;
                            IsFireDamageActive = false;
                        }
                    }

                }


                if (TheRobotMeta != null && TheRobotMeta.Health > 0)
                {
                    if (IsOn)
                    {
                        Quaternion originalRot = gameObject.transform.localRotation;
                        gameObject.transform.localRotation = Quaternion.AngleAxis(RotationDegreesZActual, -1 * Vector3.right) * Quaternion.AngleAxis(RotationDegreesYActual, -1 * Vector3.up);

                        if (MustShoot)
                        {
                            DoShoot();
                        }

                        if (lastFrameStillShooting)
                        {
                            if (shootEndTime <= System.DateTime.Now)
                            {

                                lastFrameStillShooting = false;

                            }

                        }

                    }




                }

            }
            else
            {
                DoShootEffectsEnd();

            }
        }
        else
        {
            DoShootEffectsEnd();

        }
    }

  


    public void LookAtTrue(Vector3 target, float time)
    {
        Vector3 WantedLookDestination = target;
        Vector3 targetDir = WantedLookDestination - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward + Vector3.up * 1, targetDir, 1f, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        Quaternion look = Quaternion.LookRotation(newDir);

        Vector3 eular = look.eulerAngles;
        
        LeanTween.rotate(this.gameObject, eular, 0);
    }


    public bool IsTouching = false;
    private void DoDomage(GameObject other)
    {
        RobotMeta theOther = other.GetComponent<RobotMeta>();
        if (theOther == null)
        {
            theOther = other.GetComponentInParent<RobotMeta>();
        }
        if (theOther != null)
        {
            theOther.TakeDamageFromComponentImpactWeapon(this);
        }

    }
    void OnCollisionEnter(Collision collision)
    {

        IsTouching = true;
        //   print("ENTER: " + collision.gameObject.name);
        DoDomage(collision.gameObject);
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        IsTouching = true;
        //    print("STAY: " + collisionInfo.gameObject.name);
        DoDomage(collisionInfo.gameObject);
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        //   print("EXIT: " + collisionInfo.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        IsTouching = true;
        //print("TRIGGER ENTER: " + other.gameObject.name);
        DoDomage(other.gameObject);

    }
    void OnTriggerStay(Collider other)
    {
        IsTouching = true;
        DoDomage(other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {

        //      print("TRIGGER EXIT: " + other.gameObject.name);
    }
}
