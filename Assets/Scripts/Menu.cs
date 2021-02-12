using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//class used for the function of the menu screen
public class Menu : MonoBehaviour
{
    //define references
    public TMP_Text leaderboard;
    public GameObject menuScreen;
    public GameObject leaderboardScreen;

    private void Awake()
    {
        //get the leaderboard entries from file
        string path = Application.dataPath + "/Resources/leaderboard.txt";
        string entries = File.ReadAllText(path);

        //update leaderboard text
        if (!string.IsNullOrEmpty(entries))
        {
            leaderboard.text = entries;
        }
        else
        {
            leaderboard.text = "No Entries";
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 1;
    }

    //fn used for starting the game and loading the scene and setting num of lives
    public void StartGame()
    {
        WriteNewLives();
        SceneManager.LoadScene("Runner");
    }

    //fn used for quitting the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //fn for giving 3 lives to the player to be used in the next scene
    void WriteNewLives()
    {
        string path = Application.dataPath + "/Resources/lives.txt";
        File.WriteAllText(path, "3");
    }

    //fn used for activating the leaderboard
    public void LeaderBoard()
    {
        menuScreen.SetActive(false);
        leaderboardScreen.SetActive(true);
    }

    //fn for returning back to the menu from the leaderboard
    public void BackButton()
    {
        leaderboardScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
}
