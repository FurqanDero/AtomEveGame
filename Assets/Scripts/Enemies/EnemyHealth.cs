using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 60f;
    private float currentHealth;
    private DroneAI droneAI;

    [Header("Knockback")]
    public float knockbackForce = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        droneAI = GetComponent<DroneAI>();
    }

    public void TakeDamage(float damage)
    {
        if (droneAI != null &&
            droneAI.currentState ==
            DroneAI.DroneState.Death) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " +
                  currentHealth);

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            if (droneAI != null)
                droneAI.TriggerDeath();
            else
                Destroy(gameObject);
        }
    }

    System.Collections.IEnumerator DamageFlash()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            if (droneAI != null &&
                droneAI.currentState !=
                DroneAI.DroneState.Death)
                sr.color = new Color(0.7f, 0.1f, 0.1f);
        }
    }
}