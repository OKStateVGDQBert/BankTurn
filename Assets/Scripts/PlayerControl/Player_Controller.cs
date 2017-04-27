﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    private Transform tran;
    private GameObject mainCamera;
    private GameObject menuPanel;
    private GameObject gameOverPanel;
    private GameObject startGamePanel;
    private HUDScript hud;
    private int coins = 0;
    private int lives;
    private float lastMenuPress = 0.0f;
    private float lastBullet = 0.0f;
    [SerializeField]
    private int turnSpeed = 1;
    [SerializeField]
    private float maxY = 5.0f;
    [SerializeField]
    private float minY = 5.0f;
    [SerializeField]
    private int forwardSpeed = 1;
    [SerializeField]
    private AudioSource shootSound;

    private void Awake()
    {
        lives = 2 + (2 * Data_Manager.shipType);
    }

    void Start() {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
        hud = GameObject.Find("HUD").GetComponent(typeof(HUDScript)) as HUDScript;
        mainCamera = GameObject.Find("Main Camera");
        menuPanel = GameObject.Find("Menu");
        gameOverPanel = GameObject.Find("GameOver");
        startGamePanel = GameObject.Find("StartGame");
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void FixedUpdate()
    {
        if (Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver)
        {
            // Rotate the ship
            tran.RotateAround(tran.position, tran.up, turnSpeed * Input.GetAxis("Horizontal") * Time.fixedDeltaTime);

            // Now move the ship in the direction of the current rotation.
            tran.position = tran.position + tran.forward * forwardSpeed * Time.fixedDeltaTime;

            Vector3 yComp = Vector3.zero;

            var inputY = Input.GetAxis("Vertical");

            if (Data_Manager.inverted) inputY *= -1.0f;

            if (inputY > 0)
            {
                if (tran.position.y < maxY)
                {
                    yComp = new Vector3(0, inputY * forwardSpeed * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (tran.position.y > minY)
                {
                    yComp = new Vector3(0, inputY * forwardSpeed * Time.fixedDeltaTime);
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
            if (Input.GetAxis("Jump") > 0.0f && Time.time - lastBullet > 1.0f)
            {
                Shoot();
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

    private void Shoot()
    {
        RaycastHit hit;
        bool cast = Physics.Linecast(tran.position + tran.forward * 5, tran.position + tran.forward * 50, out hit);
        DrawLine(tran.position + tran.forward * 5, tran.position + tran.forward * 50, new Color(200f/255f, 50f/255f, 50f/255f), 0.5f);
        if (!cast || hit.collider == null || hit.collider.GetType() == typeof(TerrainCollider))
        {
            lastBullet = Time.time;
            return;
        }
        Enemy_AI enemy = hit.collider.gameObject.GetComponent(typeof(Enemy_AI)) as Enemy_AI;
        if (enemy != null)
        {
            GameObject.Destroy(enemy.gameObject);
        }
        shootSound.Play();
        lastBullet = Time.time;
    }

    // Drawline retrieved from Unity Dev Questions
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public void GiveCoin()
    {
        coins++;
		hud.coinCollected();
    }

    public int GetCoin()
	{
		return coins;
    }

    public int GetLives()
	{
		return lives;
    }

    public void TakeLife()
    {
        lives--;
        if (lives < 0)
        {
			GameOver();
        }
		hud.lifeLost();
    }

	public void GameOver()
	{
		Data_Manager.gameOver = true;
        gameOverPanel.SetActive(true);
	}
}
