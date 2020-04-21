using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;


public class PlayerController2 : MonoBehaviour
{

    private bool tobiOn; // turn the tobii on and off
    public static bool fireReady = true;

    public float fireRate = .05f;
    public float z;

    public Vector2 firePosition;

    public GameObject bullet;

    void Awake()
    {
        Vector3 camPos = Camera.main.transform.position;
    }



    void Start()
    {
        tobiOn = false; // TOBII


        fireReady = true;
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
        firePosition = new Vector2(transform.position.x, transform.position.y + 1.3f);
        Instantiate(bullet, firePosition, Quaternion.identity);
        yield return new WaitForSeconds(fireRate);
        fireReady = true;

    }
    public void StartGame()
    {

    }

    public void GameOver()
    {
        Debug.Log("you got bonked on the head");
        gameObject.SetActive(false);
        GameController.instance.GameOver();
    }

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
    }




}

