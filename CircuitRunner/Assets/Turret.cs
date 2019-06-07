using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Transform cannon;
    public Transform target;
    public GameObject bulletPrefab;
    float reloadTimeMax = 3f;
    float reloadTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        this.cannon = this.transform.GetChild(0).GetChild(0);
        //this.createBullet();
    }

    // Update is called once per frame
    void Update()
    {
        //Transform from = this.cannon.transform
        //this.cannon.rotation = Quaternion.RotateTowards(from, to, step);
        this.cannon.transform.LookAt(this.target);

        this.reloadTime -= Time.deltaTime;
        if (this.reloadTime <= 0f) {
            this.createBullet();
            this.reloadTime = this.reloadTimeMax;
        }
    }

    void createBullet() {
        GameObject bullet = Instantiate(bulletPrefab, this.transform);
        bullet.transform.position = this.transform.position;
        bullet.transform.parent = this.transform.parent.parent;
    }
}
