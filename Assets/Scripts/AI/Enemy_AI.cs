using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour {

    private GameObject player;
    [SerializeField]
    private int moveSpeed = 1;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null && Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver)
         {
            transform.LookAt(player.transform);
            if (Vector3.Distance(transform.position, player.transform.position) < 60.0f)
            {
                RaycastHit hit;
#pragma warning disable 0219
                bool castMid = Physics.Linecast(transform.position, player.transform.position, out hit);
#pragma warning restore 0219
                // The collider is attached to a gameobject that is a child of the player.
                if (Data_Manager.IsPlayer(hit.collider))
				{
					transform.position = Vector3.Lerp (transform.position, player.transform.position, Time.deltaTime * moveSpeed);
				}
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (Data_Manager.IsPlayer(coll.collider))
        { 
            var PC = coll.gameObject.transform.parent.gameObject.GetComponent(typeof(Player_Controller)) as Player_Controller;
            if (PC != null)
            {
                PC.TakeLife();
                GameObject.Destroy(gameObject);
            }
        }
    }
}
