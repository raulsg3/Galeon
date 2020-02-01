using UnityEngine;
using System.Collections;

public class EnemyMovePlayer : MonoBehaviour
{
    public GameSettingsSO gameSettings;

    private GameObject player;
    private bool attacking = false;

    public Animator enemyWalkAnimator;
    public Animator enemyActionAnimator;

    public float speed = 5f;

    //Daño
    public float attackTime = 5.0f;
    private float accAttackTime = 0.0f;

    void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.Player);

        enemyWalkAnimator = this.transform.Find("Visual/Body").GetComponent<Animator>();
        enemyActionAnimator = this.transform.Find("Weapons").GetComponent<Animator>();
    }
	
	void Update () {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > player.GetComponent<PlayerData>().width)
        {
            attacking = false;
            enemyWalkAnimator.SetBool("Walking", true);

            float direction = player.transform.position.x - transform.position.x;

            Vector3 directionVector = new Vector3(direction, 0, 0);
            directionVector.Normalize();

            transform.Translate(directionVector * speed * Time.deltaTime);

            if(directionVector.x > 0){
                transform.localScale = new Vector3(-1,1, 1);
            }else if (directionVector.x < 0) {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            attacking = true;
            enemyWalkAnimator.SetBool("Walking", false);

            accAttackTime += Time.deltaTime;

            if (accAttackTime >= attackTime)
            {
                accAttackTime = 0.0f;
                player.GetComponent<PlayerController>().TakeDamage();
            }
        }

        if (attacking)
        {
            enemyActionAnimator.SetTrigger("Attack");
        }
    }
}
