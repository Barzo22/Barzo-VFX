using UnityEngine;

//TP2 - Fernando Claro

public class SlowDown : MonoBehaviour, ITrap<Player>
{
    public float slowDownFactor;
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            TrapManager.Instance.TriggerTrap(this, player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TrapManager.Instance.ReleaseTrap(this, player);
        }
    }

    public void ActivateTrap(Player target)
    {
        target.speed *= slowDownFactor;
    }

    public void DeactivateTrap(Player target)
    {
        target.speed /= slowDownFactor;
    }
}

