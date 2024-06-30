using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCam : MonoBehaviour {
    //  public GameObject player;
    public Vector3 offset;
    public float smoothSpeed;
    // Use this for initialization
    public float Setting_ExtraForwardDistance = 2f;
    public float Setting_ExtraUpwardDistance = 2f;
    Vector3 lastCamPostSync = Vector3.zero;
    float timeUntilChange = 3;
    float Setting_MaxDistance = 0.002f;
    public float Setting_YHeight = 2;
    public float CamChangeSpeed = 1;
    public static GameObjectFollower PublicAccess;
    public float RotateSpeed = 10f;
    void Start()
    {

    }
    void Update()
    {
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * RotateSpeed);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObjectFollower.PublicAccess.GameObjectToFollow != null)
        {
            FocusCameraOnNextSelected();

        }
    }
    private void FocusCameraOnNextSelected()
    {
        if (gameObject != null && GameObjectFollower.PublicAccess.GameObjectToFollow != null)
        {
            Setting_MaxDistance += 0.05f;
            Vector3 nxtPost = GameObjectFollower.PublicAccess.GameObjectToFollow.transform.position + GameObjectFollower.PublicAccess.GameObjectToFollow.transform.forward * 0.7f;
            float distanctFromCam = Vector3.Distance(gameObject.transform.position, GameObjectFollower.PublicAccess.GameObjectToFollow.transform.position);

            bool IsFocussed = false;
            if (distanctFromCam < Setting_MaxDistance)
            {
                IsFocussed = true;
            }

            {

                nxtPost.y += Setting_YHeight + 2.8312f;
                if (distanctFromCam > 50)
                {
                    nxtPost = Vector3.Lerp(gameObject.transform.position, nxtPost, 0.185f * Time.deltaTime * 10f * CamChangeSpeed);
                    gameObject.transform.position = nxtPost;
                  
                }
                else
                {
                     {

                        nxtPost = Vector3.Lerp(gameObject.transform.position, nxtPost, 0.025f * Time.deltaTime * 100f * CamChangeSpeed);
                        gameObject.transform.position = nxtPost;
                    }
                }
              }
           
        }

    }
}
