using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    

    public static Global Instance; //Creates a new instance if one does not yet exist

    void Awake()
    {
        if (Instance == null) 
        {
            DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); //deletes copies of global which do not need to exist, so right version is used to get info from
        }
    }


    public int GameLength = 10; //Length of games
    public int RedFontTimeThreshold = 5; //Time where FOnt of Timer Turns Red
    public int GridWidth = 8; //number of cubes wide
    public int GridHeight = 5; //number of cubes tall
    public string StartButtonText = "Start"; //Text on Start Button
    public string TuningButtonText = "Tuning Variables"; //Text on Tuning Variables Button
    public float Time; //Time left in game, resets every game
}
