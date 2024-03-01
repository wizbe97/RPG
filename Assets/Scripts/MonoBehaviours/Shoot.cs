using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform gun;
    public Animator gunAnimator;
    Vector2 direction;

    public GameObject bullet;
    public float bulletSpeed;
    public Transform shootPoint;

    public float fireRate;

    float readyForNextShot;
    void Start()
    {

    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)gun.position;
        FaceMouse();

        if (Input.GetMouseButton(0))
        {
            if (Time.time > readyForNextShot)
            {
                readyForNextShot = Time.time + 1 / fireRate;
                ShootGun();
            }
        }

        void FaceMouse()
        {
            gun.transform.right = direction;
        }

        void ShootGun()
        {
            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(bulletInstance.transform.right * bulletSpeed);
            Destroy(bulletInstance, 1);
        }
    }
}
