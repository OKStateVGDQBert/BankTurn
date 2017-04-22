﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour {

    private GameObject player;
    public int moveSpeed = 1;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            transform.LookAt(player.transform);
            if (Vector3.Distance(transform.position, player.transform.position) < 60.0f)
            {
                RaycastHit hit;
                bool castMid = Physics.Linecast(transform.position, player.transform.position, out hit);
                if (castMid || hit.transform.position == player.transform.position)
                {
                    transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
                }
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log("U got hit!");
        GameObject.Destroy(gameObject);
    }
}
