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
        Vector3 diff = tran.position - ship.transform.position;

        Vector3 move = (Vector3.Dot(tran.forward, diff) / tran.forward.sqrMagnitude) * tran.forward;

        tran.position = tran.position - move;

        //Debug.DrawLine(tran.position, tran.position + tran.forward*10);

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
