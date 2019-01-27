using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgain : MonoBehaviour
{

    public Text scoreTotal;
    public Text childTotal;

    void Start()
    {
        scoreTotal.text = "Final Score: " + Global.Instance.FinalScore.ToString();
        childTotal.text = "# of Children: " + Global.Instance.ChildrenNumber.ToString();
    }

    public void SceneChange()
    {
        Global.Instance.FinalScore = 0;
        Global.Instance.ChildrenNumber = 0;
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
}
