using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    private Rigidbody rb;
    private Transform tran;
    public int turnSpeed = 1;
    public int forwardSpeed = 1;

    void Start () {
        rb = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
    }
	
	void FixedUpdate () {
        rb.AddTorque(Input.GetAxis("Horizontal") * tran.up * Time.fixedDeltaTime * turnSpeed, ForceMode.VelocityChange);
        rb.AddForce(Input.GetAxis("Vertical") *  Vector3.up * Time.fixedDeltaTime * forwardSpeed, ForceMode.VelocityChange);
        rb.AddForce(tran.forward * Time.fixedDeltaTime * forwardSpeed, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("U ded!");
    }
}
