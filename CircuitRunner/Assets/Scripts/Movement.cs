using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform currentRail = null;
    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    public GameObject railContainer;
    private Vector3 offset = new Vector3(-5, 0, 0);
    void Start()
    {
        Transform rail = this.findClosestRail();
        if (rail) {
            this.currentRail = rail;
            Vector3 backPosition = rail.GetComponent<Rail>().getBackPosition();
            this.transform.position = backPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Place the player on the closest rail
        this.currentRail = this.findClosestRail();
        if (this.currentRail && !PauseMenuController.IsGamePause) {
            this.transform.rotation = this.currentRail.rotation;
            this.transform.Rotate(0,0,90f);
            Vector3 closestPosition = this.currentRail.GetComponent<Rail>().getClosestPosition(this.transform);
            Debug.DrawLine(this.transform.position, closestPosition, Color.magenta);
        }

        if (this.currentRail && !PauseMenuController.IsGamePause) {
            this.velocity += this.acceleration;
            this.transform.position += this.velocity;
            this.adjustPosition();
            this.adjustVelocity();
        }
        this.acceleration = Vector3.zero;
    }

    public Vector3 getCurrentRailUp() {
        return this.currentRail.transform.up;
    }

    void adjustVelocity() {
        float speed = this.velocity.magnitude;
        float direction = Vector3.Dot(this.velocity.normalized, this.currentRail.transform.up.normalized);
        this.velocity = Vector3.zero;
        this.velocity = this.currentRail.transform.up * speed * direction;
    }

    public void addForce(Vector3 force) {
        this.acceleration += force;
    }

    public void moveVertical(Vector3 direction) {
        Transform rail = this.findVerticalRail(direction); // WARNING: Currently, this is not guaranteed to be a Rail
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

        // TODO: MAKE SURE THE RAY CAN ONLY COLLIDE WITH RAILS (NOT PLAYER, ENEMIES, ITEMS, ETC)
        // Currently, this is accomplished by turning off player and current rail's collider isTrigger temporarily.
        // An improvement would be to add a layerMask for rails only. (While still turning off current rail.)

        bool railTrigger = this.currentRail.GetComponent<Rail>().setColliderTrigger(false);
        bool playerTrigger = this.setColliderTrigger(false);
        if (Physics.Raycast(origin, direction, out hitInfo, Mathf.Infinity)) {
            rail = hitInfo.collider.transform;
            Debug.DrawRay(this.transform.position, direction, Color.green, 20f);
        }
        this.currentRail.GetComponent<Rail>().setColliderTrigger(railTrigger);
        this.setColliderTrigger(playerTrigger);
        return rail;
    }

    public bool setColliderTrigger(bool value) {
        bool oldValue = this.GetComponent<BoxCollider>().isTrigger;
        this.GetComponent<BoxCollider>().isTrigger = value;
        return oldValue;
    }

    // void adjustVelocity() {
    //     float dot = Vector3.Dot(this.velocity, this.currentRail.transform.up);
    //     float speed = this.velocity.magnitude * Mathf.Sign(dot);
    //     this.velocity = this.currentRail.transform.up * speed;
    // }

    void adjustPosition() {
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

    void moveHorizontal_OLD(Vector3 direction) {
        //this.transform.position += direction*0.7f;//*0.1f;
        

        // If moving beyond current rail...
        Rail railScript = this.currentRail.GetComponent<Rail>();
        float horizontalDistance = railScript.getPlayerHorizontalDistance(this.transform);
        if (horizontalDistance > 0f) {
            Debug.Log("if");
            bool isClosestToFront = railScript.isClosestToFront(this.transform);
            Transform nextRail = railScript.getNextRail();
            Transform prevRail = railScript.getPrevRail();
            if (isClosestToFront && nextRail != null) {
                this.transform.position = nextRail.GetComponent<Rail>().getBackPosition();
                this.transform.position += nextRail.transform.up * horizontalDistance;
                this.currentRail = nextRail.transform;
            } else if (!isClosestToFront && prevRail != null) {
                this.transform.position = prevRail.GetComponent<Rail>().getFrontPosition();
                this.transform.position += -1 * prevRail.transform.up * horizontalDistance;
                this.currentRail = prevRail.transform;
            } else {
                Debug.Log("else");
                //this.transform.position = railScript.getClosestPosition();
                Vector3 force = direction * 0.01f;
                this.addForce(force);
            }
        }
    }

    void changeCurrentRail() {
        
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
        closest.GetComponent<Rail>().turnOnPower();
        return closest;
    }

    public void flipVelocity() {
        this.velocity = -1.1f * this.velocity;
    }

    public Vector3 getVelocity() {
        return this.velocity;
    }
}
