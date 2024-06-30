using UnityEngine;
using System.Collections;

public class CharacterFollower : MonoBehaviour
{
    // public GameObject Target
    public static CharacterFollower PublicAccess;
    public Transform Target;
    public Vector3 LocalAd;
    public bool hasDirectLineOfSight = true;
    Vector3 pointToLookAt = Vector3.zero;
    public float CamSpeed = 1f;

    Vector3 lastPointWhereIHadVision;
    void Start()
    {
        LocalAd = new Vector3(0, 4, 0);
        PublicAccess = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (Target != null)
        {
            Vector3 dirTo = Target.position + LocalAd -gameObject.transform.position;

            Vector3 dirTrue = Target.position - gameObject.transform.position;
            float distanceTo = Vector3.Distance(gameObject.transform.position, Target.position);
            dirTo.Normalize();
            dirTrue.Normalize();
            float dist = Vector3.Distance(Target.position + LocalAd, gameObject.transform.position);
            
            {
                if (dist > 12)
                {
                    gameObject.transform.Translate(dirTo * Time.deltaTime * 5 * CamSpeed, Space.World);
                }
                pointToLookAt = Vector3.Lerp(pointToLookAt, Target.position + Vector3.up * 0.5f + Target.forward * 0.5f, 0.08f);
                // gameObject.transform.LookAt(Target);
                gameObject.transform.LookAt(pointToLookAt);
            }

            RaycastHit aHit;

            Ray aRay = new Ray(gameObject.transform.position, dirTrue);
            Debug.DrawRay(aRay.origin, aRay.direction);
            if (Physics.Raycast(aRay, out aHit, distanceTo))
            {
                ComponentType aChar = aHit.collider.gameObject.GetComponentInChildren<ComponentType>();
                if (aChar != null)
                {
                    hasDirectLineOfSight = true;
                }
                else
                {
                    hasDirectLineOfSight = false;
                }
            }
            else
            {
                hasDirectLineOfSight = true;
            }

            if (hasDirectLineOfSight)
            {
                lastPointWhereIHadVision = Target.position;
            }
            else
            {
                if (lastPointWhereIHadVision != Vector3.zero)
                {
                    Vector3 safeDir = lastPointWhereIHadVision + LocalAd * 0.5f - gameObject.transform.position;
                    safeDir.Normalize();
                    float distSafe = Vector3.Distance(gameObject.transform.position, Target.position);
                    if (distSafe >= 2)
                    {
                        gameObject.transform.Translate(safeDir * Time.deltaTime * 8, Space.World);

                    }
                }
            }
        }

    }
}
