using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : MonoBehaviour {

	void OnCollisionEnter(Collision coll)
    {
        // If this script collides with the terrain, game over.
		//Debug.Log("U Ded!");
		if (coll.gameObject.layer == 8)
		{
			(GameObject.FindWithTag("Player").GetComponent(typeof(Player_Controller)) as Player_Controller).GameOver();
		}
    }

}
