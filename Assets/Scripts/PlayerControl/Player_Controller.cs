using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
    
    private Transform tran;
    public int turnSpeed = 1;
	public float maxY = 5.0f;
    public int forwardSpeed = 1;

    void Start () {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
    }
	
	void FixedUpdate ()
    {
        // Rotate the ship
        tran.RotateAround(tran.position, tran.up, turnSpeed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);

        // Now move the ship in the direction of the current rotation.
        tran.position = tran.position + tran.forward * forwardSpeed * Time.fixedDeltaTime;
    }
}
