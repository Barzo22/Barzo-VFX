using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VignetteController : MonoBehaviour
{
    [Header("Material base (NO se usa directamente)")]
    public Material vignetteMaterial;

    [Header("Referencia al Renderer Feature del URP")]
    public FullScreenPassRendererFeature vignetteFeature;

    [Header("Propiedad del shader")]
    public string intensityProperty = "_Intensity";

    [Header("Valores")]
    public float damageIntensity = 0.3f; // 0 = fuerte, 1 = invisible (invertido)
    public float fadeSpeed = 1.5f;

    private float currentIntensity;
    private Material runtimeMaterial;

    void Start()
    {
        runtimeMaterial = Instantiate(vignetteMaterial);

        vignetteFeature.passMaterial = runtimeMaterial;

        currentIntensity = 1f;
        runtimeMaterial.SetFloat(intensityProperty, currentIntensity);
    }

    void Update()
    {
        if (currentIntensity < 1f) 
        {
            currentIntensity += fadeSpeed * Time.deltaTime;
            currentIntensity = Mathf.Clamp01(currentIntensity);
            runtimeMaterial.SetFloat(intensityProperty, currentIntensity);
        }
    }

    public void TriggerVignette()
    {
        currentIntensity = damageIntensity;
        runtimeMaterial.SetFloat(intensityProperty, currentIntensity);
    }
}


