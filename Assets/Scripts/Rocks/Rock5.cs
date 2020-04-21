using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rock5 : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject[] rocks;
    public GameObject rockBabyLeft;
    public GameObject rockBabyRight;

    public float rockDirection;

    public int life;
    public TMP_Text lifeText;
    public TMP_Text tempText;
    //public Canvas worldCanvas;

    private bool bounceTime;

    void Start()
    {
        bounceTime = false;
        rocks = GameObject.FindGameObjectsWithTag("rock");
        life = Random.Range(50, 200);
        for (int i = 0; i < rocks.Length; i++)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), rocks[i].GetComponent<Collider2D>());
        }

        rb = GetComponent<Rigidbody2D>();
        GameObject worldCanvas = GameObject.Find("Canvas World");

        tempText = Instantiate(lifeText, transform.position, Quaternion.identity);
        tempText.transform.SetParent(worldCanvas.transform, false);
        tempText.text = life.ToString();


        StartCoroutine(DropRock());
    }

    // Update is called once per frame
    void Update()
    {
        MoveText();
        if (bounceTime)
        {
            MoveRock();
        }
    }

    IEnumerator DropRock()
    {
        yield return new WaitForSeconds(2f);
        int temp = Random.Range(1, 3);
        int counter = 1000;
        if (temp == 1)
        {
            GameObject pos1 = GameObject.Find("drop pos1");
            while (transform.position != pos1.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos1.transform.position, Time.deltaTime);
                yield return null;

                //transform.position = Vector3.Lerp(transform.position, pos1.transform.position, 1);
                counter--;
                if (counter <= 0)
                {
                    Debug.Log("broke that shit");
                    break;
                }
            }
            rb.isKinematic = false;
            bounceTime = true;
            Vector2 temp1 = new Vector2(rockDirection, 0f);
            rb.velocity = temp1;
            rockDirection = 1;


        }
        else
        {
            GameObject pos2 = GameObject.Find("drop pos2");
            while (transform.position != pos2.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos2.transform.position, Time.deltaTime);
                yield return null;

                //transform.position = Vector3.Lerp(transform.position, pos2.transform.position, 1);
                counter--;
                if (counter <= 0)
                {
                    Debug.Log("broke that shit");
                    break;
                }
            }
            rb.isKinematic = false;
            bounceTime = true;
            Vector2 temp1 = new Vector2(-rockDirection, 0f);
            rb.velocity = temp1;
            rockDirection = -1;

        }

        yield return null;
    }

    public void MoveRock()
    {
        Vector2 temp = new Vector2(rockDirection, 0f);
        rb.AddForce(temp);
    }

    // moves the life text with the ball
    public void MoveText()
    {
        tempText.transform.position = transform.position;
    }

    public void RockHit()
    {
        life--;
        tempText.text = life.ToString();

        if (life <= 0)
        {
            DestoryRock();
        }
        life--;
        tempText.text = life.ToString();
    }

    public void DestoryRock()
    {
        Destroy(tempText);
        Destroy(gameObject);
        GameController.instance.NextLevelCheck();
        //Instantiate(rockBabyLeft, transform.position, Quaternion.identity);
        Instantiate(rockBabyRight, transform.position, Quaternion.identity);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            RockHit();
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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "rock")
        {
            Debug.Log("klasdflksdflsah");
        }
        if (collision.gameObject.tag == "right bumper")
        {
            rockDirection = -.01f;
        }
        if (collision.gameObject.tag == "left bumper")
        {
            rockDirection = .01f;
        }
    }
}
