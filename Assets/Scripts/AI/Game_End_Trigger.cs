using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_End_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
	{
        // If our Collider is on a GameObject that has a parent.
        if (coll.gameObject.transform.parent != null)
        {
            // Grab that parent's tag and check to see if they're a player.
            if (coll.gameObject.transform.parent.gameObject.tag == "Player")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
            }
        }
	}
}
