using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : MonoBehaviour
{
    bool isPowered = false;
    public GameObject[] railInputs;
    public enum GateType {
        And,
        Or,
        AlwaysOpen
    }
    public GateType gateType = GateType.And;
    Transform wall;

    // Start is called before the first frame update
    void Start()
    {
        // Child objects
        wall = this.transform.GetChild(0);
        // topLeft = this.transform.GetChild(1);
        // bottomRight = this.transform.GetChild(2);
        // bottomLeft = this.transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        foreach (GameObject go in this.railInputs)
        {
            bool power = go.GetComponent<Rail>().getIsPowered();
            if (power) count++;
        }
        switch(this.gateType) {
            case GateType.And:
                this.isPowered = (count == this.railInputs.Length && this.railInputs.Length >= 1);
                break;
            case GateType.Or:
                this.isPowered = (count >= 1  && this.railInputs.Length >= 1);
                break;
            case GateType.AlwaysOpen:
                this.isPowered = true;
                break;
            default:
                this.isPowered = false;
                break;
        }
        float ymove = 0f;
        float zmove = 100f;
        Vector3 closedPosition = new Vector3(0f, 0f, 0f);
        Vector3 openedPosition = new Vector3(0f, 0f, 10f);
        float step = 10f * Time.deltaTime;
        if (this.isPowered) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, openedPosition, step);
            //topRight.localPosition = closedPosition + new Vector3(0f, ymove, zmove);
            //topLeft.localPosition  = closedPosition  + new Vector3(0f, ymove, zmove);

            //bottomRight.localPosition = closedPosition + new Vector3(0f, ymove, zmove);
            //bottomLeft.localPosition  = closedPosition  + new Vector3(0f, ymove, zmove);
        } else {
            this.transform.position = Vector3.MoveTowards(this.transform.position, closedPosition, step);
            //topRight.localPosition = closedPosition;
            //topLeft.localPosition  = closedPosition;

            //bottomRight.localPosition = closedPosition;
            //bottomLeft.localPosition  = closedPosition;
        }
    }
}
