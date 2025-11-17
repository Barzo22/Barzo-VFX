using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Blood Prefabs")]
    public GameObject bloodEffectPrefab; // splash pegado al enemigo (particle system prefab)
    public GameObject floorBloodPrefab;  // sangre en el suelo (decals/quad/prefab)

    [Header("Floor Raycast")]
    public LayerMask floorMask; // asignar la layer del suelo

    [Header("Dissolve")]
    [Tooltip("Nombre de la propiedad en el shader (Reference)")]
    public string dissolveProperty = "_Dissolve";
    public float dissolveDuration = 1.5f;
    // si querés que empiece con valor distinto (normalmente 0 = visible)
    public float dissolveFrom = 0f;
    public float dissolveTo = 1f;

    // estado interno
    bool isDying = false;

    // para manipular materiales de todos los renderers hijos
    Renderer[] childRenderers;
    Material[][] instantiatedMaterialsPerRenderer;

    void Awake()
    {
        currentHealth = maxHealth;

        // Capturamos todos los renderers (MeshRenderer, SkinnedMeshRenderer, SpriteRenderer si aplicara)
        childRenderers = GetComponentsInChildren<Renderer>(includeInactive: false);
        instantiatedMaterialsPerRenderer = new Material[childRenderers.Length][];

        // Instanciamos/copiamo material por material para no tocar sharedMaterial
        for (int r = 0; r < childRenderers.Length; r++)
        {
            var rend = childRenderers[r];
            var mats = rend.materials; // this creates instances in editor/runtime, but we'll replace to be safe
            Material[] newMats = new Material[mats.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                newMats[i] = new Material(mats[i]); // create instance
                // inicializamos el valor del dissolve para que comience en dissolveFrom
                if (newMats[i].HasProperty(dissolveProperty))
                    newMats[i].SetFloat(dissolveProperty, dissolveFrom);
            }
            rend.materials = newMats;
            instantiatedMaterialsPerRenderer[r] = newMats;
        }
    }

    // Llamar desde raycast o projectile: TakeDamage(damage, hitPoint, normal)
    public void TakeDamage(int amount, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (isDying) return;

        currentHealth -= amount;

        // 1) SPLASH pegado al enemy (se parenta para que se mueva con el cuerpo)
        if (bloodEffectPrefab != null)
        {
            // Queremos que la splash "apunte" hacia afuera del cuerpo: rot = LookRotation(-hitNormal)
            Quaternion rot = Quaternion.LookRotation(-hitNormal, Vector3.up);
            GameObject fx = Instantiate(bloodEffectPrefab, hitPosition, rot, transform);
            // Si tu prefab tiene ParticleSystems que simulan en World space y querés que sigan al enemigo,
            // asegurate en el prefab que "Simulation Space" esté en "Local".
        }

        // 2) Floor blood (siempre en el suelo)
        if (floorBloodPrefab != null)
        {
            if (TryGetFloorPoint(hitPosition, out Vector3 floorPos))
            {
                Instantiate(floorBloodPrefab, floorPos + Vector3.up * 0.002f, Quaternion.identity);
            }
        }

        if (currentHealth <= 0)
            StartDeath();
    }

    void StartDeath()
    {
        if (isDying) return;
        isDying = true;

        // Desactivar colisiones / scripts para que no interfieran con la animación del dissolve
        DisableGameplayComponents();

        // Iniciar coroutine que anima el dissolve y destruye al final
        StartCoroutine(DissolveAndDieCoroutine());
    }

    // Desactiva colliders y componentes de movimiento/IA si existen
    void DisableGameplayComponents()
    {
        // Desactivar colliders (evita que otros raycasts o físicas empujen al cuerpo)
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols) c.enabled = false;

        // Intentá desactivar cualquier script de movimiento/AI que hayas en el mismo GameObject
        var ai = GetComponent<MonoBehaviour>(); // no muy específico; mejor desactivar concretos si los conoces
        // Si conoces el nombre del script (ej EnemySteering), desactívalo explícitamente:
        var steering = GetComponent<MonoBehaviour>();
        // si tu EnemySteering es un script específico:
        var enemySteering = GetComponentInChildren<MonoBehaviour>();
        // (Nota: es mejor que modifiques esta función para desactivar los componentes exactos que uses)
    }

    IEnumerator DissolveAndDieCoroutine()
    {
        float t = 0f;

        // Asegurarse que la propiedad existe en los materiales; si no, avisar en consola
        bool anyHasProperty = false;
        foreach (var rendMats in instantiatedMaterialsPerRenderer)
        {
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
            Debug.LogWarning($"No material on '{name}' has property '{dissolveProperty}'. Check shader property Reference.");
            // si no hay propiedad, destruimos tras breve delay para evitar desaparición instantánea
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
            yield break;
        }

        while (t < dissolveDuration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(dissolveFrom, dissolveTo, t / dissolveDuration);

            // aplicar a todos los materiales instanciados
            for (int r = 0; r < instantiatedMaterialsPerRenderer.Length; r++)
            {
                var mats = instantiatedMaterialsPerRenderer[r];
                for (int i = 0; i < mats.Length; i++)
                {
                    var m = mats[i];
                    if (m != null && m.HasProperty(dissolveProperty))
                        m.SetFloat(dissolveProperty, v);
                }
            }

            yield return null;
        }

        // opción: spawn efectos finales
        Destroy(gameObject);
    }

    // Raycast hacia abajo desde arriba del impacto para asegurar el suelo correcto
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






