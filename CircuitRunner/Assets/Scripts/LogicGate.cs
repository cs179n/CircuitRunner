using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : MonoBehaviour
{
    bool isPowered = false;
    public List<GameObject> railInputs;
    Vector3 setposition;
    Vector3 offposition;
    public enum GateType {
        AND, OR, NOT, XOR, NAND, NOR, XNOR, ODD, EVEN, CLOSED, OPEN
    }
    public GateType gateType = GateType.AND;
    Transform wall;
    public Font font;

    // Start is called before the first frame update
    void Start()
    {
        setposition = this.transform.position;
        offposition = this.transform.position + new Vector3(0, 0, 15f);
        wall = this.transform.GetChild(0);

        // Generate a TextMesh in front of this gate
        
        GameObject textGO = new GameObject();
        textGO.transform.parent = this.transform;
        textGO.transform.localPosition = new Vector3(0,0,0);
        textGO.transform.localRotation = Quaternion.identity;
        TextMesh text = textGO.AddComponent<TextMesh>();
        text.text = this.getGateName();
        text.fontSize = 10;
        text.anchor = TextAnchor.MiddleCenter;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("parent collision " + other);
        if (other.gameObject.CompareTag("Rail")) {
            if (!this.railInputs.Contains(other.gameObject)) {
                this.railInputs.Add(other.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int count = this.getCount(this.railInputs);
        int total = this.railInputs.Count;
        this.isPowered = this.getIsPowered(count, total);

        
        this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = !this.isPowered;
        
        //this.retract();
        this.retract2();
    }

    void retract2() {
        Transform topPivot = this.transform.GetChild(1);
        Transform botPivot = this.transform.GetChild(2);
        if (this.isPowered)
        {
            topPivot.localScale -= new Vector3(0, 0.1f, 0);
            if (topPivot.localScale.y < 0f) topPivot.localScale = new Vector3(1, 0, 1);
            botPivot.localScale -= new Vector3(0, 0.1f, 0);
            if (botPivot.localScale.y < 0f) botPivot.localScale = new Vector3(1, 0, 1);
        }
        else
        {
            topPivot.localScale += new Vector3(0, 0.1f, 0);
            if (topPivot.localScale.y > 1f) topPivot.localScale = new Vector3(1, 1, 1);
            botPivot.localScale += new Vector3(0, 0.1f, 0);
            if (botPivot.localScale.y > 1f) botPivot.localScale = new Vector3(1, 1, 1);
        }
    }

    void retract() {
         // TODO: Change localposition instead
        float step = 30f * Time.deltaTime;
        Vector3 targetPosition = (this.isPowered) ? offposition : setposition;
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
    }

    public int getCount(List<GameObject> railArray) {
        int count = 0;
        foreach (GameObject rail in railArray)
        {
            bool power = rail.GetComponent<Rail>().getIsPowered();
            if (power) count++;
        }
        return count;
    }

    public bool getIsPowered(int count, int total) {
        switch(this.gateType) {
            case GateType.AND:
                return (count == total);
            case GateType.OR:
                return (count >= 1);
            case GateType.NOT:
                return (count == 0);
            case GateType.XOR:
                return (count % 2 != 0);
            case GateType.NAND:
                return (count < total);
            case GateType.NOR:
                return (count == 0);
            case GateType.XNOR:
                return (count % 2 == 0);//(count == 0 || count == total);
            case GateType.ODD:
                return (count % 2 != 0);
            case GateType.EVEN:
                return (count % 2 == 0);
            case GateType.OPEN:
                return true;
            default:
                return false;
        }
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
}
