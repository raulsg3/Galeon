﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject player;

    public GameObject bulletPrefab;
    public GameObject spawnPosition;

    // Instaciate the bullet prefab
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition.GetComponent<Transform>().position, Quaternion.identity);
        // Set the same direction as the player
        bullet.transform.localScale = player.transform.localScale;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // Set velocity depending of the player's direction
        rb.velocity = new Vector3(-10 * player.transform.localScale.x, 0, 0);
    }
}
