using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    //Salud
    public float health = 1.0f;

    //Medidas
    public float width = 1.05f;
    public float height = 1.0f;

    void Start ()
    {
	}
	
	void Update ()
    {
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if(other.tag == Tags.PlayerBullet)
        {
            if (other.gameObject.GetComponent<ForwardBullet>() != null)
                Damage(other.gameObject.GetComponent<ForwardBullet>().damage);
            else
                Damage(other.gameObject.GetComponent<ConeBullet>().damage);
        }
        */
    }

    // Llamada cuando el enemigo recibe daño
    public void Damage(float damage)
    {
        // Recibe el daño
        health -= damage;

        // Comprueba
        /*
        if (health <= 0.0f)
        {
            DropManager.DropManagerInstance.basicEnemyDrop(this.transform.position);
            GameManager.GameManagerInstance.increaseScore(enemyScore);

            if(GetComponent<EnemyShoot>() != null)
                GetComponent<EnemyShoot>().enabled = false;

            GetComponent<Animator>().SetTrigger("Destroy");
            GetComponent<CircleCollider2D>().enabled = false;

            this.GetComponent<AudioSource>().clip = SoundManager.SoundManagerInstance.getEnemyDestroyed();
            this.GetComponent<AudioSource>().Play();
        }
        */
    }
}

