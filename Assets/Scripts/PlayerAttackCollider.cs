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
            enemy.isHit = true;
            // add force
            if (enemy.health > 0f)
            {
                AddForce(collision, forceX * 2, forceY);
            }
            // enemy get damage
            enemy.GetDamage(player.power);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Decoration"))
        {
            // add force
            AddForce(collision, forceX/2, forceY/2);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            // add force
            AddForce(collision, forceX/2, forceY/2);

            // break box
            try
            {
                Box box = collision.GetComponent<Box>();
                box.isBroken = true;
            }
            catch
            {
                // Enemy box 
            }
            
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            // add force
            AddForce(collision, forceX * 2, forceY);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Cannon"))
        {
            Cannon cannon = collision.GetComponent<Cannon>();
            cannon.GetDamage(player.power);
        }
    }

    void AddForce(Collider2D col, int forceX, int forceY)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        Transform tf = col.GetComponent<Transform>();
        float distX = tf.transform.position.x - player.transform.position.x;
        rb.velocity = Vector2.zero;
        // object is right of attack
        if (distX > 0f)
        {
            rb.AddForce(new Vector2(forceX, forceY));
        }
        // object is left of attack
        else
        {
            rb.AddForce(new Vector2(-forceX, forceY));
        }
    }
}
