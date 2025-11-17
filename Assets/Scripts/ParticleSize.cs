using UnityEngine;

public class ParticleSize : MonoBehaviour
{
    public float scaleFactor = 1f; 
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        transform.localScale = Vector3.one * dist * scaleFactor;
    }
}
