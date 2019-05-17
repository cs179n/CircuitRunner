﻿using System.Collections;
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
            } else if (Input.GetKey(KeyCode.A)) {
                //this.moveHorizontal(-this.currentRail.up);
                this.addForce(-forwardForce);
            } else if (Input.GetKeyDown(KeyCode.W)) {
                this.moveVertical(this.transform.up);
            } else if (Input.GetKeyDown(KeyCode.S)) {
                this.moveVertical(-this.transform.up);
            }
        }

        if (this.currentRail) {
            this.velocity += this.acceleration;
            this.transform.position += this.velocity;
            this.adjustPosition();
        }
        this.acceleration = Vector3.zero;
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

    public bool setColliderTrigger(bool value) {
        bool oldValue = this.GetComponent<BoxCollider>().isTrigger;
        this.GetComponent<BoxCollider>().isTrigger = value;
        return oldValue;
    }

    void adjustPosition() {
        // If player is beyond current rail...
        Rail railScript = this.currentRail.GetComponent<Rail>();
        float horizontalDistance = railScript.getPlayerHorizontalDistance();
        if (horizontalDistance > 0f) {
            bool isClosestToFront = railScript.isClosestToFront();
            if (isClosestToFront) {
                Transform nextRail = railScript.getNextRail();
                if (nextRail) {
                    this.transform.position = nextRail.GetComponent<Rail>().getBackPosition();
                    this.transform.position += nextRail.transform.up * horizontalDistance;
                    this.currentRail = nextRail.transform;
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
        float horizontalDistance = railScript.getPlayerHorizontalDistance();
        if (horizontalDistance > 0f) {
            Debug.Log("if");
            bool isClosestToFront = railScript.isClosestToFront();
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
            float distance = rail.GetComponent<Rail>().getPlayerDistance();
            if (distance < smallestDistance) {
                smallestDistance = distance;
                closest = rail;
            }
        }
        return closest;
    }
}