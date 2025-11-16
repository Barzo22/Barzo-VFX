using UnityEngine;

public class EnemySteering : MonoBehaviour
{
    public Transform player;

    public float maxVelocity = 5f;
    public float maxForce = 10f;
    public float slowDownRadius = 1.5f;

    public float detectionRange = 10f;
    public float attackRange = 1.2f;

    public int damageAmount = 10;
    public float timeBetweenAttacks = 1f;
    private float lastAttackTime = 0f;

    private Vector3 velocity;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            velocity = Vector3.zero;
            return;
        }

        if (distance <= attackRange)
        {
            velocity = Vector3.zero;

            TryAttack();
            return;
        }

        Vector3 steering = Seek(player.position);
        velocity += steering * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        transform.position += velocity * Time.deltaTime;

        if (velocity.magnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(velocity, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= timeBetweenAttacks)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(damageAmount);
            }

            lastAttackTime = Time.time;
            Debug.Log("Enemy dealt damage!");
        }
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;

        float distance = desired.magnitude;
        if (distance < slowDownRadius)
        {
            float mappedSpeed = Mathf.Lerp(0, maxVelocity, distance / slowDownRadius);
            desired = desired.normalized * mappedSpeed;
        }
        else
        {
            desired = desired.normalized * maxVelocity;
        }

        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering;
    }
}


