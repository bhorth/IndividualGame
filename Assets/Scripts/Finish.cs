using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//class for finish screens when player reaches the end
public class Finish : MonoBehaviour
{
    //define references
    public GameObject player;
    PlayerMovement playerMovement;

    GameObject hudUI;
    HUD hud;

    public GameObject finishScreen;
    public GameObject loseScreen;

    public TMP_Text cola;
    public TMP_Text time;
    public TMP_Text total;

    public TMP_InputField input;

    Dictionary<string, int> leaderboard = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        //set references
        playerMovement = player.GetComponent<PlayerMovement>();
        hudUI = GameObject.Find("HUD");
        hud = hudUI.GetComponent<HUD>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //if player collides with end barrier stop player movement and display screen
        if (other.gameObject == player)
        {
            playerMovement.runInput = false;
            playerMovement.finished = true;
            DisplayFinish();
        }
    }

    void DisplayFinish()
    {
        hud.startTimer = false;

        //if user has 50 or more cans then display finish screen with name input
        if(hud.score >= 50)
        {
            finishScreen.SetActive(true);
            cola.text = "COLA: " + hud.score.ToString();
            time.text = "TIME: " + hud.time.ToString();

            //calculate total score
            int totalScore = hud.time + hud.score;
            total.text = "TOTAL SCORE: " + totalScore.ToString();
        }
        //if user did not get 50 cans then dislpay loser screen
        else
        {
            loseScreen.SetActive(true);
        }
    }

    //fn for quitting game and going back to menu
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    //fn for submitting player entry name and score value
    public void Submit()
    {
        //if no name entered then use default name
        string name = "???";
        if (!string.IsNullOrEmpty(input.text))
        {
            name = input.text;
        }

        //get player score and name for entry
        int score = hud.time + hud.score;
        string entry = name + " - " + score;

        //write to leaderboard file
        string path = Application.dataPath + "/Resources/leaderboard.txt";
        File.AppendAllText(path, entry + "\n");

        //sort the leader boards
        SortLeaderboard(path);

        SceneManager.LoadScene("Menu");
    }

    //fn for sorting the leaderboard values from highest to lowest score
    void SortLeaderboard(string path)
    {
        foreach (string line in File.ReadAllLines(path))
        {
            //parse string to get the score value and convert to an integer value
            string score = line.Substring(line.LastIndexOf('-') + 1).Replace(":", string.Empty).Replace(" ", string.Empty);
            int result = Int32.Parse(score);

            //if entry is a not a duplicate at it to the dictionary and use score value as the key
            if (!leaderboard.ContainsKey(line))
            {
                leaderboard.Add(line, result);
            }
        }

        //create a new dictionary sorted by key value
        var sortedDict = leaderboard.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        //read each entry in the sorted dictionary and add to string
        string newEntries = string.Empty;
        foreach (var player in sortedDict)
        {
            newEntries += player.Key + "\n";
        }

        //write the string value to the file replacing all older lines
        File.WriteAllText(path, newEntries);
    }
}
