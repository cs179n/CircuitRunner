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
    Transform topRight;
    Transform topLeft;
    Transform bottomRight;
    Transform bottomLeft;

    void Awake()
    {

    }

    void Start()
    {
        // Child objects
        topRight = this.transform.GetChild(0);
        topLeft = this.transform.GetChild(1);
        bottomRight = this.transform.GetChild(2);
        bottomLeft = this.transform.GetChild(3);
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
                this.isPowered = (count == this.railInputs.Length);
                break;
            case GateType.Or:
                this.isPowered = (count >= 1);
                break;
            default:
                this.isPowered = false;
                break;
        }
        float move = 4f;
        if (this.isPowered) {
            Debug.Log("gate is powered");
            
            topRight.localPosition = new Vector3(0f, 1f, -4.5f) + new Vector3(0f,move,-move);
            topLeft.localPosition  = new Vector3(0f, 1f, 4.5f)  + new Vector3(0f,move,move);

            bottomRight.localPosition = new Vector3(0f, -1f, -4.5f) + new Vector3(0f,-move,-move);
            bottomLeft.localPosition  = new Vector3(0f, -1f, 4.5f)  + new Vector3(0f,-move,move);
        } else {
            topRight.localPosition = new Vector3(0f, 1f, -4.5f);
            topLeft.localPosition  = new Vector3(0f, 1f, 4.5f);

            bottomRight.localPosition = new Vector3(0f, -1f, -4.5f);
            bottomLeft.localPosition  = new Vector3(0f, -1f, 4.5f);
        }
    }

}

