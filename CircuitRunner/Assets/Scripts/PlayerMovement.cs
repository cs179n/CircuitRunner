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
    private Quaternion targetRotation = Quaternion.identity;
    private SpriteRenderer spriteRenderer;
    private BoxCollider boxCollider;
    private float cameraRotationSpeed = 5f;

    void Awake()
    {
        
    }

    void Start()
    {
        spriteRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        boxCollider = this.transform.GetChild(0).GetComponent<BoxCollider>();

        this.currentRail = this.findClosestRail();
        if (this.currentRail) {
            this.transform.position = this.currentRail.position;
        }
        //this.transform.position += this.transform.up*200;
    }

    // Update is called once per frame
    void Update()
    {
        // Place the player on the closest rail
        this.currentRail = this.findClosestRail();
        if (this.currentRail) {
            
            this.currentRail.GetComponent<Rail>().turnOnPower();

            Vector3 closestPosition = this.currentRail.GetComponent<Rail>().getClosestPosition();
            Vector3 forwardForce = this.currentRail.transform.up * 0.01f;
            if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                this.addForce(forwardForce);
                this.spriteRenderer.flipX = false;
            } 
            else if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                this.addForce(-forwardForce);
                this.spriteRenderer.flipX = true;
            } 
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                this.moveVertical(this.transform.up);
            } 
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                this.moveVertical(-this.transform.up);
            }
        }

        if (this.currentRail) {
            this.velocity += this.acceleration;
            this.transform.position += this.velocity;
            this.adjustPosition();
            //this.adjustVelocity(this.currentRail.transform.up);
            this.adjustRotationZ();
        }
        this.acceleration = Vector3.zero;
    }

    public Transform getCurrentRail() {
        return this.currentRail;
    }

    void adjustRotationZ() {
        // Rotate player to match the current rails direction
        Vector3 current = this.transform.forward;
        Vector3 target = this.currentRail.transform.up;
        float step = Time.deltaTime * this.cameraRotationSpeed;
        Vector3 newDir = Vector3.RotateTowards(current, target, step, 0f);
        this.transform.rotation = Quaternion.LookRotation(newDir);

        // Keep the z-axis the same
        float x = this.transform.rotation.eulerAngles.x;
        float y = this.transform.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(x,y,0);
    }

    

    public void addForce(Vector3 force) {
        this.acceleration += force;
    }

    void moveVertical(Vector3 direction) {
        Transform rail;
        Vector3 point;
        this.findVerticalRail(direction, out rail, out point); // WARNING: Currently, this is not guaranteed to be a Rail
        if (rail) {
            Rail railScript = rail.GetComponent<Rail>();
            this.transform.position = point;
            Vector3 newPos = railScript.getClosestPosition();
            this.transform.position = newPos;
            this.currentRail = rail;
        }
    }

    void findVerticalRail(Vector3 direction, out Transform rail, out Vector3 point) {
        RaycastHit hitInfo;
        Vector3 origin = (this.currentRail) ? this.currentRail.GetComponent<Rail>().getClosestPosition() : this.transform.position;
        int layerMask = 1 << 9; // Rail layer is 9
        bool railTrigger = this.currentRail.GetComponent<Rail>().setColliderTrigger(false);
        bool playerTrigger = this.setColliderTrigger(false);
        if (Physics.Raycast(origin, direction, out hitInfo, Mathf.Infinity, layerMask)) {
            rail = hitInfo.collider.transform;
            point = hitInfo.point;
        } else {
            rail = null;
            point = this.transform.position;
        }
        this.currentRail.GetComponent<Rail>().setColliderTrigger(railTrigger);
        this.setColliderTrigger(playerTrigger);
    }

    void adjustPosition() {
        // If player has moved beyond the current rail...
        Rail railScript = this.currentRail.GetComponent<Rail>();
        float horizontalDistance = railScript.getPlayerHorizontalDistance();
        // TODO: In case the horizontal distance is greater than the length of te prev/next rail,
        //       it should recursively find the following rail until horizontal distance is 0.
        if (horizontalDistance > 0f) {
            if (railScript.isClosestToFront()) {
                Transform nextRail = railScript.getNextRail();
                if (nextRail) {
                    Vector3 newPosition = nextRail.GetComponent<Rail>().getBackPosition();
                    this.changeCurrentRail(nextRail, newPosition, horizontalDistance, 1);
                    this.adjustVelocity(nextRail.transform.up);
                } else {
                    this.stopAt(this.currentRail.GetComponent<Rail>().getFrontPosition());
                }
            } else {
                Transform prevRail = railScript.getPrevRail();
                if (prevRail) {
                    Vector3 newPosition = prevRail.GetComponent<Rail>().getFrontPosition();
                    this.changeCurrentRail(prevRail, newPosition, horizontalDistance, -1);
                    this.adjustVelocity(-prevRail.transform.up);
                } else {
                    this.stopAt(this.currentRail.GetComponent<Rail>().getBackPosition());
                }
            }
        } else { // horizontal <= 0
            this.transform.position = this.currentRail.GetComponent<Rail>().getClosestPosition();
        }
    }

    void adjustVelocity(Vector3 newDirection) {
        float speed = this.velocity.magnitude;
        this.velocity = newDirection * speed;
    }

    void changeCurrentRail(Transform rail, Vector3 position, float horizontalDistance, float sign) {
        this.transform.position = position;
        this.transform.position += sign * rail.transform.up * horizontalDistance;
        this.currentRail = rail.transform;

        //this.velocity = Vector3.zero;
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
        bool oldValue = this.boxCollider.isTrigger;
        this.boxCollider.isTrigger = value;
        return oldValue;
    }
}
