using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    PlayerMove player;

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
            enemy.GetDamage(player.power);
            print(player.power);
        }
    }
}
