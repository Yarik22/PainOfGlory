using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject tradePanel;
    public float interactionDistance = 1.5f;
    public KeyCode interactionKey = KeyCode.E;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player object not found! Ensure the player has the 'Player' tag assigned.");
        }
    }

    void Update()
    {
        // Check if player is null to avoid NullReferenceException
        if (player == null) return;

        if (Vector2.Distance(player.transform.position, transform.position) <= interactionDistance
            && Input.GetKeyDown(interactionKey))
        {
            if (IsMouseOver())
            {
                ToggleTradePanel();
            }
        }

        if (Input.anyKeyDown && tradePanel.activeSelf &&
            !Input.GetKeyDown(interactionKey) &&
            !Input.GetKeyDown(KeyCode.Alpha1) &&
            !Input.GetKeyDown(KeyCode.Alpha2))
        {
            CloseTradePanel();
        }
    }

    bool IsMouseOver()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return GetComponent<Collider2D>().OverlapPoint(mousePos);
    }

    void ToggleTradePanel()
    {
        if (tradePanel != null)
        {
            tradePanel.SetActive(!tradePanel.activeSelf);
        }
    }

    void CloseTradePanel()
    {
        if (tradePanel != null)
        {
            tradePanel.SetActive(false);
        }
    }
}
