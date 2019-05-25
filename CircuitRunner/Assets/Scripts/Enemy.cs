using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isDead = false;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        if (this.target == null) this.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.isDead = false; // TODO: Remove this when Movement script's adjustPosition/Velocity can be disabled
        if (this.isDead) {
            Debug.Log(this + "DEAD");
            Vector3 currentPos = this.transform.position;
            Vector3 targetPos = this.transform.position + new Vector3(2f, 2f, 0f);
            float stepPos = 30f * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(currentPos,targetPos,stepPos);
        } else {
            // move enemy
            
            Vector3 toTarget = (target.position - this.transform.position);
            Vector3 force = toTarget.normalized * 0.001f;
            this.GetComponent<Movement>().addForce(force, true);
        }
    }

    public void knockOut(Vector3 force) {
        Debug.Log("punched");
        this.isDead = true;
    }
}
