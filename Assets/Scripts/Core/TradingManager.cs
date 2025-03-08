using TMPro;
using UnityEngine;

public class TradingManager : MonoBehaviour
{
    [Header("Player References")]
    public GameObject player; // Reference to the player
    private PlayerMovementController playerController;

    [Header("Cooldown Decrease Values")]
    [SerializeField] private float attackCooldownDecreaseAmount = 0.01f; // Amount to decrease AttackCooldown
    [SerializeField] private float projectileCooldownDecreaseAmount = 0.01f; // Amount to decrease ProjectileCooldown

    [Header("Currency Costs")]
    [SerializeField] private int attackCooldownCost = 5; // Cost to decrease AttackCooldown (in coins)
    [SerializeField] private int projectileCooldownCost = 10; // Cost to decrease ProjectileCooldown (in XP)

    [Header("UI References")]
    public GameObject tradingPanel; // Reference to the trade panel UI
    [SerializeField] private TMP_Text attackCooldownText; // Reference to text showing AttackCooldown cost
    [SerializeField] private TMP_Text projectileCooldownText; // Reference to text showing ProjectileCooldown cost
    [SerializeField] private TMP_Text currentAttackCooldownText; // Reference to text showing the current AttackCooldown
    [SerializeField] private TMP_Text currentProjectileCooldownText; // Reference to text showing the current ProjectileCooldown
    [SerializeField] private TMP_Text errorText; // Reference to the error message text (not enough currency)

    private void Start()
    {
        // Find the player object and get the PlayerMovementController script
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerMovementController>();

        Debug.Log("Trading Manager initialized.");

        // Initialize UI text with current values
        UpdateUIText();
        HideErrorText(); // Ensure error text is hidden at the start
    }

    void Update()
    {
        // Check if the trade panel is active
        if (tradingPanel.activeSelf)
        {
            HandleTrading();
        }
    }

    private void HandleTrading()
    {
        // Decrease AttackCooldown with Alpha1 (KeyCode.Alpha1) - using coins
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Check if the player has enough coins
            if (CoinManager.Instance.Coins >= attackCooldownCost)
            {
                DecreaseAttackCooldown();
                CoinManager.Instance.AddCoins(-attackCooldownCost); // Deduct coins
                attackCooldownCost = Mathf.RoundToInt(attackCooldownCost * 1.2f); // Increase cost by 20%

                Debug.Log("AttackCooldown decreased by " + attackCooldownDecreaseAmount + ". Cost now: " + attackCooldownCost + " Coins.");
                UpdateUIText(); // Update the text when values change
                HideErrorText(); // Hide error message when transaction is successful
            }
            else
            {
                int requiredCoins = attackCooldownCost - CoinManager.Instance.Coins;
                ShowErrorText("Not enough Coins. Need " + requiredCoins + " more.");
                Debug.Log("Not enough Coins to decrease AttackCooldown. Required: " + attackCooldownCost + " Coins.");
            }
        }

        // Decrease ProjectileCooldown with Alpha2 (KeyCode.Alpha2) - using XP
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Check if the player has enough XP
            if (XPManager.Instance.XP >= projectileCooldownCost)
            {
                DecreaseProjectileCooldown();
                XPManager.Instance.AddXP(-projectileCooldownCost); // Deduct XP
                projectileCooldownCost = Mathf.RoundToInt(projectileCooldownCost * 1.2f); // Increase cost by 20%

                Debug.Log("ProjectileCooldown decreased by " + projectileCooldownDecreaseAmount + ". Cost now: " + projectileCooldownCost + " XP.");
                UpdateUIText(); // Update the text when values change
                HideErrorText(); // Hide error message when transaction is successful
            }
            else
            {
                int requiredXP = projectileCooldownCost - XPManager.Instance.XP;
                ShowErrorText("Not enough XP. Need " + requiredXP + " more.");
                Debug.Log("Not enough XP to decrease ProjectileCooldown. Required: " + projectileCooldownCost + " XP.");
            }
        }
    }

    private void DecreaseAttackCooldown()
    {
        // Decrease AttackCooldown
        float newAttackCooldown = Mathf.Max(0.1f, playerController.attackCooldown - attackCooldownDecreaseAmount);
        playerController.attackCooldown = newAttackCooldown;

        // Save the new value to PlayerPrefs
        PlayerPrefs.SetFloat("Attck", newAttackCooldown);
        PlayerPrefs.Save();

        Debug.Log("AttackCooldown successfully decreased. New value: " + newAttackCooldown);
        UpdateUIText(); // Update the text when values change
    }

    private void DecreaseProjectileCooldown()
    {
        // Decrease ProjectileCooldown
        float newProjectileCooldown = Mathf.Max(0.5f, playerController.projectileCooldown - projectileCooldownDecreaseAmount);
        playerController.projectileCooldown = newProjectileCooldown;

        // Save the new value to PlayerPrefs
        PlayerPrefs.SetFloat("Projectile", newProjectileCooldown);
        PlayerPrefs.Save();

        Debug.Log("ProjectileCooldown successfully decreased. New value: " + newProjectileCooldown);
        UpdateUIText(); // Update the text when values change
    }

    private void UpdateUIText()
    {
        // Update cost text
        if (attackCooldownText != null)
        {
            attackCooldownText.text = "Cost to decrease AttackCooldown: " + attackCooldownCost + " Coins";
        }

        if (projectileCooldownText != null)
        {
            projectileCooldownText.text = "Cost to decrease ProjectileCooldown: " + projectileCooldownCost + " XP";
        }

        // Update current cooldown text
        if (currentAttackCooldownText != null)
        {
            currentAttackCooldownText.text = "Current AttackCooldown: " + playerController.attackCooldown.ToString("F2") + "s";
        }

        if (currentProjectileCooldownText != null)
        {
            currentProjectileCooldownText.text = "Current ProjectileCooldown: " + playerController.projectileCooldown.ToString("F2") + "s";
        }
    }

    // Method to show the error message
    private void ShowErrorText(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.color = Color.red; // Set text color to red for errors
            errorText.gameObject.SetActive(true); // Show the error message
        }
    }

    // Method to hide the error message
    private void HideErrorText()
    {
        if (errorText != null)
        {
            errorText.gameObject.SetActive(false); // Hide the error message
        }
    }
}
