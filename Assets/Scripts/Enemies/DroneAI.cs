using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public enum DroneState
    {
        Idle, Chase, Attack, Hurt, Death
    }

    public DroneState currentState = DroneState.Idle;

    [Header("Movement")]
    public float chaseSpeed = 3f;
    public float attackRange = 0.8f;

    [Header("Attack")]
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    [Header("Hurt")]
    public float hurtDuration = 0.2f;
    private float hurtTimer = 0f;
    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private PlayerHealth playerHealth;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        player = GameObject.FindWithTag("Player")
                           .transform;
        playerHealth = player
                           .GetComponent<PlayerHealth>();

        currentState = DroneState.Chase;
    }

    void Update()
    {
        if (currentState == DroneState.Death) return;

        switch (currentState)
        {
            case DroneState.Chase:
                UpdateChase();
                break;
            case DroneState.Attack:
                UpdateAttack();
                break;
            case DroneState.Hurt:
                UpdateHurt();
                break;
        }
    }

    // ─── CHASE ───────────────────────────────────
    void UpdateChase()
    {
        if (player == null) return;

        float dist = Vector2.Distance(
            transform.position, player.position);

        if (dist <= attackRange)
        {
            currentState = DroneState.Attack;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Rush toward player
        Vector2 direction = (player.position -
                            transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
    }

    // ─── ATTACK ──────────────────────────────────
    void UpdateAttack()
    {
        if (player == null) return;

        float dist = Vector2.Distance(
            transform.position, player.position);

        if (dist > attackRange * 1.5f)
        {
            currentState = DroneState.Chase;
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            PerformAttack();
            attackTimer = attackCooldown;
        }
    }

    void PerformAttack()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);

        Debug.Log("Drone attacked Eve!");
    }

    // ─── HURT ────────────────────────────────────
    void UpdateHurt()
    {
        hurtTimer -= Time.deltaTime;

        if (hurtTimer <= 0f)
        {
            sr.color = originalColor;
            currentState = DroneState.Chase;
        }
    }

    public void TriggerHurt(Vector2 knockbackDir)
    {
        if (currentState == DroneState.Death) return;

        currentState = DroneState.Hurt;
        hurtTimer = hurtDuration;
        sr.color = Color.white;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(
            knockbackDir * knockbackForce,
            ForceMode2D.Impulse
        );
    }

    public void TriggerDeath()
    {
        currentState = DroneState.Death;
        rb.linearVelocity = Vector2.zero;
        sr.color = Color.grey;
        GetComponent<Collider2D>().enabled = false;

        // Notify room
        RoomManager room =
            Object.FindAnyObjectByType<RoomManager>();
        if (room != null)
            room.EnemyDefeated();

        Destroy(gameObject, 0.5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position, attackRange);
    }
}