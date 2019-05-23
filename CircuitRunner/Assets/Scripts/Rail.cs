using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private Transform frontTransform;
    private Transform backTransform;
    private Transform closestTransform;
    public GameObject playerGO;
    public GameObject prevRail;
    public GameObject nextRail;

    private float length;
    private bool isPowered = false;

    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        this.frontTransform = this.transform.GetChild(0);
        this.backTransform = this.transform.GetChild(1);
        this.closestTransform = this.transform.GetChild(2);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Move start and end to its corresponding positions with respect to this cyllinder
        this.frontTransform.position += this.transform.up * this.transform.localScale.y;
        this.backTransform.position -= this.transform.up * this.transform.localScale.y;

        this.length = (frontTransform.position - backTransform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        this.length = (frontTransform.position - backTransform.position).magnitude;
        if (this.playerGO.GetComponent<PlayerMovement>().getCurrentRail().gameObject.GetInstanceID() == this.GetInstanceID()) {
            this.turnOnPower();
            //this.transform.localScale = new Vector3(.5f,5f,.1f);
        } else {
            //this.turnOffPower();
            //this.transform.localScale = new Vector3(.1f,5f,.1f);
        }

        //this.transform.localScale = (this.isPowered) ? new Vector3(.5f,5f,.1f) : new Vector3(.1f,5f,.1f);
    }

    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    void LateUpdate()
    {
        this.closestTransform.position = this.getClosestPosition();
    }

    public bool getIsPowered() {
        return this.isPowered;
    }
    public void turnOnPower() {
        this.isPowered = true;
    }
    public void turnOffPower() {
        this.isPowered = false;
    }
    public void togglePower() {
        this.isPowered = !this.isPowered;
    }

    public float getPlayerHorizontalDistance() {
        Vector3 toPlayer = this.playerGO.transform.position - this.transform.position;
        Vector3 horizontal = Vector3.Project(toPlayer, this.transform.up);
        float distance = horizontal.magnitude - this.length/2f;
        return (distance > 0f) ? distance : 0f;
    }

    public float getPlayerVerticalDistance() {
        Vector3 toPlayer = this.playerGO.transform.position - this.transform.position;
        Vector3 horizontal = Vector3.Project(toPlayer, this.transform.up);
        Vector3 vertical = toPlayer - horizontal;
        return vertical.magnitude;
    }

    public Vector3 getClosestPosition() {
        if (this.getPlayerHorizontalDistance() > 0f) {
            float startDistance = (this.frontTransform.position - this.playerGO.transform.position).magnitude;
            float endDistance = (this.backTransform.position - this.playerGO.transform.position).magnitude;
            return (startDistance < endDistance) ? this.frontTransform.position : this.backTransform.position;
        } else {
            Vector3 toPlayer = this.playerGO.transform.position - this.transform.position;
            Vector3 horizontal = Vector3.Project(toPlayer, this.transform.up);
            Vector3 vertical = toPlayer - horizontal;
            return this.playerGO.transform.position - vertical;
        }
    }

    public float getPlayerDistance() {
        return (this.playerGO.transform.position - this.getClosestPosition()).magnitude;
    }

    public Vector3 getFrontPosition() {
        return this.frontTransform.position;
    }

    public Vector3 getBackPosition() {
        return this.backTransform.position;
    }

    public bool isClosestToFront() {
        float frontDistance = (this.playerGO.transform.position - this.frontTransform.position).magnitude;
        float backDistance = (this.playerGO.transform.position - this.backTransform.position).magnitude;
        return (frontDistance < backDistance);
    }

    public Transform getNextRail() {
        return (this.nextRail) ? this.nextRail.transform : null;
    }

    public Transform getPrevRail() {
        return (this.prevRail) ? this.prevRail.transform : null;
    }

    public float getLength() {
        return this.length;
    }

    public bool setColliderTrigger(bool value) {
        bool oldValue = this.GetComponent<CapsuleCollider>().isTrigger;
        this.GetComponent<CapsuleCollider>().isTrigger = value;
        return oldValue;
    }
}
