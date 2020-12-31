using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneTarget : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Blade")
        {
            ScoreManager.increaseScore(1);
            Destroy(gameObject);
        }
    }
}
