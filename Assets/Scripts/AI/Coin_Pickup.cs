using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.up, Time.deltaTime * 50);
	}

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log("Coin Collected!");
        GameObject.Destroy(gameObject);
    }
}
