using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public LaserStretch MyLaserStretch;
    public IoWeapon MyWeapon;
    public float TotalBasicDamageOnHit2 = 10;
    public float BulletLifeTime = 10f;
    float currentTime = 0;
    public float DelayDestroyeTime = 0;

    float currentDestroyTime = 0;
    public bool isBusyDestroying = false;
    public Rigidbody MyRigidBody;
    public enum BulletType
    {
        Laser,
        Gun_Bullet,
        Shotgun_Bullet,
        Grenade_Sharp,
        Grenade_Blunt,
        Rocket,
        BigRocket,
        CannonBall,
        Flame,
        ElectricShockBullet,
        ElectricShockMelee,
        NoBullet,
        ShotgunPellet,


    }
    public string GetBulletSound()
    {

        return TheType.ToString();
    }
    public void PlayBulletSound()
    {
        AudioController.Play(GetBulletSound(), transform);
    }
    public BulletType TheType;
    bool isFired = false;
    public GameObject source;
    BulletCollider bCollider;
    public GameObject ExplosionObjectToRelease;

    public RobotMeta Owner;

    public bool IsInCache = false;
    // Use this for initialization
    void Start()
    {
        bCollider = gameObject.GetComponent<BulletCollider>();
        if (bCollider == null)
        {
            bCollider = gameObject.GetComponentInChildren<BulletCollider>();
        }
     

    }
    // Update is called once per frame
    RaycastHit aHit;
    Ray myRay;
    private bool DoImpactFasterThanLight()
    {
        bool ret = false;

        myRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        if (Physics.Raycast(myRay, out aHit, 2))
        {
            ret = true;
        }

        return ret;
    }
    void Update()
    {
        if (isBusyDestroying)
        {
            currentDestroyTime += Time.deltaTime;
            if (currentDestroyTime >= DelayDestroyeTime)
            {
                currentDestroyTime = 0;
                DestroyToCacheForce();
            }
        }

        if (gameObject.transform.position.x <= -2000
              || gameObject.transform.position.x >= 2000
            || gameObject.transform.position.y <= -2000
              || gameObject.transform.position.y >= 2000
            || gameObject.transform.position.z <= -2000
              || gameObject.transform.position.z >= 2000
            )
        {
               DestroyToCache();
        }
        else
        {
            if (MustGoToCachNextTurn)
            {
                gameObject.SetActive(false);
            }
            if (isFired)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= BulletLifeTime)
                {
                    currentTime = 0;
                    DestroyToCacheForce();

                }
                else
                {


                    bool HasImpacted = false;
                    try
                    {
                        HasImpacted = DoImpactFasterThanLight();
                        if (HasImpacted == false)
                        {
                            if (MyRigidBody == null)
                            {
                                if (MyLaserStretch != null)
                                {
                                    MyLaserStretch.WantedLength += MyWeapon.myComponentType.Weapon_BulletSpeed;
                                }
                                else
                                {

                                    gameObject.transform.Translate(Vector3.forward * MyWeapon.myComponentType.Weapon_BulletSpeed * 1.0f * Time.deltaTime, Space.Self);
                                }

                            }
                            else
                            {
                            }

                            countDown -= Time.fixedDeltaTime * 7;

                            float dist = Vector3.Distance(source.gameObject.transform.position, gameObject.transform.position);
                           
                        }
                        else
                        {
                            if (aHit.collider.gameObject.transform != Owner.gameObject.transform && aHit.transform != Owner.gameObject.transform)
                            {
                                Bullet otherBullet = aHit.collider.gameObject.transform.GetComponent<Bullet>();
                                if (otherBullet == null)
                                {
                                    otherBullet = aHit.collider.gameObject.transform.GetComponentInParent<Bullet>(); ;
                                }
                                bool ignore = false;
                                if (otherBullet != null)
                                {
                                    if (otherBullet.Owner.gameObject.transform == Owner.gameObject.transform)
                                    {
                                        ignore = true;
                                    }

                                }
                                FireCollider fireCollider = aHit.collider.gameObject.transform.GetComponent<FireCollider>();
                                if (fireCollider != null)
                                {
                                    fireCollider = aHit.collider.gameObject.transform.GetComponentInParent<FireCollider>(); ;
                                }

                                if (fireCollider != null)
                                {
                                    if (fireCollider.Owner.gameObject.transform == Owner.gameObject.transform)
                                    {
                                        print("Ignore impact because of fire!!!!!!!!!!!!!!!!!!!!");
                                        ignore = true;
                                    }

                                }

                                if (ignore == false)
                                {
                                    FxMaterial aMaterial = aHit.collider.gameObject.GetComponent<FxMaterial>();
                                    if (aMaterial == null) aMaterial = aHit.collider.gameObject.GetComponentInParent<FxMaterial>();

                                    if (aMaterial != null)
                                    {
                                         aMaterial.DoDomage(this, aHit.collider.gameObject, aHit.point, aHit.collider);

                                      }

                                   
                                    DestroyToCache();
                                }
                            }
                        }
                    }
                    catch
                    {
                        // DestroyToCache();
                        // Destroy(gameObject);
                    }
                }
            }
            else
            {
                currentTime = 0;
                DestroyToCache();
            }
        }

    }
    bool MustGoToCachNextTurn = false;
    public void DestroyToCache()
    {
        if (DelayDestroyeTime == 0)
        {
            if (MyLaserStretch != null)
            {
                MyLaserStretch.WantedLength = 1;
                MyLaserStretch.ForceSetLength();
            }
            isFired = false;
            IsInCache = true;
            gameObject.SetActive(false);
        }
        else
        {
            isBusyDestroying = true;
        }
      }
    public void DestroyToCacheForce()
    {
        if (MyLaserStretch != null)
        {
            MyLaserStretch.WantedLength = 1;
            MyLaserStretch.ForceSetLength();
        }
        isFired = false;
        IsInCache = true;
        gameObject.SetActive(false);

        isBusyDestroying = false;

     }
    public float countDown = 1f;
    public void Fire(RobotMeta aShooter, IoWeapon aWeapon)
    {
        Rigidbody rigidOfRobot = aShooter.gameObject.transform.GetComponent<Rigidbody>();
        MyWeapon = aWeapon;
        currentTime = 0;
        if (MyRigidBody != null)
        {

            MyRigidBody.velocity = Vector3.zero;
            MyRigidBody.angularVelocity = Vector3.zero;
            if (rigidOfRobot != null)
            {
            
                MyRigidBody.velocity = rigidOfRobot.velocity + aWeapon.transform.forward * aWeapon.myComponentType.Weapon_BulletSpeed;
                MyRigidBody.angularVelocity = MyRigidBody.angularVelocity;
                 }
            else
            {
                MyRigidBody.velocity = Vector3.zero;
                MyRigidBody.angularVelocity = Vector3.zero;

            }

            if (MyRigidBody!=null && aWeapon != null)
            {
                MyRigidBody.velocity += aWeapon.transform.forward * aWeapon.myComponentType.Weapon_BulletSpeed;
            }
        }

        MustGoToCachNextTurn = false;
        if (bCollider == null)
        {
            bCollider = gameObject.GetComponent<BulletCollider>();
            if (bCollider == null)
            {
                bCollider = gameObject.GetComponentInChildren<BulletCollider>();
            }
        }
         IsInCache = false;
        isBusyDestroying = false;
        currentDestroyTime = 0;
        isFired = true;
        Owner = aShooter;
        source = aShooter.gameObject;
        countDown = 2f;
        bCollider.IsOnFire = true;
         gameObject.SetActive(true);
        aShooter.ShotsFired++;
        PlayBulletSound();
        if (MyLaserStretch != null)
        {
            MyLaserStretch.WantedLength = 1;
            MyLaserStretch.ForceSetLength();
            gameObject.transform.parent = MyWeapon.gameObject.transform;
        }

        if (TheType == BulletType.Shotgun_Bullet)
        {
            for (int c = 0; c < 5; c++)
            {
                Bullet newBullet = BulletCache.PublicAccess.GetBulletToShoot(BulletType.ShotgunPellet);
                Vector3 pos = gameObject.transform.position + gameObject.transform.forward * 1.5f;
                pos.x += Random.Range(-0.5f, 0.5f);
                pos.y += Random.Range(-0.5f, 0.5f);
                pos.z += Random.Range(-0.5f, 0.5f);
                newBullet.gameObject.transform.position = pos;
                Quaternion rot = gameObject.transform.transform.rotation;

                Vector3 myAngl = gameObject.transform.transform.rotation.eulerAngles;
                myAngl.x += Random.Range(-0.15f, 0.15f);
                myAngl.y += Random.Range(-0.15f, 0.15f);
                myAngl.z += Random.Range(-0.15f, 0.15f);
                myAngl.Normalize();
                rot.eulerAngles = (myAngl);
                newBullet.gameObject.transform.rotation = rot;
                newBullet.Fire(aShooter, aWeapon, myAngl);
            }


        }
        


    }
    public void Fire(RobotMeta aShooter, IoWeapon aWeapon, Vector3 forwardAngle)
    {
        Rigidbody rigidOfRobot = aShooter.gameObject.transform.GetComponent<Rigidbody>();
        MyWeapon = aWeapon;
        currentTime = 0;
        if (MyRigidBody != null)
        {

            MyRigidBody.velocity = Vector3.zero;
            MyRigidBody.angularVelocity = Vector3.zero;
            if (rigidOfRobot != null)
            {
             
                MyRigidBody.velocity = rigidOfRobot.velocity + forwardAngle * aWeapon.myComponentType.Weapon_BulletSpeed;
                MyRigidBody.angularVelocity = MyRigidBody.angularVelocity;
                  }
            else
            {
                MyRigidBody.velocity = Vector3.zero;
                MyRigidBody.angularVelocity = Vector3.zero;

            }

            if (MyRigidBody != null && aWeapon != null)
            {
                MyRigidBody.velocity += aWeapon.transform.forward * aWeapon.myComponentType.Weapon_BulletSpeed; 
            }
        }

        MustGoToCachNextTurn = false;
        if (bCollider == null)
        {
            bCollider = gameObject.GetComponent<BulletCollider>();
            if (bCollider == null)
            {
                bCollider = gameObject.GetComponentInChildren<BulletCollider>();
            }
        }
        IsInCache = false;
        isBusyDestroying = false;
        currentDestroyTime = 0;
        isFired = true;
        Owner = aShooter;
        source = aShooter.gameObject;
        countDown = 2f;
        bCollider.IsOnFire = true;
         gameObject.SetActive(true);
        aShooter.ShotsFired++;
        PlayBulletSound();
        if (MyLaserStretch != null)
        {
            MyLaserStretch.WantedLength = 1;
            MyLaserStretch.ForceSetLength();
            gameObject.transform.parent = MyWeapon.gameObject.transform;
        }

        if (TheType == BulletType.Shotgun_Bullet)
        {
            for (int c = 0; c < 5; c++)
            {
                Bullet newBullet = BulletCache.PublicAccess.GetBulletToShoot(BulletType.ShotgunPellet);
                Vector3 pos = gameObject.transform.position + gameObject.transform.forward * 1.5f;
                pos.x += Random.Range(-0.5f, 0.5f);
                pos.y += Random.Range(-0.5f, 0.5f);
                pos.z += Random.Range(-0.5f, 0.5f);
                newBullet.gameObject.transform.position = pos;
                Quaternion rot = gameObject.transform.transform.rotation;

                Vector3 myAngl = gameObject.transform.transform.rotation.eulerAngles;
                myAngl.x += Random.Range(-0.15f, 0.15f);
                myAngl.y += Random.Range(-0.15f, 0.15f);
                myAngl.z += Random.Range(-0.15f, 0.15f);
                myAngl.Normalize();
                rot.eulerAngles = (myAngl);
                newBullet.gameObject.transform.rotation = rot;
                newBullet.Fire(aShooter, aWeapon);
            }


        }
        


    }

}
