using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision!");
        if (other.gameObject.CompareTag("Item")) {
            // TODO: Instead of immediately destroying the other object
            // Tell the other object it has been collided with, and to start
            // its destruction function (which may include some animation)
            Destroy(other.gameObject);
        }
    }
}
