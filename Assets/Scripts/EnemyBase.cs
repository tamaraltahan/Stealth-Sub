using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float attackRange = 3f;
    public float wanderDistance;

    protected CircleCollider2D vision;

    public float wanderRadius;

    protected Rigidbody2D bod;
    protected PlayerController player;

    public bool playerDetected = false;

    protected Vector2 target;

    [SerializeField] protected float minRange;

    protected void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        bod = GetComponent<Rigidbody2D>();
        vision = GetComponent<CircleCollider2D>();
        target = transform.position * Random.insideUnitCircle * Random.Range(0, wanderRadius);
        vision.radius = attackRange;

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerDetected();
            playerDetected = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerLost();
            playerDetected = false;
            target = transform.position * Random.insideUnitCircle * Random.Range(0, wanderRadius);
        }
    }

    protected void PlayerLost()
    {
        attackRange -= attackRange * 0.15f;
        vision.radius = attackRange;
    }

    protected void PlayerDetected()
    {
        attackRange += attackRange * 0.15f;
        vision.radius = attackRange;
    }

    protected void Wander()
    { 
        if (!playerDetected) wanderDistance = Vector2.Distance(transform.position, target);
        if (!playerDetected && wanderDistance <= minRange) target = (Vector2)transform.position + Random.insideUnitCircle * Random.Range(0, wanderRadius);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(player.dived && playerDetected)
        {
            playerDetected = false;
        }
        Wander();
        Debug.DrawLine(this.transform.position, target, Color.yellow);
    }

    protected virtual void FixedUpdate()
    {
        if (playerDetected)
        {
            target = player.transform.position;
        }
            Propel(target);
    }

    protected void Propel(Vector2 target)
    {
        bod.AddForce(speed * 200f * Time.fixedDeltaTime * transform.up);
        Vector3 objectPosition = transform.position;

        target.x = target.x - objectPosition.x;
        target.y = target.y - objectPosition.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.fixedDeltaTime);
    }

    protected void MoveShipToPlayer()
    {
        Propel(player.transform.position);
    }
}
