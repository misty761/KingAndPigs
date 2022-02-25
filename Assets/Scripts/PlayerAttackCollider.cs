using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    PlayerMove player;
    public int forceX = 50;
    public int forceY = 100;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyMove enemy = collision.GetComponent<EnemyMove>();
            // distance x
            float distX = enemy.transform.position.x - player.transform.position.x; // + : player is left of enemy.
            // add force to enemy
            enemy.mRigidbody.velocity = new Vector2(enemy.mRigidbody.velocity.x, 0);
            if (distX > 0) enemy.mRigidbody.AddForce(new Vector2(forceX, forceY));
            else enemy.mRigidbody.AddForce(new Vector2(-forceX, forceY));
            // enemy get damage
            enemy.GetDamage(player.power);
        }
    }
}
