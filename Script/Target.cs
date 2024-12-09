using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 100;
    public float movementSpeed = 5.0f; // Default speed

    private float originalSpeed;

    private void Start()
    {
        originalSpeed = movementSpeed; // Save the original speed
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        print(gameObject.name + " took " + damage + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // Method to apply slow effect
    public void ApplySlow(float slowMultiplier, float duration)
    {
        if (movementSpeed > 0)
        {
            movementSpeed *= slowMultiplier; // Reduce the speed
            print(gameObject.name + " slowed to " + movementSpeed + " for " + duration + " seconds.");
            StartCoroutine(RemoveSlowEffect(duration));
        }
    }

    private IEnumerator RemoveSlowEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        movementSpeed = originalSpeed; // Restore the original speed
        print(gameObject.name + " speed restored to " + originalSpeed);
    }

    // Method to handle the target's death
    private void Die()
    {
        print(gameObject.name + " has been destroyed!");
        Destroy(gameObject);
    }
}
