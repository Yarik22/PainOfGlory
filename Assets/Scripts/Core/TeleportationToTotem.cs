using UnityEngine;
using UnityEngine.Audio;

public class TeleportationToTotem : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform[] totemTransforms;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float teleportCooldown = 3f;
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioSource audioSource;

    private Transform playerTransform;
    private float lastTeleportTime = -Mathf.Infinity;
    private Camera mainCamera;
    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
        playerSpriteRenderer = playerTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        foreach (Transform totemTransform in totemTransforms)
        {
            float playerToTotemDistance = Vector2.Distance(playerTransform.position, totemTransform.position);
            float mouseToTotemDistance = Vector2.Distance(mousePosition, totemTransform.position);

            if (playerToTotemDistance <= detectionRadius && mouseToTotemDistance <= 2f &&
                Input.GetKeyDown(KeyCode.F) && Time.time >= lastTeleportTime + teleportCooldown)
            {
                TeleportPlayerToTotem(totemTransform);
                lastTeleportTime = Time.time;
                break;
            }
        }
    }

    void TeleportPlayerToTotem(Transform totemTransform)
    {
        if (teleportSound && audioSource != null)
        {
            audioSource.clip = teleportSound;
            audioSource.Play();
        }

        playerTransform.position = totemTransform.position;

        playerTransform.gameObject.layer = totemTransform.gameObject.layer;

        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sortingLayerID = totemTransform.GetComponent<SpriteRenderer>().sortingLayerID;
            playerSpriteRenderer.sortingOrder = totemTransform.GetComponent<SpriteRenderer>().sortingOrder;
        }

        UpdateChildObjectsLayersAndSorting(playerTransform, totemTransform);
    }

    private void UpdateChildObjectsLayersAndSorting(Transform player, Transform totemTransform)
    {
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
