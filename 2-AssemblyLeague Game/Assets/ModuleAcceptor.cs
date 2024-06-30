using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class ModuleAcceptor : MonoBehaviour
{
    public bool HasAttachment = false;
    public string UniqueDeviceID;
    public List<ComponentType.EnumComponentType> AllowedTypes;
    public Transform PivotParent;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying == false)
        {
            if (UniqueDeviceID == null || UniqueDeviceID.Length <= 2)
            {
                UniqueDeviceID = System.Guid.NewGuid().ToString();
            }
            if (gameObject.transform.parent != null)
            {
                if (gameObject.transform.parent.gameObject.name.Contains("_PIVOT"))
                {
                    PivotParent = gameObject.transform.parent;
                }
                else
                {
                    PivotParent = null;

                }
            }

            if (PivotParent == null)
            {
                if (gameObject.transform.lossyScale.x == -1)
                {

                    GameObject spwn = (new GameObject());
                    spwn.name = gameObject.name + "_PIVOT";
                    spwn.transform.parent = gameObject.transform.parent;
                    spwn.transform.localPosition = gameObject.transform.localPosition;
                    PivotParent = spwn.transform;
                    gameObject.transform.parent = spwn.transform;
                    gameObject.transform.localPosition = Vector3.zero;
                    // Destroy(this, 1);


                }
                else
                {
                    PivotParent = gameObject.transform;


                   

                }
            }

        }
        else
        {
             enabled = false;
        }

       
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
