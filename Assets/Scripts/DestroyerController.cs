using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://github.com/cdunham927/TestProject/blob/main/Assets/Scripts/TestEnemy.cs
public class DestroyerController : EnemyBase
{

    public GameObject mine;
    public float mineDistance = 10;
    [SerializeField]
    private bool enRoute = false;

    private void DropMine()
    {
        Instantiate(mine);
    }

    private void SetMineLocation()
    {
        Vector2 target = player.transform.position + player.transform.forward * mineDistance; // n units infront of player
        Propel(target);
    }

    private void Attack(Vector2 target)
    {
        if(Vector3.Distance(transform.position,target) < 0.1f)
        {
            DropMine();
        }
        enRoute = false;
    }


}
