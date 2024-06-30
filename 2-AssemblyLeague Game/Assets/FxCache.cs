using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxCache : MonoBehaviour
{
    public Transform BaseContainer;
   
    public Dictionary<ComponentFx.FxType, List<ComponentFx>> AllSpawnedBullets;
    public static FxCache PublicAccess;
    public int DefaultNumberOfBullets = 256;
    //   public List<Bullet> AllBullets;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        AllSpawnedBullets = new Dictionary<ComponentFx.FxType, List<ComponentFx>>();
        GenerateCache(DefaultNumberOfBullets);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateCache(int size)
    {
        List<ComponentFx> allComponentsBelow = new List<ComponentFx>();
        allComponentsBelow.AddRange(BaseContainer.transform.GetComponentsInChildren<ComponentFx>());
        for (int c = 0; c < allComponentsBelow.Count; c++)
        {
            if (allComponentsBelow[c].TheFxType != ComponentFx.FxType.NotSet)
            {
                List<ComponentFx> bullets = CreateCacheList(allComponentsBelow[c].gameObject, size);
                AllSpawnedBullets.Add(allComponentsBelow[c].TheFxType, bullets);
            }
        }




    }
    public void GenerateCache(int size, ComponentFx.FxType aType)
    {

        List<ComponentFx> allComponentsBelow = new List<ComponentFx>();
        if (BaseContainer != null)
        {
            allComponentsBelow.AddRange(BaseContainer.transform.GetComponentsInChildren<ComponentFx>());
            for (int c = 0; c < allComponentsBelow.Count; c++)
            {
                if (allComponentsBelow[c].TheFxType == aType)
                {
                    List<ComponentFx> bullets = CreateCacheList(allComponentsBelow[c].gameObject, size);
                    AllSpawnedBullets[allComponentsBelow[c].TheFxType].AddRange(bullets);

                }
            }
        }

    }



    private List<ComponentFx> CreateCacheList(GameObject aBullet, int size)
    {
        List<ComponentFx> ret = new List<ComponentFx>();
        if (aBullet != null)
        {
           for (int c = 0; c < size; c++)
            {
                GameObject inst = Instantiate(aBullet, Vector3.zero, Quaternion.identity, this.transform);
                inst.transform.parent = gameObject.transform;
                ComponentFx aBull = inst.GetComponent<ComponentFx>();
                aBull.IsInCache = true;
                inst.SetActive(false);
                ret.Add(aBull);
            }
        }
        return ret;
    }

    public ComponentFx GetEffectToApply(ComponentFx.FxType aType)
    {
        ComponentFx ret = null;
        if (aType != ComponentFx.FxType.NotSet)
        {
            List<ComponentFx> options = AllSpawnedBullets[aType];

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
                ret = GetEffectToApply(aType);
            }
        }
        return ret;
    }
}
