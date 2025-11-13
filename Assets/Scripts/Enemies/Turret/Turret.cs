using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Fernando Claro

public class Turret : MonoBehaviour
{
    private Shot shot;
    private bool playerDetected = false;
    private Transform target;

    public float burstDelay = 0.1f; // Tiempo entre cada disparo en la rafaga
    public int burstCount = 3; // Cantidad de disparos en cada rafaga
    public float burstInterval = 0.2f; // Intervalo de tiempo entre rafagas

    private void Start()
    {
        shot = GetComponent<Shot>();
        StartCoroutine(BurstFireRoutine());
    }

    private void Update()
    {

    }

    public void OnDetect()
    {
        playerDetected = true;
        target = Player.PlayerTransform;
    }

    public void OnExit()
    {
        playerDetected = false;
        target = null;
    }

    private IEnumerator BurstFireRoutine()
    {
        while (true)
        {
            if (playerDetected && shot != null && target != null)
            {
                for (int i = 0; i < burstCount; i++)
                {
                    shot.ThrowBullet();
                    yield return new WaitForSeconds(burstDelay);
                }
            }
            yield return new WaitForSeconds(burstInterval);
        }
    }
}


