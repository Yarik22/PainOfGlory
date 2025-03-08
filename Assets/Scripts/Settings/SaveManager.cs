using UnityEngine;
using System;
using TMPro;

public class SaveManager : MonoBehaviour
{
    private const string CoinsKey = "Coins_";
    private const string XPKey = "XP_";
    private const string DateKey = "Date_";
    private const string AttackSpeedKey = "Attck";
    private const string ProjectileKey = "Projectile";

    public TMP_Text[] saveSlotTexts; // UI Text elements for displaying save timestamps
    public TMP_Text[] slotDescriptionTexts; // UI Text elements for displaying slot descriptions

    void Start()
    {
        UpdateSaveSlotTexts();
    }

    public void SaveGame(int slot)
    {
        string slotPrefix = slot.ToString();

        float prj = PlayerPrefs.GetFloat("Projectile", 3);
        float attck = PlayerPrefs.GetFloat("Attck", 1);
        int coins = PlayerPrefs.GetInt("Coins", 0);
        int xp = PlayerPrefs.GetInt("XP", 0);
        string saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        PlayerPrefs.SetInt(CoinsKey + slotPrefix, coins);
        PlayerPrefs.SetInt(XPKey + slotPrefix, xp);
        PlayerPrefs.SetFloat(AttackSpeedKey + slotPrefix, attck);
        PlayerPrefs.SetFloat(ProjectileKey + slotPrefix, prj);
        PlayerPrefs.SetString(DateKey + slotPrefix, saveDate);

        PlayerPrefs.Save();

        Debug.Log($"Game saved in Slot {slot}: Coins {coins}, XP {xp}, AttackSpeed {attck}, Projectiling {prj}, Date {saveDate}");

        UpdateSaveSlotTexts();
    }

    public void LoadGame(int slot)
    {
        string slotPrefix = slot.ToString();

        float prj = PlayerPrefs.GetFloat(ProjectileKey + slotPrefix, 3);
        float attck = PlayerPrefs.GetFloat(AttackSpeedKey + slotPrefix, 1);
        int coins = PlayerPrefs.GetInt(CoinsKey + slotPrefix, 0);
        int xp = PlayerPrefs.GetInt(XPKey + slotPrefix, 0);
        string saveDate = PlayerPrefs.GetString(DateKey + slotPrefix, "No Save");

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetFloat("Attck", attck);
        PlayerPrefs.SetFloat("Projectile", prj);
        PlayerPrefs.Save();

        Debug.Log($"Game loaded from Slot {slot}: Coins {coins}, XP {xp}, AttackSpeed {attck}, Projectiling {prj}, Date {saveDate}");
    }

    // Function to get description of each slot
    public string GetSlotDescription(int slot)
    {
        string slotPrefix = slot.ToString();

        int coins = PlayerPrefs.GetInt(CoinsKey + slotPrefix, 0);
        int xp = PlayerPrefs.GetInt(XPKey + slotPrefix, 0);
        float attackSpeed = PlayerPrefs.GetFloat(AttackSpeedKey + slotPrefix, 1);
        float projectileSpeed = PlayerPrefs.GetFloat(ProjectileKey + slotPrefix, 3);
        string saveDate = PlayerPrefs.GetString(DateKey + slotPrefix, "No Save");

        return $"Slot {slot} - Coins: {coins}, XP: {xp}, Attack Speed: {attackSpeed}, Projectile Speed: {projectileSpeed}, Date: {saveDate}";
    }

    private void UpdateSaveSlotTexts()
    {
        for (int i = 0; i < saveSlotTexts.Length; i++)
        {
            string date = PlayerPrefs.GetString(DateKey + (i + 1), "No Save");
            saveSlotTexts[i].text = $"Slot {i + 1}: {date}";

            if (slotDescriptionTexts.Length > i)
            {
                string description = GetSlotDescription(i + 1);
                slotDescriptionTexts[i].text = description;
            }
        }
    }
}
