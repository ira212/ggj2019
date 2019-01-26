using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _maxHealth;
    private int _currentHealth;
    private int _respawnTime;
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
        }
    }
}