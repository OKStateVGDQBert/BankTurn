using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_End_Trigger : MonoBehaviour {

    void OnTriggerEnter(Collider coll)
	{
        // If our Collider is on a GameObject that has a parent.
        if (Data_Manager.IsPlayer(coll))
        {
            // End the game
			(coll.transform.parent.gameObject.GetComponent(typeof(Player_Controller)) as Player_Controller).GameOver();
        }
	}
}
