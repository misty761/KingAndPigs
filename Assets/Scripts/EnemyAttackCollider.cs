using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    PlayerMove player;
    EnemyMove enemy;
    public int forceX = 50;
    public int forceY = 100;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        enemy = GetComponentInParent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // distance x
            float distX = enemy.transform.position.x - player.transform.position.x; // + : player is left of enemy.
            // add force to player
            if (distX > 0) player.mRigidbody.AddForce(new Vector2(-forceX, forceY));
            else player.mRigidbody.AddForce(new Vector2(forceX, forceY));
            // player get damage
            player.GetDamage(enemy.power);
        }
    }
}
