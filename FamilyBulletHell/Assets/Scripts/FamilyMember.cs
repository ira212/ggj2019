using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyMember : MonoBehaviour
{
    private int _maxHealth;
    private int _currentHealth;
    private float _speed;

    public void SpawnFamilyMember(int maxHP, float moveSpeed)
    {
        _maxHealth = maxHP;
        _currentHealth = maxHP;
        _speed = moveSpeed;
    }

    public void TakeDamage(int damageTaken)
    {
        _currentHealth -= damageTaken;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
