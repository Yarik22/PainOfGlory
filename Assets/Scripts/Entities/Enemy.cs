using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeFireDamage(float damage)
    {
        // Apply fire damage logic (e.g., over time or immediate damage)
        TakeDamage(damage);
        Debug.Log("Taking fire damage: " + damage);
    }

    public void TakePoisonDamage(float damage)
    {
        // Apply poison damage logic (e.g., over time or immediate damage)
        TakeDamage(damage);
        Debug.Log("Taking poison damage: " + damage);
    }

    public void TakeFreezeDamage(float damage)
    {
        // Apply freeze damage logic (e.g., slow down or immediate damage)
        TakeDamage(damage);
        Debug.Log("Taking freeze damage: " + damage);
    }

    private void Die()
    {
        // Logic for when the enemy dies
        Destroy(gameObject);
    }
}
