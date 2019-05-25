using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Movement movementScript;
    public GameObject chargePrefab;
    // Start is called before the first frame update
    void Start()
    {
        movementScript = this.transform.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player controls
        Vector3 forwardForce = movementScript.getCurrentRailUp() * 0.01f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            movementScript.addForce(forwardForce);
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            movementScript.addForce(-forwardForce);
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movementScript.moveVertical(this.transform.up);
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movementScript.moveVertical(-this.transform.up);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject charge1GO = Instantiate(chargePrefab, this.transform.position, Quaternion.identity);
            GameObject charge2GO = Instantiate(chargePrefab, this.transform.position, Quaternion.identity);
            charge1GO.transform.parent = GameObject.FindGameObjectWithTag("ChargeContainer").transform;
            charge2GO.transform.parent = GameObject.FindGameObjectWithTag("ChargeContainer").transform;
            charge1GO.GetComponent<Charge>().reachEnd(5f, true);
            charge2GO.GetComponent<Charge>().reachEnd(1f, false);
        }
    }
}
