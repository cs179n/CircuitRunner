using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    float distance = 1f;
    int direction = 1;
    Transform currentRail = null;
    GameObject nextCharge = null;
    public GameObject chargePrefab;
    // Start is called before the first frame update
    void Start()
    {
        this.currentRail = this.findClosestRail();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reachEnd(float distance, bool forward) {
        this.currentRail = this.findClosestRail();
        Transform endpoint = (forward) ? this.currentRail.GetChild(0) : this.currentRail.GetChild(1);
        Vector3 movement = (endpoint.position - this.transform.position);

        if (distance <= movement.magnitude) {
            this.transform.position += movement.normalized * distance;
            distance = 0;
        } else {
            this.transform.position = endpoint.transform.position;
            Rail railScript = this.currentRail.GetComponent<Rail>();
            Transform rail = (forward) ? railScript.getNextRail() : railScript.getPrevRail(); // TODO: Return an array
            if (rail) {
                distance -= movement.magnitude;
                //GameObject chargeGO = Resources.Load("PreFabs/Charge") as GameObject;
                //GameObject chargeGO = Instantiate(this.chargePrefab, this.transform.position, Quaternion.identity);
                //chargeGO.transform.parent = GameObject.FindGameObjectWithTag("ChargeContainer").transform;
                //chargeGO.GetComponent<Charge>().reachEnd(distance, forward);
            }
        }
        

    }

    Transform findClosestRail() {
        Transform closest = null;
        float smallestDistance = Mathf.Infinity;
        GameObject railContainer = GameObject.FindGameObjectWithTag("RailContainer");
        foreach(Transform rail in railContainer.transform) {
            float distance = rail.GetComponent<Rail>().getPlayerDistance(this.transform);
            if (distance < smallestDistance) {
                smallestDistance = distance;
                closest = rail;
            }
        }
//        closest.GetComponent<Rail>().turnOnPower();
        return closest;
    }
}
