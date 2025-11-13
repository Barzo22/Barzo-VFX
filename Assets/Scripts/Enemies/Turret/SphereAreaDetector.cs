using UnityEngine;

//TP2 - Fernando Claro

public class SphereAreaDetector : MonoBehaviour
{
    public GameObject torreta;
    public bool isPlayer;
    private Turret handler;

    private void Start()
    {
        handler = torreta.GetComponent<Turret>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = true;
            torreta.GetComponent<Renderer>().material.color = Color.red;
            handler.OnDetect();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = true;
            torreta.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayer = false;
            torreta.GetComponent<Renderer>().material.color = Color.green;
            handler.OnExit();
        }
    }
}

