using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHover : MonoBehaviour
{
    public RobotLeaderboardRow TheRow;
    public RobotMeta TheMeta;

    // Use this for initialization
    void Start()
    {

    }
    float wait = 0f;
    // Update is called once per frame
    void Update()
    {
        if (wait <= 0)
        {
            wait = 1f;
            if (TheMeta != null)
            {
                TheRow.SetRow(TheMeta);
            }
            gameObject.transform.LookAt(GameObjectFollower.PublicAccess.gameObject.transform);
        }
        wait -= Time.deltaTime;
    }
    

}
