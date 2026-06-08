using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 15f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph =
                other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
            Destroy(gameObject);
    }
}