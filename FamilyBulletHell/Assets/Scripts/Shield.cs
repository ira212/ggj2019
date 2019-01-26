using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _maxHealth;
    private int _currentHealth;
    public int respawnTime;
    private float _respawnTimeLeft;
    private bool _respawnActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageTaken)
    {
        _currentHealth -= damageTaken;
        if (_currentHealth <= 0)
        {
           gameObject.SetActive(false);
           _respawnActive = true;
        }
    }

    public void ShieldRespawnTimer()
    {
        while(_respawnActive == true)
        {
            _respawnTimeLeft -= Time.deltaTime;
            if(_respawnTimeLeft <= 0)
            {
                gameObject.SetActive(true);
                _respawnActive = false;
            }
        }
    }
}