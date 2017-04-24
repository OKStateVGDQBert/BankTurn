using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This camera control FAR from perfect, but frankly I don't have much time/manpower to fix it.

public class Ball_Controller : MonoBehaviour {
    
    private Transform tran;
    private GameObject player;
    private int cameraTurnSpeed = 1;
    private bool castedLeft = false;
    private bool castedRight = false;

    public GameObject[] playerModels;

    void Start()
    {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
        player = Instantiate(playerModels[Data_Manager.shipType], tran.position, tran.rotation);
        player.transform.SetParent(tran);
        switch (Data_Manager.shipType)
        {
            case 0:
                cameraTurnSpeed = 5;
                break;
            case 1:
                cameraTurnSpeed = 4;
                break;
            default:
                cameraTurnSpeed = 3;
                break;
        }
    }

    void FixedUpdate()
    {
        Vector3 diff = tran.position - player.transform.position;

        Vector3 move = (Vector3.Dot(tran.forward, diff) / tran.forward.sqrMagnitude) * tran.forward;

        tran.position = tran.position - move;

        //Debug.DrawLine(tran.position, tran.position + tran.forward*10);

        bool castLeft  = Physics.Raycast(tran.position, tran.forward - tran.right, 25, 1 << 8);
        bool castMid   = Physics.Raycast(tran.position, tran.forward             , 50, 1 << 8);
        bool castRight = Physics.Raycast(tran.position, tran.forward + tran.right, 25, 1 << 8);

        if (castLeft && castMid && castRight)
        {
            ResetCameraPosition();
            //Debug.Log("OH GOD A WALL!");
        }
        else if (castLeft && castMid)
        {
            Quaternion newRot = Quaternion.Euler(0, 45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * cameraTurnSpeed / 2.0f);
            //tran.Rotate(new Vector3(0, cameraTurnSpeed * Time.fixedDeltaTime * 5));
        }
        else if (castRight && castMid)
        {
            Quaternion newRot = Quaternion.Euler(0, -45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * cameraTurnSpeed / 2.0f);
            //tran.Rotate(new Vector3(0, -cameraTurnSpeed * Time.fixedDeltaTime * 5));
        }
        else if (castLeft && castedLeft)
        {
            Quaternion newRot = Quaternion.Euler(0, 45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * cameraTurnSpeed / 4.0f);
            //tran.Rotate(new Vector3(0, cameraTurnSpeed * Time.fixedDeltaTime));
        }
        else if (castRight && castedRight)
        {
            Quaternion newRot = Quaternion.Euler(0, -45, 0);
            tran.rotation = Quaternion.Lerp(tran.rotation, tran.rotation * newRot, Time.fixedDeltaTime * cameraTurnSpeed / 4.0f);
            //tran.Rotate(new Vector3(0, -cameraTurnSpeed * Time.fixedDeltaTime));
        }

        castedLeft = castLeft;
        castedRight = castRight;

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 8)
        {
            ResetCameraPosition();
        }
    }

    void ResetCameraPosition()
    {
		tran.position = player.transform.position;
        tran.rotation = player.transform.rotation;
    }

}
