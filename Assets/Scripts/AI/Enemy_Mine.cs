using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine : MonoBehaviour
{

	private GameObject player;
    private AudioSource crash;

    // Use this for initialization
    void Start()
	{
		player = GameObject.FindWithTag("Player");
        crash = (player.GetComponents<AudioSource>())[1];
    }

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
