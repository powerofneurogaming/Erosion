using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    Rigidbody2D rb;

    public float dropSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Vector2 buff = new Vector2(0f, dropSpeed);
        //rb.AddForce(buff);
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "bottom bumper")
        {
            Destroy(gameObject);
        } 
    }
}
