using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
    
    private Transform tran;
    public int turnSpeed = 1;
	public float maxY = 5.0f;
	public float minY = 5.0f;
    public int forwardSpeed = 1;
    private bool underPlayerControl = false;

    void Start () {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
    }
	
	void FixedUpdate ()
    {
        if (underPlayerControl)
        {
            // Rotate the ship
            tran.RotateAround(tran.position, tran.up, turnSpeed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);

            // Now move the ship in the direction of the current rotation.
            tran.position = tran.position + tran.forward * forwardSpeed * Time.fixedDeltaTime;

            Vector3 yComp = Vector3.zero;

            if (Input.GetAxis("Vertical") > 0)
            {
                if (tran.position.y < maxY)
                {
                    yComp = new Vector3(0, Input.GetAxis("Vertical") * forwardSpeed * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (tran.position.y > minY)
                {
                    yComp = new Vector3(0, Input.GetAxis("Vertical") * forwardSpeed * Time.fixedDeltaTime);
                }
            }
            tran.position = tran.position + yComp;
        }
        else
        {
            if (Time.realtimeSinceStartup > 30)
            {
                underPlayerControl = true;
                tran.SetParent(null);
            }
        }
    }
}
