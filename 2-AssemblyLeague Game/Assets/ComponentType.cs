using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class ComponentType : MonoBehaviour
{
    public ComponentFx.FxType DestructMainEffect = ComponentFx.FxType.NotSet;
    public ComponentFx.FxType DestructSMallPartsEffect = ComponentFx.FxType.Steam;
    public bool IsComponenentUsable = true;
    public bool MustSelfDestruct = false;
    public string UniqueDeviceID;
    public string DeviceName;
    public string ShortDescription;
    public string SpeechDescription;
    public float OfflinePurchaseCost;
    public float WeightInKG = 1;
    public float Health = 1;
    public float Armour = 0;
    public float PowerUsedPerFrame = 1;
    public float PowerProvidedPerFrame = 0;

    public float Legs_MaxForwardSpeed = 1;
    public float Legs_MaxUpwardSpeed = 0;
    public float Legs_MaxRotateSpeed = 1;
    public float Legs_AccelerationAdd = 1;
    public float Legs_Grip = 1;



    public bool IsOverheated;
    float Heat_MaxHeat = 100;
    public float CurrentHeat = 0;
    public float HeatPerUsage = 1;
    public float HeatCooldownTime = 1f;

    public float InstructionsPerSecond = 5;

    public float Weapon_BulletDamage = 1f;
    public float Weapon_BulletSpeed = 1f;
    public float Weapon_Shoot_DelayTime = 5f;


    public float Cockpit_SettingMaxYRotation = 360;
    public float Cockpit_SettingMaxZRotation = 15;



   
    public List<string> Functions2 = new List<string>();
    public List<IoFunctionMeta> FunctionsMeta = new List<IoFunctionMeta>();

    public RobotMeta TheRobotMeta;
    public Rigidbody TheRigidBody;

    

    public bool IsGameReady = false;

    public enum EnumComponentType
    {
        Legs,
        Cockpit,
        Shoulders,
        LeftArm,
        RightArm,
        WeaponLeft,
        TopMount,
        Backpack,
        PilotHips,
        WeaponRight,
        WeaponAnySlot,
        ArmAnySlot,
        HalfshoulderAnySlot,
        RocketSlot1,
        RocketSlot2,
        Antenna,
        TopCapPart,
        TornPart,
        HandGun,
        Bullet,
        NotUsed,
        ShoulderRocket,
    }
    public static string GetDisplayString(EnumComponentType aType)
    {
        string ret = "";
        switch (aType)
        {
            case EnumComponentType.Antenna:
                {
                    ret = "Antennas";
                    break;
                }
            case EnumComponentType.ArmAnySlot:
                {
                    ret = "Arm";
                    break;
                }
            case EnumComponentType.Backpack:
                {
                    ret = "Backpack";
                    break;
                }
            case EnumComponentType.Bullet:
                {
                    ret = "Bullet";
                    break;
                }
            case EnumComponentType.Cockpit:
                {
                    ret = "Cockpit";
                    break;
                }
            case EnumComponentType.HalfshoulderAnySlot:
                {
                    ret = "Half-Shoulders";
                    break;
                }
            case EnumComponentType.HandGun:
                {
                    ret = "Hand Gun";
                    break;
                }
            case EnumComponentType.LeftArm:
                {
                    ret = "Left Arm";
                    break;
                }
            case EnumComponentType.Legs:
                {
                    ret = "Locomotion System";
                    break;
                }
            case EnumComponentType.PilotHips:
                {
                    ret = "Pilot Seat";
                    break;
                }
            case EnumComponentType.RightArm:
                {
                    ret = "Right Arm";
                    break;
                }
            case EnumComponentType.RocketSlot1:
                {
                    ret = "Rocket Type 1";
                    break;
                }
            case EnumComponentType.RocketSlot2:
                {
                    ret = "Rocket Type 2";
                    break;
                }
            case EnumComponentType.Shoulders:
                {
                    ret = "Shoulders";
                    break;
                }
            case EnumComponentType.TopCapPart:
                {
                    ret = "Top Cap";
                    break;
                }
            case EnumComponentType.TopMount:
                {
                    ret = "Top Mounting";
                    break;
                }
            case EnumComponentType.TornPart:
                {
                    ret = "Torn Part";
                    break;
                }
            case EnumComponentType.WeaponAnySlot:
                {
                    ret = "Any Weapon";
                    break;
                }
            case EnumComponentType.WeaponLeft:
                {
                    ret = "Left-sided Weapon";
                    break;
                }
            case EnumComponentType.WeaponRight:
                {
                    ret = "Right-sided Weapon";
                    break;
                }


            default:
                {
                    ret = aType + "";
                    break;
                }
        }

        return ret;
    }
    public EnumComponentType ModuleType;

    // Use this for initialization
    void Start()
    {
        Functions2 = new List<string>();
        FunctionsMeta = new List<IoFunctionMeta>();

        IsComponenentUsable = true;
        CurrentHeat = 0;
       
        TheRigidBody = gameObject.GetComponent<Rigidbody>();
        if (TheRigidBody != null)
        {
            TheRigidBody.mass = WeightInKG;
        }
        if (TheRobotMeta == null)
        {
            TheRobotMeta = FindRobotMetaParent();
        }
        if (gameObject.transform.parent != null)
        {
            if (gameObject.transform.localScale.x < 0 || gameObject.transform.localScale.y < 0 || gameObject.transform.localScale.z < 0)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
           
        }
        //  enabled = false;
    }
    ComponentType FindLegsParent()
    {
        ComponentType ret = null;
        bool mustContinue = true;
        Transform currentTans = gameObject.transform.parent;
        while (mustContinue)
        {
            ret = currentTans.GetComponent<ComponentType>();
            if (ret == null || ret.ModuleType == EnumComponentType.Legs)
            {
                mustContinue = false;
            }
        }
        return ret;
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
    // Update is called once per frame

    private void DoHeatCheck()
    {
        if (CurrentHeat > 0)
        {
            CurrentHeat -= HeatCooldownTime * Time.deltaTime;
        }
        if (CurrentHeat < 0)
        {
            CurrentHeat = 0;
        }
        if (CurrentHeat >= Heat_MaxHeat)
        {
            SpawnHeat();
            IsOverheated = true;
        }
        else
        {
            IsOverheated = false;
        }
        if (timeBeforeHeatEffect >= 0)
        {
            timeBeforeHeatEffect -= 1f * Time.deltaTime;
        }
        if (timeBeforeHeatEffect < 0) timeBeforeHeatEffect = 0;

        if (IsOverheated)
        {
            if (TheRobotMeta != null)
            {
                TheRobotMeta.AddOverheat();
            }
        }

    }
    float timeBeforeHeatEffect = 0;
    private void SpawnHeat()
    {
        if (timeBeforeHeatEffect <= 0)
        {
            ComponentFx anFX = FxCache.PublicAccess.GetEffectToApply(ComponentFx.FxType.Steam);
            anFX.TimeToRemain = 5f;
            anFX.ApplyAttachToTransform(gameObject.transform, Vector3.up);
            timeBeforeHeatEffect = 5f;
        }
    }
    public void AddUsageHeat()
    {
        CurrentHeat += HeatPerUsage;
    }
    public void AddUsageHeat(float pecentOutOf1)
    {
        CurrentHeat += HeatPerUsage * pecentOutOf1;
    }

    private void AddAllAcceptors()
    {
        List<Transform> trans = new List<Transform>();
        trans.AddRange(gameObject.transform.GetComponentsInChildren<Transform>());
        for (int c = 0; c < trans.Count; c++)
        {
            ModuleAcceptor anAcceptor = trans[c].GetComponent<ModuleAcceptor>();
            if (anAcceptor == null)
            {
                if (trans[c].gameObject.name.ToLower().Trim().Contains("_pivot") == false)
                {

                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_shoulder_rockets"))
                    {
                        print("Adding mount_shoulder_rockets acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }

                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_rockets_shoulder_l"))
                    {
                        print("Adding Mount_Rockets_Shoulder_L acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_rockets_shoulder_r"))
                    {
                        print("Adding Mount_Rockets_Shoulder_R acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_cockpit"))
                    {
                        print("Adding cockpit acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_weapon_l"))
                    {
                        print("Adding Mount_Weapon_L acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_weapon_r"))
                    {
                        print("Adding Mount_Weapon_R acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_backpack"))
                    {
                        print("Adding Mount_backpack acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                    if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_weapon_top"))
                    {
                        print("Adding Mount_Weapon_top acceptor to " + gameObject.name);
                        anAcceptor = trans[c].gameObject.AddComponent<ModuleAcceptor>();
                    }
                }
            }
            if (anAcceptor != null)
            {
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_shoulder_rockets"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.ShoulderRocket);

                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_rockets_shoulder_l"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.RocketSlot1);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.RocketSlot2);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_rockets_shoulder_r"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.RocketSlot1);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.RocketSlot2);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_top"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.Cockpit);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.Shoulders);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_cockpit"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.Cockpit);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.Shoulders);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_weapon_l"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.WeaponAnySlot);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.ArmAnySlot);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_weapon_r"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.WeaponAnySlot);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.ArmAnySlot);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_backpack"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.Backpack);
                }
                if (trans[c].gameObject.name.ToLower().Trim().Contains("mount_weapon_top"))
                {
                    anAcceptor = trans[c].gameObject.GetComponent<ModuleAcceptor>();
                    anAcceptor.AllowedTypes = new List<EnumComponentType>();
                    anAcceptor.AllowedTypes.Add(EnumComponentType.WeaponAnySlot);
                    anAcceptor.AllowedTypes.Add(EnumComponentType.TopMount);

                }
                anAcceptor.enabled = true;
            }
        }
    }
    void Update()
    {

        if (MustSelfDestruct)
        {
            ComponentSelfDestructor aSelf = gameObject.GetComponent<ComponentSelfDestructor>();
            if (aSelf == null) aSelf = gameObject.AddComponent<ComponentSelfDestructor>();
            if (aSelf != null)
            {

                aSelf.DestructMainEffect = DestructMainEffect;
                aSelf.DestructSMallPartsEffect = DestructSMallPartsEffect;
                aSelf.MustSelfDestruct = true;
            }
            CurrentHeat = 0;
            Health = 0;
            if (ModuleType == EnumComponentType.Legs)
            {
                TheRobotMeta.BaseHasExploded();
            }
            {
                TheRobotMeta.AnyComponentHasExploded();
            }

            IsComponenentUsable = false;
        }
        else
        {
            DoHeatCheck();

            if (TheRobotMeta == null)
            {
                TheRobotMeta = FindRobotMetaParent();
            }
            if (gameObject.transform.parent != null)
            {
                if (gameObject.transform.localScale.x < 0 || gameObject.transform.localScale.y < 0 || gameObject.transform.localScale.z < 0)
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                
            }

            if (UniqueDeviceID == null || UniqueDeviceID.Length <= 2)
            {
                UniqueDeviceID = System.Guid.NewGuid().ToString();
            }

            if (Application.isPlaying == false)
            {

                AddAllAcceptors();
                //     print("DOING ROBOT PART STUFF - SHOULD ONLY HAPPEN IN PAUSE");
                if (gameObject.transform.parent != null)
                {
                    RobotPart aPart = gameObject.GetComponent<RobotPart>();
                    Computer aComputer = gameObject.GetComponent<Computer>();
                    if (aPart != null && aComputer != null)
                    {
                        aPart.MyComputer = aComputer;
                        aComputer.Part = aPart;
                    }
                    int myIndex = gameObject.transform.GetSiblingIndex();
                     if (myIndex >= 0)
                    {
                        List<ComponentType> allTypes = new List<ComponentType>();
                        allTypes.AddRange(gameObject.transform.parent.GetComponentsInChildren<ComponentType>());

                        int myIndexSorted = 0;
                        for (int c = 0; c < allTypes.Count; c++)
                        {
                            if (string.CompareOrdinal(gameObject.name, allTypes[c].gameObject.name) > 0)
                            {
                                myIndexSorted++;
                            }
                        }

                        gameObject.transform.SetSiblingIndex(myIndexSorted);
                        Vector3 newPos = gameObject.transform.position;
                        newPos.z = 0;
                        newPos.y = gameObject.transform.parent.position.y;
                        newPos.x = myIndexSorted * 7f;
                        gameObject.transform.position = newPos;

                       
                        {
                            List<MeshFilter> allMeshFilters = new List<MeshFilter>();

                            allMeshFilters.AddRange(gameObject.GetComponents<MeshFilter>());
                            allMeshFilters.AddRange(gameObject.GetComponentsInChildren<MeshFilter>());

                            for (int c = 0; c < allMeshFilters.Count; c++)
                            {
                                MeshCollider aCollider = allMeshFilters[c].GetComponent<MeshCollider>();
                                if (aCollider == null)
                                {
                                    aCollider = allMeshFilters[c].gameObject.AddComponent<MeshCollider>();

                                }
                                aCollider.sharedMesh = allMeshFilters[c].sharedMesh;
                                aCollider.convex = true;
                            }
                        }
                        {
                            List<SkinnedMeshRenderer> allMeshFilters = new List<SkinnedMeshRenderer>();

                            allMeshFilters.AddRange(gameObject.GetComponents<SkinnedMeshRenderer>());
                            allMeshFilters.AddRange(gameObject.GetComponentsInChildren<SkinnedMeshRenderer>());

                            for (int c = 0; c < allMeshFilters.Count; c++)
                            {
                                MeshCollider aCollider = allMeshFilters[c].GetComponent<MeshCollider>();
                                if (aCollider == null)
                                {
                                    aCollider = allMeshFilters[c].gameObject.AddComponent<MeshCollider>();

                                }
                                aCollider.sharedMesh = allMeshFilters[c].sharedMesh;
                                aCollider.convex = true;
                            }
                        }

                        {
                            Rigidbody aRigid = gameObject.GetComponent<Rigidbody>();
                            if (ModuleType == EnumComponentType.Legs)
                            {
                                if (aRigid == null)
                                {
                                    aRigid = gameObject.AddComponent<Rigidbody>();

                                }
                                aRigid.mass = WeightInKG * 1;
                            }
                            else
                            {
                                if (aRigid != null)
                                {
                                    DestroyImmediate(aRigid);
                                }
                            }
                        }
                        FxMaterial aFX = gameObject.transform.GetComponent<FxMaterial>();
                        if (aFX == null)
                        {
                            aFX = gameObject.AddComponent<FxMaterial>();
                        }
                        List<SkinnedMeshRenderer> allRenderers = new List<SkinnedMeshRenderer>();
                        allRenderers.AddRange(gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>());
                        for (int c = 0; c < allRenderers.Count; c++)
                        {

                             MeshFilter aFilter = allRenderers[c].GetComponent<MeshFilter>();
                           
                        }

                        List<MeshFilter> allFilters = new List<MeshFilter>();
                        allFilters.AddRange(gameObject.transform.GetComponentsInChildren<MeshFilter>());
                        Rigidbody aRigidB = gameObject.GetComponent<Rigidbody>();
                         {

                            for (int c = 0; c < allFilters.Count; c++)
                            {
                                MeshCollider aCollider = allFilters[c].GetComponent<MeshCollider>();
                                if (aCollider != null)
                                {
                                    Meshinator aMeshi = allFilters[c].GetComponent<Meshinator>();
                                    if (aMeshi == null)
                                    {
                                        aMeshi = allFilters[c].gameObject.AddComponent<Meshinator>();
                                        aMeshi.MyRigidBody = aRigidB;
                                        aMeshi.MyMeshFilter = allFilters[c];
                                        aMeshi.TheComponent = this;
                                    }

                                }

                            }
                        }





                    }

                }
            }
            else
            {
                //enabled = false;
            }
        }
    }

    public void TakeDamage(float qty, RobotMeta responsableRobot)
    {
        float actualEffect = qty - Armour;
        if (actualEffect < 1) actualEffect = 0.1f;
        Health -= actualEffect;
        RobotMeta aMeta = gameObject.transform.GetComponent<RobotMeta>();
        if (aMeta == null)
        {
            aMeta = gameObject.transform.GetComponentInParent<RobotMeta>(); ;
        }
        if (aMeta != null)
        {
            aMeta.ApplyDamageFromComponentImpact(this, actualEffect, responsableRobot);
        }

        if (Health <= 0) MustSelfDestruct = true;

    }
    public void TakeDamageAsHeat(float qty, RobotMeta responsableRobot)
    {
        float actualEffect = qty - Armour;
        if (actualEffect < 1) actualEffect = 0.1f;
        CurrentHeat += qty;
        //   print("Taking Heat Damage:"+ CurrentHeat);
        if (CurrentHeat >= Heat_MaxHeat)
        {
            TakeDamage(qty, responsableRobot);
        }
        //  Health -= actualEffect;
        RobotMeta aMeta = gameObject.transform.GetComponent<RobotMeta>();
        if (aMeta == null)
        {
            aMeta = gameObject.transform.GetComponentInParent<RobotMeta>(); ;
        }
        if (aMeta != null)
        {
            aMeta.ApplyDamageFromComponentImpact(this, actualEffect * 0.01f, responsableRobot);
        }

        if (Health <= 0) MustSelfDestruct = true;

    }
    public void TurnPartOnOff(bool isOn)
    {
        {
            IoLegsMech anAnimator = gameObject.GetComponent<IoLegsMech>();
            if (anAnimator != null)
            {
                anAnimator.IsOn = isOn;
            }
            List<IoLegsMech> anims = new List<IoLegsMech>();
            anims.AddRange(gameObject.GetComponentsInChildren<IoLegsMech>());
            for (int x = 0; x < anims.Count; x++)
            {
                anims[x].IsOn = isOn;
            }
        }
        {
            Computer anAnimator = gameObject.GetComponent<Computer>();
            if (anAnimator != null)
            {
                anAnimator.IsEnabled = isOn;
            }
            List<Computer> anims = new List<Computer>();
            anims.AddRange(gameObject.GetComponentsInChildren<Computer>());
            for (int x = 0; x < anims.Count; x++)
            {
                anims[x].IsEnabled = isOn;
            }
        }
        {
            Animator anAnimator = gameObject.GetComponent<Animator>();
            if (anAnimator != null)
            {
                anAnimator.enabled = isOn;
            }
            List<Animator> anims = new List<Animator>();
            anims.AddRange(gameObject.GetComponentsInChildren<Animator>());
            for (int x = 0; x < anims.Count; x++)
            {
                anims[x].enabled = isOn;
            }
        }
        {
            Rigidbody anAnimator = gameObject.GetComponent<Rigidbody>();
            if (anAnimator != null)
            {
                if (isOn)
                {
                    anAnimator.constraints = RigidbodyConstraints.FreezeRotation;// = isOn;
                }
                else
                {
                    anAnimator.constraints = RigidbodyConstraints.FreezeAll;// = isOn;


                }
            }
            List<Rigidbody> anims = new List<Rigidbody>();
            anims.AddRange(gameObject.GetComponentsInChildren<Rigidbody>());
            for (int x = 0; x < anims.Count; x++)
            {
                if (isOn)
                {
                    anims[x].constraints = RigidbodyConstraints.FreezeRotation;//
                }
                else
                {
                    anims[x].constraints = RigidbodyConstraints.FreezeAll;//

                }
            }
        }
        {
            IoWeapon anAnimator = gameObject.GetComponent<IoWeapon>();
            if (anAnimator != null)
            {
                anAnimator.IsOn = isOn;
            }
            List<IoWeapon> anims = new List<IoWeapon>();
            anims.AddRange(gameObject.GetComponentsInChildren<IoWeapon>());
            for (int x = 0; x < anims.Count; x++)
            {
                anims[x].IsOn = isOn;
            }
        }
        {
            IoShield anAnimator = gameObject.GetComponent<IoShield>();
            if (anAnimator != null)
            {
                anAnimator.IsOn = isOn;
            }
            List<IoShield> anims = new List<IoShield>();
            anims.AddRange(gameObject.GetComponentsInChildren<IoShield>());
            for (int x = 0; x < anims.Count; x++)
            {
                anims[x].IsOn = isOn;
            }
        }
    }

   

    void OnCollisionEnter(Collision collision)
    {

        if (TheRobotMeta != null)
        {
            TheRobotMeta.DoObjectCollision(this, collision.gameObject);
        }
    }
   

    void OnTriggerEnter(Collider other)
    {
        if (TheRobotMeta != null)
        {
            TheRobotMeta.DoObjectCollision(this, other.gameObject);
        }
    }

    


    public void InitAllFunctionsIO(string currentIO)
    {
        Functions2 = new List<string>();
        FunctionsMeta = new List<IoFunctionMeta>();
        IoBackPack ioBackPack = gameObject.GetComponent<IoBackPack>();
        IoCockpit ioCockPit = gameObject.GetComponent<IoCockpit>();
        IoLegsMech ioLegs = gameObject.GetComponent<IoLegsMech>();
        IoShield ioShield = gameObject.GetComponent<IoShield>();
        IoWeapon ioWeapon = gameObject.GetComponent<IoWeapon>();
        string test = "";
        if (ioBackPack != null)
        {
            test += "IoBackPack";
            ioBackPack.ResetInit();
            ioBackPack.SetComponentFunctions(this, currentIO);
        }
        if (ioCockPit != null)
        {
            test += "ioCockPit";
            ioCockPit.ResetInit();
            ioCockPit.SetComponentFunctions(this, currentIO);
        }
        if (ioLegs != null)
        {
            test += "ioLegs";
            ioLegs.ResetInit();
            ioLegs.SetComponentFunctions(this, currentIO);
        }
        if (ioShield != null)
        {
            test += "ioShield";
            ioShield.ResetInit();
            ioShield.SetComponentFunctions(this, currentIO);
        }
        if (ioWeapon != null)
        {
            test += "ioWeapon";
            // ioWeapon.re
            // string disp = aRow.TheRobot.ModuleList[c].IoNumberCustomMap + " - " + aType.DeviceName;
            ioWeapon.ResetInit();
            ioWeapon.SetComponentFunctions(this, currentIO);
        }
    }
    public string GenerateExampleCode(ComponentType.IoFunctionMeta aFunction, string actualIO)
    {
        string ret = "";
        aFunction.FunctionExample = "";

        ret += "MOV " + aFunction.InputVariables.InputVariableMain.Register + " " + aFunction.InputVariables.InputVariableMainValue;
        ret += "\n";
        for (int c = 0; c < aFunction.InputVariables.InputVariables.Count; c++)
        {

            ret += "MOV " + aFunction.InputVariables.InputVariables[c].Register + " " + aFunction.InputVariables.InputVariables[c].ExampleValue;
            ret += "\n";

        }

        ret += "IO " + actualIO + "";
        ret += "\n";
        return ret;

    }


    public IoWeapon GetIoWeapob()
    {
        IoWeapon theWeapon = gameObject.GetComponent<IoWeapon>();
        
        return theWeapon;
    }

    public class IoFunctionMeta
    {
        public enum EnumIODataSetType
        {
            NoExtra,
            WorldPositionBxCxDx,
            AngleBx,
            DirectionBxCxDx,
            NumberBx,
            DropDownBx,
            //NumberBetween0And100,
            //NumberBetweenNegative100And100,

        }
       
        public string FunctionName;
        public string FunctionDescription;
        public string FunctionExample;
        public IoInput InputVariables;
        public IoOutput OutputVariables;

        public List<Computer.CodeGenAction.EnumActionTarget> GetPossibleTargets()
        {

            List<Computer.CodeGenAction.EnumActionTarget> ret = new List<Computer.CodeGenAction.EnumActionTarget>();
            switch (InputVariables.DataSetType)
            {
                case EnumIODataSetType.WorldPositionBxCxDx:
                    {
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_02_OwnID);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_101_ScanClosestLocation);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_102_ScanFurthestLocation);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_103_ScanShotsFired);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_104_ScanLeastShotsFired);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_105_ScanMostHealth);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_106_ScanLeastHealth);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_107_ScanMostAccurate);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_108_ScanLeastAccurate);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_109_ScanMostShotsHit);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_110_ScanLeastShotsHit);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_111_ScanHighestLocation);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_112_ScanLowestLocation);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_113_ScanFastest);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_114_ScanSlowest);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_115_ScanFastestMaxSpeed);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_116_ScanSlowestMaxSpeed);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_117_ScanMostModules);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_118_ScanLeastModules);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_119_ScanHeaviest);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_120_ScanLightest);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_121_ScanMostLinesOfCode);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_122_ScanLeastLinesOfCode);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_123_ScanBestScore);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_124_ScanLowestScore);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_125_ScanMostKills);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_126_ScanLeastKills);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_127_ScanLastRobotThatHitMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_128_ScanLastRobotThatIHit);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_129_ScanWarmestHeatLevel);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_130_ScanColdestHeatLevel);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_131_ScanClosestEnemyBehindMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_132_ScanFurthestEnemyBehindMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_133_ScanClosestEnemyInfrontOfMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_134_ScanFurthestEnemyInfrontOfMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_135_ScanClosestEnemyToMyRight);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_136_ScanFurthestEnemyToMyRight);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_137_ScanClosestEnemeyToMyLeft);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_138_ScanFurthestEnemyToMyLeft);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_139_ScanClosestEnemyWithAMeleeWeapon);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_140_ScanFurthestEnemyWithAMeleeWeapon);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_141_ScanClosestEnemyTargettingMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_142_ScanFurthestEnemyTargettingMe);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_143_ScanClosestEnemyWithWheels);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_144_ScanFurthestEnemyWithWheels);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_145_ScanClosestEnemyWithWalkerLegs);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_149_ScanClosestEnemyWithJet);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_150_ScanFurthestEnemyWithJet);
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.I_151_ScanClosestOtherOwnRobot);                       
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.SpecificPosition);
                        break;
                    }
                case EnumIODataSetType.AngleBx:
                    {
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.SpecificAngle);

                        break;
                    }
                case EnumIODataSetType.NumberBx:
                    {
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.SpecificValue);

                        break;
                    }
                case EnumIODataSetType.DropDownBx:
                    {
                         ret.Add(Computer.CodeGenAction.EnumActionTarget.DropDown);

                        break;
                    }
                default:
                    {
                        ret.Add(Computer.CodeGenAction.EnumActionTarget.None);
                        break;
                    }
            }
            return ret;
        }

    }
    public class IoInput
    {
        public IoFunctionMeta.EnumIODataSetType DataSetType;
        public IoVariable InputVariableMain;
        public string InputVariableMainValue;
        public List<IoVariable> InputVariables;

    }
    public class IoOutput
    {
        public IoFunctionMeta.EnumIODataSetType DataSetType;
        public List<IoVariable> OutputVariables;

    }
    public class IoVariable
    {
        public enum EnumRegisters
        {
            Ax,
            Bx,
            Cx,
            Dx,
            Ex,
            Fx,
            Gx,
        }
        public enum EnumRegisterDataType
        {
            FunctionID,
            WorldPosX,
            WorldPosY,
            WorldPosZ,
            AngleAroundZ,
            AngleAroundY,
            SpeedPercent,
            TransformState,
            StrafeState

        }
        public EnumRegisters Register;
        public EnumRegisterDataType RegisterType;

        public string RegisterDescription;
        public string ExampleValue = "0";
        public List<IoVariableDropDownOption> DropDownOptions;

    }
    public class IoVariableDropDownOption
    {
        public string Value;
        public string DisplayText;

    }
}
