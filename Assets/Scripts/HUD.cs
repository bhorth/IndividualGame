using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//class used for the ui screens
public class HUD : MonoBehaviour
{
    //define references
    public GameObject player;
    PlayerMovement playerMovement;
    public Slider slider;
    public GameObject end;
    public GameObject pauseMenu;
    public GameObject deadScreen;
    public GameObject gameOverScreen;
    public GameObject timeUpScreen;
    public GameObject timeUpNoLivesScreen;
    float distance;
    Vector3 startingPos;

    public int score = 0;
    public TMP_Text scoreText;

    public int time = 60;
    public TMP_Text timer;

    public TMP_Text livesText;

    public bool startTimer = false;
    public bool playerDead = false;
    bool isPaused;
    string lives;

    public Button restartButton;
    public TMP_Text warning;

    // Start is called before the first frame update
    void Start()
    {
        //get num of player lives after loading scene or restart
        ReadLives();
        
        //set references
        livesText.text = "LIVES x" + lives;
        distance = Vector3.Distance(player.transform.position, end.transform.position);
        startingPos = player.transform.position;
        slider.maxValue = distance;
        playerMovement = player.GetComponent<PlayerMovement>();

        //call timer function each second
        InvokeRepeating("Timer", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "COLA x" + score;
        timer.text = time.ToString();

        //if player is still alive update slider value to show players distance to the end
        if (!playerDead)
        {
            slider.value = Vector3.Distance(player.transform.position, startingPos);
        }
        //player is dead
        else
        {
            //get lives and decrease num of lives
            int life = Int32.Parse(lives);
            life -= 1;
            livesText.text = "LIVES x" + life.ToString();

            //if player has no lifes left displayer game over screen
            if (life == 0 && time > 0)
            {
                gameOverScreen.SetActive(true);
            }

            //if player has lives then display the death screen
            if(life > 0 && time > 0)
            {
                deadScreen.SetActive(true);
                WriteLives(life.ToString());
            }
        }

        //check if player still has time left
        TimeUp();

        //if player is not dead and still has time and has not finished then allow game to be paused
        if (Input.GetKeyDown(KeyCode.Escape) && !playerDead && time > 0 && lives != "0" && !playerMovement.finished)
        {
            isPaused = !isPaused;
        }

        //if the game is pause bring up pause menu and pause game
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;

            //cannot restart game if player has 1 live left
            if(lives == "1")
            {
                restartButton.enabled = false;
                warning.text = "No Restarts Allowed. You have one life left!";
            }
        }

        //if leaving paused menu disable the menu and unpause the game
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //fn for decreasing the time on screen
    void Timer()
    {
        //start timer if flag activated and player is still alive
        if(time > 0 && startTimer && !playerDead)
        {
            time -= 1;
        }
    }

    //fn used to check if the player has run out of time
    void TimeUp()
    {
        //if no time is left stop player movement and player is finished
        if (time == 0)
        {
            var playerStop = player.GetComponent<PlayerMovement>();
            playerStop.finished = true;

            //get lives
            int life = Int32.Parse(lives);
            life -= 1;
            livesText.text = "LIVES x" + life.ToString();
            WriteLives(life.ToString());

            //if no lives left display time up and no lives left screen
            if (life == 0)
            {
                timeUpNoLivesScreen.SetActive(true);
            }
            //if has lives display time up screen
            else
            {
                timeUpScreen.SetActive(true);
            }
        }

    }

    //fn reads num of lives from a file
    void ReadLives()
    {
        string path = Application.dataPath + "/Resources/lives.txt";
        lives = File.ReadAllText(path);
    }

    //fn writes num of lives from a file
    void WriteLives(string life)
    {
        string path = Application.dataPath + "/Resources/lives.txt";
        File.WriteAllText(path, life);
    }

    //fn restarts the game and takes away one life
    public void Restart()
    {
        int life = Int32.Parse(lives);
        life -= 1;
        livesText.text = "LIVES x" + life.ToString();
        WriteLives(life.ToString());
        SceneManager.LoadScene("Runner");
    }

    //fn restarts game if player runs out of time
    public void TimeUpRestart()
    {
        SceneManager.LoadScene("Runner");
    }

    //fn used for returning to the main menu
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
