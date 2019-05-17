using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject targetGO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        float x = targetGO.transform.position.x;
        float y = targetGO.transform.position.y;
        float z = this.transform.position.z;
        this.transform.position = new Vector3(x,y,z);
    }
}
