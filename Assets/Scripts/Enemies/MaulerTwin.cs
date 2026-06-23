using UnityEngine;

public class MaulerTwin : MonoBehaviour
{
    public enum TwinType { Melee, Ranged }
    public enum TwinState { Active, Hurt, Death }

    [Header("Twin Setup")]
    public TwinType twinType = TwinType.Melee;
    public TwinState currentState = TwinState.Active;

    [Header("Health")]
    public float maxHealth = 250f;
    private float currentHealth;

    [Header("Melee Settings")]
    public float meleeSpeed = 2.5f;
    public float meleeRange = 1f;
    public float meleeDamage = 22f;
    public float meleeCooldown = 1.1f;
    private float meleeTimer = 0f;

    [Header("Ranged Settings")]
    public float rangedMoveSpeed = 2.5f;
    public float preferredRange = 5f;
    public float tooCloseRange = 3f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float rangedDamage = 18f;
    public float shootCooldown = 1.8f;
    private float shootTimer = 0f;

    [Header("Enrage")]
    public bool isEnraged = false;
    public float enrageSpeedMultiplier = 1.8f;
    public float enrageCooldownMultiplier = 0.45f;

    [Header("Hurt")]
    public float hurtDuration = 0.2f;
    private float hurtTimer = 0f;
    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private PlayerHealth playerHealth;
    private Color baseColor;
    private CameraShakeBoss cameraShake;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        currentHealth = maxHealth;

        GameObject playerObj =
            GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth =
                playerObj.GetComponent<PlayerHealth>();
        }

        cameraShake = Object
            .FindAnyObjectByType<CameraShakeBoss>();
    }

    void Update()
    {
        if (currentState == TwinState.Death) return;
        if (player == null) return;

        switch (currentState)
        {
            case TwinState.Active:
                if (twinType == TwinType.Melee)
                    UpdateMelee();
                else
                    UpdateRanged();
                break;
            case TwinState.Hurt:
                UpdateHurt();
                break;
        }
    }

    // ─── MELEE BEHAVIOUR ─────────────────────────
    void UpdateMelee()
    {
        float dist = Vector2.Distance(
            transform.position, player.position);

        float speed = isEnraged ?
            meleeSpeed * enrageSpeedMultiplier :
            meleeSpeed;

        if (dist > meleeRange)
        {
            Vector2 dir = (player.position -
                          transform.position).normalized;
            rb.linearVelocity = dir * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            meleeTimer -= Time.deltaTime;
            if (meleeTimer <= 0f)
            {
                PerformMeleeAttack();
                float cooldown = isEnraged ?
                    meleeCooldown *
                    enrageCooldownMultiplier :
                    meleeCooldown;
                meleeTimer = cooldown;
            }
        }

        FaceTarget();
    }

    void PerformMeleeAttack()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(meleeDamage);

        Rigidbody2D playerRb =
            player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 dir = (player.position -
                          transform.position).normalized;
            playerRb.AddForce(dir * 8f,
                ForceMode2D.Impulse);
        }

        if (cameraShake != null)
            cameraShake.Shake(0.3f, 0.2f);
    }

    // ─── RANGED BEHAVIOUR ────────────────────────
    void UpdateRanged()
    {
        float dist = Vector2.Distance(
            transform.position, player.position);

        float speed = isEnraged ?
            rangedMoveSpeed * enrageSpeedMultiplier :
            rangedMoveSpeed;

        if (dist < tooCloseRange)
        {
            Vector2 dir = (transform.position -
                          player.position).normalized;
            rb.linearVelocity = dir * speed;
        }
        else if (dist > preferredRange)
        {
            Vector2 dir = (player.position -
                          transform.position).normalized;
            rb.linearVelocity = dir * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                FireProjectile();
                float cooldown = isEnraged ?
                    shootCooldown *
                    enrageCooldownMultiplier :
                    shootCooldown;
                shootTimer = cooldown;
            }
        }

        FaceTarget();
    }

    void FireProjectile()
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
            projRb.linearVelocity =
                dir * projectileSpeed;

        EnemyProjectile ep =
            proj.GetComponent<EnemyProjectile>();
        if (ep != null)
            ep.damage = rangedDamage;

        Destroy(proj, 4f);
    }

    // ─── SHARED ──────────────────────────────────
    void FaceTarget()
    {
        Vector2 dir = (player.position -
                      transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) *
                      Mathf.Rad2Deg;
        transform.rotation =
            Quaternion.Euler(0, 0, angle);
    }

    void UpdateHurt()
    {
        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0f)
        {
            sr.color = isEnraged ? Color.red : baseColor;
            currentState = TwinState.Active;
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentState == TwinState.Death) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " +
                  currentHealth);

        if (currentHealth <= 0)
        {
            TriggerDeath();
            return;
        }

        currentState = TwinState.Hurt;
        hurtTimer = hurtDuration;
        sr.color = Color.white;

        rb.linearVelocity = Vector2.zero;
        Vector2 knockDir = (transform.position -
            player.position).normalized;
        rb.AddForce(knockDir * knockbackForce,
            ForceMode2D.Impulse);
    }

    void TriggerDeath()
    {
        currentState = TwinState.Death;
        rb.linearVelocity = Vector2.zero;
        sr.color = Color.grey;
        GetComponent<Collider2D>().enabled = false;

        Debug.Log(gameObject.name + " defeated!");

        // Notify boss manager
        MaulerBossManager bossManager =
            Object.FindAnyObjectByType<MaulerBossManager>();
        if (bossManager != null)
            bossManager.OnTwinDefeated(this);

        Destroy(gameObject, 0.8f);
    }

    public void EnableEnrage()
    {
        isEnraged = true;
        sr.color = Color.red;

        // Boost damage on enrage
        meleeDamage *= 1.3f;
        rangedDamage *= 1.3f;

        if (cameraShake != null)
            cameraShake.Shake(0.5f, 0.4f);

        Debug.Log(gameObject.name + " IS ENRAGED!");
    }
}