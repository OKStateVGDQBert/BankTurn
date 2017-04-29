using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class contains the AI Behaviour for the coin.
public class Coin_Pickup : MonoBehaviour {
    
	void Update () {
        // Rotate the coin every frame.
        transform.Rotate(transform.up, Time.deltaTime * 50);
	}

    void OnCollisionEnter(Collision coll)
    {
        // If we collide with the player, kill ourselves and give the player a coin.
        if (Data_Manager.IsPlayer(coll.collider))
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
