using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float ExtraBoost = 2;
    public MouseScrollZoom MyScroll;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 relative = transform.InverseTransformDirection(gameObject.transform.forward);
            float boost = 1;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                boost = ExtraBoost;
            }
            gameObject.transform.Translate(boost * relative * MoveSpeed * Time.deltaTime * 1, Space.Self);

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 relative = transform.InverseTransformDirection(gameObject.transform.forward);
            float boost = 1;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                boost = ExtraBoost;
            }
            gameObject.transform.Translate(boost * relative * MoveSpeed * Time.deltaTime * -1, Space.Self);

        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 relative = transform.InverseTransformDirection(gameObject.transform.right);
            float boost = 1;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                boost = ExtraBoost;
            }
            gameObject.transform.Translate(boost * relative * MoveSpeed * Time.deltaTime * -1, Space.Self);

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 relative = transform.InverseTransformDirection(gameObject.transform.right);
            float boost = 1;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                boost = ExtraBoost;
            }
            gameObject.transform.Translate(boost * relative * MoveSpeed * Time.deltaTime * 1, Space.Self);

        }
    }
}
