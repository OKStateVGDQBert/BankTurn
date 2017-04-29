using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    private Transform tran;
    private GameObject menuPanel;
    private GameObject gameOverPanel;
    private GameObject startGamePanel;
    private RectTransform mainCanvas;
    private RectTransform CursorLoc;
    private HUDScript hud;
    private int coins = 0;
    private int lives;
    private float lastMenuPress = 0.0f;
    private float lastBullet = 0.0f;
    [SerializeField]
    private float maxY = 5.0f;
    [SerializeField]
    private float minY = 5.0f;
    [SerializeField]
    private int moveSpeed = 1;
    [SerializeField]
    private AudioSource shootSound;

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
        if (Data_Manager.underPlayerControl && !Data_Manager.inMenu && !Data_Manager.gameOver)
        {
            Vector3 yComp = Vector3.zero;

            var inputY = Input.GetAxis("Vertical");

            if (Data_Manager.inverted) inputY *= -1.0f;

            tran.eulerAngles = tran.parent.eulerAngles + new Vector3(inputY * -30, Input.GetAxis("Horizontal") * 30, 0);

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
            if (Time.time - lastMenuPress > 1.0f && Input.GetAxis("Cancel") > 0)
            {
                Data_Manager.inMenu = true;
                menuPanel.SetActive(true);
                lastMenuPress = Time.time;
            }
            if (Input.GetAxis("Jump") > 0.0f && Time.time - lastBullet > 0.5f)
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
        Vector3 shootPos = Input.mousePosition;
        if (Data_Manager.xboxCursor)
        {
            shootPos = CursorLoc.anchoredPosition;
            shootPos.x += mainCanvas.sizeDelta.x * 0.5f;
            shootPos.y += mainCanvas.sizeDelta.y * 0.5f;
            shootPos.x /= mainCanvas.sizeDelta.x;
            shootPos.y /= mainCanvas.sizeDelta.y;
            shootPos = Camera.main.ViewportToScreenPoint(shootPos);

        }
        shootPos.z = 30.0f;
        shootPos = Camera.main.ScreenToWorldPoint(shootPos);
        bool cast = Physics.Linecast(tran.position + tran.forward * 5, shootPos, out hit);
        DrawLine(tran.position + tran.forward * 5, shootPos, new Color(200f/255f, 50f/255f, 50f/255f), 0.4f);
        shootSound.Play();
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
