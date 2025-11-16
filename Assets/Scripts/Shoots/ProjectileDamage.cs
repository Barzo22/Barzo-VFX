using UnityEngine;
public class ProjectileDamage : MonoBehaviour
{
    public int damage = 1;

    public void SetDamage(int value)
    {
        damage = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            Vector3 hitPos = collision.contacts[0].point;
            Vector3 hitNormal = collision.contacts[0].normal;

            enemy.TakeDamage(damage, hitPos, hitNormal);
        }

        Destroy(gameObject);
    }
}





