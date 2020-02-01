using UnityEngine;
using System.Collections;

public class EnemyMovePlayer : MonoBehaviour
{
    private GameObject player;
    public Animator enemyWalkAnimator;
    public Animator enemyActionAnimator;

    public float speed = 5f;

	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.Player);
        enemyWalkAnimator = this.transform.Find("Visual/Body").GetComponent<Animator>();
        enemyActionAnimator = this.transform.Find("Weapons").GetComponent<Animator>();
    }
	
	void Update () {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > player.GetComponent<PlayerData>().width)
        {
            float direction = player.transform.position.x - transform.position.x;

            Vector3 directionVector = new Vector3(direction, 0, 0);
            directionVector.Normalize();

            transform.Translate(directionVector * speed * Time.deltaTime);

            enemyWalkAnimator.SetBool("Walking", true);
        }
        else
        {
            enemyWalkAnimator.SetBool("Walking", false);
        }
	}
}
