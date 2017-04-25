using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Over_Panel : MonoBehaviour {
	
	private Player_Controller player;
	public GameObject coinOb;
	private Text coins;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent(typeof(Player_Controller)) as Player_Controller;
		coins = coinOb.GetComponent(typeof(Text)) as Text;

	}

	void OnEnable()
	{
		if (player == null)
		player = GameObject.FindWithTag("Player").GetComponent(typeof(Player_Controller)) as Player_Controller;
		if (coins == null) 
		coins = coinOb.GetComponent(typeof(Text)) as Text;
		if (coins != null)
		coins.text = "Total Coins: " + player.GetCoin();
	}
}
