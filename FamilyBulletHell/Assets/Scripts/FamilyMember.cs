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

    public void Update()
    {
        HealthText.text = _currentHealth.ToString();
    }

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
        Player player = GetComponent<Player>();
        if (_currentHealth <= 0)
        {
            OnFamilyMemberDeath?.Invoke();
        }
        else if (_currentHealth <= 5)
        {
            if (player != null)
            {
                if (AudioManager.Instance.IsPlaying("Partner1-Happy"))
                {
                    AudioManager.Instance.TransitionTracks("Partner1-Happy", "Partner1-Unhappy");
                }
                else
                {
                    if (_isParent)
                    {
                        if (AudioManager.Instance.IsPlaying("Partner2-Happy"))
                        {
                            AudioManager.Instance.TransitionTracks("Partner2-Happy", "Partner2-Unhappy");
                        }
                    }
                    else
                    {
                        if (AudioManager.Instance.IsPlaying("Child-Happy"))
                        {
                            AudioManager.Instance.TransitionTracks("Child-Happy", "Child-Unhappy");
                        }
                    }
                }
            }
        }
        else
        {
            if (player != null)
            {
                if (AudioManager.Instance.IsPlaying("Partner1-Unhappy"))
                {
                    AudioManager.Instance.TransitionTracks("Partner1-Unhappy", "Partner1-Happy");
                }
                else
                {
                    if (_isParent)
                    {
                        if (AudioManager.Instance.IsPlaying("Partner2-Unhappy"))
                        {
                            AudioManager.Instance.TransitionTracks("Partner2-Unhappy", "Partner2-Happy");
                        }
                    }
                    else
                    {
                        if (AudioManager.Instance.IsPlaying("Child-Unhappy"))
                        {
                            AudioManager.Instance.TransitionTracks("Child-Unhappy", "Child-Happy");
                        }
                    }
                }
            }
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
