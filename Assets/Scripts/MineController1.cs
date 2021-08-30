using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController1 : MonoBehaviour
{
    public float armTime = 0.5f;
    bool isArmed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isArmed && armTime > 0)
        {
            armTime -= Time.deltaTime;
        }     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            //play animation
            //detonate destroyers, don't harm battleships
        }
    }
}
