using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class WeaponConstMod : MonoBehaviour
{
    public GameObject Prefab_MuzzleFlash;

    public List<IoWeapon> currentWeapons = new List<IoWeapon>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentWeapons = new List<IoWeapon>();
        currentWeapons.AddRange(gameObject.transform.GetComponentsInChildren<IoWeapon>());
        for (int c = 0; c < currentWeapons.Count; c++)
        {
            for (int x = 0; x < currentWeapons[c].BulletExitPoints.Count; x++)
            {
                ParticleSystem partSystem = currentWeapons[c].BulletExitPoints[x].gameObject.transform.GetComponentInChildren<ParticleSystem>();
                if (partSystem == null)
                {
                    GameObject aNew = Instantiate(Prefab_MuzzleFlash, currentWeapons[c].BulletExitPoints[x].gameObject.transform.position, currentWeapons[c].BulletExitPoints[x].gameObject.transform.rotation, currentWeapons[c].BulletExitPoints[x].gameObject.transform);

                }
            }

        }
    }
}
