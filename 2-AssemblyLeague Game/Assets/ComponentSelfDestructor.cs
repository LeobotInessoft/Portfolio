using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSelfDestructor : MonoBehaviour
{
    public ComponentFx.FxType DestructMainEffect = ComponentFx.FxType.NotSet;
    public ComponentFx.FxType DestructSMallPartsEffect = ComponentFx.FxType.Steam;
    public float MainExplosionFXTime = 2f;
    public float SecondaryExplosionFXTime = 10f;
    
    public bool MustSelfDestruct = false;
    bool IsBusySelfDestructing = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MustSelfDestruct)
        {
            if (IsBusySelfDestructing == false)
            {

                IsBusySelfDestructing = true;
                Computer aMeta = GetComponent<Computer>();
                if (aMeta == null)
                {
                    DestructAsNormalPart();
                    //   Destroy(gameObject);
                }
                else
                {
                    DestructAsRootPart();
                }
            }
            else
            {

            }
        }
    }
    private void DestructAsRootPart()
    {
        SetAllMeshCollidersToConvex();
         List<Transform> childs = new List<Transform>();
        childs.AddRange(gameObject.transform.GetComponentsInChildren<Transform>());
        List<MeshRenderer> allRenders = new List<MeshRenderer>();
        MeshRenderer thsRender = gameObject.transform.GetComponent<MeshRenderer>();
        if (thsRender != null) allRenders.Add(thsRender);
        allRenders.AddRange(gameObject.transform.GetComponentsInChildren<MeshRenderer>());
   
      
        for (int c = 0; c < childs.Count; c++)
        {
            Vector3 currentWorldPos = childs[c].transform.position;

            childs[c].transform.parent = null;
            childs[c].transform.position = currentWorldPos;
            ExplodeChilds(childs[c]);
        }
      

        ComponentFx aFX2 = FxCache.PublicAccess.GetEffectToApply(DestructMainEffect);
        if (aFX2 != null)
        {
            aFX2.TimeToRemain = MainExplosionFXTime;
            aFX2.ApplyAtLocation(gameObject.transform.position);
        }

        for (int c = 0; c < allRenders.Count; c++)
        {

            ComponentFx aFX = FxCache.PublicAccess.GetEffectToApply(DestructSMallPartsEffect);
            if (aFX != null)
            {
                aFX.TimeToRemain = SecondaryExplosionFXTime;
                aFX.ApplyAttachToTransform(allRenders[c].transform, Vector3.up);
            }
        }
    
       gameObject.SetActive(false);
    }
    private void DestructAsNormalPart()
    {

        SetAllMeshCollidersToConvex(); 
        List<MeshRenderer> allRenders = new List<MeshRenderer>();
        MeshRenderer thsRender = gameObject.transform.GetComponent<MeshRenderer>();
        if (thsRender != null) allRenders.Add(thsRender);
        allRenders.AddRange(gameObject.transform.GetComponentsInChildren<MeshRenderer>());
        
        gameObject.transform.parent = null;
        List<Transform> childs = new List<Transform>();
        childs.Add(gameObject.transform);
        childs.AddRange(gameObject.transform.GetComponentsInChildren<Transform>());
        for (int c = 0; c < childs.Count; c++)
        {
            Vector3 currentWorldPos = childs[c].transform.position;

            childs[c].transform.parent = null;
            childs[c].transform.position = currentWorldPos;
            ExplodeChilds(childs[c]);
        }
        ComponentFx aFX2 = FxCache.PublicAccess.GetEffectToApply(DestructMainEffect);
        if (aFX2 != null)
        {
            aFX2.TimeToRemain = MainExplosionFXTime;
            aFX2.ApplyAtLocation(gameObject.transform.position);
        }
        for (int c = 0; c < allRenders.Count; c++)
        {
            ComponentFx aFX = FxCache.PublicAccess.GetEffectToApply(DestructSMallPartsEffect);
            if (aFX != null)
            {
                aFX.TimeToRemain = SecondaryExplosionFXTime;
                aFX.ApplyAttachToTransform(allRenders[c].transform, Vector3.up);


            }
        }
      


    }
    private void SetAllMeshCollidersToConvex()
    {
        List<MeshCollider> allRenders = new List<MeshCollider>();
        MeshCollider thsRender = gameObject.GetComponent<MeshCollider>();
        if (thsRender != null) allRenders.Add(thsRender);
        allRenders.AddRange(gameObject.transform.GetComponentsInChildren<MeshCollider>());
        for (int c = 0; c < allRenders.Count; c++)
        {
            allRenders[c].convex = true;
        }
    }
    private void ExplodeChilds(Transform aTrans)
    {
        MeshCollider aMeshCollider = aTrans.gameObject.GetComponent<MeshCollider>();
        Collider aCollider = aTrans.gameObject.GetComponent<Collider>();
        if (aCollider == null)
        {

            aTrans.gameObject.name += "NO COLIDER";
        }
        if (aMeshCollider != null)
        {
            aMeshCollider.convex = true;
        }
        Rigidbody aRig = aTrans.GetComponent<Rigidbody>();
        if (aRig == null) aRig = aTrans.gameObject.AddComponent<Rigidbody>();
        aTrans.gameObject.AddComponent<DestroyIfBelowGround>();

        ComponentType aTypeChild = aTrans.GetComponent<ComponentType>();
        if (aTypeChild != null)
        {
            aTypeChild.MustSelfDestruct = true;

           
        }



    }


}
