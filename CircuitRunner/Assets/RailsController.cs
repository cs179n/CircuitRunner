using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Adjust position of all rails are right next to each other based on the nextRail and prevRail
        foreach(Transform rail in this.transform) {
            Transform nextRail = rail.GetComponent<Rail>().getNextRail();
            if (nextRail) {
                // TODO
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
