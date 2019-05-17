using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform currentRail = null;
    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    public GameObject railContainer;

    void Start()
    {
        this.currentRail = this.findClosestRail();
        if (this.currentRail) {
            this.transform.position = this.currentRail.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Place the player on the closest rail
        this.currentRail = this.findClosestRail();
        if (this.currentRail) {
            Vector3 closestPosition = this.currentRail.GetComponent<Rail>().getClosestPosition();
            Debug.DrawLine(this.transform.position, closestPosition, Color.magenta);
            Vector3 forwardForce = this.currentRail.transform.up * 0.01f;
            if (Input.GetKey(KeyCode.D)) {
                //this.moveHorizontal(this.currentRail.up);
                this.addForce(forwardForce);
                this.GetComponent<SpriteRenderer>().flipX = false;
            } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                this.addForce(-forwardForce);
                this.GetComponent<SpriteRenderer>().flipX = true;

            } else if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                this.moveVertical(this.transform.up);
            } else if (Input.GetKeyDown(KeyCode.S)) {
                this.moveVertical(-this.transform.up);
            }
        }

        if (this.currentRail) {
            this.velocity += this.acceleration;
            this.transform.position += this.velocity;
            this.adjustPosition();
            this.adjustVelocity(this.currentRail.transform.up);
        }
        this.acceleration = Vector3.zero;
    }

    void adjustVelocity(Vector3 newDirection) {
        // This function is called when the player moves to another rail
        // Change the direction of velocity to be the same as the new rail. Magnitude stays the same.
        float speed = this.velocity.magnitude;
        float sign = Mathf.Sign(Vector3.Dot(this.velocity, newDirection));
        this.velocity = newDirection * speed * sign;
    }

    public void addForce(Vector3 force) {
        this.acceleration += force;
    }

    void moveVertical(Vector3 direction) {
        Transform rail = this.findVerticalRail(direction); // WARNING: Currently, this is not guaranteed to be a Rail
        if (rail) {
            Rail railScript = rail.GetComponent<Rail>();
            Vector3 newPos = railScript.getClosestPosition();
            this.transform.position = newPos;
            this.currentRail = rail;
        }
    }

    Transform findVerticalRail(Vector3 direction) {
        Transform rail = null;
        RaycastHit hitInfo;
        Vector3 origin = (this.currentRail) ? this.currentRail.GetComponent<Rail>().getClosestPosition() : this.transform.position;

        // TODO: MAKE SURE THE RAY CAN ONLY COLLIDE WITH RAILS (NOT PLAYER, ENEMIES, ITEMS, ETC)
        // Currently, this is accomplished by turning off player and current rail's collider isTrigger temporarily.
        // An improvement would be to add a layerMask for rails only. (While still turning off current rail.)

        bool railTrigger = this.currentRail.GetComponent<Rail>().setColliderTrigger(false);
        bool playerTrigger = this.setColliderTrigger(false);
        if (Physics.Raycast(origin, direction, out hitInfo, Mathf.Infinity)) {
            rail = hitInfo.collider.transform;
        }
        this.currentRail.GetComponent<Rail>().setColliderTrigger(railTrigger);
        this.setColliderTrigger(playerTrigger);
        return rail;
    }

    void adjustPosition() {
        // If player has moved beyond the current rail...
        Rail railScript = this.currentRail.GetComponent<Rail>();
        float horizontalDistance = railScript.getPlayerHorizontalDistance();
        if (horizontalDistance > 0f) {
            if (railScript.isClosestToFront()) {
                Transform nextRail = railScript.getNextRail();
                if (nextRail) {
                    Vector3 newPosition = nextRail.GetComponent<Rail>().getBackPosition();
                    this.changeCurrentRail(nextRail, newPosition, horizontalDistance, 1);
                } else {
                    this.stopAt(this.currentRail.GetComponent<Rail>().getFrontPosition());
                }
            } else {
                Transform prevRail = railScript.getPrevRail();
                if (prevRail) {
                    Vector3 newPosition = prevRail.GetComponent<Rail>().getFrontPosition();
                    this.changeCurrentRail(prevRail, newPosition, horizontalDistance, -1);
                } else {
                    this.stopAt(this.currentRail.GetComponent<Rail>().getBackPosition());
                }
            }
        }
    }

    void changeCurrentRail(Transform rail, Vector3 position, float horizontalDistance, float sign) {
        this.transform.position = position;
        this.transform.position += sign * rail.transform.up * horizontalDistance;
        this.currentRail = rail.transform;
        //this.adjustVelocity(rail.transform.up);
    }

    void stopAt(Vector3 position) {
        // Set the position and then set acceleration and velocity to zero
        this.transform.position = position;
        this.acceleration = Vector3.zero;
        this.velocity = Vector3.zero;
    }

    Transform findClosestRail() {
        // Returns the rail that is closest in distance
        Transform closest = null;
        float smallestDistance = Mathf.Infinity;
        foreach(Transform rail in railContainer.transform) {
            float distance = rail.GetComponent<Rail>().getPlayerDistance();
            if (distance < smallestDistance) {
                smallestDistance = distance;
                closest = rail;
            }
        }
        return closest;
    }

    public bool setColliderTrigger(bool value) {
        // Sets the Collider's isTrigger to true or false
        // Returns the old value
        bool oldValue = this.GetComponent<BoxCollider>().isTrigger;
        this.GetComponent<BoxCollider>().isTrigger = value;
        return oldValue;
    }
}
