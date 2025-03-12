using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("UI Elements for Key Bindings")]
    public TMP_Text moveUpText;
    public TMP_Text moveDownText;
    public TMP_Text moveLeftText;
    public TMP_Text moveRightText;
    public TMP_Text primaryAttackText;
    public TMP_Text secondaryAttackText;

    public Button moveUpButton;
    public Button moveDownButton;
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button primaryAttackButton;
    public Button secondaryAttackButton;

    private string keyToRebind = null;
    private KeyCode newKey;

    [Header("Default Keys")]
    private KeyCode moveUp = KeyCode.W;
    private KeyCode moveDown = KeyCode.S;
    private KeyCode moveLeft = KeyCode.A;
    private KeyCode moveRight = KeyCode.D;
    private KeyCode primaryAttack = KeyCode.Mouse0;
    private KeyCode secondaryAttack = KeyCode.Mouse1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadKeys();
        UpdateUI();

        // Assign button listeners for rebinding keys
        moveUpButton.onClick.AddListener(() => StartRebind("MoveUp"));
        moveDownButton.onClick.AddListener(() => StartRebind("MoveDown"));
        moveLeftButton.onClick.AddListener(() => StartRebind("MoveLeft"));
        moveRightButton.onClick.AddListener(() => StartRebind("MoveRight"));
        primaryAttackButton.onClick.AddListener(() => StartRebind("PrimaryAttack"));
        secondaryAttackButton.onClick.AddListener(() => StartRebind("SecondaryAttack"));
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(keyToRebind))
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    newKey = key;
                    AssignNewKey();
                    break;
                }
            }
        }
    }

    private void StartRebind(string keyName)
    {
        keyToRebind = keyName;
        Debug.Log("Press a key to rebind: " + keyName);
    }

    private void AssignNewKey()
    {
        PlayerPrefs.SetString(keyToRebind, newKey.ToString());
        PlayerPrefs.Save();
        keyToRebind = null;
        LoadKeys();
        UpdateUI();
    }

    private void LoadKeys()
    {
        moveUp = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveUp", moveUp.ToString()));
        moveDown = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveDown", moveDown.ToString()));
        moveLeft = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeft", moveLeft.ToString()));
        moveRight = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRight", moveRight.ToString()));
        primaryAttack = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PrimaryAttack", primaryAttack.ToString()));
        secondaryAttack = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SecondaryAttack", secondaryAttack.ToString()));
    }

    private void UpdateUI()
    {
        moveUpText.text = moveUp.ToString();
        moveDownText.text = moveDown.ToString();
        moveLeftText.text = moveLeft.ToString();
        moveRightText.text = moveRight.ToString();
        primaryAttackText.text = primaryAttack.ToString();
        secondaryAttackText.text = secondaryAttack.ToString();
    }

    public KeyCode GetKey(string action)
    {
        switch (action)
        {
            case "MoveUp": return moveUp;
            case "MoveDown": return moveDown;
            case "MoveLeft": return moveLeft;
            case "MoveRight": return moveRight;
            case "PrimaryAttack": return primaryAttack;
            case "SecondaryAttack": return secondaryAttack;
            default: return KeyCode.None;
        }
    }
}
