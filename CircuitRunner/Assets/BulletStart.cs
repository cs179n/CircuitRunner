using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStart : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject target;
    Vector3 targetPos;
    
    void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player");
        this.transform.GetChild(0).LookAt(this.target.transform);
        this.targetPos = this.target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.moveForward();
        this.spin();
        
        Destroy(this.gameObject, 3f);
        
        
    }

    void moveForward() {
        Vector3 currentPos = this.transform.position;
        float step = 10f * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(currentPos, this.targetPos, step);
    }

    void spin() {
        transform.GetChild(0).GetChild(0).Rotate (0,0,500f*Time.deltaTime);
    }
}
