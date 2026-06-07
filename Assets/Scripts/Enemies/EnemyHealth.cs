using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 60f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " +
                  currentHealth);

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
            Die();
    }

    System.Collections.IEnumerator DamageFlash()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(0.7f, 0.1f, 0.1f);
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " defeated!");
        Destroy(gameObject);
    }
}