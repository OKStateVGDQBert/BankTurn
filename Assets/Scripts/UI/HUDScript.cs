using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

	//Lives Stuff
	[SerializeField]
	private Text livesText;

	//Co Stuff
	[SerializeField]
	private Text coinText;
   
	//Player Stuff
	private Player_Controller player;   

	// Use this for initialization
	void Start () {
		//Get player info
		player = GameObject.FindWithTag("Player").GetComponent(typeof(Player_Controller)) as Player_Controller;
		coinText.text = "" + player.GetCoin();
		livesText.text = "" + player.GetLives();
    }

	void Update()
	{
		if (Data_Manager.gameOver) gameObject.SetActive(false);
	}

	//in  script with oncollision, call lifelost when player crashes or gets hit by enemy
	public void lifeLost()
	{
		livesText.text = "" + player.GetLives();
    } 
	
	//ig call this in script with coin collection
	public void coinCollected()
	{
		coinText.text = "" + player.GetCoin();
    }
}


