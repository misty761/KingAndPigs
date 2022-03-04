using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    PlayerMove player;
    Vector2 pos;
    float distSqrt;
    Animator animator;
    public float distSqrtFire = 2f;
    public float distFireY = 0.4f;
    public Cannon cannon;
    public float distSqrtStopLight = 1f;
    public GameObject pfPig;
    EnemyMove enemy;
    float timeLight;
    float timeLightRandom;
    public float timeLightMin = 0.1f;
    public float timeLightMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyMove>();

        ResetLight();
    }

    // Update is called once per frame
    void Update()
    {
        // return
        if (GameManager.instance.state != GameManager.State.Play) return;

        // distance
        pos = player.DistPlayer(transform.position);
        distSqrt = pos.sqrMagnitude;
        
        // light on match
        if (distSqrt < distSqrtFire 
            && pos.y > -distFireY && pos.y < distFireY
            && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            timeLight += Time.deltaTime;

            if (timeLight > timeLightRandom)
            {
                ResetLight();

                animator.SetTrigger("Light");
            } 
        }

        // stop light
        if (distSqrt < distSqrtStopLight
            && pos.y > -distFireY && pos.y < distFireY)
        {
            SpawnPig();
        }

    }

    void ResetLight()
    {
        timeLightRandom = Random.Range(timeLightMin, timeLightMax);
        timeLight = 0f;
    }

    public void Fire()
    {
        cannon.FireCannon();
    }

    void SpawnPig()
    {
        // spawn pig
        Instantiate(pfPig, transform.position, Quaternion.Euler(Vector2.zero));
        // destroy gameobject
        enemy.DestroyGo();
    }
}
