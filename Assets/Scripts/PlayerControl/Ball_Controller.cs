using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Controller : MonoBehaviour {
    
    private Transform tran;
    public GameObject ship;
    public int cameraTurnSpeed = 1;

    void Start()
    {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
    }

    void FixedUpdate()
    {
        tran.position = ship.transform.position;
        //Debug.DrawLine(tran.position, tran.position + tran.forward*10);

        RaycastHit leftSide;
        RaycastHit rightSide;

        Physics.Raycast(tran.position, tran.right * -1, out leftSide);
        Physics.Raycast(tran.position, tran.right, out rightSide);

        bool castLeft  = Physics.Raycast(tran.position, tran.forward - tran.right, 25);
        bool castMid   = Physics.Raycast(tran.position, tran.forward             , 25);
        bool castRight = Physics.Raycast(tran.position, tran.forward + tran.right, 25);

        if (castLeft && castMid && castRight)
        {
            //Debug.Log("OH GOD A WALL!");
        }
        else if (castLeft && castMid)
        {
            tran.Rotate(new Vector3(0, cameraTurnSpeed * Time.fixedDeltaTime * 5));
        }
        else if (castRight && castMid)
        {
            tran.Rotate(new Vector3(0, -cameraTurnSpeed * Time.fixedDeltaTime * 5));
        }
        else if (castLeft)
        {
            tran.Rotate(new Vector3(0, cameraTurnSpeed * Time.fixedDeltaTime));
        }
        else if (castRight)
        {
            tran.Rotate(new Vector3(0, -cameraTurnSpeed * Time.fixedDeltaTime));
        }

    }

}
