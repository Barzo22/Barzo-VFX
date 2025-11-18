using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Blood Prefabs")]
    public GameObject bloodEffectPrefab;
    public GameObject floorBloodPrefab;

    [Header("Floor Raycast")]
    public LayerMask floorMask;

    [Header("Dissolve")]
    public string dissolveProperty = "_Dissolve";
    public float dissolveDuration = 1.5f;
    public float dissolveFrom = 0f;
    public float dissolveTo = 1f;

    [Header("Animation")]
    public Animator anim;

    private bool isDying = false;
    private Renderer[] childRenderers;
    private Material[][] instantiatedMaterialsPerRenderer;
    private Rigidbody rb;
    private Collider[] colliders;

    void Awake()
    {
        currentHealth = maxHealth;

        // Animator
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        if (anim == null)
            Debug.LogError("[EnemyHealth] NO se encontró Animator en el enemigo.");

        // Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // física desactivada mientras está vivo
        rb.useGravity = false;

        // Colliders
        colliders = GetComponentsInChildren<Collider>();

        // Preparar materiales instanciados para dissolve
        childRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        instantiatedMaterialsPerRenderer = new Material[childRenderers.Length][];
        for (int r = 0; r < childRenderers.Length; r++)
        {
            var rend = childRenderers[r];
            var mats = rend.sharedMaterials;
            Material[] newMats = new Material[mats.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] == null) { newMats[i] = null; continue; }
                newMats[i] = new Material(mats[i]);
                if (newMats[i].HasProperty(dissolveProperty))
                    newMats[i].SetFloat(dissolveProperty, dissolveFrom);
            }

            rend.materials = newMats;
            instantiatedMaterialsPerRenderer[r] = newMats;
        }
    }

    public void TakeDamage(int amount, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (isDying) return;

        currentHealth -= amount;

        // Efectos de sangre
        if (bloodEffectPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(-hitNormal, Vector3.up);
            Instantiate(bloodEffectPrefab, hitPosition, rot, transform);
        }
        if (floorBloodPrefab != null && TryGetFloorPoint(hitPosition, out Vector3 floorPos))
        {
            Instantiate(floorBloodPrefab, floorPos + Vector3.up * 0.002f, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            StartDeath();
        }
    }

    void StartDeath()
    {
        if (isDying) return;

        // INSTANTÁNEO → bloquear daño & colisiones YA
        isDying = true;

        // Cambiar capa YA para que balas NO choquen más
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

        // Desactivar TODOS los colliders YA (evita que reciba más impactos)
        foreach (var col in colliders)
            col.enabled = false;

        // Volvemos a activar SOLO el collider raíz para tocar el suelo
        Collider rootCol = GetComponent<Collider>();
        if (rootCol != null)
            rootCol.enabled = true;

        // Resetear fuerzas inmediatamente
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Desactivar scripts de movimiento/AI YA
        foreach (var s in GetComponents<MonoBehaviour>())
        {
            if (s != this) s.enabled = false;
        }

        // Ahora sí: animación
        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(DeathSequence());
    }



    IEnumerator DeathSequence()
    {
        // Esperar a que termine la animación de muerte
        if (anim != null)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            float animLength = state.length;
            yield return new WaitForSeconds(animLength);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        // Iniciar dissolve y destrucción
        yield return DissolveAndDieCoroutine();
    }

    IEnumerator DissolveAndDieCoroutine()
    {
        bool anyHasProperty = false;
        foreach (var rendMats in instantiatedMaterialsPerRenderer)
        {
            if (rendMats == null) continue;
            foreach (var m in rendMats)
            {
                if (m != null && m.HasProperty(dissolveProperty))
                {
                    anyHasProperty = true;
                    break;
                }
            }
            if (anyHasProperty) break;
        }

        if (!anyHasProperty)
        {
            Destroy(gameObject);
            yield break;
        }

        float t = 0f;
        while (t < dissolveDuration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(dissolveFrom, dissolveTo, t / dissolveDuration);
            for (int r = 0; r < instantiatedMaterialsPerRenderer.Length; r++)
            {
                var mats = instantiatedMaterialsPerRenderer[r];
                if (mats == null) continue;
                for (int i = 0; i < mats.Length; i++)
                {
                    var m = mats[i];
                    if (m != null && m.HasProperty(dissolveProperty))
                        m.SetFloat(dissolveProperty, v);
                }
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    bool TryGetFloorPoint(Vector3 hitPos, out Vector3 floorPos)
    {
        RaycastHit hit;
        Vector3 start = hitPos + Vector3.up * 2f;
        if (Physics.Raycast(start, Vector3.down, out hit, 20f, floorMask))
        {
            floorPos = hit.point;
            return true;
        }
        floorPos = hitPos;
        return false;
    }
}







