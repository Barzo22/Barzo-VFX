using UnityEngine;

//TP2 - Fernando Claro

public class Shot : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float lifeTime;
    public int damage;

    public GameObject projectilePrefab;

    private bool canShoot = false;
    private bool isOnCooldown = false; // Indicador de cooldown
    private float cooldownTimer = 0f; // Temporizador de cooldown

    void Start()
    {
        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy"))
        {
            canShoot = true;
        }
    }

    void Update()
    {
        if (canShoot && gameObject.CompareTag("Player") && Input.GetMouseButtonDown(0))
        {
            if (!isOnCooldown)
            {
                ThrowBullet();
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    public void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    public void ThrowBullet()
    {
        GameObject projectile = Instantiate(projectilePrefab,target.position, target.rotation);
        projectile.transform.position = target.position;
        Rigidbody rb = projectile.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.velocity = target.forward * speed;

        ProjectileDamage projectileDamage = projectile.GetComponent<ProjectileDamage>();
        if (projectileDamage != null)
        {
            projectileDamage.SetDamage(damage);
        }

        Destroy(projectile, lifeTime);
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

}