using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsCollider : MonoBehaviour
{
    public IoLegsMech MyLegs;
    public List<Transform> ObjectsTouching = new List<Transform>();
    public bool IsTouching = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        IsTouching = true;
   //     print("ENTER: " + collision.gameObject.name);
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        IsTouching = true;
       // print("STAY: " + collisionInfo.gameObject.name);
    }
    void OnCollisionExit(Collision collisionInfo)
    {
    //    print("EXIT: " + collisionInfo.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        IsTouching = true;
   //     print("TRIGGER ENTER: " + other.gameObject.name);

    }
    void OnTriggerStay(Collider other)
    {
        IsTouching = true;
     //   print("TRIGGER STAY: " + other.gameObject.name);
    }
    void OnTriggerExit(Collider other)
    {

    //    print("TRIGGER EXIT: " + other.gameObject.name);
    }

}
