using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsController : MonoBehaviour
{
    public Material Powered;
    public Material Unpowered;
    bool hasLinkedRails = false;
    float linkDistanceThreshold = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLinkedRails) {
            this.linkRails();
            hasLinkedRails = true;
        }
    }

    private void linkRails() {
        // This algorithm attempts to populate the nextRail and prevRail variables of each Rail.
        // It will only do this for those variables that were not already set in the editor manually.
        // For each Rail's Front / Back, it will find the closest Back / Front, respectively, and if they are within a certain distance, link them.
        // Performance: O(n^2). It might be better to do this algorithm in "chunks" every frame, instead of all at once on the first Update()
        foreach(Transform rail1 in this.transform) {
            Rail railScript = rail1.GetComponent<Rail>();
            float smallestDistance_nextRail = Mathf.Infinity;
            float smallestDistance_prevRail = Mathf.Infinity;
            Transform closestNextRail = null;
            Transform closestPrevRail = null;
            foreach(Transform rail2 in this.transform) {
                if (rail1 == rail2)
                    continue;
                if (railScript.nextRail == null) {
                    Vector3 frontToBack = (rail2.GetChild(1).position - rail1.GetChild(0).position); // rail1.Front --> rail2.Back
                    if (frontToBack.magnitude < smallestDistance_nextRail) {
                        smallestDistance_nextRail = frontToBack.magnitude;
                        closestNextRail = rail2;
                    }
                }
                if (railScript.nextRail == null) {
                    Vector3 backToFront = (rail2.GetChild(0).position - rail1.GetChild(1).position); // rail1.Back --> rail2.Front
                    if (backToFront.magnitude < smallestDistance_prevRail) {
                        smallestDistance_prevRail = backToFront.magnitude;
                        closestPrevRail = rail2;
                    }
                }
            }
            if (closestNextRail && smallestDistance_nextRail <= linkDistanceThreshold) {
                railScript.nextRail = closestNextRail.gameObject;
            }
            if (closestPrevRail && smallestDistance_prevRail <= linkDistanceThreshold) {
                railScript.prevRail = closestPrevRail.gameObject;
            }
        }
    }
    public Material GetPowered() {
        return Powered;
    }
    public Material GetUnpowered() {
        return Unpowered;
    }
}
