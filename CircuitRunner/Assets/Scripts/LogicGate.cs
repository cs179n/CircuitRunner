using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : MonoBehaviour
{
    bool isPowered = false;
    public GameObject[] railInputs;
    Vector3 offset = new Vector3(0, 0, 15f);
    Vector3 setposition;
    Vector3 offposition;
    public enum GateType {
        AND,
        OR,
        NOT,
        XOR,
        NAND,
        NOR,
        XNOR,
        ODD,
        EVEN,
        CLOSED,
        OPEN
    }
    public GateType gateType = GateType.AND;
    Transform wall;

    // Start is called before the first frame update
    void Start()
    {
        setposition = this.transform.position;
        offposition = this.transform.position + offset;
        wall = this.transform.GetChild(0);

        GameObject textGO = new GameObject();
        textGO.transform.parent = this.transform;
        textGO.transform.localPosition = new Vector3(0,0,0);
        textGO.transform.localRotation = Quaternion.identity;
        TextMesh text = textGO.AddComponent<TextMesh>();
        text.text = this.getGateName();
        text.fontSize = 10;
    }

    string getGateName() {
        string name = "";
        switch(this.gateType) {
            case GateType.AND:
                name += "AND";
                break;
             case GateType.OR:
                name += "OR";
                break;
            case GateType.NOT:
                name += "NOT";
                break;
            case GateType.XOR:
                name += "XOR";
                break;
            case GateType.NAND:
                name += "NAND";
                break;
            case GateType.NOR:
                name += "NOR";
                break;
            case GateType.XNOR:
                name += "XNOR";
                break;
            case GateType.ODD:
                name += "ODD";
                break;
            case GateType.EVEN:
                name += "EVEN";
                break;
            case GateType.OPEN:
                name += "OPEN";
                break;
            case GateType.CLOSED:
                name += "CLOSED";
                break;
            default:
                name += "--default--";
                break;
        }
        return name;
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
            case GateType.AND:
                this.isPowered = (count == total);
                break;
            case GateType.OR:
                this.isPowered = (count >= 1);
                break;
            case GateType.NOT:
                this.isPowered = (count == 0);
                break;
            case GateType.XOR:
                this.isPowered = (count % 2 != 0);
                break;
            case GateType.NAND:
                this.isPowered = (count < total);
                break;
            case GateType.NOR:
                this.isPowered = (count == 0);
                break;
            case GateType.XNOR:
                this.isPowered = (count == 0 || count == total);
                break;
            case GateType.ODD:
                this.isPowered = (count % 2 != 0);
                break;
            case GateType.EVEN:
                this.isPowered = (count % 2 == 0);
                break;
            case GateType.OPEN:
                this.isPowered = true;
                break;
            default:
                this.isPowered = false;
                break;
        }
        Vector3 closedPosition = new Vector3(0f, 0f, 0f);
        //Vector3 openedPosition = new Vector3(0f, 0f, 20f);
        float step = 30f * Time.deltaTime;

        if (this.isPowered) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, offposition, step);
        } else {
            this.transform.position = Vector3.MoveTowards(this.transform.position, setposition, step);
        }
    }
}
