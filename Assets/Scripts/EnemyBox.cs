using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour
{
    Animator animator;
    PlayerMove player;
    Vector2 pos;
    public float distX = 1f;
    public float distY = 0.5f;
    public int forceX = 50;
    public int forceY = 100;
    Rigidbody2D mRigidbody;
    bool bJump;
    public GameObject pfBox;
    public GameObject pfEnemy;
    bool isFalling;
    public int power = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMove>();
        mRigidbody = GetComponent<Rigidbody2D>();
        bJump = false;
        isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state != GameManager.State.Play) return;

        // jump when player is close
        pos = player.DistPlayer(transform.position);
        if (pos.y > -distY && pos.y < distY)
        {
            // player is left
            if (pos.x > -distX && pos.x < 0)
            {
                animator.SetTrigger("Jump");
            }
            // player is right
            else if (pos.x > 0 && pos.x < distX)
            {
                animator.SetTrigger("Jump");
            }
        }

        // jump
        if (!bJump)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                bJump = true;
                // player is left
                if (pos.x < 0f)
                {
                    mRigidbody.AddForce(new Vector2(-forceX, forceY));
                }
                // player is right
                else
                {
                    mRigidbody.AddForce(new Vector2(forceX, forceY));
                }
            }
        }

        // state of falling
        if (mRigidbody.velocity.y > 0)
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }
        animator.SetBool("Fall", isFalling);   
        
        // spawn pig and box
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            GameObject goBox = Instantiate(pfBox, transform.position, Quaternion.Euler(Vector2.zero));
            Box box = goBox.GetComponent<Box>();
            box.isSpawn = false;
            box.BreakBox();
            Instantiate(pfEnemy, transform.position, Quaternion.Euler(Vector2.zero));
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.GetDamage(power);
        }

        // break box
        if (bJump) animator.SetTrigger("Collision");
    }
}
