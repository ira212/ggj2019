using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private Rigidbody _playerRB;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        _playerRB.velocity = movement * Global.Instance.SquareSpeed;
    }

    public void Spawn(Vector3 spawnPosition)
    {
        GetComponent<FamilyMember>().SpawnFamilyMember(Global.Instance.StartHP, Global.Instance.SquareSpeed, true);
        gameObject.transform.position = spawnPosition;

    }
}
