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
    private Vector3 _destination;

    private float _minX, _minY, _maxX, _maxY;

    public void InitBehavior(float attention, float speed, float lowX, float highX, float lowY, float highY)
    {
        _attentionSpan = attention;
        _speed = speed;

        // set bounding box variables
        _minX = lowX;
        _minY = lowY;
        _maxX = highX;
        _maxY = highY;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWalking)
        {
            _walkTime -= Time.deltaTime;
            transform.position += _direction * _speed * Time.deltaTime;

            if (_walkTime <= 0 || Vector3.Magnitude(transform.position - _destination) <= 0.5f)
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

                float xDir = Random.Range(_minX, _maxX);
                float yDir = Random.Range(_minX, _maxY);
                _destination = new Vector3(xDir, yDir, 0);
                _direction = Vector3.Normalize(_destination - transform.position);
                _walkTime = Random.Range(1.0f, 3.0f);
            }
        }
    }
}
