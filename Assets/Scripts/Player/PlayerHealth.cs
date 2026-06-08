using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isDead = false;
    private HUDManager hudManager;

    void Start()
    {
        currentHealth = maxHealth;
        hudManager = Object.FindAnyObjectByType<HUDManager>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(
            currentHealth - damage, 0f);

        if (hudManager != null)
            hudManager.UpdateHP(
                currentHealth, maxHealth);

        StartCoroutine(DamageFlash());

        Debug.Log("Eve HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    System.Collections.IEnumerator DamageFlash()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            if (!isDead)
                sr.color = new Color(1f, 0.4f, 0.8f);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Eve defeated!");

        GetComponent<PlayerController>().enabled = false;
        GetComponent<ShootingController>().enabled = false;

        StartCoroutine(DeathSequence());
    }

    System.Collections.IEnumerator DeathSequence()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            if (sr != null) sr.color = Color.red;
            yield return new WaitForSecondsRealtime(0.15f);
            if (sr != null) sr.color = Color.clear;
            yield return new WaitForSecondsRealtime(0.15f);
        }

        if (sr != null) sr.enabled = false;

        yield return new WaitForSecondsRealtime(0.5f);

        GameManager gameManager =
            Object.FindAnyObjectByType<GameManager>();
        if (gameManager != null)
            gameManager.TriggerGameOver();
    }
}