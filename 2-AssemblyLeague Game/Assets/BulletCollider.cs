using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletCollider : MonoBehaviour
{
    public Bullet sourceBullet;
    public bool IsOnFire = false;
    // Use this for initialization
    void Start()
    {
        sourceBullet = gameObject.GetComponent<Bullet>();
        if (sourceBullet == null)
        {
            sourceBullet = gameObject.transform.parent.gameObject.GetComponent<Bullet>();
        }
        if (sourceBullet.ExplosionObjectToRelease != null)
        {
            sourceBullet.ExplosionObjectToRelease.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnEnable()
    {
        if (sourceBullet != null && sourceBullet.MyWeapon != null)
        {
            Collider[] checkResult = Physics.OverlapSphere(gameObject.transform.position, 1);

             for (int c = 0; c < checkResult.Length; c++)
            {
                if (checkResult[c].gameObject.transform != sourceBullet.MyWeapon.gameObject.transform)
                {
                    List<Transform> allT = new List<Transform>();
                    allT.AddRange(sourceBullet.MyWeapon.gameObject.transform.GetComponentsInChildren<Transform>());
                    if (allT.Contains(checkResult[c].gameObject.transform) == false)
                    {
                       
                       
                               // print("DO IMMEIDATE IMPACT ");
                                DoCollision(checkResult[c].gameObject, gameObject.transform.position, checkResult[c]);
                            
                    }
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //  print("TRIGGER other: " + other.gameObject.name);
        Vector3 contactPoint = other.transform.position;
        MeshCollider aCol = other.gameObject.transform.GetComponent<MeshCollider>();
        BoxCollider aCol1 = other.gameObject.transform.GetComponent<BoxCollider>();
        SphereCollider aCol2 = other.gameObject.transform.GetComponent<SphereCollider>();
        CapsuleCollider aCol3 = other.gameObject.transform.GetComponent<CapsuleCollider>();
        if (aCol != null)
        {
            if (aCol.convex)
            {
                contactPoint = aCol.ClosestPoint(gameObject.transform.position);
            }
        }
        if (aCol1 != null)
        {
            //if (aCol1.convex)
            {
                contactPoint = aCol1.ClosestPoint(gameObject.transform.position);
            }
        }
        if (aCol2 != null)
        {
            //if (aCol2.convex)
            {
                contactPoint = aCol2.ClosestPoint(gameObject.transform.position);
            }
        }
        if (aCol3 != null)
        {
            //if (aCol3.convex)
            {
                contactPoint = aCol3.ClosestPoint(gameObject.transform.position);
            }
        }

        DoCollision(other.gameObject, contactPoint, other);
        if (sourceBullet.MyLaserStretch != null)
        {
            sourceBullet.MyLaserStretch.WantedLength = 0;
            sourceBullet.MyLaserStretch.ForceSetLength();
        }
      }
    void OnCollisionEnter(Collision collision)
    {

        //     print("COLLISION other: " + collision.gameObject.name);
        DoCollision(collision.gameObject, collision.contacts[0].point, collision.collider);
    }
    public void DoCollision(GameObject target, Vector3 contactPoint, Collider aCollider)
    {
        if (IsOnFire)
        {

               //print("ONFIRE other: " + target.gameObject.name);
            if (sourceBullet == null)
            {
                sourceBullet = gameObject.GetComponent<Bullet>();
                if (sourceBullet == null)
                {
                    sourceBullet = gameObject.transform.parent.gameObject.GetComponent<Bullet>();
                }
            }
            List<Transform> possibles = new List<Transform>();
            try
            {

                possibles.AddRange(sourceBullet.source.GetComponentsInChildren<Transform>());
            }
            catch { }
            if (possibles.Contains(target.transform))
            {
                //   print("COLLISION WITH SELF!");
            }
            else
            {
                if (sourceBullet.Owner != null)
                {
                    if (aCollider.gameObject.transform != sourceBullet.Owner.gameObject.transform && target.transform != sourceBullet.Owner.gameObject.transform)
                    {
                        Bullet otherBullet = aCollider.gameObject.transform.GetComponent<Bullet>();
                        if (otherBullet == null)
                        {
                            otherBullet = aCollider.gameObject.transform.GetComponentInParent<Bullet>(); ;
                        }
                        bool ignore = false;
                        if (otherBullet != null)
                        {
                            if (otherBullet.Owner.gameObject.transform == sourceBullet.Owner.gameObject.transform)
                            {
                                ignore = true;
                            }

                        }
                        FireCollider fireCollider = aCollider.gameObject.transform.GetComponent<FireCollider>();
                        if (fireCollider != null)
                        {
                            fireCollider = aCollider.gameObject.transform.GetComponentInParent<FireCollider>(); ;
                        }

                        if (fireCollider != null)
                        {
                          //  if (fireCollider.Owner.gameObject.transform == sourceBullet.Owner.gameObject.transform)
                            {
                              //  print("Ignore impact because of fire!!!!!!!!!!!!!!!!!!!!");
                                ignore = true;
                            }

                        }
                        MeleeCollider meleeCollider = aCollider.gameObject.transform.GetComponent<MeleeCollider>();
                        if (meleeCollider != null)
                        {
                            meleeCollider = aCollider.gameObject.transform.GetComponentInParent<MeleeCollider>(); ;
                        }

                        if (meleeCollider != null)
                        {
                           // if (meleeCollider.Owner.gameObject.transform == sourceBullet.Owner.gameObject.transform)
                            {
                             //   print("Ignore impact because of fire!!!!!!!!!!!!!!!!!!!!");
                                ignore = true;
                            }

                        }
                        if (ignore == false)
                        {
                            FxMaterial aMaterial = target.gameObject.GetComponent<FxMaterial>();
                            if (aMaterial == null) aMaterial = target.gameObject.GetComponentInParent<FxMaterial>();

                            if (aMaterial != null)
                            {                             
                                aMaterial.DoDomage(sourceBullet, target.gameObject, contactPoint, aCollider);                               
                            }

                            if (sourceBullet.ExplosionObjectToRelease != null)
                            {
                                sourceBullet.ExplosionObjectToRelease.transform.parent = null;// target.transform;
                                sourceBullet.ExplosionObjectToRelease.SetActive(true);
                            }
                            sourceBullet.DestroyToCache();
                           
                            IsOnFire = false;
                        }
                    }
                }
            }

        }

    }

    bool IsOtherObjectAWall(GameObject anObject)
    {
        bool ret = false;
        ArenaWall aWall = anObject.GetComponent<ArenaWall>();
        if (aWall != null)
        {
            ret = true;
        }
        return ret;
    }
}
