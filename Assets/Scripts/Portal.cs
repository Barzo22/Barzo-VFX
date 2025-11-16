using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetPortal;
    public float exitDistance = 1f;
    public float teleportCooldown = 0.3f;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        TeleportObject(other.transform);
    }

    void TeleportObject(Transform obj)
    {
        if (!canTeleport || targetPortal == null) return;

        // Si es un player con CharacterController lo pauso
        CharacterController cc = obj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Nueva posición exactamente en el portal + offset
        Vector3 offset = targetPortal.forward * exitDistance;
        obj.position = targetPortal.position + offset;

        // Rotación
        obj.rotation = targetPortal.rotation;

        // Reactivar controller
        if (cc != null) cc.enabled = true;

        // Evitar loop infinito
        Portal linked = targetPortal.GetComponent<Portal>();
        if (linked != null)
            linked.BlockTeleport(teleportCooldown);

        canTeleport = false;
        Invoke(nameof(ResetTeleport), teleportCooldown);
    }

    public void BlockTeleport(float time)
    {
        canTeleport = false;
        Invoke(nameof(ResetTeleport), time);
    }

    void ResetTeleport()
    {
        canTeleport = true;
    }
}

