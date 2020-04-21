using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DropInTest : MonoBehaviour
{
    public Rigidbody2D rb;


    public int life;
    public TMP_Text lifeText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 temp = new Vector2(-.3f, 0f);
        rb.AddForce(temp);
        Debug.Log( lifeText.transform.position);
        Debug.Log(lifeText.rectTransform.position);
    }

    public void RockHit()
    {
        life--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            Debug.Log("hey bitch");
        }
    }
}
