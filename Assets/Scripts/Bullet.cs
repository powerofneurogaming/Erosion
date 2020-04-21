using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;

    public float bulletSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 temp = new Vector2(0f, bulletSpeed);
        rb.AddForce(temp);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 temp = new Vector2(0f, bulletSpeed);
        //rb.velocity = temp;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "top bumper")
        {
            Destroy(gameObject);
        }
    }
}
