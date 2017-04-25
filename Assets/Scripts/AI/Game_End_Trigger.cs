using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_End_Trigger : MonoBehaviour {

    private GameObject gameOverPanel;

    void Awake()
    {
        gameOverPanel = GameObject.Find("GameOver");
    }

    void OnTriggerEnter(Collider coll)
	{
        // If our Collider is on a GameObject that has a parent.
        if (Data_Manager.IsPlayer(coll))
        {
            Data_Manager.gameOver = true;
            gameOverPanel.SetActive(true);
        }
	}
}
