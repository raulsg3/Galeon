using UnityEngine;
using System.Collections;

public class EnemyMovePlayer : MonoBehaviour
{
    private GameObject player;

    public float speed = 5f;

	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.Player);
	}
	
	void Update () {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > player.GetComponent<PlayerData>().width)
        {
            float direction = player.transform.position.x - transform.position.x;

            Vector3 directionVector = new Vector3(direction, 0, 0);
            directionVector.Normalize();

            transform.Translate(directionVector * speed * Time.deltaTime);
        }
	}
}
