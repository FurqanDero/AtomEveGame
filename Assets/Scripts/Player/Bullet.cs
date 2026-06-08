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
            EnemyHealth enemyHealth =
                other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

                // Send knockback direction
                DroneAI droneAI =
                    other.GetComponent<DroneAI>();
                if (droneAI != null)
                {
                    Vector2 knockbackDir =
                        (other.transform.position -
                        transform.position).normalized;
                    droneAI.TriggerHurt(knockbackDir);
                }
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