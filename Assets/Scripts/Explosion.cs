using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int explosionForceX = 150;
    public int explosionForceY = 150;
    public float damage;
    Vector2 dist;

    public enum ExplosionFrom
    {
        Bomb,
        CannonBall
    }
    public ExplosionFrom explosionFrom;

    // sound
    public AudioClip audioExplosion;

    // time
    public float timeExplosion = 0.01f;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        // sound
        SoundManager.instance.PlaySound(audioExplosion, transform.position, 1f);

        damage = 0f;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // distance
        Vector3 _vec = collision.ClosestPoint(transform.position);
        dist = collision.transform.position - _vec;
        float distSqr = dist.sqrMagnitude;
        damage = (1f - distSqr * 3f) / 0.9f;
        if (explosionFrom == ExplosionFrom.CannonBall) damage = 0.32f * GameManager.instance.factorStageMax;
        if (damage < 0f) damage = 0f;
        //print("damage : " + damage);
        float forceX = damage * explosionForceX;
        float forceY = damage * explosionForceY;

        if (time < timeExplosion)
        {
            // enemy
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyMove enemy = collision.GetComponent<EnemyMove>();

                if (enemy != null)
                {
                    // ���� �����̰� ����
                    // ���� �����ʿ� ���� ��
                    AddForce(enemy.mRigidbody, forceX, forceY);

                    if (explosionFrom == ExplosionFrom.Bomb)
                    {
                        //enemy.GetDamage(damage);
                    }
                }
            }
            /*
            // cannon
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Cannon"))
            {
                
                Cannon cannon = collision.GetComponent<Cannon>();

                // ���� �����̰� ����
                AddForce(cannon.mRigidbody, forceX, forceY);

                cannon.GetDamage(damage);
                
            }
            */
            // player
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerMove player = collision.GetComponent<PlayerMove>();

                // �÷��̾ �����̰� ����
                AddForce(player.mRigidbody, forceX, forceY);

                //player.GetDamage(damage);
                player.GetDamage(1);
            }
            // decoration
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Decoration"))
            {
                Decoration decoration = collision.GetComponent<Decoration>();

                // decoration�� �����̰� ����
                float factor = 2.5f;
                try
                {
                    AddForce(decoration.mRigidbody, forceX * factor, forceY * factor);
                }
                catch { }
               
            }
            // bomb
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            {
                try
                {
                    Bomb bomb = collision.GetComponent<Bomb>();
                    bomb.ExplosionBomb(); 
                }
                catch { }  
            }

        }

    }

    void AddForce(Rigidbody2D rb, float x, float y)
    {
        rb.velocity = Vector2.zero;
        // rigidbody�� �����ʿ� ���� ��
        if (dist.x > 0)
        {
            rb.AddForce(new Vector2(x, y));
        }
        // rigidbody�� ���ʿ� ���� ��
        else
        {
            rb.AddForce(new Vector2(-x, y));
        }
    }
}