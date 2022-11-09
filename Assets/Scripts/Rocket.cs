using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    

    public float speed;
    public float degrees;
    public float maxVelocity;
    float buffer = 0f;
    
    
    
    Rigidbody2D rb;
    float leftConstraint;
    float rightConstraint;
    float bottomConstraint;
    float topConstraint;

    Animator anim;
    SpriteRenderer spr;

    public GameObject backthrust;
    public GameObject frontthrust;
  

    // Start is called before the first frame update
    void Start()
    {

        
        spr=GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        
        Camera cam = Camera.main;
        float distanceZ = Mathf.Abs (cam.transform.position.z + transform.position.z);
        leftConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).x;
        rightConstraint = cam.ScreenToWorldPoint (new Vector3 (Screen.width, 0.0f, distanceZ)).x;
        bottomConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, 0.0f, distanceZ)).y;
        topConstraint = cam.ScreenToWorldPoint (new Vector3 (0.0f, Screen.height, distanceZ)).y;
        
        rb=GetComponent<Rigidbody2D>();

        maxVelocity = maxVelocity*maxVelocity;

        backthrust.SetActive(false);
        frontthrust.SetActive(false);

        InvokeRepeating("spawnAsteroids", 0f, 5f);
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.name == "shoot2" || col.gameObject.name == "shoot2(Clone)"){
            return;
        }
        rb.velocity= new Vector2(transform.right.x, transform.right.y) * 0f;
        anim.SetTrigger("bigexplosion");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    bool up;
    bool down;
    
    void Update(){
        if(Input.GetKeyDown(KeyCode.W))
        {
            up = true;
            backthrust.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.W)){
            up = false;
            backthrust.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            down = true;
            frontthrust.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.S)){
            down = false;
            frontthrust.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Shoot();
        }
        
    }

    public GameObject shot;

    void Shoot(){
        GameObject a = Instantiate(shot, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
        a   .GetComponent<Rigidbody2D>().velocity += (Vector2)transform.right * 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(up)
        {
            rb.velocity += new Vector2(transform.right.x, transform.right.y) * speed;
        }

        if(down){
            rb.velocity -= new Vector2(transform.right.x, transform.right.y) * speed/2;
        }

        if(Input.GetKey("a"))
        {
            rb.rotation += degrees;
        }
        if(Input.GetKey("d"))
        {
            rb.rotation -= degrees;
        }
        if (transform.position.x < leftConstraint - buffer) {
             transform.position = new Vector3 (rightConstraint + buffer, transform.position.y, transform.position.z);
         }
         if (transform.position.x > rightConstraint + buffer) {
             transform.position = new Vector3 (leftConstraint - buffer, transform.position.y, transform.position.z);
         }
         if (transform.position.y < bottomConstraint - buffer) {
             transform.position = new Vector3 (transform.position.x, topConstraint + buffer, transform.position.z);
         }
         if (transform.position.y > topConstraint + buffer) {
             transform.position = new Vector3 (transform.position.x, bottomConstraint - buffer, transform.position.z);
         }
         if(rb.velocity.sqrMagnitude > maxVelocity){
            rb.velocity = rb.velocity.normalized * Mathf.Sqrt(maxVelocity);
         }

        
    }

    public GameObject asteroid;

    void spawnAsteroids(){
        Instantiate(asteroid, new Vector2(-3f, Random.Range(-1.25f, 1.25f)),Quaternion.identity);
        int r = Random.Range(1,5);
        
        switch(r){
            case 1:
                Instantiate(asteroid, new Vector2(-3f, Random.Range(-1.25f, 1.25f)),Quaternion.identity);
                break;
            case 2:
                Instantiate(asteroid, new Vector2(Random.Range(-2.7f, 2.7f), 2f),Quaternion.identity);
                break;
            case 3:
                Instantiate(asteroid, new Vector2(Random.Range(-2.7f, 2.7f), -2f),Quaternion.identity);
                break;
            case 4:
                Instantiate(asteroid, new Vector2(3f, Random.Range(-1.25f, 1.25f)),Quaternion.identity);
                break;
        }
       return;
    }

    IEnumerator flash(){
        GetComponent<PolygonCollider2D>().enabled = false;
        for(var n = 0; n < 5; n++)
        {
            spr.enabled = true;
            backthrust.SetActive(true);
            frontthrust.SetActive(true);
            yield return new WaitForSeconds(.2f);
            spr.enabled = false;
            backthrust.SetActive(false);
            frontthrust.SetActive(false);
            yield return new WaitForSeconds(.2f);
        }
        spr.enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    void reset(){
        Debug.Log("hi");
        this.transform.position = new Vector2(0,0);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.rotation = 0f;
        rb.angularVelocity = 0f;
        StartCoroutine(flash());

    }
}
