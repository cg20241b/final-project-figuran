using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage; // Instant damage
    public int damageOverTime; // DoT
    public float damageDuration = 3.0f; // DoT duration
    public bool applySlowEffect; // Toggle to apply slow
    public float slowAmount = 0.5f; // Slowdown multiplier (e.g., 0.5 for 50% speed)
    public float slowDuration = 2.0f; // Slow effect duration
    public GameObject impactEffectPrefab;   
    public float impactEffectLifeTime = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the hit object is a valid target
        if (collision.gameObject.CompareTag("Target"))
        {
            // Log the hit object
            print("hit " + collision.gameObject.name + "!");

            // Apply effects to the target
            Target target = collision.gameObject.GetComponent<Target>();
            if (target != null)
            {
                // Apply instant damage
                target.TakeDamage(bulletDamage);

                // Apply damage over time
                StartCoroutine(ApplyDamageOverTime(target, damageOverTime, damageDuration));

                // Apply slow effect if enabled
                if (applySlowEffect)
                {
                    target.ApplySlow(slowAmount, slowDuration);
                }
            }
        }

        // Create impact effect
        if (impactEffectPrefab)
        {
            ContactPoint contact = collision.GetContact(0);
            GameObject impactEffect = Instantiate(impactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(impactEffect, impactEffectLifeTime);
        }

        // Destroy the bullet
        Destroy(gameObject);
    }

    private IEnumerator ApplyDamageOverTime(Target target, int damagePerSecond, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (target != null)
            {
                target.TakeDamage(damagePerSecond);
            }
            elapsedTime += 1.0f; // Apply damage every second
            yield return new WaitForSeconds(1.0f);
        }
    }
}
