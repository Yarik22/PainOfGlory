using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int maxHealth = 10; // Enemy dies after 10 hits

    private NavMeshAgent agent;
    private float lastAttackTime;
    private int currentHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && IsOnSameLayer())
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }

        if (distanceToPlayer < attackRange && Time.time > lastAttackTime + attackCooldown && IsOnSameLayer())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    bool IsOnSameLayer()
    {
        return gameObject.layer == player.gameObject.layer;
    }

    void Attack()
    {
        Debug.Log("Enemy Attacked the Player!");
        // Implement player damage logic here
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            TakeDamage();
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over! Enemy caught the player.");
            EndGame();
        }
    }

    void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Enemy Hit! Health left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Defeated!");
        Destroy(gameObject);
    }

    void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
