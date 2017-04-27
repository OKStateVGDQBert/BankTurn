using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine : MonoBehaviour
{

	private GameObject player;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindWithTag("Player");
	}

	void OnCollisionEnter(Collision coll)
	{
		if (Data_Manager.IsPlayer(coll.collider))
		{
			var PC = coll.gameObject.transform.parent.gameObject.GetComponent(typeof(Player_Controller)) as Player_Controller;
			if (PC != null)
			{
				PC.GameOver();
				GameObject.Destroy(gameObject);
			}
		}
	}
}
