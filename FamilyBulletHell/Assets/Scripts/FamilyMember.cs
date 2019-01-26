using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void TakeDamage(int damageTaken)
    {
        _currentHealth -= damageTaken;
        if (_currentHealth <= 0)
        {
            OnFamilyMemberDeath?.Invoke();
        }
    }

    public bool IsParent()
    {
        return _isParent;
    }
}
