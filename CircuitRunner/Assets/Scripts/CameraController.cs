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
        this.followPosition(targetGO.transform.position);
        this.followRotation(targetGO.transform.rotation);
    }

    void followPosition(Vector3 position) {
        float x = position.x;
        float y = position.y;
        float z = this.transform.position.z;
        this.transform.position = new Vector3(x,y,z);
    }

    void followRotation(Quaternion rotation) {
        float x = this.transform.rotation.x;
        float y = this.transform.rotation.y;
        float z = rotation.eulerAngles.z;
        this.transform.rotation = Quaternion.Euler(x,y,z);
    }
}
