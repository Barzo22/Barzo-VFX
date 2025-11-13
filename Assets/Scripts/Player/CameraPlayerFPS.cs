using UnityEngine;

//TP2 - Ariadna Delpiano

public class CameraPlayerFPS : MonoBehaviour
{
    public float mouseSensibilidad = 100f;
    public Transform playerFPS;

    private float rotationX = 0f; // Para la rotación en el eje Y (vertical)

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Obtener los movimientos del mouse en ambos ejes
        float mouseX = Input.GetAxis("Mouse X") * mouseSensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensibilidad * Time.deltaTime;

        // Rotación horizontal del jugador (alrededor del eje Y)
        playerFPS.Rotate(Vector3.up * mouseX);

        // Acumular y limitar la rotación vertical
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limitar la rotación vertical

        // Aplicar la rotación vertical a la cámara (alrededor del eje X)
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}



