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
    public enum GateShape { /* width x height */
        _1x1, _1x3
    }
    public GateType gateType = GateType.AND;
    public GateShape gateShape = GateShape._1x1;
    public Font font;


    Transform topPivot, botPivot, frameworkLeft, frameworkRight, frameworkTop, frameworkBottom, body;

    // Start is called before the first frame update
    void Start()
    {
        body            = this.transform.GetChild(0).GetChild(0);
        topPivot        = this.transform.GetChild(0).GetChild(1);
        botPivot        = this.transform.GetChild(0).GetChild(2);
        frameworkLeft   = this.transform.GetChild(0).GetChild(3).GetChild(0);
        frameworkRight  = this.transform.GetChild(0).GetChild(3).GetChild(1);
        frameworkTop    = this.transform.GetChild(0).GetChild(3).GetChild(2);
        frameworkBottom = this.transform.GetChild(0).GetChild(3).GetChild(3);

        createText();
        reshape();
    }

    void reshape() {
        
        switch (this.gateShape) {
            case GateShape._1x3:
                this.frameworkLeft.localScale    = new Vector3(0.025f, 1.0f, 0.1f);
                this.frameworkRight.localScale   = new Vector3(0.025f, 1.0f, 0.1f);
                this.frameworkTop.localScale     = new Vector3(0.0125f, 0.45f, 0.1f);
                this.frameworkBottom.localScale  = new Vector3(0.0125f, 0.45f, 0.1f);
                break;
            case GateShape._1x1:
                this.frameworkLeft.localScale    = new Vector3(0.025f, 1.0f, 0.1f);
                this.frameworkRight.localScale   = new Vector3(0.025f, 1.0f, 0.1f);
                this.frameworkTop.localScale     = new Vector3(0.05f, 0.45f, 0.1f);
                this.frameworkBottom.localScale  = new Vector3(0.05f, 0.45f, 0.1f);
                break;
            default:
                break;
        }
    }

    void createText() {
        GameObject textGO = new GameObject();
        textGO.name = "Label";
        textGO.transform.parent = this.transform;
        textGO.transform.localPosition = new Vector3(0,0.5f,0);
        textGO.transform.localRotation = Quaternion.identity;
        textGO.transform.localScale = new Vector3(0.5f,1,1);

        TextMesh text = textGO.AddComponent<TextMesh>();
        text.text = this.getGateName();
        text.fontSize = 12;
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

        
        this.body.GetComponent<BoxCollider>().enabled = !this.isPowered;
        
        //this.retract();
        this.retract();
    }

    void retract() {
        // Open and closes the gate depending on it's powered state
        if (this.isPowered)
        {
            this.topPivot.localScale -= new Vector3(0, 0.1f, 0);
            if (this.topPivot.localScale.y <= 0f) {
                this.topPivot.localScale = new Vector3(1, 0, 1);
                //this.topPivot.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            this.botPivot.localScale -= new Vector3(0, 0.1f, 0);
            if (this.botPivot.localScale.y <= 0f) {
                this.botPivot.localScale = new Vector3(1, 0, 1);
                //this.botPivot.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            this.topPivot.localScale += new Vector3(0, 0.1f, 0);
            if (this.topPivot.localScale.y >= 1f) {
                this.topPivot.localScale = new Vector3(1, 1, 1);
                //this.topPivot.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            this.botPivot.localScale += new Vector3(0, 0.1f, 0);
            if (this.botPivot.localScale.y >= 1f) {
                this.botPivot.localScale = new Vector3(1, 1, 1);
                //this.botPivot.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void retract_OLD() {
        // setposition must be a position calculated at Start()
        // offposition must be a field variable
        //float step = 30f * Time.deltaTime;
        //Vector3 targetPosition = (this.isPowered) ? offposition : setposition; 
        //this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
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
