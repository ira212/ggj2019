using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgain : MonoBehaviour
{

    public Text scoreTotal;

    void Start()
    {
        scoreTotal.text = "Final Score: " + Global.Instance.FinalScore.ToString();
    }

    public void SceneChange()
    {
        Global.Instance.FinalScore = 0;
        SceneManager.LoadScene(sceneName: "Gameplay");
    }
}
