using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // called when the bullet hits something
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag(Tags.Enemy))
        {
            col.gameObject.GetComponent<EnemyLife>().Damage(50f);
        }
        Destroy(this.gameObject);
    }

}
