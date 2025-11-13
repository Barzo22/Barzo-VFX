using UnityEngine;

//TP2 - Fernando Claro

public class ProjectileDamage : MonoBehaviour
{
    private int damage;

    public void SetDamage(int value)
    {
        damage = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

