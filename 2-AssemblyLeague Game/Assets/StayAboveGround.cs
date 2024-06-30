using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAboveGround : MonoBehaviour
{

    RaycastHit aHit;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray aRay = new Ray(gameObject.transform.position + Vector3.up * -7 * 0.5f, Vector3.down);

        if (Physics.Raycast(aRay, out aHit, 1000) == false)
        {
            gameObject.transform.parent.transform.Translate(Vector3.up * Time.deltaTime * 10, Space.World);
        }





    }
}
