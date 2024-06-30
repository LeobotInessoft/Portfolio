using UnityEngine;
using System.Collections;

public class Arena : MonoBehaviour
{
    public Terrain Therrain;
    public static Arena PublicAccess;
    public SpawnPlane SpawnerPlane;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Vector3 FindASpawnPosition(int robotSlot, int totalRobots)
    {
        Vector3 ret = new Vector3();
        ret = gameObject.transform.position;
        float range = 100;
        Transform aTrans = SpawnerPlane.GetSpawnPosition(robotSlot, totalRobots);
        if (aTrans == null)
        {
            if (Therrain == null)
            {
                ret.x = Random.Range(-range, range);
                ret.z = Random.Range(-range, range);
                ret.y = 450f;
            }
            else
            {
                ret.x = Random.Range(-range, range);
                ret.z = Random.Range(-range, range);
                ret.y = 450f;

            }
        }
        else
        {
            ret = aTrans.position;
        }
        RaycastHit aHit;

        if (Physics.Raycast(ret, Vector3.down, out aHit))
        {
            ret = aHit.point;
            ret.y += 1f; ;
        }

        return ret;

    }
}
