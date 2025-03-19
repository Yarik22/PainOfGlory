using UnityEngine;

public class SpawnAtRespawnTag : MonoBehaviour
{
    // Tag of the object where the GameObject should spawn
    public string respawnTag = "Respawn";

    // Start is called before the first frame update
    void Start()
    {
        // Find the object with the tag "Respawn"
        GameObject respawnObject = GameObject.FindGameObjectWithTag(respawnTag);

        // If an object with the tag "Respawn" is found
        if (respawnObject != null)
        {
            // Move this GameObject to the position of the respawn object
            transform.position = respawnObject.transform.position;

            // Set the layer of the spawned object and all its children to match the respawn object's layer
            SetLayerRecursively(gameObject, respawnObject.layer);

            // Set the sorting layer of the spawned object and all its children to match the respawn object's sorting layer
            SetSortingLayerRecursively(gameObject, respawnObject.GetComponent<SpriteRenderer>().sortingLayerName);
        }
        else
        {
            Debug.LogWarning("No object found with the tag 'Respawn'.");
        }
    }

    // Recursively set the layer for the object and all its children
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    // Recursively set the sorting layer for the object and all its children
    void SetSortingLayerRecursively(GameObject obj, string sortingLayerName)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
        }

        foreach (Transform child in obj.transform)
        {
            SetSortingLayerRecursively(child.gameObject, sortingLayerName);
        }
    }
}
