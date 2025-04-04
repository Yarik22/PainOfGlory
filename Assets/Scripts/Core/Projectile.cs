using UnityEngine;

public class Projectile : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>()?.TakeDamage();
            Destroy(gameObject);
        }
    }
}
