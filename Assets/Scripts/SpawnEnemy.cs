using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] pfEnemies;

    // Start is called before the first frame update
    void Start()
    {
        float random = Random.Range(0f, 1f);
        if (random < (float)1/pfEnemies.Length)
        {
            Instantiate(pfEnemies[0], transform.position, Quaternion.Euler(Vector2.zero));
        }
        else if (random < (float)2 /pfEnemies.Length)
        {
            Instantiate(pfEnemies[1], transform.position, Quaternion.Euler(Vector2.zero));
        }
        else
        {
            Instantiate(pfEnemies[2], transform.position, Quaternion.Euler(Vector2.zero));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
