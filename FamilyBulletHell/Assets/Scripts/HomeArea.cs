using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeArea : MonoBehaviour
{
    private Collider _homeAreaCollider;

    // Start is called before the first frame update
    void Start()
    {
        _homeAreaCollider = gameObject.GetComponent<Collider>();
    }

    // Checks if a given game object is within this game object
    public bool Contains (GameObject myObject)
    {
        Collider myObjectCollider = myObject.GetComponent<Collider>();

        // check if myObject is within myself
        if (myObjectCollider != null && myObjectCollider.bounds.Intersects(_homeAreaCollider.bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
