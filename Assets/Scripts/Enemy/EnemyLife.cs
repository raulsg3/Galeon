using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {

    // Salud del enemigo
    private float health = 100f;
    public float enemyScore = 100f;

    public GameObject dieParticles;
    public SpriteRenderer visualSprite;
    float feedbackProgress = 1f;
    public GameSettingsSO gameSettings;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == Tags.Sword)
        {
            Damage(34f);
        }
    }
    void Update()
    {
        if(feedbackProgress < 1)
        {
            feedbackProgress += Time.deltaTime * gameSettings.enemyFeedbackSpeed;
            visualSprite.color = Color.Lerp(Color.red,Color.white,feedbackProgress);
        }
    }

    // Llamada cuando el enemigo recibe daño
    public void Damage(float damage)
    {
        // Recibe el daño
        health -= damage;
        feedbackProgress = 0f;
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
        Instantiate(dieParticles,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
}

