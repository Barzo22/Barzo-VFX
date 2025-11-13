using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Ariadna Delpiano

public class Enemy : Entity
{
    public float stoppingDistance;
    public float rotationSpeed;
    public float shootCooldown;

    private void Update()
    {
        if (Player.PlayerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.PlayerTransform.position, speed * Time.deltaTime);

            Vector3 targetDirection = Player.PlayerTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
