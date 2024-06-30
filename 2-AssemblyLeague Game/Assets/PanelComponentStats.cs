using UnityEngine;
using System.Collections;
using System.Linq;

using System.Collections.Generic;

public class PanelComponentStats : MonoBehaviour
{
    public ComponentStatSlider Stat1;
    public ComponentStatSlider Stat2;
    public ComponentStatSlider Stat3;
    public ComponentStatSlider Stat4;
    public ComponentStatSlider Stat5;
    public ComponentStatSlider Stat6;
    public ComponentStatSlider Stat7;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetStats(ComponentType aType, RobotConstructor rc)
    {
        if (aType == null)
        {
            Stat1.Clear();
            Stat2.Clear();
            Stat3.Clear();
            Stat4.Clear();
            Stat5.Clear();
            Stat6.Clear();
            Stat7.Clear();

        }
        else
        {
            switch (aType.ModuleType)
            {
                case ComponentType.EnumComponentType.Legs:
                    {
                        List<ComponentType> comps = rc.GetAllComponentsOfType(aType.ModuleType);
                        float minArmour = comps.OrderBy(x => x.Armour).FirstOrDefault().Armour;
                        float maxArmour = comps.OrderByDescending(x => x.Armour).FirstOrDefault().Armour;

                        float minWeight = comps.OrderBy(x => x.WeightInKG).FirstOrDefault().WeightInKG;
                        float maxWeight = comps.OrderByDescending(x => x.WeightInKG).FirstOrDefault().WeightInKG;

                        float minPowerProvided = comps.OrderBy(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;
                        float maxPowerProvided = comps.OrderByDescending(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;


                        float minPowerUsed = comps.OrderBy(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;
                        float maxPowerUsed = comps.OrderByDescending(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;



                        float minSpeed = comps.OrderBy(x => x.Legs_MaxForwardSpeed).FirstOrDefault().Legs_MaxForwardSpeed;
                        float maxSpeed = comps.OrderByDescending(x => x.Legs_MaxForwardSpeed).FirstOrDefault().Legs_MaxForwardSpeed;

                        float minAccel = comps.OrderBy(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;
                        float maxAccel = comps.OrderByDescending(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;

                        float minRot = comps.OrderBy(x => x.Legs_MaxRotateSpeed).FirstOrDefault().Legs_MaxRotateSpeed;
                        float maxRot = comps.OrderByDescending(x => x.Legs_MaxRotateSpeed).FirstOrDefault().Legs_MaxRotateSpeed;


                        float minInstru = comps.OrderBy(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;
                        float maxInstru = comps.OrderByDescending(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;

                        Stat1.SetData("Armour", aType.Armour + "", aType.Armour, minArmour, maxArmour);
                        Stat2.SetData("Weight", aType.WeightInKG + "KG", aType.WeightInKG, minWeight, maxWeight);
                        Stat3.SetData("Power Provided", aType.PowerProvidedPerFrame + "KW", aType.PowerProvidedPerFrame, minPowerProvided, maxPowerProvided);
                        Stat4.SetData("Power Used", aType.PowerUsedPerFrame + "KW", aType.PowerUsedPerFrame, minPowerUsed, maxPowerUsed);

                        Stat5.SetData("Max Speed", aType.Legs_MaxForwardSpeed + "KM/H", aType.Legs_MaxForwardSpeed, minSpeed, maxSpeed);
                        Stat6.SetData("Acceleration", aType.Legs_AccelerationAdd + "HM/S", aType.Legs_AccelerationAdd, minAccel, maxAccel);
                        Stat7.SetData("Rotation Speed", aType.Legs_MaxRotateSpeed + "", aType.Legs_MaxRotateSpeed, minRot, maxRot);

                        Stat7.Clear();
                        break;
                    }
                case ComponentType.EnumComponentType.Shoulders:
                case ComponentType.EnumComponentType.Cockpit:
                    {
                        List<ComponentType> comps = rc.GetAllComponentsOfType(ComponentType.EnumComponentType.Shoulders);
                        comps.AddRange(rc.GetAllComponentsOfType(ComponentType.EnumComponentType.Cockpit));
                        float minArmour = comps.OrderBy(x => x.Armour).FirstOrDefault().Armour;
                        float maxArmour = comps.OrderByDescending(x => x.Armour).FirstOrDefault().Armour;

                        float minWeight = comps.OrderBy(x => x.WeightInKG).FirstOrDefault().WeightInKG;
                        float maxWeight = comps.OrderByDescending(x => x.WeightInKG).FirstOrDefault().WeightInKG;

                        float minPowerProvided = comps.OrderBy(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;
                        float maxPowerProvided = comps.OrderByDescending(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;


                        float minPowerUsed = comps.OrderBy(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;
                        float maxPowerUsed = comps.OrderByDescending(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;

                        float minHealth = comps.OrderBy(x => x.Health).FirstOrDefault().Health;
                        float maxHealth = comps.OrderByDescending(x => x.Health).FirstOrDefault().Health;


                        float minSpeed = comps.OrderBy(x => x.Legs_MaxForwardSpeed).FirstOrDefault().Legs_MaxForwardSpeed;
                        float maxSpeed = comps.OrderByDescending(x => x.Legs_MaxForwardSpeed).FirstOrDefault().Legs_MaxForwardSpeed;

                        float minAccel = comps.OrderBy(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;
                        float maxAccel = comps.OrderByDescending(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;

                        float minRot = comps.OrderBy(x => x.Cockpit_SettingMaxYRotation + x.Cockpit_SettingMaxZRotation).FirstOrDefault().Cockpit_SettingMaxYRotation + comps.OrderBy(x => x.Cockpit_SettingMaxYRotation + x.Cockpit_SettingMaxZRotation).FirstOrDefault().Cockpit_SettingMaxZRotation;

                        float maxRot = comps.OrderByDescending(x => x.Cockpit_SettingMaxYRotation + x.Cockpit_SettingMaxZRotation).FirstOrDefault().Cockpit_SettingMaxYRotation + comps.OrderBy(x => x.Cockpit_SettingMaxYRotation + x.Cockpit_SettingMaxZRotation).FirstOrDefault().Cockpit_SettingMaxZRotation;


                        float minInstru = comps.OrderBy(x => x.InstructionsPerSecond).FirstOrDefault().InstructionsPerSecond;
                        float maxInstru = comps.OrderByDescending(x => x.InstructionsPerSecond).FirstOrDefault().InstructionsPerSecond;


                        Stat1.SetData("Armour", aType.Armour + "", aType.Armour, minArmour, maxArmour);
                        Stat2.SetData("Weight", aType.WeightInKG + "KG", aType.WeightInKG, minWeight, maxWeight);
                        Stat3.SetData("Power Provided", aType.PowerProvidedPerFrame + "KW", aType.PowerProvidedPerFrame, minPowerProvided, maxPowerProvided);
                        Stat4.SetData("Power Used", aType.PowerUsedPerFrame + "KW", aType.PowerUsedPerFrame, minPowerUsed, maxPowerUsed);
                        Stat5.SetData("CPU", aType.InstructionsPerSecond + " I/S", aType.InstructionsPerSecond, minInstru, maxInstru);



                        Stat6.Clear();
                        Stat7.Clear();

                        break;
                    }
                case ComponentType.EnumComponentType.WeaponAnySlot:
                    {
                        List<ComponentType> comps = rc.GetAllComponentsOfType(ComponentType.EnumComponentType.Shoulders);
                        comps.AddRange(rc.GetAllComponentsOfType(ComponentType.EnumComponentType.Cockpit));
                        float minArmour = comps.OrderBy(x => x.Armour).FirstOrDefault().Armour;
                        float maxArmour = comps.OrderByDescending(x => x.Armour).FirstOrDefault().Armour;

                        float minWeight = comps.OrderBy(x => x.WeightInKG).FirstOrDefault().WeightInKG;
                        float maxWeight = comps.OrderByDescending(x => x.WeightInKG).FirstOrDefault().WeightInKG;

                        float minPowerProvided = comps.OrderBy(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;
                        float maxPowerProvided = comps.OrderByDescending(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;


                        float minPowerUsed = comps.OrderBy(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;
                        float maxPowerUsed = comps.OrderByDescending(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;

                        float minHealth = comps.OrderBy(x => x.Health).FirstOrDefault().Health;
                        float maxHealth = comps.OrderByDescending(x => x.Health).FirstOrDefault().Health;


                        float minSpeed = comps.OrderBy(x => x.Legs_MaxForwardSpeed).FirstOrDefault().Legs_MaxForwardSpeed;
                        float maxSpeed = comps.OrderByDescending(x => x.Legs_MaxForwardSpeed).FirstOrDefault().Legs_MaxForwardSpeed;

                        float minAccel = comps.OrderBy(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;
                        float maxAccel = comps.OrderByDescending(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;

                        float minRot = comps.OrderBy(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;
                        float maxRot = comps.OrderByDescending(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;

                        float minPower = comps.OrderBy(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;
                        float maxPower = comps.OrderByDescending(x => x.Legs_AccelerationAdd).FirstOrDefault().Legs_AccelerationAdd;

                        float minDamage = comps.OrderBy(x => x.Weapon_BulletDamage).FirstOrDefault().Weapon_BulletDamage;
                        float maxDamage = comps.OrderByDescending(x => x.Weapon_BulletDamage).FirstOrDefault().Weapon_BulletDamage;

                        float minDelay = comps.OrderBy(x => x.Weapon_Shoot_DelayTime).FirstOrDefault().Weapon_Shoot_DelayTime;
                        float maxDelay = comps.OrderByDescending(x => x.Weapon_Shoot_DelayTime).FirstOrDefault().Weapon_Shoot_DelayTime;

                        int bulletExitPoints = 0;
                        IoWeapon aWeap = aType.GetIoWeapob();
                        if (aWeap != null)
                        {
                            bulletExitPoints = aWeap.BulletExitPoints.Count;
                        }
                        Stat1.SetData("Armour", aType.Armour + "", aType.Armour, minArmour, maxArmour);
                        Stat2.SetData("Weight", aType.WeightInKG + "KG", aType.WeightInKG, minWeight, maxWeight);
                        Stat3.SetData("Power Provided", aType.PowerProvidedPerFrame + "KW", aType.PowerProvidedPerFrame, minPowerProvided, maxPowerProvided);
                        Stat4.SetData("Power Used", aType.PowerUsedPerFrame + "KW", aType.PowerUsedPerFrame, minPowerUsed, maxPowerUsed);
                        Stat5.SetData("Bullets", bulletExitPoints + "", bulletExitPoints, 0, 10);
                        Stat6.SetData("Bullet Damage", aType.Weapon_BulletDamage + "", aType.Weapon_BulletDamage, minDamage, maxDamage);
                        Stat7.SetData("Reload Time", aType.Weapon_Shoot_DelayTime + "s", aType.Weapon_Shoot_DelayTime, minDelay, maxDelay);
                        break;
                    }
                default:
                    {
                        List<ComponentType> comps = rc.GetAllComponent(true);
                        if (comps != null && comps.Count > 0)
                        {
                            float minArmour = comps.OrderBy(x => x.Armour).FirstOrDefault().Armour;
                            float maxArmour = comps.OrderByDescending(x => x.Armour).FirstOrDefault().Armour;

                            float minWeight = comps.OrderBy(x => x.WeightInKG).FirstOrDefault().WeightInKG;
                            float maxWeight = comps.OrderByDescending(x => x.WeightInKG).FirstOrDefault().WeightInKG;

                            float minPowerProvided = comps.OrderBy(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;
                            float maxPowerProvided = comps.OrderByDescending(x => x.PowerProvidedPerFrame).FirstOrDefault().PowerProvidedPerFrame;


                            float minPowerUsed = comps.OrderBy(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;
                            float maxPowerUsed = comps.OrderByDescending(x => x.PowerUsedPerFrame).FirstOrDefault().PowerUsedPerFrame;

                            Stat1.SetData("Armour", aType.Armour + "", aType.Armour, minArmour, maxArmour);
                            Stat2.SetData("Weight", aType.WeightInKG + "KG", aType.WeightInKG, minWeight, maxWeight);
                            Stat3.SetData("Power Provided", aType.PowerProvidedPerFrame + "KW", aType.PowerProvidedPerFrame, minPowerProvided, maxPowerProvided);
                            Stat4.SetData("Power Used", aType.PowerUsedPerFrame + "KW", aType.PowerUsedPerFrame, minPowerUsed, maxPowerUsed);
                            Stat5.Clear();
                            Stat6.Clear();
                            Stat7.Clear();
                        }
                        else
                        {
                            Stat1.Clear();
                            Stat2.Clear();
                            Stat3.Clear();
                            Stat4.Clear();
                            Stat5.Clear();
                            Stat6.Clear();
                            Stat7.Clear();
                        }
                        break;
                    }
            }
        }
    }
}
