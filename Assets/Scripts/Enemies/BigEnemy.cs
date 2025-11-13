using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Fernando Claro

public class BigEnemy : Enemy
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Health = 0;
            }
        }
    }
}


