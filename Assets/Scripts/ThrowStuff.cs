using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStuff : MonoBehaviour
{
    Vector2 pos;
    PlayerMove player;
    Animator animator;
    EnemyMove enemy;
    public float distX = 1f;
    public float distY = 0.5f;
    public GameObject pfStuff;
    public GameObject pfPig;
    public Transform throwPosition;
    public int forceX = 50;
    public int forceY = 100;
    GameObject stuff;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state != GameManager.State.Play) return;

        // Throw when player is close
        pos = player.DistPlayer(transform.position);
        if (pos.y > -distY && pos.y < distY)
        {
            // player is left
            if (pos.x > -distX && pos.x < 0)
            {
                enemy.speed = 0f;
                enemy.isLookingRight = false;
                enemy.SetSprite();
                animator.SetTrigger("Throw");    
            }
            // player is right
            else if (pos.x > 0 && pos.x < distX)
            {
                enemy.speed = 0f;
                enemy.isLookingRight = true;
                enemy.SetSprite();
                animator.SetTrigger("Throw");   
            }
        }

        // spawn pig
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            SpawnPig();
        }

        // attacked by player
        if (enemy.isHit)
        {
            SpawnStuff();
            SpawnPig();
        }
        
    }

    public void Throw()
    {
        // spawn stuff
        SpawnStuff();

        // Throw
        // player is left
        Rigidbody2D mRigidibody = stuff.GetComponent<Rigidbody2D>();
        if (pos.x < 0f)
        {
            mRigidibody.AddForce(new Vector2(-forceX, forceY));
        }
        // player is right
        else
        {
            mRigidibody.AddForce(new Vector2(forceX, forceY));
        }
    }

    public void SpawnStuff()
    {
        // spawn stuff
        stuff = Instantiate(pfStuff, throwPosition.position, Quaternion.Euler(Vector2.zero));
        
        try
        {
            Box box = stuff.GetComponent<Box>();
            box.isThrowed = true;
            box.isBroken = true;
        }
        catch
        {
            // not box
        }
    }

    public void SpawnPig()
    {
        // spawn pig
        Instantiate(pfPig, transform.position, Quaternion.Euler(Vector2.zero));
        // destroy gameobject
        enemy.DestroyGo();
    }
}
