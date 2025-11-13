using System.Collections;
using UnityEngine;

//TP2 - Fernando Claro

public class DistanceEnemy : Entity
{
    [SerializeField] public float detectionRange = 100f; // Distancia máxima para empezar a seguir al jugador
    [SerializeField] public float stoppingDistance = 3f; // Distancia mínima para dejar de acercarse
    public float rotationSpeed = 5f; // Velocidad de rotación
    public float shootCooldown = 2f; // Tiempo de espera entre disparos

    private Shot shotComponent; // Referencia al componente Shot
    private bool canShoot = true; // Controla si el enemigo puede disparar

    private void Start()
    {
        shotComponent = GetComponent<Shot>(); // Obtener referencia al componente Shot
    }

    private void Update()
    {
        if (Player.PlayerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, Player.PlayerTransform.position);

            // Solo actuar si el jugador está dentro del rango de detección
            if (distance <= detectionRange)
            {
                // Mover hacia el jugador si está más lejos que el stoppingDistance
                if (distance > stoppingDistance)
                {
                    Vector3 direction = (Player.PlayerTransform.position - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;
                }


                // Rotar hacia el jugador
                Vector3 targetDirection = Player.PlayerTransform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Disparar si puede
                if (canShoot && shotComponent != null)
                {
                    shotComponent.ThrowBullet(); // Disparar la bala
                    StartCoroutine(ShootCooldown()); // Iniciar cooldown
                }
            }
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false; // Desactivar disparo
        yield return new WaitForSeconds(shootCooldown); // Esperar cooldown
        canShoot = true; // Activar disparo nuevamente
    }
}



