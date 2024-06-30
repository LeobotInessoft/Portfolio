using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCache : MonoBehaviour
{

    public GameObject Prefab_Laser;
    public GameObject Prefab_Gun_Bullet;
    public GameObject Prefab_Shotgun_Bullet;
    public GameObject Prefab_Shotgun_Pellet;
    public GameObject Prefab_Grenade_Sharp;
    public GameObject Prefab_Grenade_Blunt;
    public GameObject Prefab_Rocket;
    public GameObject Prefab_BigRocket;
    public GameObject Prefab_CannonBall;
    public GameObject Prefab_Flame;
    public GameObject Prefab_ElectricShockBullet;
    public GameObject Prefab_ElectricShockMelee;
    public Dictionary<Bullet.BulletType, List<Bullet>> AllSpawnedBullets;
    public static BulletCache PublicAccess;
    public int DefaultNumberOfBullets = 256;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        AllSpawnedBullets = new Dictionary<Bullet.BulletType, List<Bullet>>();
        GenerateCache(DefaultNumberOfBullets);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateCache(int size)
    {
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Laser, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Laser, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Gun_Bullet, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Gun_Bullet, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Shotgun_Bullet, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Shotgun_Bullet, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Grenade_Sharp, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Grenade_Sharp, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Grenade_Blunt, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Grenade_Blunt, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Rocket, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Rocket, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_BigRocket, size);
            AllSpawnedBullets.Add(Bullet.BulletType.BigRocket, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_CannonBall, size);
            AllSpawnedBullets.Add(Bullet.BulletType.CannonBall, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Flame, size);
            AllSpawnedBullets.Add(Bullet.BulletType.Flame, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_ElectricShockBullet, size);
            AllSpawnedBullets.Add(Bullet.BulletType.ElectricShockBullet, bullets);
        }

        {
            List<Bullet> bullets = CreateCacheList(Prefab_ElectricShockMelee, size);
            AllSpawnedBullets.Add(Bullet.BulletType.ElectricShockMelee, bullets);
        }
        {
            List<Bullet> bullets = CreateCacheList(Prefab_Shotgun_Pellet, size);
            AllSpawnedBullets.Add(Bullet.BulletType.ShotgunPellet, bullets);
        }
    }
    public void GenerateCache(int size, Bullet.BulletType aType)
    {
       // print("Spawning Extra " + aType);
        switch (aType)
        {
            case Bullet.BulletType.Laser:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Laser, size);
                    AllSpawnedBullets[Bullet.BulletType.Laser].AddRange (bullets);
                    break;
                }
            case Bullet.BulletType.Gun_Bullet:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Gun_Bullet, size);
                    AllSpawnedBullets[Bullet.BulletType.Gun_Bullet].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.Shotgun_Bullet:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Shotgun_Bullet, size);
                    AllSpawnedBullets[Bullet.BulletType.Shotgun_Bullet].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.Grenade_Sharp:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Grenade_Sharp, size);
                    AllSpawnedBullets[Bullet.BulletType.Grenade_Sharp].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.Grenade_Blunt:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Grenade_Blunt, size);
                    AllSpawnedBullets[Bullet.BulletType.Grenade_Blunt].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.Rocket:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Rocket, size);
                    AllSpawnedBullets[Bullet.BulletType.Rocket].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.BigRocket:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_BigRocket, size);
                    AllSpawnedBullets[Bullet.BulletType.BigRocket].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.CannonBall:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_CannonBall, size);
                    AllSpawnedBullets[Bullet.BulletType.CannonBall].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.Flame:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Flame, size);
                    AllSpawnedBullets[Bullet.BulletType.Flame].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.ElectricShockBullet:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_ElectricShockBullet, size);
                    AllSpawnedBullets[Bullet.BulletType.ElectricShockBullet].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.ElectricShockMelee:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_ElectricShockMelee, size);
                    AllSpawnedBullets[Bullet.BulletType.ElectricShockMelee].AddRange(bullets);
                    break;
                }
            case Bullet.BulletType.ShotgunPellet:
                {
                    List<Bullet> bullets = CreateCacheList(Prefab_Shotgun_Pellet, size);
                    AllSpawnedBullets[Bullet.BulletType.ShotgunPellet].AddRange(bullets);
                    break;
                }
        }


    }
    private List<Bullet> CreateCacheList(GameObject aBullet, int size)
    {
        List<Bullet> ret = new List<Bullet>();
        for (int c = 0; c < size; c++)
        {
            GameObject inst = Instantiate(aBullet, Vector3.zero, Quaternion.identity, this.transform);

            Bullet aBull = inst.GetComponent<Bullet>();
            aBull.IsInCache = true;
            inst.SetActive(false);
            ret.Add(aBull);
        }
        return ret;
    }

    public Bullet GetBulletToShoot(Bullet.BulletType aType)
    {
        Bullet ret = null;
        List<Bullet> options = AllSpawnedBullets[aType];
        
        for (int c = 0; c < options.Count; c++)
        {
            if (options[c] != null && options[c].gameObject != null)
            {
                if (options[c].IsInCache)
                {
                    ret = options[c];
                    break;
                }
            }
        }
        if (ret == null)
        {
            GenerateCache(5, aType);
            ret= GetBulletToShoot(aType);
        }
        return ret;
    }

}
