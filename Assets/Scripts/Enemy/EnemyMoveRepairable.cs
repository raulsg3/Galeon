using UnityEngine;
using System.Collections;

public class EnemyMoveRepairable : MonoBehaviour
{
    private RepairableObject target = null;
    private bool destroying = false;

    public Animator enemyWalkAnimator;
    public Animator enemyActionAnimator;

    public float speed = 5f;

    //Daño
    public float attackTime = 5.0f;
    private float accAttackTime = 0.0f;

    void Start () {
        enemyWalkAnimator = this.transform.Find("Visual/Body").GetComponent<Animator>();
        enemyActionAnimator = this.transform.Find("Weapons").GetComponent<Animator>();
    }
	
	void Update () {
        if (target == null || target.IsDestoyed())
            GetRepairableTarget();

        if (!destroying && target != null && !target.IsDestoyed())
        {
            float direction = target.transform.position.x - transform.position.x;

            Vector3 directionVector = new Vector3(direction, 0, 0);
            directionVector.Normalize();

            transform.Translate(directionVector * speed * Time.deltaTime);
            enemyWalkAnimator.SetBool("Walking", true);
        }

        if (destroying && target != null)
        {
            if (!target.IsDestoyed())
            {
                accAttackTime += Time.deltaTime;

                if (accAttackTime >= attackTime)
                {
                    accAttackTime = 0.0f;
                    target.TakeDamage();
                }
            }

            if (target.IsDestoyed())
            {
                destroying = false;
            }
            enemyWalkAnimator.SetBool("Walking", false);
            
        }

        if(destroying)
        {
            enemyActionAnimator.SetTrigger("Attack");
        }
	}

    //Busca el objetivo destruible más cercano
    void GetRepairableTarget()
    {
        GameObject closestTarget = RepairableManager.repairableManagerInstance.GetClosestObjectInFloor(transform.position);
        if (closestTarget != null)
            target = closestTarget.GetComponent<RepairableObject>();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag(Tags.RepairableObject))
            destroying = true;
    }
}
