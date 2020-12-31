using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneTarget : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(rb);
        
    }
    void Update(){
        rb.velocity = new Vector2(0.0f, -1.0f);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //user collide with ToneTarget
        if (col.tag == "Blade")
        {
            ScoreManager.increaseScore(1);
            Destroy(gameObject);
        }
    }
}
