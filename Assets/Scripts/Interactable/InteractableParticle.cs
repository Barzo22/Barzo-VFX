using UnityEngine;

public class InteractableParticles : MonoBehaviour
{
    [Header("Partículas a desactivar (ParticleSystem)")]
    public ParticleSystem[] particles;

    [Header("Opcional: GameObjects a desactivar")]
    public GameObject[] particleGameObjects;

    [Header("Interacción")]
    public float interactionDistance = 3f;

    [Tooltip("Transform del jugador. Si está vacío se buscará automáticamente.")]
    public Transform player;

    [Tooltip("Tecla para interactuar")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Cambio de Material")]
    [Tooltip("Renderers a los que se les cambiará el material al interactuar")]
    public Renderer[] targetRenderers;

    [Tooltip("Material que se aplicará al interactuar")]
    public Material newMaterial;

    private Material[] originalMaterials;
    private bool materialsSwapped = false;

    void Start()
    {
        // Asignar player automáticamente
        if (player == null)
        {
            if (Player.PlayerTransform != null)
            {
                player = Player.PlayerTransform;
            }
            else
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                if (go != null) player = go.transform;
            }
        }

        // Guardar materiales originales
        if (targetRenderers != null && targetRenderers.Length > 0)
        {
            originalMaterials = new Material[targetRenderers.Length];
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    originalMaterials[i] = targetRenderers[i].material;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist > interactionDistance) return;

        if (Input.GetKeyDown(interactKey))
        {
            TurnOffParticles();
            SwapMaterials();
        }
    }

    public void TurnOffParticles()
    {
        // ParticleSystems
        if (particles != null)
        {
            foreach (var ps in particles)
            {
                if (ps == null) continue;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                ps.gameObject.SetActive(false);
            }
        }

        // GameObjects
        if (particleGameObjects != null)
        {
            foreach (var go in particleGameObjects)
            {
                if (go == null) continue;

                var ps = go.GetComponentInChildren<ParticleSystem>();
                if (ps != null)
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                go.SetActive(false);
            }
        }
    }

    void SwapMaterials()
    {
        if (targetRenderers == null || targetRenderers.Length == 0) return;
        if (newMaterial == null) return;

        if (!materialsSwapped)
        {
            // Aplicar nuevo material
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    targetRenderers[i].material = newMaterial;
            }
        }
        else
        {
            // Volver a materiales originales (opcional)
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    targetRenderers[i].material = originalMaterials[i];
            }
        }

        materialsSwapped = !materialsSwapped;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}




