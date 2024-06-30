using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxMaterial : MonoBehaviour
{
    public Meshinator TheMeshinator;
    float TimeToRemain = 2f;
    public EnumMaterialType ThisMaterialType;
    public enum EnumMaterialType
    {
        NotSet,
        MetaMachine,
        Metal,
        Sand,
        Stone,
        Wood,
        Flesh,
        WaterFilled,
        ElectricMachine,

    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private ComponentFx.FxType GetBulletImpactEffectType(Bullet bulletTyoe)
    {
        ComponentFx.FxType ret = ComponentFx.FxType.NotSet;
        switch (ThisMaterialType)
        {
            case EnumMaterialType.MetaMachine:
                {
                    ret = GetImpactEffectForMetal(bulletTyoe);
                    break;
                }
            case EnumMaterialType.ElectricMachine:
                {
                    ret = GetImpactEffectForElectricMachine(bulletTyoe);
                    break;
                }
            default:
                {
                    ret = ComponentFx.FxType.SandImpactDecal;
                    break;
                }
        }



        return ret;

    }
    private ComponentFx.FxType GetImpactEffectForMetal(Bullet bulletTyoe)
    {
        ComponentFx.FxType ret = ComponentFx.FxType.NotSet;
        switch (bulletTyoe.TheType)
        {
            case Bullet.BulletType.BigRocket:
                {
                    ret = ComponentFx.FxType.SmallExplosion;
                    break;
                }
            case Bullet.BulletType.CannonBall:
                {
                    ret = ComponentFx.FxType.MuzzleFlash;
                    break;
                }
            case Bullet.BulletType.ElectricShockBullet:
                {
                    ret = ComponentFx.FxType.PlasmaExplosion;
                    break;
                }
            case Bullet.BulletType.ElectricShockMelee:
                {
                    ret = ComponentFx.FxType.Sparks;
                    break;
                }
            case Bullet.BulletType.Flame:
                {
                    ret = ComponentFx.FxType.Steam;
                    break;
                }
            case Bullet.BulletType.Grenade_Blunt:
                {
                    ret = ComponentFx.FxType.SmallExplosion;
                    break;
                }
            case Bullet.BulletType.Grenade_Sharp:
                {
                    ret = ComponentFx.FxType.MediumExplosion;
                    break;
                }
            case Bullet.BulletType.Gun_Bullet:
                {
                    ret = ComponentFx.FxType.MetalImpactDecal;
                    break;
                }
            case Bullet.BulletType.Laser:
                {
                    ret = ComponentFx.FxType.BloodLeakSprayEffect;
                    break;
                }
            case Bullet.BulletType.Rocket:
                {
                    ret = ComponentFx.FxType.BigExplosion;
                    break;
                }
            case Bullet.BulletType.Shotgun_Bullet:
                {
                    ret = ComponentFx.FxType.SmallSparks;
                    break;
                }

            default:
                {
                    ret = ComponentFx.FxType.SandImpactDecal;
                    break;
                }
        }
        return ret;
    }
    private ComponentFx.FxType GetImpactEffectForElectricMachine(Bullet bulletTyoe)
    {
        ComponentFx.FxType ret = ComponentFx.FxType.NotSet;
        switch (bulletTyoe.TheType)
        {
            case Bullet.BulletType.BigRocket:
                {
                    ret = ComponentFx.FxType.SmallExplosion;
                    break;
                }
            case Bullet.BulletType.CannonBall:
                {
                    ret = ComponentFx.FxType.MuzzleFlash;
                    break;
                }
            case Bullet.BulletType.ElectricShockBullet:
                {
                    ret = ComponentFx.FxType.PlasmaExplosion;
                    break;
                }
            case Bullet.BulletType.ElectricShockMelee:
                {
                    ret = ComponentFx.FxType.Sparks;
                    break;
                }
            case Bullet.BulletType.Flame:
                {
                    ret = ComponentFx.FxType.Steam;
                    break;
                }
            case Bullet.BulletType.Grenade_Blunt:
                {
                    ret = ComponentFx.FxType.SmallExplosion;
                    break;
                }
            case Bullet.BulletType.Grenade_Sharp:
                {
                    ret = ComponentFx.FxType.MediumExplosion;
                    break;
                }
            case Bullet.BulletType.Gun_Bullet:
                {
                    ret = ComponentFx.FxType.MetalImpactDecal;
                    break;
                }
            case Bullet.BulletType.Laser:
                {
                    ret = ComponentFx.FxType.BloodLeakSprayEffect;
                    break;
                }
            case Bullet.BulletType.Rocket:
                {
                    ret = ComponentFx.FxType.BigExplosion;
                    break;
                }
            case Bullet.BulletType.Shotgun_Bullet:
                {
                    ret = ComponentFx.FxType.SmallSparks;
                    break;
                }

            default:
                {
                    ret = ComponentFx.FxType.SandImpactDecal;
                    break;
                }
        }
        return ret;
    }
   
    public void DoDomage(MeleeCollider asBullet, GameObject other, Vector3 atPosition, Collider aCollider)
    {
        if (FxCache.PublicAccess != null)
        {
            if (Random.Range(0, 5000) < 10)
            {
                ComponentFx.FxType aFxType = ComponentFx.FxType.SmallFlareSparks;
                ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(aFxType);
                aComp.TimeToRemain = TimeToRemain;             
                aComp.ApplyAttachToTransform(other.transform, -1 * asBullet.transform.forward, atPosition);
            }

            {
                Meshinator aMeshinator = aCollider.gameObject.GetComponent<Meshinator>();
                if (aMeshinator != null)
                {
                    Vector3 vel = asBullet.transform.forward * 5f;// asBullet.TotalBasicDamageOnHit;

                    aMeshinator.Impact(atPosition, vel, Meshinator.ImpactShapes.SphericalImpact, Meshinator.ImpactTypes.Compression);
                }
                if (Random.Range(0, 5000) < 10)
                {
                    ComponentFx.FxType aFxType = ComponentFx.FxType.SmallFire;
                    ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(aFxType);
                    aComp.TimeToRemain = TimeToRemain;
                    aComp.ApplyAttachToTransform(other.transform, -1 * asBullet.transform.forward, atPosition);
                }
               

                RobotMeta aMeta = aCollider.gameObject.transform.GetComponent<RobotMeta>();
                if (aMeta == null)
                {
                    aMeta = aCollider.gameObject.transform.GetComponentInParent<RobotMeta>(); ;
                }
                ComponentType aPart = aCollider.gameObject.transform.GetComponent<ComponentType>();
                if (aPart == null)
                {
                    aPart = aCollider.gameObject.transform.GetComponentInParent<ComponentType>(); ;

                }

                if (aMeta != null)
                {
                    if (asBullet.Owner != null && asBullet.Owner.RuntimeRobotID != aMeta.RuntimeRobotID)
                    {
                        if (asBullet.Owner.IsDead == false)
                        {
                           aPart.TakeDamage(asBullet.MyWeapon.myComponentType.Weapon_BulletDamage, asBullet.Owner);
                        }
                    }
                    if (aPart.ModuleType != ComponentType.EnumComponentType.Legs)
                    {
                        //    aPart.MustSelfDestruct = true;
                    }
                    if (Random.Range(0, 5000) < 10)
                    {
                        DoDamageToDestructables(other, asBullet.MyWeapon.myComponentType.Weapon_BulletDamage);

                    }
                }
            }
        }
    }
    public void DoDomage(FireCollider asBullet, GameObject other, Vector3 atPosition, Collider aCollider)
    {
        if (FxCache.PublicAccess != null)
        {
            if (Random.Range(0, 5000) < 10)
            {
                ComponentFx.FxType aFxType = ComponentFx.FxType.SmallFire;
                ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(aFxType);
                aComp.TimeToRemain = TimeToRemain;

             
                aComp.ApplyAttachToTransform(other.transform, -1 * asBullet.transform.forward, atPosition);
            }

            {
                Meshinator aMeshinator = aCollider.gameObject.GetComponent<Meshinator>();
                if (aMeshinator != null)
                {
                    Vector3 vel = asBullet.transform.forward * 5f;// asBullet.TotalBasicDamageOnHit;

                    aMeshinator.Impact(atPosition, vel, Meshinator.ImpactShapes.SphericalImpact, Meshinator.ImpactTypes.Compression);
                }
                if (Random.Range(0, 5000) < 10)
                {
                    ComponentFx.FxType aFxType = ComponentFx.FxType.SmallFire;
                    ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(aFxType);
                    aComp.TimeToRemain = TimeToRemain;
                    aComp.ApplyAttachToTransform(other.transform, -1 * asBullet.transform.forward, atPosition);
                }               

                RobotMeta aMeta = aCollider.gameObject.transform.GetComponent<RobotMeta>();
                if (aMeta == null)
                {
                    aMeta = aCollider.gameObject.transform.GetComponentInParent<RobotMeta>(); ;
                }
                ComponentType aPart = aCollider.gameObject.transform.GetComponent<ComponentType>();
                if (aPart == null)
                {
                    aPart = aCollider.gameObject.transform.GetComponentInParent<ComponentType>(); ;

                }

                if (aMeta != null)
                {
                    if (asBullet.Owner != null && asBullet.Owner.RuntimeRobotID != aMeta.RuntimeRobotID)
                    {
                        if (asBullet.Owner.IsDead == false)
                        {
                            aPart.TakeDamageAsHeat(asBullet.myWeapon.myComponentType.Weapon_BulletDamage, asBullet.Owner);
                        }
                    }
                    if (aPart.ModuleType != ComponentType.EnumComponentType.Legs)
                    {
                        //    aPart.MustSelfDestruct = true;
                    }
                    if (Random.Range(0, 5000) < 10)
                    {
                        DoDamageToDestructablesAsHeat(other, asBullet.myWeapon.myComponentType.Weapon_BulletDamage);
                    }
                }
            }
        }

    }
    public void DoDomage(Bullet asBullet, GameObject other, Vector3 atPosition, Collider aCollider)
    {
        Meshinator aMeshinator = aCollider.gameObject.GetComponent<Meshinator>();
        if (aMeshinator != null)
        {
            Vector3 vel = asBullet.transform.forward * 5f;// asBullet.TotalBasicDamageOnHit;
           
            aMeshinator.Impact(atPosition, vel, Meshinator.ImpactShapes.SphericalImpact, Meshinator.ImpactTypes.Compression);
        }
        ComponentFx.FxType aFxType = GetBulletImpactEffectType(asBullet);
        ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(aFxType);
        aComp.TimeToRemain = TimeToRemain;
        aComp.ApplyAttachToTransform(other.transform, -1 * asBullet.transform.forward, atPosition);


        RobotMeta aMeta = aCollider.gameObject.transform.GetComponent<RobotMeta>();
        if (aMeta == null)
        {
            aMeta = aCollider.gameObject.transform.GetComponentInParent<RobotMeta>(); ;
        }
        ComponentType aPart = aCollider.gameObject.transform.GetComponent<ComponentType>();
        if (aPart == null)
        {
            aPart = aCollider.gameObject.transform.GetComponentInParent<ComponentType>(); ;

        }

        if (aMeta != null)
        {
            if (asBullet.Owner != null && asBullet.Owner.RuntimeRobotID != aMeta.RuntimeRobotID)
            {
                if (asBullet.Owner.IsDead == false)
                {
                    Rigidbody ofBullet = asBullet.MyRigidBody;
                    float extra = 1;
                    if (ofBullet != null)
                    {
                        extra = ofBullet.velocity.magnitude*0.1f;
                    }
                    aPart.TakeDamage(asBullet.MyWeapon.myComponentType.Weapon_BulletDamage * extra, asBullet.Owner);
                    if (aPart.ModuleType != ComponentType.EnumComponentType.Legs)
                    {
                        //    aPart.MustSelfDestruct = true;
                    }
                    if (asBullet.MyRigidBody != null)
                    {
                        Rigidbody aRigidTarget = aMeta.gameObject.transform.GetComponent<Rigidbody>();
                        if (aRigidTarget != null)
                        {
                            aRigidTarget.AddForceAtPosition(asBullet.MyRigidBody.velocity * asBullet.MyRigidBody.mass, asBullet.transform.position);
                        }

                    }
                }
                DoDamageToDestructables(other, asBullet.MyWeapon.myComponentType.Weapon_BulletDamage);

            }
        }
    }

    private void DoDamageToDestructables(GameObject other, float damage)
    {
        FxDestructable aDes = other.transform.GetComponent<FxDestructable>();
        if (aDes != null)
        {
            aDes.DoDomage(damage);
        }

    }
    private void DoDamageToDestructablesAsHeat(GameObject other, float damage)
    {
        FxDestructable aDes = other.transform.GetComponent<FxDestructable>();
        if (aDes != null)
        {
            aDes.DoDomageAsHeat(damage);
        }

    }bool IsTouching = false;
   }
