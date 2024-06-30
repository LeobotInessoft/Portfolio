using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSizeFinder : MonoBehaviour
{
    public float Size = 0;
    public GameObject CurrentTarget;
    public GameObject aBoxCollider;
    public static ObjectSizeFinder PublicAccess;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTarget != null)
        {
            FindSize(CurrentTarget);
        }
    }

    public float FindSize(GameObject Target)
    {
        CurrentTarget = Target;
        {
            List<MeshCollider> allTrans = new List<MeshCollider>();
            MeshCollider myCol = Target.transform.GetComponent<MeshCollider>();
            if (myCol != null)
            {
                allTrans.Add(myCol);
            }
            allTrans.AddRange(Target.transform.GetComponentsInChildren<MeshCollider>());

            MakeBoxColliderFit(Target);
            if (aBoxCollider != null)
            {
                aBoxCollider.gameObject.transform.forward = Target.transform.forward;

                Vector3 pos = aBoxCollider.gameObject.transform.position + Target.transform.forward * 1 + Target.transform.right * 1 + Target.transform.up * 1;

               
                Size = aBoxCollider.transform.localScale.x * aBoxCollider.transform.localScale.y * aBoxCollider.transform.localScale.z;
            }
        }
        return Size;
    }
    private void MakeBoxColliderFit(GameObject anObject)
    {
        List<MeshCollider> allMeshColliders = new List<MeshCollider>();
        MeshCollider myCol = anObject.transform.GetComponent<MeshCollider>();
        if (myCol != null)
        {
            allMeshColliders.Add(myCol);
        }
        allMeshColliders.AddRange(anObject.transform.GetComponentsInChildren<MeshCollider>());

        List<Transform> allTrans = new List<Transform>();
        allTrans.Add(anObject.transform);
        allTrans.AddRange(anObject.transform.GetComponentsInChildren<Transform>());

        List<int> toRemove = new List<int>();
        for (int c = 0; c < allTrans.Count; c++)
        {
            if(allTrans[c].gameObject.name.ToLower().StartsWith("audioobject:"))
            {
                toRemove.Add(c);
            }
        }
        if (toRemove.Count > 0)
        {
            for (int c = toRemove.Count-1; c >= 0; c--)
            {
               // print("C: " + c);
                allTrans.RemoveAt(toRemove[c]);
            }
        }
        Vector3 mostX = anObject.transform.position;
        Vector3 leastX = anObject.transform.position;
        Vector3 mostY = anObject.transform.position;
        Vector3 leastY = anObject.transform.position;
        Vector3 mostZ = anObject.transform.position;
        Vector3 leastZ = anObject.transform.position;

      
        for (int c = 0; c < allTrans.Count; c++)
        {

            if (allTrans[c].position.x <= leastX.x)
            {
                leastX = allTrans[c].position;
            }

            if (allTrans[c].position.x >= mostX.x)
            {
                mostX = allTrans[c].position;
            }

            if (allTrans[c].position.y <= leastY.y)
            {
                leastY = allTrans[c].position;
            }

            if (allTrans[c].position.y >= mostY.y)
            {
                mostY = allTrans[c].position;
            }
            if (allTrans[c].position.z <= leastZ.z)
            {
                leastZ = allTrans[c].position;
            }

            if (allTrans[c].position.z >= mostZ.z)
            {
                mostZ = allTrans[c].position;
            }




        }

        
        float dist = 51.5f;
        RaycastHit rayHit;
        if (anObject != null && aBoxCollider != null)
        {
            Vector3 posAtMostx = anObject.transform.position + aBoxCollider.gameObject.transform.right * dist;
            Vector3 posAtLeastx = anObject.transform.position - aBoxCollider.gameObject.transform.right * dist;

            Vector3 posAtMosty = anObject.transform.position + aBoxCollider.gameObject.transform.up * dist;
            Vector3 posAtLeasty = anObject.transform.position - aBoxCollider.gameObject.transform.up * dist;

            Vector3 posAtMostz = anObject.transform.position + aBoxCollider.gameObject.transform.forward * dist;
            Vector3 posAtLeastz = anObject.transform.position - aBoxCollider.gameObject.transform.forward * dist;

            Ray aRay = new Ray();
            Vector3 direction = new Vector3();
            Vector3 postToUse = new Vector3();


            postToUse = posAtLeastx;
            direction = postToUse - anObject.transform.position;
            direction.Normalize();
            aRay = new Ray(postToUse, -1 * direction);
            int castLayer = CamPicTaker.PicTakLayer;
            if (Physics.Raycast(aRay, out rayHit, dist + 0.5f, castLayer))
            {
                posAtLeastx = rayHit.point;
                //  print("Hit " + rayHit.collider.gameObject.name);
            }
            else
            {
                posAtLeastx = anObject.transform.position;
            }

            postToUse = posAtLeasty;
            direction = postToUse - anObject.transform.position;
            direction.Normalize();
            aRay = new Ray(postToUse, -1 * direction);
            if (Physics.Raycast(aRay, out rayHit, dist + 0.5f, castLayer))
            {
                posAtLeasty = rayHit.point;
                //   print("Hit " + rayHit.collider.gameObject.name);
            }
            else
            {
                posAtLeasty = anObject.transform.position;
            }

            postToUse = posAtLeastz;
            direction = postToUse - anObject.transform.position;
            direction.Normalize();
            aRay = new Ray(postToUse, -1 * direction);
            if (Physics.Raycast(aRay, out rayHit, dist + 0.5f, castLayer))
            {
                posAtLeastz = rayHit.point;
                //      print("Hit " + rayHit.collider.gameObject.name);
            }
            else
            {
                posAtLeastz = anObject.transform.position;
            }


            postToUse = posAtMostx;
            direction = postToUse - anObject.transform.position;
            direction.Normalize();
            aRay = new Ray(postToUse, -1 * direction);
            if (Physics.Raycast(aRay, out rayHit, dist + 0.5f, castLayer))
            {
                posAtMostx = rayHit.point;
                //     print("Hit "+ rayHit.collider.gameObject.name);
            }
            else
            {
                posAtMostx = anObject.transform.position;
            }
            postToUse = posAtMosty;
            direction = postToUse - anObject.transform.position;
            direction.Normalize();
            aRay = new Ray(postToUse, -1 * direction);
            if (Physics.Raycast(aRay, out rayHit, dist + 0.5f, castLayer))
            {
                posAtMosty = rayHit.point;
                //     print("Hit " + rayHit.collider.gameObject.name);
            }
            else
            {
                posAtMosty = anObject.transform.position;
            }
            postToUse = posAtMostz;
            direction = postToUse - anObject.transform.position;
            direction.Normalize();
            aRay = new Ray(postToUse, -1 * direction);
            if (Physics.Raycast(aRay, out rayHit, dist + 0.5f, castLayer))
            {
                posAtMostz = rayHit.point;
                //     print("Hit " + rayHit.collider.gameObject.name);
            }
            else
            {
                posAtMostz = anObject.transform.position;
            }

            if (posAtMostx.x >= mostX.x)
            {
                mostX = posAtMostx;
            }
            if (posAtMosty.y >= mostY.y)
            {
                mostY = posAtMosty;
            }
            if (posAtMostz.z >= mostZ.z)
            {
                mostZ = posAtMostx;
            }

            if (posAtLeastx.x <= leastX.x)
            {
                leastX = posAtLeastx;
            }
            if (posAtLeasty.y <= leastY.y)
            {
                leastY = posAtLeasty;
            }
            if (posAtLeastz.z <= leastZ.z)
            {
                leastZ = posAtLeastz;
            }
            Vector3 min = new Vector3();
            min.x = leastX.x - anObject.transform.position.x;
            min.x = leastY.y - anObject.transform.position.y;
            min.x = leastZ.z - anObject.transform.position.z;

            Vector3 max = new Vector3();
            max.x = mostX.x - anObject.transform.position.x;
            max.x = mostY.y - anObject.transform.position.y;
            max.x = mostZ.z - anObject.transform.position.z;
            aBoxCollider.gameObject.transform.localScale = new Vector3(mostX.x - leastX.x + 1, mostY.y - leastY.y + 1, mostZ.z - leastZ.z + 1);

            Vector3 newCenter = new Vector3();
            newCenter.x = leastX.x + aBoxCollider.gameObject.transform.localScale.x * 0.5f;
            newCenter.y = leastY.y + aBoxCollider.gameObject.transform.localScale.y * 0.5f;
            newCenter.z = leastZ.z + aBoxCollider.gameObject.transform.localScale.z * 0.5f;

            aBoxCollider.gameObject.transform.position = newCenter;
        }
    }

}
