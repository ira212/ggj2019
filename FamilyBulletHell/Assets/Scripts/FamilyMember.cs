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
    public Text HealthText;
    private Canvas HealthCanvas;

    public void SpawnFamilyMember(int maxHP, float moveSpeed, bool isParent)
    {
        _maxHealth = maxHP;
        _currentHealth = maxHP;
        _speed = moveSpeed;
        _isParent = isParent;
    }

    public void HealthMonitor(Camera myCamera)
    {
        HealthText.text = _currentHealth.ToString();
        gameObject.GetComponentInChildren<Canvas>().worldCamera = myCamera;
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
