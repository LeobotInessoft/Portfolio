using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxTester : MonoBehaviour
{
    public Transform TetTarget;
    public ComponentFx.FxType TypeToTest;
    public bool DoTestAtPos = false;
    public bool DoTestAtTransform = false;
    public bool DoTestAtTransformAtAngle = false;
    public bool DoTestAtTransformAtAngleOfRay = false;
    public bool DoTestAtTransformAtAngleOfRayAtHit = false;
    float waitTime = 0;
    float delay = 10f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DoTestAtPos)
        {
            waitTime -= Time.deltaTime;
            // if (waitTime <= 0)
            {
                waitTime = delay;
                ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(TypeToTest);
                aComp.gameObject.SetActive(true);
           
                aComp.ApplyAtLocation(TetTarget.position);
                  DoTestAtPos = false;
            }

        }
        if (DoTestAtTransform)
        {
            waitTime -= Time.deltaTime;
            // if (waitTime <= 0)
            {
                waitTime = delay;
                ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(TypeToTest);
                aComp.gameObject.SetActive(true);

                aComp.ApplyAttachToTransform(TetTarget);
                DoTestAtTransform = false;
            }

        }
        if (DoTestAtTransformAtAngle)
        {
            waitTime -= Time.deltaTime;
            // if (waitTime <= 0)
            {
                waitTime = delay;
                ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(TypeToTest);
                aComp.gameObject.SetActive(true);

                aComp.ApplyAttachToTransform(TetTarget, TetTarget.forward);
                DoTestAtTransformAtAngle = false;
            }

        }
        if (DoTestAtTransformAtAngleOfRay)
        {
            waitTime -= Time.deltaTime;
            // if (waitTime <= 0)
            {
                waitTime = delay;
                RaycastHit aHit;
                Vector3 dir= TetTarget.position- gameObject.transform.position;
                dir.Normalize();
                Ray aray= new Ray(gameObject.transform.position, dir);
                if (Physics.Raycast(aray, out aHit))
                {

                    ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(TypeToTest);
                    aComp.gameObject.SetActive(true);

                    aComp.ApplyAttachToTransform(TetTarget, -1* dir);
                    DoTestAtTransformAtAngleOfRay = false;
                }
            }

        }
        if (DoTestAtTransformAtAngleOfRayAtHit)
        {
            waitTime -= Time.deltaTime;
            // if (waitTime <= 0)
            {
                waitTime = delay;
                RaycastHit aHit;
                Vector3 dir = TetTarget.position - gameObject.transform.position;
                dir.Normalize();
                Ray aray = new Ray(gameObject.transform.position, dir);
                if (Physics.Raycast(aray, out aHit))
                {

                    ComponentFx aComp = FxCache.PublicAccess.GetEffectToApply(TypeToTest);
                    aComp.gameObject.SetActive(true);

                    aComp.ApplyAttachToTransform(TetTarget, -1 * dir, aHit.point);
                    DoTestAtTransformAtAngleOfRayAtHit = false;
                }
            }

        }
    }
}
