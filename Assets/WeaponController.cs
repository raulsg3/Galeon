using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject spawnPosition;

    // Instaciate the bullet prefab
    void Shoot()
    {
        Debug.Log("DISPARA!");
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition.GetComponent<Transform>(), Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 10, 0);
    }
}
