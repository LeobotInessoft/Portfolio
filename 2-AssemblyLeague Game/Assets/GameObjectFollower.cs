using UnityEngine;
using System.Collections;

public class GameObjectFollower : MonoBehaviour
{
    public bool AutoFollowKillerOnDeath = true;
    public FollowCam FollowCam;
    public OrbitCam OrbitCam;
    public DragMouseOrbit MouseOrbitCam;
    public FreeCam FreeCam;
    public MouseLook MouseLook;
    public MouseScrollZoom MouseZoom;
    public float DefaultMouseZoom = -8;
    public void SwitchToCam(EnumFollowType followType)
    {
        FollowType = followType;
        switch (FollowType)
        {
            case EnumFollowType.Behind:
                {
                    MouseZoom.RestoreFoV();
                    if (MouseZoom.locPos.z == 0) MouseZoom.locPos.z = DefaultMouseZoom;
                    MouseLook.enabled = false;
                    MouseOrbitCam.enabled = false;
                    FollowCam.enabled = true;
                    OrbitCam.enabled = false;
                    FreeCam.enabled = false;

                    break;
                }
            case EnumFollowType.FreeCam:
                {
                    MouseZoom.RestoreFoV();
                    DefaultMouseZoom = MouseZoom.locPos.z;
                    MouseZoom.locPos = new Vector3();
                    MouseZoom.Mover.localPosition = MouseZoom.locPos;

                    MouseLook.enabled = true;
                    MouseOrbitCam.enabled = false;
                    FollowCam.enabled = false;
                    OrbitCam.enabled = false;
                    FreeCam.enabled = true;

                    break;
                }
            case EnumFollowType.Orbit:
                {
                    MouseZoom.RestoreFoV();
                    if (MouseZoom.locPos.z == 0) MouseZoom.locPos.z = DefaultMouseZoom;
                    MouseLook.enabled = false;
                    MouseOrbitCam.enabled = true;
                    FollowCam.enabled = false;
                    OrbitCam.enabled = true;
                    FreeCam.enabled = false;

                    break;
                }
        }
    }
    public void SwitchToRoboto(RobotMeta aMeta)
    {
        for (int c = 0; c < Match.PublicAccess.AllSpawnedRobots.Count; c++)
        {
            RobotMeta tmp = Match.PublicAccess.AllSpawnedRobots[c].GetComponent<RobotMeta>();
            if (tmp != null)
            {
                if (tmp.RuntimeRobotID == aMeta.RuntimeRobotID)
                {
                    print("Found");
                    GameObjectToFollow = tmp.gameObject;

                    break;
                }
            }
        }

     
    }
    public enum EnumFollowType
    {
        Behind,
        Top,
        Orbit,
        FreeMouse,
        FreeCam
    }
    public EnumFollowType FollowType = EnumFollowType.Behind;
    public float Setting_ExtraForwardDistance = 2f;
    public float Setting_ExtraUpwardDistance = 2f;
    Vector3 lastCamPostSync = Vector3.zero;
    public GameObject GameObjectToFollow;
    public GameObject GameObjectToFollow222;
    float timeUntilChange = 3;
    float Setting_MaxDistance = 0.002f;
    public float Setting_YHeight = 2;
    public float CamChangeSpeed = 1;
    public static GameObjectFollower PublicAccess;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
        AutoFollowKillerOnDeath = true;
    }
    public void SetGameObjectToFollow(GameObject gameObjectToFollow)
    {
        GameObjectToFollow = gameObjectToFollow;
    }
    public void StopFollowingGameObject()
    {
        GameObjectToFollow = null;
    }
    private void SpiceUpSettings()
    {
        Setting_MaxDistance = Random.Range(26.1f, 36);
        Setting_YHeight = Random.Range(5.5f, 8.5f);
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    private void FocusCameraOnNextSelected()
    {
        if (gameObject != null && GameObjectToFollow != null)
        {
            Setting_MaxDistance += 0.05f;
            Vector3 nxtPost = GameObjectToFollow.transform.position + GameObjectToFollow.transform.forward * 0.7f;
            float distanctFromCam = Vector3.Distance(gameObject.transform.position, GameObjectToFollow.transform.position);

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
                    //if(distanctFromCam>10)
                    {

                        nxtPost = Vector3.Lerp(gameObject.transform.position, nxtPost, 0.025f * Time.deltaTime * 100f * CamChangeSpeed);
                        gameObject.transform.position = nxtPost;
                    }
                }
                gameObject.transform.LookAt(GameObjectToFollow.transform.position);
            }
            if (IsFocussed)
            {
                gameObject.transform.LookAt(GameObjectToFollow.transform.position + GameObjectToFollow.transform.forward * Setting_ExtraForwardDistance + GameObjectToFollow.transform.up * Setting_ExtraUpwardDistance);
            
            }
        }

    }
    float lookSPeed = 0.5f;
    private void Lookat(Vector3 Point)
    {
        Vector3 direction = Point - transform.position;
        direction.Normalize();
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lookSPeed * Time.time);
    }
}
