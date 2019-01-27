using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TweakablesMenuController : MonoBehaviour
{

    public Text childSpawnTimeMinPlaceholder;
    public Text childSpawnTimeMaxPlaceholder;
    public Text bulletSpeedMinPlaceholder;
    public Text bulletSpeedMaxPlaceholder;
    public Text goalAreaRespawnTimerPlaceholder;
    public Text startHPPlaceholder;
    public Text goalAreaHealPlaceholder;
    public Text bulletDamageSpeedFactorPlaceholder;
    public Text bulletHealChancePlaceholder;
    public Text bulletHealAmountPlaceholder;
    public Text squareScalePlaceholder;
    public Text triangleScalePlaceholder;
    public Text childScalePlaceholder;
    public Text squareSpeedPlaceholder;
    public Text triangleSpeedPlaceholder;
    public Text childSpeedPlaceholder;
    public Text triangleAttentionSpanPlaceholder;
    public Text childSpawnTimeMin;
    public Text childSpawnTimeMax;
    public Text bulletSpeedMin;
    public Text bulletSpeedMax;
    public Text goalAreaRespawnTimer;
    public Text startHP;
    public Text goalAreaHeal;
    public Text bulletDamageSpeedFactor;
    public Text bulletHealChance;
    public Text bulletHealAmount;
    public Text squareScale;
    public Text triangleScale;
    public Text childScale;
    public Text squareSpeed;
    public Text triangleSpeed;
    public Text childSpeed;
    public Text triangleAttentionSpan;


    private void Start()
    {
        childSpawnTimeMinPlaceholder.text = Global.Instance.childSpawnTimeMin.ToString();
        childSpawnTimeMaxPlaceholder.text = Global.Instance.childSpawnTimeMax.ToString();
        bulletSpeedMinPlaceholder.text = Global.Instance.bulletSpeedMin.ToString();
        bulletSpeedMaxPlaceholder.text = Global.Instance.bulletSpeedMax.ToString();
        goalAreaRespawnTimerPlaceholder.text = Global.Instance.GoalAreaRespawnTimer.ToString();
        startHPPlaceholder.text = Global.Instance.StartHP.ToString();
        goalAreaHealPlaceholder.text = Global.Instance.GoalAreaHeal.ToString();
        bulletDamageSpeedFactorPlaceholder.text = Global.Instance.BulletDamageSpeedFactor.ToString();
        bulletHealChancePlaceholder.text = Global.Instance.BulletHealChance.ToString();
        bulletHealAmountPlaceholder.text = Global.Instance.BulletHealAmount.ToString();
        squareScalePlaceholder.text = Global.Instance.SquareScale.ToString();
        triangleScalePlaceholder.text = Global.Instance.TriangleScale.ToString();
        childScalePlaceholder.text = Global.Instance.ChildScale.ToString();
        squareSpeedPlaceholder.text = Global.Instance.SquareSpeed.ToString();
        triangleSpeedPlaceholder.text = Global.Instance.TriangleSpeed.ToString();
        childSpeedPlaceholder.text = Global.Instance.ChildSpeed.ToString();
        triangleAttentionSpanPlaceholder.text = Global.Instance.TriangleAttSpan.ToString();

    }

    public void SaveOptions()
    {
        if(childSpawnTimeMin.text != "")
        {
            float.TryParse(childSpawnTimeMin.text, out Global.Instance.childSpawnTimeMin);
        }
        if (childSpawnTimeMax.text != "")
        {
            float.TryParse(childSpawnTimeMax.text, out Global.Instance.childSpawnTimeMax);
        }
        if (bulletSpeedMin.text != "")
        {
            float.TryParse(bulletSpeedMin.text, out Global.Instance.bulletSpeedMin);
        }
        if (bulletSpeedMax.text != "")
        {
            float.TryParse(bulletSpeedMax.text, out Global.Instance.bulletSpeedMax);
        }
        if (goalAreaRespawnTimer.text != "")
        {
            float.TryParse(goalAreaRespawnTimer.text, out Global.Instance.GoalAreaRespawnTimer);
        }
        if (startHP.text != "")
        {
            int.TryParse(startHP.text, out Global.Instance.StartHP);
        }
        if (goalAreaHeal.text != "")
        {
            float.TryParse(goalAreaHeal.text, out Global.Instance.GoalAreaHeal);
        }
        if (bulletDamageSpeedFactor.text != "")
        {
            float.TryParse(bulletDamageSpeedFactor.text, out Global.Instance.BulletDamageSpeedFactor);
        }
        if (bulletHealChance.text != "")
        {
            float.TryParse(bulletHealChance.text, out Global.Instance.BulletHealChance);
        }
        if (bulletHealAmount.text != "")
        {
            float.TryParse(bulletHealAmount.text, out Global.Instance.BulletHealAmount);
        }
        if (squareScale.text != "")
        {
            float.TryParse(squareScale.text, out Global.Instance.SquareScale);
        }
        if (triangleScale.text != "")
        {
            float.TryParse(triangleScale.text, out Global.Instance.TriangleScale);
        }
        if (childScale.text != "")
        {
            float.TryParse(childScale.text, out Global.Instance.ChildScale);
        }
        if (squareSpeed.text != "")
        {
            float.TryParse(squareSpeed.text, out Global.Instance.SquareSpeed);
        }
        if (triangleSpeed.text != "")
        {
            float.TryParse(triangleSpeed.text, out Global.Instance.TriangleSpeed);
        }
        if (childSpeed.text != "")
        {
            float.TryParse(childSpeed.text, out Global.Instance.ChildSpeed);
        }
        if (triangleAttentionSpan.text != "")
        {
            float.TryParse(triangleAttentionSpan.text, out Global.Instance.TriangleAttSpan);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
}