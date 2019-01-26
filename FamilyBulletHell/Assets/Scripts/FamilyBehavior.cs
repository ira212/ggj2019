using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyBehavior : MonoBehaviour
{
    private float _attentionSpan;
    private float _speed;

    private float _waitTime;
    private float _walkTime;
    
    private bool _isWalking;
    private bool _isWaiting;

    private Vector3 _direction;

    public void InitBehavior(float attention, float speed)
    {
        _attentionSpan = attention;
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWalking)
        {
            _walkTime -= Time.deltaTime;
            transform.position += _direction * _speed * Time.deltaTime;

            if (_walkTime <= 0)
            {
                _isWalking = false;
                _isWaiting = true;

                _waitTime = Random.Range(1.0f, 2.0f);
            }
        }
        else
        {
            _waitTime -= Time.deltaTime;

            if (_waitTime <= 0)
            {
                _isWaiting = false;
                _isWalking = true;

                float xDir = Random.Range(-1, 1);
                float yDir = Random.Range(-1, 1);
                _direction = new Vector3(xDir, yDir, 0).normalized;
                _walkTime = Random.Range(1.0f, 3.0f);
            }
        }
    }
}
