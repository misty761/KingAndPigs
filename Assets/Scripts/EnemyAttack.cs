using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject col;

    // Start is called before the first frame update
    void Start()
    {
        col.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
