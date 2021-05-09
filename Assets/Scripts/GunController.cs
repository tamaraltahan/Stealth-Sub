using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    PlayerController player;
    public float rotationSpeed;
    GameObject enemy;
    [SerializeField] bool isRearGun;
    public bool rotationComplete = false;
    public GameObject ProjectileObject;
    [SerializeField] float rotationDegree = 90f;
    public float attackCD = 5f;
    [SerializeField] float cools;
    [SerializeField] private Quaternion defaultRotation;

    public Transform[] spawns;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        enemy = this.transform.parent.gameObject;
        defaultRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        cools = attackCD;
    }

    private void TurnGunsToPlayer()
    {
        Vector3 target = player.transform.position;
        Vector3 objectPosition = transform.position;

        target.x = target.x - objectPosition.x;
        target.y = target.y - objectPosition.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);
        //gun.transform.localRotation = Quaternion.RotateTowards(gun.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, targetRotation) <= 1f)
        {
            rotationComplete = true;
        }
        else
        {
            rotationComplete = false;
        }
    }

    private void ResetGuns()
    {
        if (!isRearGun)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, enemy.transform.rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation * enemy.transform.rotation, Time.deltaTime * rotationSpeed);
        }

    }
    // Update is called once per frame
    void Update()
    {
        bool detected = enemy.GetComponent<EnemyBase>().playerDetected;
        if (detected) {
            TurnGunsToPlayer();
        }
        else
        {
            ResetGuns();
        }

        if (rotationComplete && detected)
        {
            if (cools <= 0) Shoot();
        }


        if (cools > 0) cools -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject missile = ProjectileObject;
        missile.GetComponent<ShellController>().parentGun = this.transform;
        for (int i = 0; i < spawns.Length; ++i)
        {
            Instantiate(missile, this.transform.position, this.transform.rotation);
        }
        cools = attackCD;
    }

}
