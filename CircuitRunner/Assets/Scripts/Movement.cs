using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool debugger = false;
    // Start is called before the first frame update
    public Transform currentRail = null;
    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    public GameObject railContainer;
    private Vector3 prevPosition;

    void Start()
    {
        this.railContainer = GameObject.FindGameObjectWithTag("RailContainer");
        // Transform rail = this.findClosestRail();
        // if (rail) {
        //     this.currentRail = rail;
        //     Vector3 backPosition = rail.GetComponent<Rail>().getBackPosition();
        //     this.transform.position = backPosition;
        // }
        this.prevPosition = this.transform.position;
    }

    void Update()
    {
        Transform closestRail = this.findClosestRail();
        if (closestRail == null || PauseMenuController.IsGamePause) {
            return;
        }
        this.currentRail = closestRail;
        Vector3 closestPosition = this.currentRail.GetComponent<Rail>().getClosestPosition(this.transform);
        // Place the player on the closest rail
        //this.currentRail = closestRail;
        if (this.currentRail && !PauseMenuController.IsGamePause) {
            this.transform.rotation = this.currentRail.rotation;
            this.transform.Rotate(0,0,90f);
            //Vector3 closestPosition = this.currentRail.GetComponent<Rail>().getClosestPosition(this.transform);
            //Debug.DrawLine(this.transform.position, closestPosition, Color.magenta);
        }

        this.eulerStep();

        this.adjustHorizontalPosition();
        this.adjustVerticalPosition();
        this.adjustVelocity();

        this.acceleration = Vector3.zero;
        this.prevPosition = this.transform.position;
    }

    void adjustVerticalPosition() {
        Rail railScript = this.currentRail.GetComponent<Rail>();
        float verticalDistance = railScript.getPlayerVerticalDistance(this.transform);
        if (verticalDistance > 0f) {
            this.transform.position = railScript.getClosestPosition(this.transform);
        }
    }

    Vector3 gravity(Vector3 gravityPosition, Transform rail) {
        Vector3 gravityForce = (gravityPosition - this.transform.position).normalized;
        Vector3 proj = Vector3.Project(gravityForce, rail.transform.up);
        gravityForce -= proj; // rejection vector
        float gravityFactor = 0.05f;
        return gravityForce * gravityFactor;
    }

    void eulerStep() {
        this.velocity += this.acceleration;
        this.transform.position += this.velocity;
    }

    bool hasCrossedRail(Transform closestRail) {
        Vector3 prevToRail = (closestRail.transform.position-this.prevPosition);
        Vector3 toRail = (closestRail.transform.position-this.transform.position);
        float prevDot = Vector3.Dot(prevToRail, closestRail.transform.right);
        float dot = Vector3.Dot(toRail, closestRail.transform.right);
        bool crossedRail = (prevDot * dot < 0);
        if (debugger) Debug.DrawRay(this.transform.position, toRail, Color.red, 1f);
        if (debugger) Debug.Log(Mathf.Sign(dot) + ", " + Mathf.Sign(prevDot) + " crossedRail: "+crossedRail);
        return crossedRail;
    }

    public Vector3 getCurrentRailUp() {
        
        return (this.currentRail) ? this.currentRail.transform.up : Vector3.zero;
    }

    void adjustVelocity() {
        float speed = this.velocity.magnitude;
        float direction = Vector3.Dot(this.velocity.normalized, this.currentRail.transform.up.normalized);
        this.velocity = this.currentRail.transform.up * speed * direction;
    }

    public void addForce(Vector3 force, bool stayOnRail=false) {
        if (stayOnRail && this.currentRail) {
            force = Vector3.Project(force, this.currentRail.transform.up);
        }
        this.acceleration += force;
    }

    public void moveVertical_TEST(Vector3 direction) {
        Vector3 force = direction.normalized * 0.5f;
        this.addForce(force);
    }

    public void moveVertical(Vector3 direction) {
        Transform rail = this.findVerticalRail(direction);
        if (rail) {
            Rail railScript = rail.GetComponent<Rail>();
            Vector3 newPos = railScript.getClosestPosition(this.transform);
            this.transform.position = newPos;
            this.currentRail = rail;
        }
    }

    Transform findVerticalRail(Vector3 direction) {
        Transform rail = null;
        RaycastHit hitInfo;
        Vector3 origin = (this.currentRail) ? this.currentRail.GetComponent<Rail>().getClosestPosition(this.transform) : this.transform.position;
        int layerMask = 1 << 9; // Rail is layer 9
        bool railTrigger = (this.currentRail) ? this.currentRail.GetComponent<Rail>().setColliderTrigger(false) : false;
        bool playerTrigger = this.setColliderTrigger(false);
         if (Physics.Raycast(origin, direction, out hitInfo, Mathf.Infinity, layerMask)) {
            rail = hitInfo.collider.transform;
            Debug.DrawRay(this.transform.position, direction*(this.transform.position-hitInfo.collider.transform.position).magnitude, Color.green, 10f);
        }
        if (this.currentRail) this.currentRail.GetComponent<Rail>().setColliderTrigger(railTrigger);
        this.setColliderTrigger(playerTrigger);
        return rail;
    }

    public bool setColliderTrigger(bool value) {
        bool oldValue = this.GetComponent<BoxCollider>().isTrigger;
        this.GetComponent<BoxCollider>().isTrigger = value;
        return oldValue;
    }

    void adjustHorizontalPosition() {
        // If player is beyond current rail...
        Rail railScript = this.currentRail.GetComponent<Rail>();
        float horizontalDistance = railScript.getPlayerHorizontalDistance(this.transform);
        if (horizontalDistance > 0f) {
            bool isClosestToFront = railScript.isClosestToFront(this.transform);
            if (isClosestToFront) {
                Transform nextRail = railScript.getNextRail();
                if (nextRail) {
                    this.transform.position = nextRail.GetComponent<Rail>().getBackPosition();
                    this.transform.position += nextRail.transform.up * horizontalDistance;
                    this.currentRail = nextRail.transform;
                    this.adjustVelocity();
                } else {
                    this.transform.position = this.currentRail.GetComponent<Rail>().getFrontPosition();
                    this.acceleration = Vector3.zero;
                    this.velocity = Vector3.zero;
                }
            } else {
                Transform prevRail = railScript.getPrevRail();
                if (prevRail) {
                    this.transform.position = prevRail.GetComponent<Rail>().getFrontPosition();
                    this.transform.position += -1 * prevRail.transform.up * horizontalDistance;
                    this.currentRail = prevRail.transform;
                    this.adjustVelocity();
                } else {
                    this.transform.position = this.currentRail.GetComponent<Rail>().getBackPosition();
                    this.acceleration = Vector3.zero;
                    this.velocity = Vector3.zero;
                }
            }
        }
    }

    Transform findClosestRail() {
        Transform closest = null;
        float smallestDistance = Mathf.Infinity;
        foreach(Transform rail in railContainer.transform) {
            float distance = rail.GetComponent<Rail>().getPlayerDistance(this.transform);
            if (distance < smallestDistance) {
                smallestDistance = distance;
                closest = rail;
            }
        }
//        closest.GetComponent<Rail>().turnOnPower();
        return closest;
    }

    public void flipVelocity() {
        this.velocity = -1.1f * this.velocity;
    }

    public Vector3 getVelocity() {
        return this.velocity;
    }
}
