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

    public GameObject hitEffectPrefab; // Reference to the particle effect prefab

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

    public void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Enemy Hit! Health left: " + currentHealth);

        // Instantiate the hit effect at the enemy's position
        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 1f); // Destroy the particle effect after 1 second
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Defeated!");

        // Reward the player with random coins and XP
        int coinsReward = Random.Range(5, 11); // 5 to 10 coins
        int xpReward = Random.Range(1, 4); // 1 to 3 XP

        CoinManager.Instance.AddCoins(coinsReward);
        XPManager.Instance.AddXP(xpReward);

        Destroy(gameObject);
    }

    void EndGame()
    {
        GameObject sounds = GameObject.FindGameObjectWithTag("Sounds");
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("XP", 0);
        PlayerPrefs.Save();
        if (sounds != null)
            Destroy(sounds);
        if (npc != null)
            Destroy(npc);
        SceneManager.LoadScene("MainMenu");
    }
}
