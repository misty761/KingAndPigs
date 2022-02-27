using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    Rigidbody2D mRigidbody;
    public float forceX = 50;
    public float forceY = 100;
    public float offsetX = 0.05f;
    public float offsetY = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mRigidbody.AddForce(new Vector2(forceX, forceY));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
