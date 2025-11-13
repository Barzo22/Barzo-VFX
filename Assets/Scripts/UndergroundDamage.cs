using UnityEngine;

//TP2 - Ariadna Delpiano

public class UndergroundDamage : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(entity.maxLife);
        }
    }
}
