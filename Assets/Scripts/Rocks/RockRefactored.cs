using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RockRefactored : MonoBehaviour
{
    public Rigidbody2D rb;
    public int iniLife = 50;
    public int life=50;

    public float speed = 1;
    public bool bouncing = false;
    public int splitCount = 0;
    public int splitMultiplier = 0;

    public TMP_Text lifeText;
    public TMP_Text tempText;

    public Vector3 velocity;

    public float waitTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        life = iniLife;
        //GameObject[] allRocks = GameObject.FindGameObjectsWithTag("rock");
        //foreach(GameObject obj in allRocks)
        //{
        //    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
        //}
        Physics.IgnoreLayerCollision(8, 8);
        transform.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        if (!bouncing)
        {
            StartCoroutine(InitialMovement());
        }

        GameObject worldCanvas = GameObject.Find("Canvas World");
        tempText = Instantiate(lifeText, transform.position, Quaternion.identity);
        tempText.transform.SetParent(worldCanvas.transform, false);
        tempText.text = life.ToString();

    }

    IEnumerator InitialMovement()
    {
        yield return new WaitForSeconds(2);
        //Vector3 targetLoc = Vector3.Scale(transform.position,new Vector3(-1, 1, 1));
        Vector3 targetLoc = (Random.Range(1, 3) == 1) ? GameObject.Find("drop pos1").transform.position : GameObject.Find("drop pos2").transform.position;
        while (transform.position != targetLoc)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetLoc, Time.deltaTime * 5);
            yield return null;
        }
        rb.isKinematic = false;
        bouncing = true;
        Debug.Log("start bouncing");
        velocity = Vector3.Scale(transform.position, Vector3.one * -1).normalized * speed;
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        tempText.transform.position = transform.position;
        //if (life <= 0) { DestoryRock(); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            life--;
             Destroy(collision.gameObject);
            tempText.text = life.ToString();
            if (life <= 0) { DestoryRock(); }
           
        }
        //Debug.Log(gameObject.name + "\t" + collision.gameObject.tag);
        if (collision.gameObject.tag=="right bumper")
        {
            if (velocity.x > 0) { velocity = Vector3.Scale(velocity, new Vector3(-1, 1, 1)); }
            //Debug.Log(gameObject.name + "\t" + collision.gameObject.tag);
        }
        if(collision.gameObject.tag == "left bumper")
        {
            if (velocity.x < 0) { velocity = Vector3.Scale(velocity, new Vector3(-1, 1, 1)); }
            //Debug.Log(gameObject.name + "\t" + collision.gameObject.tag);
        }
        if (collision.gameObject.tag == "top bumper")
        {
            if (velocity.y > 0) { velocity = Vector3.Scale(velocity, new Vector3(1, -1, 1)); }
            //Debug.Log(gameObject.name + "\t" + collision.gameObject.tag);
        }
        if(collision.gameObject.tag == "bottom bumper")
        {
            if (velocity.y < 0) { velocity = Vector3.Scale(velocity, new Vector3(1, -1, 1)); }
            //Debug.Log(gameObject.name + "\t" + collision.gameObject.tag);
        }
    }

    public void DestoryRock()
    {
        Debug.Log("called once");
        
        if (splitCount > 0 && splitMultiplier >0)
        {
            for(int i=0; i < splitMultiplier; i++)
            {
                var obj=Instantiate(gameObject, transform.position, Quaternion.identity);
                Debug.Log(i+"\t"+gameObject.name);
                RockRefactored script = obj.GetComponent<RockRefactored>();
                script.iniLife = iniLife/2;
                script.bouncing = true;
                obj.transform.localScale *= 0.8f;
                script.splitCount = splitCount-1;
                script.splitMultiplier = splitMultiplier;
                script.speed = speed;
                if (i > 0) { script.velocity= Vector3.Scale(script.velocity, new Vector3(-1, 1, 1)); }
                if (obj.transform.position.y > 0) { script.velocity = Vector3.Scale(script.velocity, new Vector3(1, -1, 1)); }
                obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            }
        }
        Destroy(tempText.gameObject);
        Destroy(gameObject);
        GameController.instance.NextLevelCheck();
    }
}
