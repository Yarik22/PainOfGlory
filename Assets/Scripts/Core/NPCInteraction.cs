using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject tradePanel; // Reference to the trade panel UI
    public float interactionDistance = 1.5f; // Maximum distance for interaction with NPC
    public KeyCode interactionKey = KeyCode.E; // The key to press for interaction

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player by tag
    }

    void Update()
    {
        // Check if player is within range and the E key is pressed
        if (Vector2.Distance(player.transform.position, transform.position) <= interactionDistance
            && Input.GetKeyDown(interactionKey))
        {
            // If mouse is over the NPC
            if (IsMouseOver())
            {
                ToggleTradePanel(); // Toggle the trade panel visibility
            }
        }

        // Close trade panel if any key other than '1' or '2' is pressed while the panel is open
        if (Input.anyKeyDown && tradePanel.activeSelf &&
            !Input.GetKeyDown(interactionKey) &&
            !Input.GetKeyDown(KeyCode.Alpha1) &&
            !Input.GetKeyDown(KeyCode.Alpha2))
        {
            CloseTradePanel();
        }
    }

    // Check if the mouse is currently over the NPC's collider
    bool IsMouseOver()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get mouse position
        mousePos.z = 0f; // Ensure it's on the 2D plane
        return GetComponent<Collider2D>().OverlapPoint(mousePos); // Check if mouse is over NPC
    }

    // Toggle the trade panel's active state
    void ToggleTradePanel()
    {
        if (tradePanel != null)
        {
            tradePanel.SetActive(!tradePanel.activeSelf); // Toggle between active and inactive
        }
    }

    // Close the trade panel
    void CloseTradePanel()
    {
        if (tradePanel != null)
        {
            tradePanel.SetActive(false);
        }
    }
}
