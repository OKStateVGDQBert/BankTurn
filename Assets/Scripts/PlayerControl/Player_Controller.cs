using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    // Our transform
    private Transform tran;
    // These are the panels that we toggle with buttons.
    private GameObject menuPanel;
    private GameObject gameOverPanel;
    private GameObject startGamePanel;
    // The canvas's rect and the cursor's rect
    private RectTransform mainCanvas;
    private RectTransform CursorLoc;
    // Our hud
    private HUDScript hud;
    private int coins = 0;
    private int lives;
    // These are to keep track of the last time you opened the menu or fired a bullet to prevent spam.
    private float lastMenuPress = 0.0f;
    private float lastBullet = 0.0f;
    // The max and min height of the ship
    [SerializeField]
    private float maxY = 5.0f;
    [SerializeField]
    private float minY = 5.0f;
    // Movement multiplier
    [SerializeField]
    private int moveSpeed = 1;
    // The sound the ship makes when it shoots.
    [SerializeField]
    private AudioSource shootSound;

    // This is called before start. This is so that the HUD can ask for the lives.
    private void Awake()
    {
        lives = 2 + (2 * Data_Manager.shipType);
    }

    void Start() {
        tran = gameObject.GetComponent(typeof(Transform)) as Transform;
        mainCanvas = GameObject.Find("Menu_Canvas").GetComponent(typeof(RectTransform)) as RectTransform;
        hud = GameObject.Find("HUD").GetComponent(typeof(HUDScript)) as HUDScript;
        menuPanel = GameObject.Find("Menu");
        gameOverPanel = GameObject.Find("GameOver");
        startGamePanel = GameObject.Find("StartGame");
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        CursorLoc = GameObject.Find("Cursor").GetComponent(typeof(RectTransform)) as RectTransform;
    }

    void FixedUpdate()
    {
        // If the game is under player control, they aren't in a menu, and the game isn't over
        if (Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver)
        {
            Vector3 yComp = Vector3.zero;

            var inputY = Input.GetAxis("Vertical");

            // Invert the Y if the inverted option is set to true.
            if (Data_Manager.inverted) inputY *= -1.0f;

            // Here we do the rotating based on movement.
            tran.eulerAngles = tran.parent.eulerAngles + new Vector3(inputY * -30, Input.GetAxis("Horizontal") * 30, 0);

            // If the input is positive, make sure they don't go above the max, if it's negative make sure they don't go below the min.
            if (inputY > 0)
            {
                if (tran.position.y < maxY)
                {
                    yComp = new Vector3(0, inputY * moveSpeed * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (tran.position.y > minY)
                {
                    yComp = new Vector3(0, inputY * moveSpeed * Time.fixedDeltaTime);
                }
            }

            tran.position = tran.position + yComp + (tran.parent.right * Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed);
            
            // If player presses the menu key, open the menu.
            if (Time.time - lastMenuPress > 1.0f && Input.GetAxis("Cancel") > 0)
            {
                Data_Manager.inMenu = true;
                menuPanel.SetActive(true);
                lastMenuPress = Time.time;
            }

            // Shoot if player presses the jump key
            if (Input.GetAxis("Jump") > 0.0f && Time.time - lastBullet > 0.5f)
            {
                Shoot();
            }
        }
        // Basically, if we aren't in a play mode, check what mode we are in and respond to it.
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

    // Shoot our trusty phaser!
    private void Shoot()
    {
        RaycastHit hit;
        Vector3 shootPos = Input.mousePosition;

        // If the cursor is on, take it's location in the canvas, convert it to viewport, then convert it to screen.
        if (Data_Manager.xboxCursor)
        {
            shootPos = CursorLoc.anchoredPosition;
            shootPos.x += mainCanvas.sizeDelta.x * 0.5f;
            shootPos.y += mainCanvas.sizeDelta.y * 0.5f;
            shootPos.x /= mainCanvas.sizeDelta.x;
            shootPos.y /= mainCanvas.sizeDelta.y;
            shootPos = Camera.main.ViewportToScreenPoint(shootPos);

        }

        // Move the position forward from the screen, the convert it to world.
        shootPos.z = 30.0f;
        shootPos = Camera.main.ScreenToWorldPoint(shootPos);

        // Cast and draw a line from in front of the ship to the position.
        bool cast = Physics.Linecast(tran.position + tran.forward * 5, shootPos, out hit);
        DrawLine(tran.position + tran.forward * 5, shootPos, new Color(200f/255f, 50f/255f, 50f/255f), 0.4f);
        shootSound.Play();
        // If we don't hit an enemy, ignore.
        if (!cast || hit.collider == null || hit.collider.GetType() == typeof(TerrainCollider))
        {
            lastBullet = Time.time;
            return;
        }

        // If the collided object has an enemy script, kill it.
        Enemy_AI enemy = hit.collider.gameObject.GetComponent(typeof(Enemy_AI)) as Enemy_AI;
        if (enemy != null)
        {
            GameObject.Destroy(enemy.gameObject);
        }
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
