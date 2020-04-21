using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Rigidbody2D rb;

    public int life;
    public GameObject[] allRocks;
    public float rockDirection;

    private bool bounce;
    private bool colorChange;
    private bool divide;
    // Start is called before the first frame update
    void Start()
    {
        bounce = false;
        allRocks = GameObject.FindGameObjectsWithTag("rock");
        life = Random.Range(250, 500);
        for (int i = 0; i < allRocks.Length; i++)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), allRocks[i].GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropRock()
    {

    }

    public void DestoryRock()
    {
        Destroy(gameObject);
        GameController.instance.NextLevelCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            if (life <= 1)
            {
                DestoryRock();
            }
            else
            {
                life--;
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "right stopper")
        {
            rockDirection = 0f;
        }
        if (collision.gameObject.tag == "left stopper")
        {
            rockDirection = 0f;
        }
    }
}
