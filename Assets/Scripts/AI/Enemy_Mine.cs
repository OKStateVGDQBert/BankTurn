using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class contains the AI Behaviour for the mine.
public class Enemy_Mine : MonoBehaviour
{
    // Reference to the player
	private GameObject player;
    private AudioSource crash;

    // Use this for initialization
    void Start()
	{
        // Find the player and store it.
		player = GameObject.FindWithTag("Player");
        crash = (player.GetComponents<AudioSource>())[1];
    }

    // If the mine collides with a player, kill that player. Mines OP.
	void OnCollisionEnter(Collision coll)
	{
		if (Data_Manager.IsPlayer(coll.collider))
		{
			var PC = coll.gameObject.transform.parent.gameObject.GetComponent(typeof(Player_Controller)) as Player_Controller;
			if (PC != null)
			{
				PC.GameOver();
                crash.Play();
				GameObject.Destroy(gameObject);
			}
		}
	}
}
