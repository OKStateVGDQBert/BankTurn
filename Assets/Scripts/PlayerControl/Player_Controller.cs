using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
    
    private Transform tran;
    private GameObject mainCamera;
    private GameObject menuPanel;
    private GameObject gameOverPanel;
    private GameObject startGamePanel;
    private int coins = 0;
    private int lives = 3;
    private float lastMenuPress = 0.0f;
    [SerializeField]
    private int turnSpeed = 1;
    [SerializeField]
    private float maxY = 5.0f;
    [SerializeField]
    private float minY = 5.0f;
    [SerializeField]
    private int forwardSpeed = 1;

    void Start () {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
		mainCamera = GameObject.Find ("Main Camera");
        menuPanel = GameObject.Find ("Menu");
        gameOverPanel = GameObject.Find ("GameOver");
        startGamePanel = GameObject.Find("StartGame");
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
	
	void FixedUpdate ()
    {
        if (Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver)
        {
            // Rotate the ship
            tran.RotateAround(tran.position, tran.up, turnSpeed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);

            // Now move the ship in the direction of the current rotation.
            tran.position = tran.position + tran.forward * forwardSpeed * Time.fixedDeltaTime;

            Vector3 yComp = Vector3.zero;

            if (Input.GetAxis("Vertical") > 0)
            {
                if (tran.position.y < maxY)
                {
                    yComp = new Vector3(0, Input.GetAxis("Vertical") * forwardSpeed * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (tran.position.y > minY)
                {
                    yComp = new Vector3(0, Input.GetAxis("Vertical") * forwardSpeed * Time.fixedDeltaTime);
                }
            }
            tran.position = tran.position + yComp;
			mainCamera.transform.position = mainCamera.transform.position + yComp;
            if (Time.time - lastMenuPress > 1.0f && Input.GetAxis("Cancel") > 0)
            {
                Data_Manager.inMenu = true;
                menuPanel.SetActive(true);
                lastMenuPress = Time.time;
            }
        }
        else
        {
            if (Time.time - lastMenuPress > 1.0f && Data_Manager.inMenu)
            {

                if (Input.GetAxis("Cancel") > 0)
                {
                    menuPanel.SetActive(false);
                    Data_Manager.inMenu = false;
                    lastMenuPress = Time.time;
                }
            }
            if (!Data_Manager.underPlayerControl)
            {
                if (Input.GetAxis("Jump") > 0)
                {
                    (startGamePanel.GetComponent(typeof(Panel_Fade)) as Panel_Fade).StartFade();
                    Data_Manager.underPlayerControl = true;
                    tran.SetParent(null);
                }
            }
            if (Data_Manager.gameOver)
            {
                if (Input.GetAxis("Jump") > 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
                }
            }
        }
    }

    public void GiveCoin()
    {
        coins++;
    }

    public int GetCoin()
    {
        return coins;
    }

    public void TakeLife()
    {
        lives--;
        if (lives < 0)
        {
            Data_Manager.gameOver = true;
            gameOverPanel.SetActive(true);
        }
    }
}
