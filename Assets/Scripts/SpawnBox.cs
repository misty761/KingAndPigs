using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public GameObject pfBox;
    public GameObject pfBoxEnemy;
    public float probBox = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        float random = Random.Range(0, 1f);
        if(random < probBox)
        {
            Instantiate(pfBox, transform.position, Quaternion.Euler(Vector2.zero));
        }    
        else if (random < probBox * 2)
        {
            Instantiate(pfBoxEnemy, transform.position, Quaternion.Euler(Vector2.zero));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
