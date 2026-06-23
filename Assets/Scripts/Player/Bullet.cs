using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 20f;
    public float lifetime = 3f;

    private Rigidbody2D rb;
    private float lifetimeTimer;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        lifetimeTimer = lifetime;
    }

    void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
            ReturnToPool();
    }

    public void SetDirection(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check for shield block first
            ShieldDroneAI shield =
                other.GetComponent<ShieldDroneAI>();

            if (shield != null)
            {
                Vector2 bulletDir = rb.linearVelocity.normalized;

                if (shield.IsShieldBlocking(bulletDir))
                {
                    // Blocked! Bullet bounces off, no damage
                    Debug.Log("Shield blocked the shot!");
                    ReturnToPool();
                    return;
                }
            }

            EnemyHealth enemyHealth =
                other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

                Vector2 knockbackDir =
                    (other.transform.position -
                    transform.position).normalized;

                DroneAI droneAI =
                    other.GetComponent<DroneAI>();
                if (droneAI != null)
                    droneAI.TriggerHurt(knockbackDir);

                GunnerAI gunnerAI =
                    other.GetComponent<GunnerAI>();
                if (gunnerAI != null)
                    gunnerAI.TriggerHurt(knockbackDir);

                if (shield != null)
                    shield.TriggerHurt(knockbackDir);
            }

            ReturnToPool();
        }

        if (other.CompareTag("Wall"))
            ReturnToPool();
    }

    void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}