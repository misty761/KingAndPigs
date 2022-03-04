using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try 
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dying")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    animator.SetTrigger("Attack");
                }
            }
        }
        catch { }
    }
}
