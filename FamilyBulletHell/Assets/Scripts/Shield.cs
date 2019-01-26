using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _maxHealth;
    private int _currentHealth;
    private float _respawnTimeLeft;
    private bool _shieldActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TakeDamage(int damageTaken)
    {
        _currentHealth -= damageTaken;
        if (_currentHealth <= 0)
        {
           gameObject.SetActive(false);
           _shieldActive = false;
        }
    }

    public void ShieldRespawnTimer(float respawnTime)
    {
        _respawnTimeLeft = respawnTime;
        while(_shieldActive == false)
        {
            _respawnTimeLeft -= Time.deltaTime;
            if(_respawnTimeLeft <= 0)
            {
                gameObject.SetActive(true);
                _shieldActive = true;
            }
        }
    }

    public bool IsShieldActive()
    {
        return _shieldActive;
    }
}