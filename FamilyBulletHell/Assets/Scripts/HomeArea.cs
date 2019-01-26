using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeArea : MonoBehaviour
{

    public System.Action OnHomeAreaEntry;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Checks if a given game object is within this game object
    public bool Contains (GameObject myObject)
    {
        Collider myObjectCollider = myObject.GetComponent<Collider>();
        Collider HomeAreaCollider = gameObject.GetComponent<Collider>();
        // check if myObject is within myself
        if (myObjectCollider.bounds.Intersects(HomeAreaCollider.bounds))
            return true;
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            OnHomeAreaEntry?.Invoke();
        }
       
    }
}
