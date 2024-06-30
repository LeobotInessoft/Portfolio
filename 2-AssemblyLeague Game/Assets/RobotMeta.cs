using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RobotMeta : MonoBehaviour
{
    public bool IsEventLoggingOn = false;
    ComponentType myComponentType;
    public bool IsDead; //done
    public float MinutesSurvived;//done

    public RobotMeta ClosestRobotBehindMe;
    public RobotMeta ClosestRobotLeftOfMe;
    public RobotMeta ClosestRobotRightOfMe;
    public RobotMeta ClosestRobotInFrontOfMe;

    public RobotMeta FurthestRobotBehindMe;
    public RobotMeta FurthestRobotLeftOfMe;
    public RobotMeta FurthestRobotightOfMe;
    public RobotMeta FurthestRobotInFrontOfMe;

    public RobotMeta ClosestRobotWithMelee;
    public RobotMeta FurthestRobotWithMelee;
    public RobotMeta ClosestRobotTargettingMe;
    public RobotMeta FurthestRobotTargettingMe;
    public RobotMeta ClosestRobotWithWheels;
    public RobotMeta FurthestRobotWithWheels;
    public RobotMeta ClosestRobotWithWalkerLegs;
    public RobotMeta FurthestRobotWithWalkerLegs;
    public RobotMeta ClosestRobotWithSpiderLegs;
    public RobotMeta FurthestRobotWithSpiderLegs;
    public RobotMeta ClosestRobotWithJet;
    public RobotMeta FurthestRobotWithJet;
    public RobotMeta ClosestRobotOwnOtherRobot;
    public RobotMeta FurthestRobotOwnOtherRobot;
    public RobotMeta ClosestRobot;
    public RobotMeta FurthestRobot;

    public int CurrentTargetID;

    public float Heat;
    public float LinesOfCode;
    public float EffectiveWeight;
    public float EffectiveSpeed;
    public float EffectiveSpeedMax;
    public float NumberOfModules;
    public float Armour;
    public float PhysicalSize;
    public float NumberOfCockpits;
    public float NumberOfShoulders;
    public float NumberOfBackPacks;
    public float NumberOfWeapons;
    public float NumberOfEMPWeapons;
    public float NumberOfBallisticWeapons;
    public float NumberOfRocketWeapons;
    public float NumberOfMeleeWeapons;
    public float NumberOfLaserWeapons;
    public float NumberOfFireWeapons;
    public float NumberOfMineLayerWeapons;

    public IoLegsMech.EnumLegType EnumLegType;

    public Vector3 PosInfrontOfMe;
    public Vector3 PosToLeftOfMe;
    public Vector3 PosToRightOfMe;
    public Vector3 PosToBackOfMe;
    public Vector3 PosIAmLookingAt;


    public float DetermineAngleBetweenObjectOnYAxis() { return 0; }
    public float DetermineAngleBetweenObjectOnZAxis() { return 0; }

    public float Health; //done
    public int ShotsFired; //done
    public int ShotsHit; //done
    public int Kills; //done
    public float DamageReceived;
    public float DamageGiven;


    public System.DateTime TimeStarted; //done
    public System.DateTime TimeDied; //done
    // public int RamDamage;

    public int RuntimeRobotID;
    public int RuntimeRobotOwnderID;
    public int RuntimeRobotLeagueID; //done
    public string RuntimeRobotOwnerName; //done
    public string FileName;
    public float RuntimeScore = 0;
    public int RuntimeRank = 0;
    public RobotConstructor.RobotTemplate Template;
    public Match CurrentMatch;

    public bool IsOverheated;
    public float RuntimeOverheat = 0;
    float decreaseOfOverheat = 1f;

    int MaxLogSize = 50;
    public List<string> EventLog = new List<string>();
    public void AddStatShotHit()
    {
        ShotsHit++;
    }
    public void AddStatShotFired()
    {
        ShotsFired++;
    }
    public void AddStatKill()
    {
        Kills++;
    }
    public void AddStatDamageReceived(float damage)
    {
        DamageReceived += damage;
    }
    public void AddStatDamageGiven(float damage)
    {
        DamageGiven += damage;
    }
    public void AddEventLog(string detail)
    {
        if (IsEventLoggingOn)
        {
            if (EventLog.Count == 0)
            {
                EventLog.Add(detail);
            }
            else
            {
                EventLog.Insert(0, detail);
            }
        }
        if (EventLog.Count >= MaxLogSize) EventLog.RemoveAt(EventLog.Count - 1);
        if (EventLog.Count >= MaxLogSize) EventLog.RemoveAt(EventLog.Count - 1);


    }
    public void AddOverheat()
    {
        RuntimeOverheat++;
    }

    
    // Use this for initialization
    void Start()
    {
        IsEventLoggingOn = false;
        EventLog = new List<string>();
        myComponentType = GetComponent<ComponentType>();
        IsOverheated = false;
        TimeStarted = System.DateTime.UtcNow;
    }

    public void TakeDamage(FireCollider aFire)
    {
        AddEventLog("Taking damage from fire");
        
    }
    public void BaseHasExploded()
    {
        Health = 0;

    }
    public void AnyComponentHasExploded()
    {
        try
        {
            List<ComponentType> remainingComponents = new List<ComponentType>();
            if (gameObject != null)
            {
                remainingComponents.AddRange(gameObject.transform.GetComponentsInChildren<ComponentType>());
                remainingComponents.Remove(gameObject.transform.GetComponent<ComponentType>());
                List<ComponentType> trueList = new List<ComponentType>();
                for (int c = 0; c < remainingComponents.Count; c++)
                {

                    bool mayAdd = true;
                    IoBackPack aBack = remainingComponents[c].gameObject.transform.GetComponent<IoBackPack>();
                    if (aBack != null) mayAdd = false;

                    if (mayAdd)
                    {
                        trueList.Add(remainingComponents[c]);
                    }
                }
                if (trueList.Count == 0)
                {
                    BaseHasExploded();

                }
            }
         }
        catch
        {

        }
    }
    public RobotMeta LastRobotThatHitMe;
    public void ApplyDamageFromComponentImpact(ComponentType aType, float healthImpact, RobotMeta responsableRobot)
    {
        if (IsDead == false)
        {
            LastRobotThatHitMe = responsableRobot;
            Health -= healthImpact;
            AddStatDamageReceived(healthImpact);
            responsableRobot.AddStatDamageGiven(healthImpact);
            responsableRobot.AddStatShotHit();
            AddEventLog("Taking " + healthImpact + "damage at " + aType);
          }
    }
    private void DoDie()
    {
        if (IsDead == false)
        {
            IsDead = true;
            TimeDied = System.DateTime.UtcNow;
            MinutesSurvived = (float)(TimeDied - TimeStarted).TotalMinutes;

            myComponentType.MustSelfDestruct = true;
            if (LastRobotThatHitMe != null)
            {
                LastRobotThatHitMe.AddStatKill();
            }
        }
    }
    public void TakeDamage(MeleeCollider aFire)
    {
        AddEventLog("Taking damage from melee");
        // print(gameObject.name + " Taking damage from MELEE COLLDER by" + aFire.MyWeapon.gameObject.name);
    }
    public void TakeDamage(BulletCollider aFire)
    {
        AddEventLog("Taking damage from bullet");
        //    print(gameObject.name + " Taking damage from BULLET COLLDER by" + aFire.sourceBullet.Owner.gameObject.name);
    }
    public void TakeDamage(Bullet aFire)
    {
        AddEventLog("Taking damage from bullet");
        //      print(gameObject.name + " Taking damage from BULLET by" + aFire.Owner.gameObject.name);
    }
    public void TakeDamageFromComponentImpact(ComponentType aType)
    {
        AddEventLog("Taking damage from direct impact");
        print(gameObject.name + " Taking damage from DIRECT by" + aType.TheRobotMeta.gameObject.name);
    }
    public void TakeDamageFromComponentImpactWeapon(IoWeapon aType)
    {
        AddEventLog("Taking damage from weapon impact");

        print(gameObject.name + " Taking damage from WEAPON by" + aType.TheRobotMeta.gameObject.name);
    }
    float timeBeforeHeatEffect = 0;
    private void SpawbDizzy()
    {
        if (timeBeforeHeatEffect <= 0)
        {
            ComponentFx anFX = FxCache.PublicAccess.GetEffectToApply(ComponentFx.FxType.Dizzy);
            anFX.TimeToRemain = 5f;
            anFX.ApplyAttachToTransform(gameObject.transform, Vector3.up, gameObject.transform.position + Vector3.up * 5);
            timeBeforeHeatEffect = 5f;
        }
    }

  
    // Update is called once per frame
    void Update()
    {

        if (IsDead == false)
        {
            MinutesSurvived = (float)(System.DateTime.UtcNow - TimeStarted).TotalMinutes;
            if (CurrentMatch != null)
            {
                UpdateIntKnowledge(CurrentMatch.GetAllRobotMetas());
            }
        }
        else
        {
                   }

        if (timeBeforeHeatEffect >= 0)
        {
            timeBeforeHeatEffect -= 1f * Time.deltaTime;
        }
        if (timeBeforeHeatEffect < 0) timeBeforeHeatEffect = 0;

        RuntimeOverheat -= decreaseOfOverheat * Time.deltaTime;
        if (RuntimeOverheat >= 100)
        {
            IsOverheated = true;
            if (RuntimeOverheat >= 101)
            {
                RuntimeOverheat = 101;
            }
            SpawbDizzy();
        }
        else
        {
            IsOverheated = false;
         
        }
        if (RuntimeOverheat < 0) RuntimeOverheat = 0;
        if (CurrentMatch != null && CurrentMatch.HasMatchStarted)
        {
          
            if (Health <= 0)
            {
                if (IsDead == false)
                {
                    DoDie();
                            }
            }
            RuntimeScore = DetermineMyScore();
           
        }

    }
    public float DetermineMyScore()
    {
        float total = 0;
        float totalFromKills = 100 * Kills;
        float totalFromTime = (50f * ((int)MinutesSurvived));
        float totalFromHits = 0.01f * DamageGiven;// -DamageReceived;
        if (totalFromHits < 0) totalFromHits = 0;
        total = totalFromHits + totalFromKills + totalFromTime + Health;
        return total;

    }
    public void DoObjectCollision(ComponentType myComponentBeingHit, GameObject other)
    {
        //todo
        //direct collision

       


    }
    public static float KineticEnergy(Rigidbody rb)
    {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * rb.mass * Mathf.Pow(rb.velocity.magnitude, 2);
    }

    public void UpdateIntKnowledge(List<RobotMeta> allRobots)
    {
        List<ComponentType> allMyComponents = new List<ComponentType>();
        List<IoWeapon> allMyWeapons = new List<IoWeapon>();
        ComponentType mainComp = gameObject.transform.GetComponent<ComponentType>();
        allMyComponents.AddRange(gameObject.transform.GetComponentsInChildren<ComponentType>());
        allMyWeapons.AddRange(gameObject.transform.GetComponentsInChildren<IoWeapon>());
        Computer cpu = mainComp.gameObject.transform.GetComponent<Computer>();

        IoLegsMech legs = mainComp.gameObject.transform.GetComponent<IoLegsMech>();
        if (legs != null)
        {
            EnumLegType = legs.LegsType;
            PosIAmLookingAt = legs.ActualLookDestination;
        }

        PosInfrontOfMe = gameObject.transform.forward * 5f;
        PosToLeftOfMe = gameObject.transform.right * -5f;
        PosToRightOfMe = gameObject.transform.right * 5f;
        PosToBackOfMe = gameObject.transform.forward * -5f;

        ClosestRobotBehindMe = allRobots.OrderBy(x => Vector3.Distance(PosToBackOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();


        ClosestRobotLeftOfMe = allRobots.OrderBy(x => Vector3.Distance(PosToLeftOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();

        ClosestRobotRightOfMe = allRobots.OrderBy(x => Vector3.Distance(PosToRightOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();

        ClosestRobotInFrontOfMe = allRobots.OrderBy(x => Vector3.Distance(PosInfrontOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();



        FurthestRobotBehindMe = allRobots.OrderByDescending(x => Vector3.Distance(PosToBackOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();

        FurthestRobotLeftOfMe = allRobots.OrderByDescending(x => Vector3.Distance(PosToLeftOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();

        FurthestRobotightOfMe = allRobots.OrderByDescending(x => Vector3.Distance(PosToRightOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();

        FurthestRobotInFrontOfMe = allRobots.OrderByDescending(x => Vector3.Distance(PosInfrontOfMe, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();



        ClosestRobotWithMelee = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.NumberOfMeleeWeapons > 0).FirstOrDefault();

        FurthestRobotWithMelee = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.NumberOfMeleeWeapons > 0).FirstOrDefault();

        ClosestRobotTargettingMe = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.CurrentTargetID == RuntimeRobotID).FirstOrDefault();

        FurthestRobotTargettingMe = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.CurrentTargetID == RuntimeRobotID).FirstOrDefault();

        ClosestRobotWithWheels = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Wheeled).FirstOrDefault();

        FurthestRobotWithWheels = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Wheeled).FirstOrDefault();

        ClosestRobotWithWalkerLegs = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Walker).FirstOrDefault();

        FurthestRobotWithWalkerLegs = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Walker).FirstOrDefault();

        ClosestRobotWithSpiderLegs = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Spider).FirstOrDefault();

        FurthestRobotWithSpiderLegs = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Spider).FirstOrDefault();

        ClosestRobotWithJet = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Aircraft).FirstOrDefault();

        FurthestRobotWithJet = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.EnumLegType == IoLegsMech.EnumLegType.Aircraft).FirstOrDefault();




        ClosestRobotOwnOtherRobot = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.RuntimeRobotOwnderID == RuntimeRobotOwnderID).FirstOrDefault();

         FurthestRobotOwnOtherRobot = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false && x.RuntimeRobotOwnderID == RuntimeRobotOwnderID).FirstOrDefault();
        ;

        ClosestRobot = allRobots.OrderBy(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();

        FurthestRobot = allRobots.OrderByDescending(x => Vector3.Distance(gameObject.transform.position, new Vector3(x.gameObject.transform.position.x, x.gameObject.transform.position.y, x.gameObject.transform.position.z))).Where(x => x.RuntimeRobotID != RuntimeRobotID && x.IsDead == false).FirstOrDefault();


         Heat = mainComp.CurrentHeat;
         LinesOfCode = cpu.CodeText.Count(x => x == '\n'); ;
         EffectiveWeight = mainComp.WeightInKG;
         EffectiveSpeed= legs.CurrentVelocity;
         EffectiveSpeedMax= mainComp.Legs_MaxForwardSpeed;
         NumberOfModules= allMyComponents.Count;
         Armour = Template.CalculateTotalArmour;
         PhysicalSize = Template.CalculateSqrMeterSize;
         NumberOfCockpits = allMyComponents.Count(x=>x.ModuleType== ComponentType.EnumComponentType.Cockpit);
         NumberOfShoulders = allMyComponents.Count(x => x.ModuleType == ComponentType.EnumComponentType.Shoulders);
         NumberOfBackPacks = allMyComponents.Count(x => x.ModuleType == ComponentType.EnumComponentType.Backpack);
         NumberOfWeapons= allMyComponents.Count(x=>x.ModuleType== ComponentType.EnumComponentType.WeaponAnySlot);
         NumberOfEMPWeapons = allMyWeapons.Count(x => x.TheBulletType ==  Bullet.BulletType.ElectricShockBullet)+allMyWeapons.Count(x => x.TheBulletType ==  Bullet.BulletType.ElectricShockMelee);
         NumberOfBallisticWeapons = allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Gun_Bullet) + allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Shotgun_Bullet);
         NumberOfRocketWeapons = allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Grenade_Blunt) + allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Grenade_Sharp) + allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Rocket) + allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.BigRocket); ;
         NumberOfMeleeWeapons = allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Flame) + allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.ElectricShockMelee); ;
         NumberOfLaserWeapons = allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Laser) ;
         NumberOfFireWeapons=  allMyWeapons.Count(x => x.TheBulletType == Bullet.BulletType.Flame);
      
    }
}
