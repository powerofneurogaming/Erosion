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
        life = Random.Range(50, 75);
        for (int i = 0; i < allRocks.Length; i++)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), allRocks[i].GetComponent<Collider2D>());
        }
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(InitialMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (bounce) Move();
    }

    /// <summary>
    /// Moves the rock in a specific pathing when the game first starts, then sets bounce to True.
    /// </summary>
    /// <returns></returns>
    IEnumerator InitialMovement()
    {
        yield return new WaitForSeconds(2f);
        int temp = Random.Range(1, 3);
        rockDirection = Random.Range(-1, 0);

        Vector3 posBuffer = new Vector3((-1 * transform.position.x), transform.position.y, transform.position.z);
        Debug.Log(posBuffer);
        while (transform.position != posBuffer)
        {
             transform.position = Vector3.MoveTowards(transform.position, posBuffer, Time.deltaTime*2);
             yield return null;

             //transform.position = Vector3.Lerp(transform.position, pos1.transform.position, 1);
        }
        rb.isKinematic = false;
        bounce = true;
        Vector2 buff = new Vector2(-1*(transform.position.x), -1*transform.position.y);
        rb.velocity = buff;
        //rockDirection = 1;

    }

    IEnumerator Move()
    {
        var x = Random.Range(-4, 8);
        var y = Random.Range(-2, 4);
        Vector2 temp = new Vector2(1f, 1f);
        //rb.velocity = new Vector3(x, y, 0f);
        rb.AddForce(temp);

        yield return new WaitForSeconds(15f);
    }

    public void Hit()
    {
        if (life <= 1) DestoryRock();

        else life--;

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
            Hit();
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
