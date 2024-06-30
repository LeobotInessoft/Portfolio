using UnityEngine;
using System.Collections;

public class RobotPart : MonoBehaviour
{
    
    public Computer MyComputer;
    public RobotMeta aPartMeta;
    // Use this for initialization
    void Start()
    {
        aPartMeta = gameObject.GetComponent<RobotMeta>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

  


}
