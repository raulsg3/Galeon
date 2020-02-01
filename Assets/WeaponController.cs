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
        //Instantiate(myPrefab, spawnPosition., Quaternion.identity);
    }
}
