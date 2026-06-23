using UnityEngine;

public class ShieldDroneAI : MonoBehaviour
{
    public enum ShieldState
    {
        Chase, Attack, Hurt, Death
    }

    public ShieldState currentState = ShieldState.Chase;

    [Header("Movement")]
    public float chaseSpeed = 2f;
    public float attackRange = 1f;

    [Header("Attack")]
    public float attackDamage = 15f;
    public float attackCooldown = 1.2f;
    private float attackTimer = 0f;

    [Header("Shield")]
    public float shieldArcAngle = 90f;
    public float rotationSpeed = 90f; // degrees per second
    // Bullets from within this angle in front 
    // are blocked

    [Header("Hurt")]
    public float hurtDuration = 0.2f;
    private float hurtTimer = 0f;
    public float knockbackForce = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private PlayerHealth playerHealth;
    private Color originalColor;

    [Header("Visual")]
    public Transform shieldVisual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        GameObject playerObj =
            GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth =
                playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (currentState == ShieldState.Death) return;
        if (player == null) return;

        FaceTarget();

        switch (currentState)
        {
            case ShieldState.Chase:
                UpdateChase();
                break;
            case ShieldState.Attack:
                UpdateAttack();
                break;
            case ShieldState.Hurt:
                UpdateHurt();
                break;
        }
    }

    void UpdateChase()
    {
        float dist = Vector2.Distance(
            transform.position, player.position);

        if (dist <= attackRange)
        {
            currentState = ShieldState.Attack;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (player.position -
                      transform.position).normalized;
        rb.linearVelocity = dir * chaseSpeed;
    }

    void UpdateAttack()
    {
        float dist = Vector2.Distance(
            transform.position, player.position);

        if (dist > attackRange * 1.5f)
        {
            currentState = ShieldState.Chase;
            return;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);
            attackTimer = attackCooldown;
        }
    }

    void UpdateHurt()
    {
        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0f)
        {
            sr.color = originalColor;
            currentState = ShieldState.Chase;
        }
    }

    void FaceTarget()
    {
        Vector2 dir = (player.position -
                      transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) *
                            Mathf.Rad2Deg;

        Quaternion targetRotation =
            Quaternion.Euler(0, 0, targetAngle);

        // Slow rotation speed — gives player a window
        // to flank
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // ─── SHIELD CHECK ────────────────────────────
    // Called by Bullet to check if shot is blocked
    public bool IsShieldBlocking(Vector2 bulletDirection)
    {
        // Direction the drone is facing
        Vector2 facingDir = transform.right;

        // Direction the bullet came from (reversed)
        Vector2 incomingDir = -bulletDirection;

        float angle = Vector2.Angle(
            facingDir, incomingDir);

        // If incoming angle is within shield arc, block
        return angle <= shieldArcAngle / 2f;
    }

    public void TriggerHurt(Vector2 knockbackDir)
    {
        if (currentState == ShieldState.Death) return;

        currentState = ShieldState.Hurt;
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
        currentState = ShieldState.Death;
        rb.linearVelocity = Vector2.zero;
        sr.color = Color.grey;
        GetComponent<Collider2D>().enabled = false;

        RoomManager room =
            Object.FindAnyObjectByType<RoomManager>();
        if (room != null)
            room.EnemyDefeated();

        Destroy(gameObject, 0.5f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw shield arc
        Gizmos.color = Color.cyan;
        Vector3 facing = transform.right * 1.5f;
        Gizmos.DrawLine(
            transform.position,
            transform.position + facing
        );
    }
}