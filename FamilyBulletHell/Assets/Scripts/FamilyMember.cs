using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamilyMember : MonoBehaviour
{
    public System.Action OnFamilyMemberDeath;

    private int _maxHealth;
    private int _currentHealth;
    private float _speed;
    private bool _isParent;

    public void SpawnFamilyMember(int maxHP, float moveSpeed, bool isParent)
    {
        _maxHealth = maxHP;
        _currentHealth = maxHP;
        _speed = moveSpeed;
        _isParent = isParent;
    }

    public void HealthMonitor(Transform self)
    {
        GameObject HealthCanvas;
        GameObject HealthText;

        Canvas myCanvas;
        Text myText;

        HealthCanvas = new GameObject();
        HealthText = new GameObject();

        HealthCanvas.AddComponent<Canvas>();
        HealthCanvas.name = "Health Canvas";
        HealthText.AddComponent<Text>();
        HealthText.name = "Health Text";

        myCanvas = HealthCanvas.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        HealthCanvas.AddComponent<CanvasScaler>();
        HealthCanvas.AddComponent<GraphicRaycaster>();

        HealthText.transform.parent = HealthText.transform;
        myText = HealthText.AddComponent<Text>();
        myText.font = (Font)Resources.Load("MyFont");
        myText.text = _currentHealth.ToString();
        myText.fontSize = 100;
    }

    public void TakeDamage(int damageTaken)
    {
        _currentHealth -= damageTaken;
        Debug.Log("You took " + damageTaken + " damage.");
        Debug.Log("You have " + _currentHealth + "health left.");
        if (_currentHealth <= 0)
        {
            OnFamilyMemberDeath?.Invoke();
        }
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public int GetHealth()
    {
        return (_currentHealth);
    }

    public bool IsParent()
    {
        return _isParent;
    }
}
