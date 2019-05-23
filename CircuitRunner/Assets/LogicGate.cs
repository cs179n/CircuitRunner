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
        Not,
        Xor,
        Nand,
        Nor,
        Xnor,
        Odd,
        Even,
        AlwaysClosed,
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
        int total = this.railInputs.Length;
        switch(this.gateType) {
            case GateType.And:
                this.isPowered = (count == total);
                break;
            case GateType.Or:
                this.isPowered = (count >= 1);
                break;
            case GateType.Not:
                this.isPowered = (count == 0);
                break;
            case GateType.Xor:
                this.isPowered = (count == 1);
                break;
            case GateType.Nand:
                this.isPowered = (count < total);
                break;
            case GateType.Nor:
                this.isPowered = (count == 0);
                break;
            case GateType.Xnor:
                this.isPowered = (count == 0 || count == total);
                break;
            case GateType.Odd:
                this.isPowered = (count % 2 != 0);
                break;
            case GateType.Even:
                this.isPowered = (count % 2 == 0);
                break;
            case GateType.AlwaysOpen:
                this.isPowered = true;
                break;
            default:
                this.isPowered = false;
                break;
        }
        Vector3 closedPosition = new Vector3(0f, 0f, 0f);
        Vector3 openedPosition = new Vector3(0f, 0f, 20f);
        float step = 10f * Time.deltaTime;
        if (this.isPowered) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, openedPosition, step);
        } else {
            this.transform.position = Vector3.MoveTowards(this.transform.position, closedPosition, step);
        }
    }
}
