using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ComponentFx : MonoBehaviour
{
    float currentTimeActive = 0;
  public  float TimeToRemain = 100f;

    public bool IsInCache = false;
    public FxType TheFxType;
    public enum FxType
    {
        NotSet,
        BigExplosion,
        MediumExplosion,
        SmallExplosion,
        Smoke,
        WildFire,
        HeavySmoke,
        Sparks,
        Steam,
        LightPoint,
        WhiteSmoke,
        SmallSteam,
        PlasmaExplosion,
        GroundMushroomExplosionBig,
        SmallSparks,
        GroundMushroomExplosionMedium,
        GroundMushroomExplosionSmall,
        GroundMushroomExplosionHuge,
        ShootingStarLikeFireBall,
        BurnoutTyreSmoke,
        TinyExplosion,
        ElectricBall,
        Fireworks,
        ComplexFire,
        SmallFire,
        DrippingFire,
        FlareSparks,
        BlueElectricFire,
        YellowGhostFire,
        FireTorch,
        HeavyFire,
        Dizzy,
        SmallFlareSparks,
        Afterburner,
        SanfswirlFootsteps,
        WoodImpactDecal,
        FleshImpactSmallDecal,
        FleshImpactBigDecal,
        MetalImpactDecal,
        SandImpactDecal,
        StoneImpactDecal,
        WaterLeakImpactDecal,
        BloodLeakSprayEffect,
        WaterSprayHoseDecal,
        WaterSprayHoseMobileDecal,
        MuzzleFlash,
        BloodStreamEffect,






    }

    // Use this for initialization
    void Start()
    {
        StartStopAllParticles(false);
    }
    public void ApplyAtLocation(Vector3 target)
    {
        gameObject.SetActive(true);
         IsInCache = false;
        gameObject.transform.position = target;
        currentTimeActive = 0;

    }
    public void ApplyAttachToTransform(Transform target)
    {
        gameObject.SetActive(true);
        IsInCache = false;
        gameObject.transform.position = target.position;
        gameObject.transform.parent = target;

        currentTimeActive = 0;

    }
    public void ApplyAttachToTransform(Transform target, Vector3 faceDirection)
    {
        gameObject.SetActive(true);
        IsInCache = false;
        gameObject.transform.parent = target;
        gameObject.transform.position = target.position;
        gameObject.transform.forward = faceDirection;
        currentTimeActive = 0;

    }
    
    public void ApplyAttachToTransform(Transform target, Vector3 faceDirection, Vector3 worldPosition)
    {
        gameObject.SetActive(true);
        IsInCache = false;
        gameObject.transform.parent = target;
        gameObject.transform.position = worldPosition;
        gameObject.transform.forward = faceDirection;
        currentTimeActive = 0;

    }
    private void StartStopAllParticles(bool start)
    {
        //todo 
        //List<ParticleSystem> parts = GetAllParticles();
        //print("FOund " + parts.Count + " patrs");
        //for (int c = 0; c < parts.Count; c++)
        //{
        //    if (start)
        //    {
        //        parts[c].Play();

        //    }
        //    else
        //    {
        //        parts[c].Stop();

        //    }
        //}
        //     gameObject.SetActive(start);
    }
    List<ParticleSystem> GetAllParticles()
    {
        List<ParticleSystem> ret = new List<ParticleSystem>();

        ret.AddRange(gameObject.transform.GetComponents<ParticleSystem>());
        ret.AddRange(gameObject.transform.GetComponentsInChildren<ParticleSystem>());

        return ret;
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying == false)
        {
            //     print("DOING ROBOT PART STUFF - SHOULD ONLY HAPPEN IN PAUSE");
            if (gameObject.transform.parent != null)
            {
                int myIndex = gameObject.transform.GetSiblingIndex();
                if (myIndex >= 0)
                {
                    List<ComponentFx> allTypes = new List<ComponentFx>();
                    allTypes.AddRange(gameObject.transform.parent.GetComponentsInChildren<ComponentFx>());

                    int myIndexSorted = 0;
                    for (int c = 0; c < allTypes.Count; c++)
                    {
                        if (string.CompareOrdinal(gameObject.name, allTypes[c].gameObject.name) > 0)
                        {
                            myIndexSorted++;
                        }
                    }

                    gameObject.transform.SetSiblingIndex(myIndexSorted);
                    Vector3 newPos = gameObject.transform.position;
                    newPos.z = 0;
                    newPos.y = gameObject.transform.parent.position.y;
                    newPos.x = myIndexSorted * 7f;
                    gameObject.transform.position = newPos;


                }

            }
        }
        else
        {
            currentTimeActive += Time.deltaTime;
            if (currentTimeActive > TimeToRemain)
            {
                IsInCache = true;
                gameObject.transform.parent = FxCache.PublicAccess.gameObject.transform;
                gameObject.transform.localScale =new Vector3(1, 1, 1);
                gameObject.SetActive(false);
            }

        }
    }
}
