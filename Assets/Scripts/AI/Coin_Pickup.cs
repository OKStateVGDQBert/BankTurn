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
        // If our Collider is on a GameObject that has a parent.
        if (coll.gameObject.transform.parent != null)
        {
            // Grab that parent's tag and check to see if they're a player.
            if (coll.gameObject.transform.parent.gameObject.tag == "Player")
            {
                var PC = coll.gameObject.transform.parent.gameObject.GetComponent(typeof(Player_Controller)) as Player_Controller;
                if (PC != null)
                {
                    PC.GiveCoin();
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}
