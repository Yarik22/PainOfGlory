using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int Coins { get; private set; } = 0;
    [SerializeField] private TMP_Text coinText;

    private const string CoinKey = "Coins";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure persistence
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
    }

    private void Start()
    {
        LoadCoins();
        UpdateUI();
    }

    private void Update()
    {
        LoadCoins();  // Update coins from PlayerPrefs every frame
        UpdateUI();   // Update UI based on current value
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        SaveCoins();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + Coins;
        }
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinKey, Coins);
        PlayerPrefs.Save();
    }

    private void LoadCoins()
    {
        Coins = PlayerPrefs.GetInt(CoinKey, 0); // Retrieve current coin count from PlayerPrefs
    }
}
