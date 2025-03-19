using UnityEngine;

public class TeleportationToTotemScene : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform[] totemTransforms;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float teleportCooldown = 3f;
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioSource audioSource;

    private float lastTeleportTime = -Mathf.Infinity;
    private Camera mainCamera;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the scene.");
            return;
        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (audioSource == null || mainCamera == null)
        {
            Debug.LogError("AudioSource or Camera.main is not assigned.");
            return;
        }

        // Find all players by tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
        {
            Debug.LogWarning("No objects found with 'Player' tag.");
            return;
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        foreach (GameObject player in players)
        {
            Vector2 playerPosition = player.transform.position;

            foreach (Transform totemTransform in totemTransforms)
            {
                // Check if player is within detection radius
                float playerToTotemDistance = Vector2.Distance(playerPosition, totemTransform.position);
                float mouseToTotemDistance = Vector2.Distance(mousePosition, totemTransform.position);

                if (playerToTotemDistance <= detectionRadius && mouseToTotemDistance <= 2f &&
                    Input.GetKeyDown(KeyCode.F) && Time.time >= lastTeleportTime + teleportCooldown)
                {
                    // Only teleport the player who presses the key, not all players.
                    TeleportPlayerToTotem(player.transform, totemTransform);
                    lastTeleportTime = Time.time; // Set cooldown
                    break;
                }
            }
        }
    }


    void TeleportPlayerToTotem(Transform playerTransform, Transform totemTransform)
    {
        if (teleportSound && audioSource != null)
        {
            audioSource.clip = teleportSound;
            audioSource.Play();
        }

        playerTransform.position = totemTransform.position;

        // Synchronize player layer and sorting layers with the totem
        SpriteRenderer playerSpriteRenderer = playerTransform.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            SpriteRenderer totemSpriteRenderer = totemTransform.GetComponent<SpriteRenderer>();
            playerSpriteRenderer.sortingLayerID = totemSpriteRenderer.sortingLayerID;
            playerSpriteRenderer.sortingOrder = totemSpriteRenderer.sortingOrder;
        }

        UpdateChildObjectsLayersAndSorting(playerTransform, totemTransform);
    }

    private void UpdateChildObjectsLayersAndSorting(Transform player, Transform totemTransform)
    {
        // Update layer and sorting order for any child objects of the player
        foreach (Transform child in player)
        {
            child.gameObject.layer = totemTransform.gameObject.layer;

            SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
            if (childSpriteRenderer != null)
            {
                childSpriteRenderer.sortingLayerID = totemTransform.GetComponent<SpriteRenderer>().sortingLayerID;
                childSpriteRenderer.sortingOrder = totemTransform.GetComponent<SpriteRenderer>().sortingOrder;
            }

            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.sortingLayerID = totemTransform.GetComponent<SpriteRenderer>().sortingLayerID;
                childRenderer.sortingOrder = totemTransform.GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}
