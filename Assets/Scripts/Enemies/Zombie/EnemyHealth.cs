using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Prefabs")]
    public GameObject bloodEffectPrefab; // splash pegado al enemigo
    public GameObject floorBloodPrefab;  // sangre en el piso

    [Header("Layers")]
    public LayerMask floorMask; // ASIGNAR: solo "Floor" o where the blood should land

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Vector3 hitPosition, Vector3 hitNormal)
    {
        currentHealth -= amount;

        // 1) SPLASH
        if (bloodEffectPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(-hitNormal, Vector3.up);
            Instantiate(bloodEffectPrefab, hitPosition, rot, transform);       
            bloodEffectPrefab.transform.Rotate(0, 180, 0); // lo invertís
        }

        // 2) FLOOR BLOOD (SIEMPRE EN EL SUELO)
        if (floorBloodPrefab != null)
        {
            if (TryGetFloorPoint(hitPosition, out Vector3 floorPos))
            {
                Instantiate(
                    floorBloodPrefab,
                    floorPos + Vector3.up * 0.002f,   // evitar z-fighting
                    Quaternion.identity
                );
            }
        }

        if (currentHealth <= 0)
            Die();
    }

    // RAYCAST QUE IGNORA AL ENEMIGO → FUNCIONA SIEMPRE
    bool TryGetFloorPoint(Vector3 hitPos, out Vector3 floorPos)
    {
        RaycastHit hit;

        // Empezamos MUCHO más arriba para evitar tocar el collider del enemigo
        Vector3 start = hitPos + Vector3.up * 2f;

        // layerMask evita chocar con el enemigo y solo golpea el suelo
        if (Physics.Raycast(start, Vector3.down, out hit, 20f, floorMask))
        {
            floorPos = hit.point;
            return true;
        }

        floorPos = hitPos;
        return false;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

