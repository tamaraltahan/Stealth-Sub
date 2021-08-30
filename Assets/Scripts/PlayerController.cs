using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float lerpSpeed; //rotation speed
    Rigidbody2D bod;
    Vector2 input;


    [Header("Oxygen")]
    public float diveCD = 1f;
    private float cools;
    public bool dived = false;
    public float maxOxygen;
    float currentOxygen;
    public Image OxygenMeter;
    SpriteRenderer meter;
    Color cyan = new Color(0, 204, 204);
    Color orange = new Color(255, 128, 0);
    public Text oxygenPercentText;
    private Animator anim;


    [Header("Weapon")]
    public GameObject spawnPoint;
    public GameObject torpedo;
    public float timeToAttack;
    float attackCD;

    [Header("Health")]
    public int maxHP = 100;
    private int currHP;



    private void Start()
    {
        currHP = maxHP;
        currentOxygen = maxOxygen;
        cools = diveCD;
        anim.Play("Breach");
    }

    void Awake()
    {
        meter = GetComponent<SpriteRenderer>();
        bod = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.x != 0)
        {
            transform.Rotate(0, 0, lerpSpeed * -input.x);
        }
        if (input.y != 0)
        {
            // Mathf.Clamp01(input.y) use this for only forward movement if I want to
            bod.AddForce(transform.up * input.y * speed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && attackCD <= 0)
        {
            shoot();
        }
        if (Input.GetKeyDown(KeyCode.Space) && cools <= 0)
        {
            Dive();
        }

        if (currentOxygen < maxOxygen && !dived)
        {
            currentOxygen += Time.deltaTime / 2;
        }


        if (dived && currentOxygen >= 0)
        {
            currentOxygen -= Time.deltaTime / 3;
            if (dived && currentOxygen <= 0)
            {
                Dive();
            }
        }

        if(cools > 0) cools -= Time.deltaTime;
        if (attackCD > 0) attackCD -= Time.deltaTime;
        OxygenMeter.fillAmount = currentOxygen / maxOxygen;
        oxygenPercentText.text = (OxygenMeter.fillAmount * 100).ToString("F0") + "%";
        changeColor();
    }

    void changeColor()
    {
        
        if(OxygenMeter.fillAmount <= 0.5)
        {
            //Debug.Log("Fill Amount: " + OxygenMeter.fillAmount);
            if(OxygenMeter.fillAmount > 0.25)
            {
                OxygenMeter.material.color = orange;
            }
            else if(OxygenMeter.fillAmount <= 0.25)
            {
                OxygenMeter.material.color = Color.red;
            }
        }
        if(OxygenMeter.fillAmount > .5)
        {
            OxygenMeter.material.color = cyan;
        }

    }

    void shoot()
    {
        Instantiate(torpedo, spawnPoint.transform.position, spawnPoint.transform.rotation);
        attackCD = timeToAttack;
    }

    void Dive()
    {
        cools = diveCD;
        if (dived)
        {
            dived = false;
            anim.Play("Breach");
        }
        else
        {
            dived = true;
            anim.Play("Dive");
        }
    }

    public void DoDamage(int x)
    {
        currHP -= x;

        if(currHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Death");
        Destroy(gameObject);
    }

}
