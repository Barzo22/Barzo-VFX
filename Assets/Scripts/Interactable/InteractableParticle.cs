using UnityEngine;
using UnityEngine.UI;   // Necesario si usás Text o Image

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

    [Header("UI de interacción")]
    [Tooltip("Cartel que dice 'Presiona E'")]
    public GameObject interactionUI;

    [Header("Cambio de Material")]
    public Renderer[] targetRenderers;
    public Material newMaterial;

    private Material[] originalMaterials;
    private bool materialsSwapped = false;

    void Start()
    {
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

        if (interactionUI != null)
            interactionUI.SetActive(false);

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

        // Mostrar / Ocultar cartel
        if (interactionUI != null)
            interactionUI.SetActive(dist <= interactionDistance);

        // Si está lejos no deja interactuar
        if (dist > interactionDistance) return;

        if (Input.GetKeyDown(interactKey))
        {
            TurnOffParticles();
            SwapMaterials();

            // Ocultar cartel después de interactuar
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    public void TurnOffParticles()
    {
        if (particles != null)
        {
            foreach (var ps in particles)
            {
                if (ps == null) continue;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                ps.gameObject.SetActive(false);
            }
        }

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
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    targetRenderers[i].material = newMaterial;
            }
        }
        else
        {
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





