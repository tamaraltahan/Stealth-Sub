using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
 
    private Rigidbody2D bod;
    public float speed;
    int status;
    Vector2 start;
    Vector2 end;
    Vector2 maxDistance;
    public int damage = 22;
    public Transform parentGun;

    //bool passed = Vector3.Dot((end - start).normalized, (p - end).normalized) > 0;

    private void OnEnable()
    {
        bod.AddForce(parentGun.up * speed);
        Invoke("Disable", 4f);
        start = this.transform.position;
        //end = GameObject.Find("Player").transform.position;
        //end = transform.position + transform.forward * 10f;
        float[] percentages = { 1f, 0.8f, 0.7f, 0.6f };
        maxDistance = Vector2.Lerp(start, end, percentages[status]);
    }

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
        status = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        //not working & I'm sad :(
        //i fucking give up
        bool passed = Vector3.Dot((end - start).normalized, (maxDistance - end).normalized) > 0;
        if (passed || (Vector2)transform.position == end)
        {
            Destroy(gameObject);
        }

        Debug.DrawLine(this.transform.position, maxDistance, Color.red);
        Debug.DrawLine(this.transform.position, end, Color.blue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().DoDamage(damage);
            Destroy(gameObject);
        }
    }


    private void Disable()
    {
        Destroy(gameObject);
    }
}
