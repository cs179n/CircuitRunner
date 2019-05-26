using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private Transform frontTransform;
    private Transform backTransform;
    private Transform closestTransform;
    public GameObject prevRail;
    public GameObject nextRail;
    RailsController railControllerScript;
    public GameObject playerGO;
    

    private float length;
    private float poweredTimer;

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
        this.railControllerScript = this.transform.parent.GetComponent<RailsController>();
    }

    // Update is called once per frame
    void Update()
    {
        this.length = (frontTransform.position - backTransform.position).magnitude;

        poweredTimer -= Time.deltaTime;
        if (poweredTimer < 0f) poweredTimer = 0f;
    }

    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    void LateUpdate()
    {
        this.GetComponent<MeshRenderer>().material = (this.poweredTimer > 0f) ? railControllerScript.GetPowered() : railControllerScript.GetUnpowered();
    }

    public float getPlayerHorizontalDistance(Transform target) {
        Vector3 toPlayer = target.position - this.transform.position;
        Vector3 horizontal = Vector3.Project(toPlayer, this.transform.up);
        float distance = horizontal.magnitude - this.length/2;
        if (distance < 0f) return 0f;
        return distance;
    }
    public float getPlayerVerticalDistance(Transform target)
    {
        Vector3 toPlayer = target.position - this.transform.position;
        Vector3 horizontal = Vector3.Project(toPlayer, this.transform.up);
        Vector3 vertical = toPlayer - horizontal;
        return vertical.magnitude;
    }

    public Vector3 getClosestPosition(Transform target) {
        if (this.getPlayerHorizontalDistance(target) > 0f) {
            float startDistance = (this.frontTransform.position - target.position).magnitude;
            float endDistance = (this.backTransform.position - target.position).magnitude;
            return (startDistance < endDistance) ? this.frontTransform.position : this.backTransform.position;
        } else {
            float vDistance = this.getPlayerVerticalDistance(target);
            if (vDistance >= -0.01f && vDistance < 0.1f) poweredTimer = 1.5f;
            Vector3 toPlayer = target.position - this.transform.position;
            Vector3 horizontal = Vector3.Project(toPlayer, this.transform.up);
            Vector3 vertical = toPlayer - horizontal;
            return target.position - vertical;
        }
    }

    public float getPlayerDistance(Transform target) {
        return (target.position - this.getClosestPosition(target)).magnitude;
    }

    public Vector3 getFrontPosition() {
        return this.frontTransform.position;
    }

    public Vector3 getBackPosition() {
        return this.backTransform.position;
    }

    public bool isClosestToFront(Transform target) {
        float frontDistance = (target.position - this.frontTransform.position).magnitude;
        float backDistance = (target.position - this.backTransform.position).magnitude;
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

    public bool getIsPowered() {
        return (poweredTimer > 0f);
    }

    public void turnOnPower() {
        Debug.Log("on");
        this.poweredTimer = 1.5f;
    }

    public void turnOffPower() {
        // ????
    }

    public void setNextRail(Transform rail) {
        this.nextRail = rail.gameObject;
    }

    public void setPrevRail(Transform rail) {
        this.prevRail = rail.gameObject;
    }
}
