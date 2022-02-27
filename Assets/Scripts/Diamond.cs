using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public int point = 50;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // effect
            Instantiate(effect, transform.position, Quaternion.Euler(Vector2.zero));
            // score
            GameManager.instance.Scored(point);
            // Destroy
            Destroy(gameObject);
        }
    }
}
