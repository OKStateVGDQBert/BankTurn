using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class contains the AI Behaviour for the general enemy.
public class Enemy_AI : MonoBehaviour {

    // Keep a reference to the player
    private GameObject player;
    private AudioSource crash;
    // Movement multiplayer
    [SerializeField]
    private int moveSpeed = 1;

	// Use this for initialization
	void Start () {
        // Find the player and store it.
        player = GameObject.FindWithTag("Player");
        // The crash sound is the first audiosource on the player.
        crash = (player.GetComponents<AudioSource>())[0];
	}
	
	void Update () {
		if (player != null && Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver)
         {
            // Make sure the enemy is always facing the player
            transform.LookAt(player.transform);

            // If we're close enough, do a cast. If the cast hits, then move to them. 
            if (Vector3.Distance(transform.position, player.transform.position) < 60.0f)
            {
                RaycastHit hit;
                Physics.Linecast(transform.position, player.transform.position, out hit);
                // The collider is attached to a gameobject that is a child of the player.
                if (Data_Manager.IsPlayer(hit.collider))
				{
					transform.position = Vector3.Lerp (transform.position, player.transform.position, Time.deltaTime * moveSpeed);
				}
            }
        }
    }

    // If we hit something, check to see if it's the player, if it is then take a life and kill ourselves.
    void OnCollisionEnter(Collision coll)
    {
        if (Data_Manager.IsPlayer(coll.collider))
        { 
            var PC = coll.gameObject.transform.parent.gameObject.GetComponent(typeof(Player_Controller)) as Player_Controller;
            if (PC != null)
            {

                PC.TakeLife();
                crash.Play();
                GameObject.Destroy(gameObject);
            }
        }
    }
}
