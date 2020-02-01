using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {

    // Salud del enemigo
    public float health = 1.0f;
    public float enemyScore = 100f;


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == Tags.Sword)
        {
            Damage(50f);
        }
    }

    // Llamada cuando el enemigo recibe daño
    public void Damage(float damage)
    {
        // Recibe el daño
        health -= damage;

        // Comprueba
        if (health <= 0.0f)
        {
            // DropManager.DropManagerInstance.basicEnemyDrop(this.transform.position);
            // GameManager.GameManagerInstance.increaseScore(enemyScore);

            // if(GetComponent<EnemyShoot>() != null)
            //     GetComponent<EnemyShoot>().enabled = false;

            // GetComponent<Animator>().SetTrigger("Destroy");
            // GetComponent<CircleCollider2D>().enabled = false;

            // this.GetComponent<AudioSource>().clip = SoundManager.SoundManagerInstance.getEnemyDestroyed();
            // this.GetComponent<AudioSource>().Play();
            EnemyDestroyedAnimEnd();
        }
    }

    public void EnemyDestroyedAnimEnd()
    {
        Destroy(this.gameObject);
    }
}

