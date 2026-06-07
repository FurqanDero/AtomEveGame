using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float dashSpeed = 18f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Auto Aim")]
    public float autoAimRange = 10f;
    public LayerMask enemyLayer;

    // ─── State ───────────────────────────────────
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    // ─── References ──────────────────────────────
    private Transform nearestEnemy;
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        FindNearestEnemy();
        HandleRotation();
        HandleDashInput();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        HandleMovement();
    }

    // ─── INPUT ───────────────────────────────────
    void HandleInput()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    // ─── MOVEMENT ────────────────────────────────
    void HandleMovement()
    {
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = dashDirection * dashSpeed;

            if (dashTimer <= 0)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    // ─── DASH ────────────────────────────────────
    void HandleDashInput()
    {
        dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            !isDashing &&
            dashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Dash in movement direction
        // If not moving, dash forward
        dashDirection = moveInput.magnitude > 0 ?
            moveInput : transform.right;

        Debug.Log("Dash!");
    }

    // ─── AUTO AIM ────────────────────────────────
    void FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            autoAimRange,
            enemyLayer
        );

        float closestDist = Mathf.Infinity;
        nearestEnemy = null;

        foreach (Collider2D enemy in enemies)
        {
            float dist = Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

            if (dist < closestDist)
            {
                closestDist = dist;
                nearestEnemy = enemy.transform;
            }
        }
    }

    // ─── ROTATION ────────────────────────────────
    void HandleRotation()
    {
        Vector2 lookDirection;

        if (nearestEnemy != null)
        {
            // Face nearest enemy
            lookDirection = (nearestEnemy.position -
                            transform.position).normalized;
        }
        else
        {
            // Face movement direction
            if (moveInput.magnitude > 0)
                lookDirection = moveInput;
            else
                return;
        }

        float angle = Mathf.Atan2(
            lookDirection.y,
            lookDirection.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.deltaTime * 15f
        );
    }

    // ─── PUBLIC GETTERS ──────────────────────────
    public Transform GetNearestEnemy()
    {
        return nearestEnemy;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    // ─── GIZMOS ──────────────────────────────────
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position, autoAimRange);
    }
}