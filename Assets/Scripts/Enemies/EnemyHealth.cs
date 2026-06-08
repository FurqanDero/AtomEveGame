using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 60f;
    private float currentHealth;
    private DroneAI droneAI;
    private GunnerAI gunnerAI;

    void Start()
    {
        currentHealth = maxHealth;
        droneAI = GetComponent<DroneAI>();
        gunnerAI = GetComponent<GunnerAI>();
    }

    public void TakeDamage(float damage)
    {
        if (droneAI != null &&
            droneAI.currentState ==
            DroneAI.DroneState.Death) return;
        if (gunnerAI != null &&
            gunnerAI.currentState ==
            GunnerAI.GunnerState.Death) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " +
                  currentHealth);

        if (currentHealth <= 0)
        {
            if (droneAI != null)
                droneAI.TriggerDeath();
            else if (gunnerAI != null)
                gunnerAI.TriggerDeath();
            else
                Destroy(gameObject);
            return;
        }

        // Trigger hurt
        Vector2 knockback = Vector2.right;
        if (droneAI != null)
            droneAI.TriggerHurt(knockback);
        else if (gunnerAI != null)
            gunnerAI.TriggerHurt(knockback);

        StartCoroutine(DamageFlash());
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
}