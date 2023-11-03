using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

// -------------------Game Object -------------------------
{
    [SerializeField]
    GameObject player, startPanel, blockPrefab, diamondPrefab, startButton, ExitSMS, Setting, Teach, congra, GameOverPanel, Shoping, Info, UnavailableSMS;

    // ----------------TMP_Text----------------------
    [SerializeField]
    TMP_Text scoreText, diamondText, diamondText2, highScoreText, gamesText;

    //---------------Variable------------------

    // float speed;
    bool hasGameStarted, hasGameFinished, isMovingForward;
    int score, diamonds, highScore, games;
    float size;
    Vector3 lastPos;

    public static GameManager instance;
    AudioManager audioManager;


    public bool enableMouseInput = true;

    private int clickCount = 0;




    public float initialSpeed = 5f;
    public float speedIncrement = 0.1f;


    // ----------------Awake Function --------------
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        // Increase the player's speed gradually
        initialSpeed += speedIncrement * Time.deltaTime;


        score = 0;
        //speed = 10f;
        player = GameObject.Find("Player");

        hasGameFinished = false;
        hasGameStarted = false;
        isMovingForward = true;





        diamonds = PlayerPrefs.HasKey("Diamond") ? PlayerPrefs.GetInt("Diamond") : 0;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        games = PlayerPrefs.HasKey("Games") ? PlayerPrefs.GetInt("Games") : 0;



        scoreText.text = score.ToString();
        diamondText.text = diamonds.ToString();
        diamondText2.text = diamonds.ToString();
        gamesText.text = "Games Played : " + games.ToString();
        highScoreText.text = "" + highScore.ToString();

        lastPos = blockPrefab.transform.position;
        size = blockPrefab.transform.localScale.x;
        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }


        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    // ------------Update Function ----------------
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Shoping.SetActive(false);
            ShopBack();
            InfoBack();
        }



        // Increase the player's speed gradually
        initialSpeed += speedIncrement * Time.deltaTime;


        if (hasGameFinished)
        {

            startPanel.SetActive(false);
            //RestartGame();
            // startPanel.SetActive(false);
            GameOverPanel.SetActive(true);


            // Time.timeScale = 0f;



            audioManager.PauseSFX();


        }

        if (!hasGameStarted)
        {
            //  Time.timeScale = 1f;


            if (Input.GetMouseButtonDown(0) && IsPointerOverGameObject(startButton))

            {


            }

            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;


            if (clickCount == 1)
            {
                audioManager.PlaySFX(audioManager.wallTouch);
                Teach.SetActive(false);
                player.GetComponent<Rigidbody>().velocity = Vector3.right * initialSpeed;

            }
            else if (clickCount > 1)
            {

                player.GetComponent<Rigidbody>().velocity = (isMovingForward ? Vector3.back : Vector3.right) * initialSpeed;
                isMovingForward = !isMovingForward;
            }
        }
        
    }




    // ------------ Other Game Function -------







    public void ExitNo()
    {
        ExitSMS.SetActive(false);
    }

    public void ExitYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void QuitGame()
    {
        ExitSMS.SetActive(true);
    }

    public void Shop()
        {
        Shoping.SetActive(true);
        }
   public void ShopBack()
    {
        Shoping.SetActive(false);
    }
  






    public void Informatiom()
    {
        Info.SetActive(true);
    }
    public void InfoBack()
    {
        Info.SetActive(false);
    }


    public void UnavSMS()
    {
        UnavailableSMS.SetActive(true);

    }
    public void UnavSMS_Ok()
    {
        UnavailableSMS.SetActive(false);

    }
    public void settingExit()
    {
        Setting.SetActive(false);
    }


    public void StartGame()
    {




        startPanel.SetActive(false);


        Teach.SetActive(true);

        hasGameStarted = true;
 
        hasGameFinished = false;
      
        BlockSpawn();
        



    }






    public void RestartGame()
    {

        GameOverPanel.SetActive(false);
      //  Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      
    }
    // UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    private bool IsPointerOverGameObject(GameObject gameObject)
    {
        // Check if the mouse pointer is over the given game object
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        return collider.bounds.Contains(mousePosition);
    }

    void BlockSpawn()
    {
        InvokeRepeating("SpawnPlatform", 0.2f, 0.2f);
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();

        if (score > highScore)
        {
            congra.SetActive(true);
        }
    }

    public void UpdateDiamond()
    {
        diamonds++;
        PlayerPrefs.SetInt("Diamond", diamonds);
        diamondText.text = diamonds.ToString();

        diamondText2.text = diamonds.ToString();
    }

    public void GameOver()
    {
       
        hasGameFinished = true;
        startPanel.SetActive(true);

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "High Score : " + highScore.ToString();
        }

        games++;
        gamesText.text = "Games Played : " + games.ToString();
        PlayerPrefs.SetInt("Games", games);

        CancelInvoke("SpawnPlatform");
    }

    void SpawnPlatform()
    {
        int temp = Random.Range(0, 2);
        Vector3 pos = lastPos;
        if(temp == 0)
        {
            pos.x += size;
        }
        else
        {
            pos.z -= size;
        }

        Instantiate(blockPrefab, pos, Quaternion.identity);
        lastPos = pos;

        if(Random.Range(0,4) == 0)
        {
            Instantiate(diamondPrefab, lastPos + Vector3.up * 6f, diamondPrefab.transform.rotation);
        }
    }
}
