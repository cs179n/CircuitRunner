using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGateCollision : MonoBehaviour
{
    // Start is called before the first frame update
    LogicGate parentScript;
    void Start()
    {
        this.parentScript = this.transform.parent.GetComponent<LogicGate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with " + other);
        if (other.gameObject.CompareTag("Rail")) {
            if (!this.parentScript.railInputs.Contains(other.gameObject)) {
                this.parentScript.railInputs.Add(other.gameObject);
            }
        }
    }

}
