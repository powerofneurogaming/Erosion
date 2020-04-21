using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;


public class PlayerController : MonoBehaviour
{

    public bool tobiOn; // turn the tobii on and off
    public static bool fireReady = true;

    public float fireRate = .05f;
    public float z;

    public Vector2 firePosition1;
    public Vector2 firePosition2;
    public Vector2 firePosition3;
    public Vector2 firePosition4;
    public Vector2 firePosition5;

    public Animator playerAnimator;
    public SpriteRenderer bulletImage;

    public bool volcanoSwitch;
    public bool tornadoSwitch;
    public bool waterSwtich;


    public GameObject bullet;

    void Awake()
    {
        Vector3 camPos = Camera.main.transform.position;
    }



    void Start()
    {
        tobiOn = false; // TOBII

        playerAnimator = GetComponent<Animator>();

        //fireReady = false;
        z = 11f;
    }

    void Update()
    {
        if (tobiOn)
        {
            TobiiOnCall();
            if (fireReady)
            {
                StartCoroutine(FireBullet());
            }
        }
        if (tobiOn == false)
        {
            TobiiOffCall();
            if (fireReady)
            {
                StartCoroutine(FireBullet());
            }
        }

    }

    public IEnumerator FireBullet()
    {
        fireReady = false;
        firePosition1 = new Vector2(transform.position.x, transform.position.y + 1.3f);
        //firePosition2 = new Vector2(transform.position.x+.5f, transform.position.y + 1.5f);
        //firePosition3 = new Vector2(transform.position.x+1, transform.position.y + 1.9f);
        //firePosition4 = new Vector2(transform.position.x-.5f, transform.position.y + 1.5f);
        //firePosition5 = new Vector2(transform.position.x-1, transform.position.y + 1.9f);

        Instantiate(bullet, firePosition1, Quaternion.identity);
        //Instantiate(bullet, firePosition2, Quaternion.identity);
        //Instantiate(bullet, firePosition3, Quaternion.identity);
        //Instantiate(bullet, firePosition4, Quaternion.identity);
        //Instantiate(bullet, firePosition5, Quaternion.identity);


        yield return new WaitForSeconds(fireRate);
        fireReady = true;

    }
    public void StartGame()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void GameOver()
    {
        Debug.Log("you got bonked on the head");
        gameObject.SetActive(false);
        GameController.instance.GameOver();
    }


    // Options Menu shit below


    public void ChangeToTornado()
    {
        //playerAnimator.Play("tornado");
        playerAnimator.SetInteger("AnimationList", 1);
    }
    public void ChangeToVolcano()
    {
        //playerAnimator.Play("volcano");
        playerAnimator.SetInteger("AnimationList", 2);
    }


    // Tobii and colision shit below
    public void TobiiOnCall()
    {
        float x = TobiiAPI.GetGazePoint().Screen.x;
        float y = TobiiAPI.GetGazePoint().Screen.y;
        Vector3 tobiPos = new Vector3(x, y, z - .5f);
        tobiPos = Camera.main.ScreenToWorldPoint(tobiPos);

        //             Debug.Log(x + " , " + y);

        Vector3 temp = transform.position;
        temp.x = tobiPos.x;
        //temp.y = tobiPos.y;
        //temp.z = z;
        transform.position = temp;
    }

    public void TobiiOffCall()
    {
        //Vector3 deckPos = Zdeck.transform.position;
        Vector3 temp = transform.position;
        Vector3 mousePose = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, z - .5f));
        temp.x = mousePose.x;
        //temp.y = mousePose.y;
        //temp.z = mousePose.z;
        transform.position = temp;
        //Debug.Log(mousePose);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "rock")
        {
            GameOver();
        }

        if (collision.gameObject.tag == "spike")
        {
            GameOver();
        }
        if (collision.gameObject.tag == "right bumper")
        {
            
        }
    }




}

