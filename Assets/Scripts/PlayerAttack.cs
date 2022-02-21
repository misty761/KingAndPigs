using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    ButtonAttack buttonAttack;
    public GameObject col;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        col.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state != GameManager.State.Play) return;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            try
            {
                if (buttonAttack == null) buttonAttack = FindObjectOfType<ButtonAttack>();

                if (Input.GetKeyDown(KeyCode.DownArrow) || buttonAttack.isTouchDown)
                {
                    animator.SetTrigger("Attack");
                    buttonAttack.isTouchDown = false;
                }
            }
            catch
            {

            }

        }
    }

    public void StartAttack()
    {
        col.SetActive(true);
    }

    public void FinishAttack()
    {
        col.SetActive(false);
    }
}
