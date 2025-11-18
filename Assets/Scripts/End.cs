using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reinicia la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

