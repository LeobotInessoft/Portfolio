using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {

    public Transform Target;
    public IOHandler MyIoHandler;
    public Vector3 LastLookAtPos;
    // Use this for initialization
    void Start()
    {
        MyIoHandler.IoHandler += new IOHandler.DelegateHandleIO(MyIoHandler_IoHandler);
        LastLookAtPos = transform.position;
    }

    void MyIoHandler_IoHandler(ref Computer.StandardStack runtimeStack)
    {
       
        Vector3 worldPos = Target.position;

        LastLookAtPos = Vector3.Lerp(LastLookAtPos, worldPos, 0.01f);


        gameObject.transform.LookAt(LastLookAtPos);
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
