using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{ 
    public void SceneChange()
    {
        Global.Instance.FinalScore = 0;
        SceneManager.LoadScene(sceneName: "Gameplay");
    }
}
