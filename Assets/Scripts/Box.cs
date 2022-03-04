using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public AudioClip audioBreak;
    public GameObject[] parts;
    public bool isBroken;
    public GameObject[] pfItems;
    public float[] probItems;
    public bool isSpawn;
    public bool isThrowed;
    public int power = 1;

    // Start is called before the first frame update
    void Start()
    {
        //isBroken = false;
        isSpawn = true;
        //isThrowed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // player hit by box which was throwed by pig
        if (isThrowed)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
                player.GetDamage(power);
            }
        }

        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            // break box
            if (isBroken)
            {
                BreakBox();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // break box
        if (isBroken)
        {
            BreakBox();
        }
    }

    public void BreakBox()
    {
        // sound
        SoundManager.instance.PlaySound(audioBreak, transform.position, 1f);
        // spawn parts
        for (int i = 0; i < parts.Length; i++)
        {
            GameObject goPart = Instantiate(parts[i], transform.position, Quaternion.Euler(Vector2.zero));
            Part part = goPart.GetComponent<Part>();
            goPart.transform.position = new Vector2(transform.position.x + part.offsetX, transform.position.y + part.offsetY);
        }
        if (isSpawn)
        {
            // spawn item
            float random = Random.Range(0f, 1f);
            if (random < probItems[0]) Instantiate(pfItems[0], transform.position, Quaternion.Euler(Vector2.zero));
            else if (random < probItems[1]) Instantiate(pfItems[1], transform.position, Quaternion.Euler(Vector2.zero));
        }
        // destroy gameObject
        Destroy(gameObject);
    }

}
