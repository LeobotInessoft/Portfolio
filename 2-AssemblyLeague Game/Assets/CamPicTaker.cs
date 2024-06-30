using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPicTaker : MonoBehaviour
{
    public bool MustTakePic = false;
    public Camera TheCamera;
    public RenderTexture TheRenderTexture;
    public GameObject Target;
    public bool LookAtWhenTakingPic = true;
    public static readonly int PicTakLayer = 8;
    public bool BetCubeGroupAsPic;

    public float ExtraForward = 5.5f;
    public float ExtraRight = 1.5f;
    public float ExtraTop = 1.5f;
    public string picName = "pic";
    public Transform CameraLocationToTakePicFrom;
    public GameObject aBoxCollider;

   
    // Use this for initialization
    void Start()
    {

    }
    public void DoTakePicLogic()
    {
        if (Target != null)
        {
            if (BetCubeGroupAsPic)
            {
                List<MeshCollider> allTrans = new List<MeshCollider>();
                MeshCollider myCol = Target.transform.GetComponent<MeshCollider>();
                if (myCol != null)
                {
                    allTrans.Add(myCol);
                }
                allTrans.AddRange(Target.transform.GetComponentsInChildren<MeshCollider>());

                MakeBoxColliderFit(Target);

                aBoxCollider.gameObject.transform.forward = Target.transform.forward;

                Vector3 pos = aBoxCollider.gameObject.transform.position + Target.transform.forward * ExtraForward + Target.transform.right * ExtraRight + Target.transform.up * ExtraTop;
                if (CameraLocationToTakePicFrom != null)
                {
                    pos = CameraLocationToTakePicFrom.transform.position;
                }
                else
                {
                    BoxCollider aBox = aBoxCollider.GetComponent<BoxCollider>();
                    pos = aBox.ClosestPoint(pos);
                    pos = pos + Target.transform.forward * aBoxCollider.transform.localScale.y + Target.transform.forward * ExtraForward + Target.transform.right * ExtraRight + Target.transform.up * ExtraTop;
                }
                gameObject.transform.position = pos;
                if (LookAtWhenTakingPic)
                {
                    gameObject.transform.LookAt(aBoxCollider.gameObject.transform.position);
                }
            }
            else
            {
                Vector3 pos = Target.gameObject.transform.position + Target.transform.forward * ExtraForward + Target.transform.right * ExtraRight + Target.transform.up * ExtraTop;
                gameObject.transform.position = pos;
                aBoxCollider.gameObject.transform.position = Target.gameObject.transform.position;
                aBoxCollider.gameObject.transform.forward = Target.transform.forward;
                if (LookAtWhenTakingPic)
                {
                    gameObject.transform.LookAt(Target.gameObject.transform.position);
                }
            }
            if (MustTakePic)
            {
                MustTakePic = false;
                if (LookAtWhenTakingPic)
                {
                    gameObject.transform.LookAt(aBoxCollider.gameObject.transform.position);
                }
                MakeSquarePngFromOurVirtualThingy();
            }
        }
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
        int castLayer = PicTakLayer;
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
    // Update is called once per frame
    void Update()
    {

        DoTakePicLogic();
    }


    public void MakeSquarePngFromOurVirtualThingy()
    {

        int defLayer = 0;

        List<Transform> allTrans = new List<Transform>();
        allTrans.Add(Target.transform);
        allTrans.AddRange(Target.transform.GetComponentsInChildren<Transform>());
        for (int c = 0; c < allTrans.Count; c++)
        {
            GameObject theObj = allTrans[c].gameObject;
            if (theObj != null)
            {
                theObj.layer = PicTakLayer;

            }
        }

        try
        {
            MakeBoxColliderFit(Target);

            TheCamera.enabled = true;
           
            int sqr = 512;

            
            RenderTexture tempRT = new RenderTexture(sqr, sqr, 24);
           
            TheCamera.targetTexture = tempRT;
            TheCamera.Render();

            RenderTexture.active = tempRT;
            Texture2D virtualPhoto =
                new Texture2D(sqr, sqr, TextureFormat.RGB24, false);
            // false, meaning no need for mipmaps
            virtualPhoto.ReadPixels(new Rect(0, 0, sqr, sqr), 0, 0);

            RenderTexture.active = null; //can help avoid errors 
            TheCamera.targetTexture = TheRenderTexture;
           
            byte[] bytes;
            bytes = virtualPhoto.EncodeToPNG();

            System.IO.File.WriteAllBytes(OurTempSquareImageLocation(), bytes);
            
            TheCamera.enabled = false;


        }
        catch
        {
            TheCamera.enabled = false;


        }
        for (int c = 0; c < allTrans.Count; c++)
        {
            GameObject theObj = allTrans[c].gameObject;
            if (theObj != null)
            {
                theObj.layer = 0;

            }
        }
    }

    public byte[] GetLastPicBytes()
    {
        byte[] ret = new byte[0];

        string fileName = OurTempSquareImageLocation();
        if (System.IO.File.Exists(fileName))
        {
            ret = System.IO.File.ReadAllBytes(fileName);
        }
        return ret;
    }
    public string OurTempSquareImageLocation()
    {
        string r = Application.persistentDataPath + "/" + picName + ".png";
        print("PIC Taken AT: " + r);
        return r;
    }
}
