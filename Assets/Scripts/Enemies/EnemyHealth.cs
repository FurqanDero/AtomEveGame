using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 60f;
    private float currentHealth;
    private DroneAI droneAI;
    private GunnerAI gunnerAI;
    private ShieldDroneAI shieldDroneAI;

    void Start()
    {
        currentHealth = maxHealth;
        droneAI = GetComponent<DroneAI>();
        gunnerAI = GetComponent<GunnerAI>();
        shieldDroneAI = GetComponent<ShieldDroneAI>();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead()) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " +
                  currentHealth);

        if (currentHealth <= 0)
        {
            TriggerDeath();
            return;
        }

        StartCoroutine(DamageFlash());
    }

    bool IsDead()
    {
        if (droneAI != null &&
            droneAI.currentState ==
            DroneAI.DroneState.Death) return true;
        if (gunnerAI != null &&
            gunnerAI.currentState ==
            GunnerAI.GunnerState.Death) return true;
        if (shieldDroneAI != null &&
            shieldDroneAI.currentState ==
            ShieldDroneAI.ShieldState.Death) return true;
        return false;
    }

    void TriggerDeath()
    {
        if (droneAI != null)
            droneAI.TriggerDeath();
        else if (gunnerAI != null)
            gunnerAI.TriggerDeath();
        else if (shieldDroneAI != null)
            shieldDroneAI.TriggerDeath();
        else
            Destroy(gameObject);
    }

    System.Collections.IEnumerator DamageFlash()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            if (!IsDead())
                sr.color = new Color(0.7f, 0.1f, 0.1f);
        }
    }
}