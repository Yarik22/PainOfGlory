using TMPro;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    public int XP { get; private set; } = 0;
    [SerializeField] private TMP_Text xpText;

    private const string XPKey = "XP";

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
        LoadXP();
        UpdateUI();
    }

    private void Update()
    {
        LoadXP();  // Update XP from PlayerPrefs every frame
        UpdateUI();   // Update UI based on current value
    }

    public void AddXP(int amount)
    {
        XP += amount;
        SaveXP();
    }

    private void UpdateUI()
    {
        if (xpText != null)
        {
            xpText.text = "XP: " + XP;
        }
    }

    private void SaveXP()
    {
        PlayerPrefs.SetInt(XPKey, XP);
        PlayerPrefs.Save();
    }

    private void LoadXP()
    {
        XP = PlayerPrefs.GetInt(XPKey, 0); // Retrieve current XP count from PlayerPrefs
    }
}
