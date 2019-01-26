using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;
    private int _damage;
    private Material BulletColor;
    private float _timeInWorld;

    public void Spawn(Vector3 spawnPosition, Vector3 destination, float projectileSpeed)
    {
        gameObject.transform.position = spawnPosition;
        _direction = Vector3.Normalize(destination - spawnPosition);
        _speed = projectileSpeed;
        float _BulletHeal = Random.Range(0.0f, 1.0f);
        if(_BulletHeal <= Global.Instance.BulletHealChance)
        {
            _damage = (int)(Global.Instance.BulletHealAmount);
            BulletColor = GetComponent<Renderer>().material;
            BulletColor.color = Color.green;
        }
        else if(_BulletHeal > Global.Instance.BulletHealChance)
        {
            _damage = (int)(Global.Instance.BulletDamageSpeedFactor / _speed);
            BulletColor = GetComponent<Renderer>().material;
            BulletColor.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position + (_direction * _speed * Time.deltaTime);
        transform.position = newPos;
        _timeInWorld = _timeInWorld + Time.deltaTime;
        if(_timeInWorld >= 10.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Shield>() != null)
        {
            collision.GetComponent<Shield>().TakeDamage(_damage);
            if (collision.GetComponent<Shield>().IsShieldActive())
            {
                Destroy(gameObject);
                return;
            }
        }

        if (collision.GetComponent<FamilyMember>() != null)
        {
            Debug.Log("Collision!");
            collision.GetComponent<FamilyMember>().TakeDamage(_damage);
            Destroy(gameObject);
        }        
    }
}
