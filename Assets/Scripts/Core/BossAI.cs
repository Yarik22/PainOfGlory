using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int maxHealth = 50;

    public GameObject hitEffectPrefab;

    private NavMeshAgent agent;
    private float lastAttackTime;
    private int currentHealth;
    private bool isDisabled;

    private Transform player; // Замість публічної змінної для гравця

    void Start()
    {
        // Знайти гравця за тегом
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        currentHealth = maxHealth;

        CheckIfDisabled();
    }

    void Update()
    {
        CheckIfDisabled();

        if (isDisabled)
        {
            if (agent.enabled) agent.ResetPath();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            if (agent.enabled) agent.SetDestination(player.position);
        }
        else
        {
            if (agent.enabled) agent.ResetPath();
        }

        if (distanceToPlayer < attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void CheckIfDisabled()
    {
        float projectileCooldown = PlayerPrefs.GetFloat("Projectile", 1f);
        float attackCooldown = PlayerPrefs.GetFloat("Attck", 1f);

        isDisabled = (projectileCooldown > 0.75f && attackCooldown > 0.5f);

        if (isDisabled)
        {
            if (agent.enabled) agent.enabled = false;
            Debug.Log("Boss is inactive due to player's high cooldowns.");
        }
        else
        {
            if (!agent.enabled) agent.enabled = true;
        }
    }

    void Attack()
    {
        Debug.Log("Boss Attacked the Player!");

        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            TakeDamage();
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over! The Boss defeated the player.");
            EndGame();
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Boss Hit! Health left: " + currentHealth);

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
        Debug.Log("Boss Defeated! Congratulations!");

        int coinsReward = Random.Range(50, 101);
        int xpReward = Random.Range(20, 41);

        CoinManager.Instance.AddCoins(coinsReward);
        XPManager.Instance.AddXP(xpReward);

        SceneManager.LoadScene("WinScene");
    }

    void EndGame()
    {
        Debug.Log("Game Over! Player lost to the Boss.");

        GameObject sounds = GameObject.FindGameObjectWithTag("Sounds");
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");

        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("XP", 0);
        PlayerPrefs.Save();

        if (sounds != null) Destroy(sounds);
        if (npc != null) Destroy(npc);

        SceneManager.LoadScene("MainMenu");
    }
}
