using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This camera control FAR from perfect, but frankly I don't have much time/manpower to fix it.

public class Ball_Controller : MonoBehaviour {
    
    private Transform tran;
    public GameObject ship;
    public int cameraTurnSpeed = 1;

    private bool castedLeft = false;
    private bool castedRight = false;

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
        bool castMid   = Physics.Raycast(tran.position, tran.forward             , 50);
        bool castRight = Physics.Raycast(tran.position, tran.forward + tran.right, 25);

        if (castLeft && castMid && castRight)
        {
            //Debug.Log("OH GOD A WALL!");
        }
        else if (castLeft && castMid)
        {
            Quaternion newRot = Quaternion.Euler(0, 45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * 1.5f);
            //tran.Rotate(new Vector3(0, cameraTurnSpeed * Time.fixedDeltaTime * 5));
        }
        else if (castRight && castMid)
        {
            Quaternion newRot = Quaternion.Euler(0, -45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * 1.5f);
            //tran.Rotate(new Vector3(0, -cameraTurnSpeed * Time.fixedDeltaTime * 5));
        }
        else if (castLeft && castedLeft)
        {
            Quaternion newRot = Quaternion.Euler(0, 45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * 0.2f);
            //tran.Rotate(new Vector3(0, cameraTurnSpeed * Time.fixedDeltaTime));
        }
        else if (castRight && castedRight)
        {
            Quaternion newRot = Quaternion.Euler(0, -45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * 0.2f);
            //tran.Rotate(new Vector3(0, -cameraTurnSpeed * Time.fixedDeltaTime));
        }

        castedLeft = castLeft;
        castedRight = castRight;

    }

}
