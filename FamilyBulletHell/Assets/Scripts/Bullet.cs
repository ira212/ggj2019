using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;
    private int _damage;

    public void Spawn(Vector3 spawnPosition, Vector3 destination, float projectileSpeed)
    {
        gameObject.transform.position = spawnPosition;
        _direction = Vector3.Normalize(destination - spawnPosition);
        _speed = projectileSpeed;
        _damage = (int)(_speed * Global.Instance.BulletDamageSpeedFactor);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position + (_direction * _speed * Time.deltaTime);
        transform.position = newPos;
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
