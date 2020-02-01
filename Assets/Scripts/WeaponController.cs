using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject player;
    public BoxCollider2D swordCollider;
    public GameSettingsSO gameSettings;

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
        rb.velocity = new Vector3(-gameSettings.bulletSpeed * player.transform.localScale.x, 0, 0);
        // Move backwards the player and stop it a while
        player.GetComponent<Rigidbody2D>().velocity = new Vector3(gameSettings.shootRecoil * player.transform.localScale.x, 0, 0);
        player.GetComponent<PlayerController>().currentStopTime = gameSettings.stopTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("ALGOOOOO");
    }

    void StartCut()
    {
        swordCollider.enabled = true;
    }

    void EndCut()
    {
        swordCollider.enabled = false;
    }
}
