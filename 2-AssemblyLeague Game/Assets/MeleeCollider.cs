using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour {

    public RobotMeta Owner;
    public IoWeapon MyWeapon;
    public List<Transform> ObjectsTouching = new List<Transform>();
    public bool IsTouching = false;
    public bool KeepDoingDamageOnCollisionStay = false;
    public bool DoWeaponShootEffectOnContact = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Owner == null) Owner = gameObject.transform.GetComponent<RobotMeta>();
        if (Owner == null) Owner = gameObject.transform.GetComponentInParent<RobotMeta>();
        if (MyWeapon == null) MyWeapon = gameObject.transform.GetComponent<IoWeapon>();
        if (MyWeapon == null) MyWeapon = gameObject.transform.GetComponentInParent<IoWeapon>();
    }


    private void DoDomage(GameObject other, Vector3 connectPoint, Collider aCollider)
    {
        {
            FxMaterial aMaterial = other.GetComponent<FxMaterial>();
            if (aMaterial == null) aMaterial = other.GetComponentInParent<FxMaterial>();

            if (aMaterial != null)
            {
                 aMaterial.DoDomage(this, other, connectPoint, aCollider);

            }

            RobotMeta theOther = other.GetComponent<RobotMeta>();
            if (theOther == null)
            {
                theOther = other.GetComponentInParent<RobotMeta>();
            }
            if (theOther != null && MyWeapon!= null)
            {
                theOther.TakeDamage(this);
                if (DoWeaponShootEffectOnContact)
                {

                    MyWeapon.MustShoot = true;
                }
            }


        }
    }

    void OnCollisionEnter(Collision collision)
    {

        IsTouching = true;
        //  print("ENTER: " + collision.gameObject.name);
        DoDomage(collision.gameObject, collision.contacts[0].point, collision.collider);
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        IsTouching = true;
        if (KeepDoingDamageOnCollisionStay)
        {
            //   print("STAY: " + collisionInfo.gameObject.name);
            DoDomage(collisionInfo.gameObject, collisionInfo.contacts[0].point, collisionInfo.collider);
        }
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        //   print("EXIT: " + collisionInfo.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        IsTouching = true;
        //  print("TRIGGER ENTER: " + other.gameObject.name);
         {
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
            DoDomage(other.gameObject, contactPoint, other);
        }

    }
    void OnTriggerStay(Collider other)
    {
        IsTouching = true;
        if (KeepDoingDamageOnCollisionStay)
        {  
            
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
            DoDomage(other.gameObject, contactPoint, other);
        }
    }
    void OnTriggerExit(Collider other)
    {

        //    print("TRIGGER EXIT: " + other.gameObject.name);
    }
}
