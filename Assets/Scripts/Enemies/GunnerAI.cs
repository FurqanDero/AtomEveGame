using UnityEngine;

public class GunnerAI : MonoBehaviour
{
    public enum GunnerState
    {
        Idle, Reposition, Shoot, Hurt, Death
    }

    public GunnerState currentState =
        GunnerState.Idle;

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float preferredRange = 5f;
    public float tooCloseRange = 3f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float shootCooldown = 2f;
    private float shootTimer = 0f;
    public float projectileSpeed = 8f;
    public float projectileDamage = 15f;

    [Header("Hurt")]
    public float hurtDuration = 0.2f;
    private float hurtTimer = 0f;

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

        GameObject playerObj =
            GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth =
                playerObj.GetComponent<PlayerHealth>();
        }

        currentState = GunnerState.Reposition;
    }

    void Update()
    {
        if (currentState == GunnerState.Death) return;
        if (player == null) return;

        float dist = Vector2.Distance(
            transform.position, player.position);

        switch (currentState)
        {
            case GunnerState.Reposition:
                UpdateReposition(dist);
                break;
            case GunnerState.Shoot:
                UpdateShoot(dist);
                break;
            case GunnerState.Hurt:
                UpdateHurt();
                break;
        }

        // Always face player
        FacePlayer();
    }

    // ─── REPOSITION ──────────────────────────────
    void UpdateReposition(float dist)
    {
        if (dist < tooCloseRange)
        {
            // Back away
            Vector2 dir = (transform.position -
                          player.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        else if (dist > preferredRange)
        {
            // Move closer
            Vector2 dir = (player.position -
                          transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        else
        {
            // At preferred range — stop and shoot
            rb.linearVelocity = Vector2.zero;
            currentState = GunnerState.Shoot;
        }
    }

    // ─── SHOOT ───────────────────────────────────
    void UpdateShoot(float dist)
    {
        rb.linearVelocity = Vector2.zero;

        // Reposition if out of range
        if (dist < tooCloseRange ||
            dist > preferredRange * 1.5f)
        {
            currentState = GunnerState.Reposition;
            return;
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            FireAtPlayer();
            shootTimer = shootCooldown;
        }
    }

    void FireAtPlayer()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        Vector2 dir = (player.position -
                      transform.position).normalized;

        Rigidbody2D projRb =
            proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
            projRb.linearVelocity = dir * projectileSpeed;

        // Set damage
        EnemyProjectile ep =
            proj.GetComponent<EnemyProjectile>();
        if (ep != null)
            ep.damage = projectileDamage;

        // Auto destroy
        Destroy(proj, 4f);

        Debug.Log("Gunner fired!");
    }

    // ─── HURT ────────────────────────────────────
    void UpdateHurt()
    {
        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0f)
        {
            sr.color = originalColor;
            currentState = GunnerState.Reposition;
        }
    }

    public void TriggerHurt(Vector2 knockbackDir)
    {
        if (currentState == GunnerState.Death) return;

        currentState = GunnerState.Hurt;
        hurtTimer = hurtDuration;
        sr.color = Color.white;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(
            knockbackDir * 4f,
            ForceMode2D.Impulse
        );
    }

    public void TriggerDeath()
    {
        currentState = GunnerState.Death;
        rb.linearVelocity = Vector2.zero;
        sr.color = Color.grey;
        GetComponent<Collider2D>().enabled = false;

        RoomManager room =
            Object.FindAnyObjectByType<RoomManager>();
        if (room != null)
            room.EnemyDefeated();

        Destroy(gameObject, 0.5f);
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector2 dir = (player.position -
                      transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) *
                      Mathf.Rad2Deg;
        transform.rotation =
            Quaternion.Euler(0, 0, angle);
    }
}