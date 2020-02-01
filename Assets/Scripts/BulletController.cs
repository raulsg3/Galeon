using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // called when the bullet hits something 
    public GameObject OnHitParticles;
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag(Tags.Enemy))
        {
            col.gameObject.GetComponent<EnemyLife>().Damage(105f);
        }
        Instantiate(OnHitParticles,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }

}
