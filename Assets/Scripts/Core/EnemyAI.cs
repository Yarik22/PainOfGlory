using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int maxHealth = 10;

    public GameObject hitEffectPrefab;

    private NavMeshAgent agent;
    private float lastAttackTime;
    private int currentHealth;

    private Transform nearestPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Find the nearest player
        FindNearestPlayer();

        // If a player is found, move towards them
        if (nearestPlayer != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, nearestPlayer.position);

            if (distanceToPlayer < detectionRange)
            {
                agent.SetDestination(nearestPlayer.position);
            }
            else
            {
                agent.ResetPath();
            }

            if (distanceToPlayer < attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    // Find the nearest player in range
    void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = float.MaxValue;
        nearestPlayer = null;

        foreach (GameObject playerObj in players)
        {
            float distance = Vector2.Distance(transform.position, playerObj.transform.position);

            // If the player is closer than the previous closest, update nearestPlayer
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestPlayer = playerObj.transform;
            }
        }
    }

    void Attack()
    {
        Debug.Log("Enemy Attacked the Player!");
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

        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Defeated!");

        int coinsReward = Random.Range(5, 11);
        int xpReward = Random.Range(1, 4);

        CoinManager.Instance.AddCoins(coinsReward);
        XPManager.Instance.AddXP(xpReward);

        Destroy(gameObject);
    }

    void EndGame()
    {
        GameObject sounds = GameObject.FindGameObjectWithTag("Sounds");
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        GameObject networkObj = GameObject.FindGameObjectWithTag("Network");
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("XP", 0);
        PlayerPrefs.Save();
        if (sounds != null)
            Destroy(sounds);
        if (npc != null)
            Destroy(npc);
        if (networkObj != null)
            Destroy(networkObj);
        SceneManager.LoadScene("MainMenu");
    }
}
