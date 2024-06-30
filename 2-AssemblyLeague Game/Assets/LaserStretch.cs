using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStretch : MonoBehaviour
{
    public float WantedLength;
    public bool IsEnabled;
    public Transform ObjectToStrectch;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnabled)
        {
            ForceSetLength();
        }
    }
    public void ForceSetLength()
    {
        Vector3 tmp = ObjectToStrectch.transform.localScale;
        tmp.z = WantedLength;
        ObjectToStrectch.transform.localScale = tmp;

    }
}
