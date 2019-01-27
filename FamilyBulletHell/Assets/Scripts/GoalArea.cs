using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    public System.Action OnHealingTriggered;
    public System.Action<GoalArea> OnRespawnReady;

    // How long the goal area remains active
    private float _duration;

    // # of seconds per 1 health
    private float _healDelay;
    private float _timeBeforeHeal;

    private bool _isActive;
    private bool _healingActive;

    private float _areaRespawnTimer;
    private float _respawnTimeRemaining;

    private float _pointsTimer;

    private Collider _areaCollider;

    private void Start()
    {
        _areaCollider = GetComponent<Collider>();
        _pointsTimer = 0.0f;
    }

    public void ActivateGoalArea(Vector3 spawnPos, float duration, float healDelay)
    {
        transform.position = spawnPos;
        _duration = duration;
        _healDelay = healDelay;
        _timeBeforeHeal = _healDelay;
        _isActive = true;

        _areaRespawnTimer = Global.Instance.GoalAreaRespawnTimer;

        gameObject.SetActive(true);
    }

    public void DeactivateGoalArea()
    {
        _isActive = false;
        gameObject.SetActive(false);
        _respawnTimeRemaining = _areaRespawnTimer;
    }

    private void Update()
    {
        if (_isActive)
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0)
            {
                // deactivate goal area
                DeactivateGoalArea();
            }

            if (_healingActive)
            {
                _timeBeforeHeal -= Time.deltaTime;
                if (_timeBeforeHeal <= 0)
                {
                    // heal family members and reset timer
                    OnHealingTriggered?.Invoke();
                    _timeBeforeHeal = _healDelay;
                }
            }
        }
    }

    public bool Contains(GameObject obj)
    {
        Collider objectCollider = obj.GetComponent<Collider>();
        if (objectCollider != null && _areaCollider.bounds.Intersects(objectCollider.bounds))
        {
            return true;
        }

        return false;
    }

    public void ActivateHealing()
    {
        _healingActive = true;
    }

    public void DeactivateHealing()
    {
        _healingActive = false;
        _timeBeforeHeal = _healDelay;
    }

    public bool IsHealingActive()
    {
        return _healingActive;
    }

    public bool IsAreaActive()
    {
        return _isActive;
    }

    public void UpdateRespawnTimer(float delta)
    {
        _respawnTimeRemaining -= delta;
        if (_respawnTimeRemaining <= 0)
        {
            // tell GameManager to activate this again?
            OnRespawnReady?.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            AudioManager.Instance.PlaySFX("Partner1-Enters-GoalZone");
        }
        else
        {
            FamilyMember fam = other.GetComponent<FamilyMember>();
            if (fam != null)
            {
                if (fam.IsParent())
                {
                    AudioManager.Instance.PlaySFX("Partner2-Enters-GoalZone");
                }
                else
                {
                    AudioManager.Instance.PlaySFX("Partner2-Enters-GoalZone");
                }
            }
        }
    }
}
