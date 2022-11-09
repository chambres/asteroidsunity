using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    Rigidbody2D rb;

    public float m_Thrust = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.name == "Asteroids"){
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        rb.mass = Random.Range(1,4)*4;

        switch(rb.mass){
            case 12:
                m_Thrust=1f;
                break;
            case 4:
                m_Thrust=3f;
                break;
            case 1:
                m_Thrust=5f;
                break;
        }
        rb.AddForce((Vector2.zero - (Vector2)(transform.position)).normalized * 100f);
        

        switch(rb.mass){
            case 12:
                this.transform.localScale = new Vector3(1f, 1f, 1);
                break;
            case 4:
                this.transform.localScale = new Vector3(.5f, .5f, 1);
                break;
            case 1:
                this.transform.localScale = new Vector3(.35f, .35f, 1);
                break;
        }
        
    }

    void OnTriggerEnter2D(Collider2D a){
        if(a.gameObject.name == "despawn"){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update(){
        
    }    
}