using TMPro;
using UnityEngine;

public class TradingManager : MonoBehaviour
{
    [Header("Player References")]
    public GameObject player;
    private PlayerMovementController playerController;

    [Header("Cooldown Decrease Values")]
    [SerializeField] private float attackCooldownDecreaseAmount = 0.01f;
    [SerializeField] private float projectileCooldownDecreaseAmount = 0.01f;

    [Header("Currency Costs")]
    [SerializeField] private int attackCooldownCost = 5;
    [SerializeField] private int projectileCooldownCost = 10;

    [Header("UI References")]
    public GameObject tradingPanel;
    [SerializeField] private TMP_Text attackCooldownText;
    [SerializeField] private TMP_Text projectileCooldownText;
    [SerializeField] private TMP_Text currentAttackCooldownText;
    [SerializeField] private TMP_Text currentProjectileCooldownText;
    [SerializeField] private TMP_Text errorText;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerMovementController>();

        Debug.Log("Trading Manager initialized.");

        UpdateUIText();
        HideErrorText();
    }

    void Update()
    {
        if (tradingPanel.activeSelf)
        {
            HandleTrading();
        }
    }

    private void HandleTrading()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (CoinManager.Instance.Coins >= attackCooldownCost)
            {
                DecreaseAttackCooldown();
                CoinManager.Instance.AddCoins(-attackCooldownCost);
                attackCooldownCost = Mathf.RoundToInt(attackCooldownCost * 1.2f);

                Debug.Log("AttackCooldown decreased by " + attackCooldownDecreaseAmount + ". Cost now: " + attackCooldownCost + " Coins.");
                UpdateUIText();
                HideErrorText();
            }
            else
            {
                int requiredCoins = attackCooldownCost - CoinManager.Instance.Coins;
                ShowErrorText("Not enough Coins. Need " + requiredCoins + " more.");
                Debug.Log("Not enough Coins to decrease AttackCooldown. Required: " + attackCooldownCost + " Coins.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (XPManager.Instance.XP >= projectileCooldownCost)
            {
                DecreaseProjectileCooldown();
                XPManager.Instance.AddXP(-projectileCooldownCost);
                projectileCooldownCost = Mathf.RoundToInt(projectileCooldownCost * 1.2f);

                Debug.Log("ProjectileCooldown decreased by " + projectileCooldownDecreaseAmount + ". Cost now: " + projectileCooldownCost + " XP.");
                UpdateUIText();
                HideErrorText();
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
        float newAttackCooldown = Mathf.Max(0.1f, playerController.attackCooldown - attackCooldownDecreaseAmount);
        playerController.attackCooldown = newAttackCooldown;

        PlayerPrefs.SetFloat("Attck", newAttackCooldown);
        PlayerPrefs.Save();

        Debug.Log("AttackCooldown successfully decreased. New value: " + newAttackCooldown);
        UpdateUIText();
    }

    private void DecreaseProjectileCooldown()
    {
        float newProjectileCooldown = Mathf.Max(0.5f, playerController.projectileCooldown - projectileCooldownDecreaseAmount);
        playerController.projectileCooldown = newProjectileCooldown;

        PlayerPrefs.SetFloat("Projectile", newProjectileCooldown);
        PlayerPrefs.Save();

        Debug.Log("ProjectileCooldown successfully decreased. New value: " + newProjectileCooldown);
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        if (attackCooldownText != null)
        {
            attackCooldownText.text = "Cost to decrease AttackCooldown: " + attackCooldownCost + " Coins";
        }

        if (projectileCooldownText != null)
        {
            projectileCooldownText.text = "Cost to decrease ProjectileCooldown: " + projectileCooldownCost + " XP";
        }

        if (currentAttackCooldownText != null)
        {
            currentAttackCooldownText.text = "Current AttackCooldown: " + playerController.attackCooldown.ToString("F2") + "s";
        }

        if (currentProjectileCooldownText != null)
        {
            currentProjectileCooldownText.text = "Current ProjectileCooldown: " + playerController.projectileCooldown.ToString("F2") + "s";
        }
    }

    private void ShowErrorText(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
        }
    }

    private void HideErrorText()
    {
        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }
}
