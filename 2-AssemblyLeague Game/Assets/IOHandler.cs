using UnityEngine;
using System.Collections;
 [ExecuteInEditMode]
public class IOHandler : MonoBehaviour
{
    public delegate void DelegateHandleIO(ref Computer.StandardStack runtimeStack);
    public event DelegateHandleIO IoHandler;
    public string UniqueDeviceID;
    public int IONumber;
    public string DeviceName;
    public string DeviceDescription;
    public string DeviceUsage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UniqueDeviceID == null || UniqueDeviceID.Length <= 2)
        {
            UniqueDeviceID = System.Guid.NewGuid().ToString();
        }
    }
    public virtual void HandleIO(Computer.Program parentProgram, ref Computer.StandardStack runtimeStack, RobotPart part)
    {
       
        if (IoHandler != null)
        {
            IoHandler(ref runtimeStack);
        }
    }

}
