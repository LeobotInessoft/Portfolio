using UnityEngine;
using System.Collections;

public class BallControll : MonoBehaviour {
    public IOHandler MyIoHandler;
	// Use this for initialization
	void Start () {
        MyIoHandler.IoHandler += new IOHandler.DelegateHandleIO(MyIoHandler_IoHandler);
	}

    void MyIoHandler_IoHandler(ref Computer.StandardStack runtimeStack)
    {
        if (int.Parse(runtimeStack.Ax.Val) >= 0)
        {
            gameObject.transform.Translate(Vector3.up * 0.1f, Space.Self);
        }
        else
        {
            gameObject.transform.Translate(Vector3.up *-0.1f, Space.Self);
        
        }
     }
	
	// Update is called once per frame
	void Update () {
	
	}
}
