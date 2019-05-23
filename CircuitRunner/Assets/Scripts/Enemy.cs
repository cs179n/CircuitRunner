using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isDead = false;
    private GameObject hitter;
    private Transform currentRail = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.isDead) {
            Vector3 currentPos = this.transform.position;
            Vector3 targetPos = this.transform.position + new Vector3(2f, 2f, 0f);
            float stepPos = 30f * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(currentPos,targetPos,stepPos);
        }
    }

    public void punched(Vector3 force) {
        Debug.Log("punched");
        this.isDead = true;
    }
}
