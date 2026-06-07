using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Shooting")]
    public float fireRate = 0.25f;
    public Transform firePoint;

    private float fireTimer = 0f;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            TryShoot();
            fireTimer = fireRate;
        }
    }

    void TryShoot()
    {
        Transform target =
            playerController.GetNearestEnemy();

        // Only shoot if enemy in range
        if (target == null) return;

        // Get bullet from pool
        GameObject bullet = BulletPool.Instance?.GetBullet();
        if (bullet == null) return;

        // Position at fire point
        bullet.transform.position = firePoint != null ?
            firePoint.position : transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        // Aim at enemy
        Vector2 direction = (target.position -
            bullet.transform.position).normalized;

        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
            b.SetDirection(direction);
    }
}