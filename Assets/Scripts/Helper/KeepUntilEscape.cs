using UnityEngine;

public class KeepUntilEscape : MonoBehaviour
{
    // Flag to indicate if the object should be kept across scene loads
    private bool shouldDestroyOnEscape = false;

    void Start()
    {
        // Prevent the object from being destroyed across scene loads
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape) && !shouldDestroyOnEscape)
        {
            // Mark the object for destruction and destroy it
            shouldDestroyOnEscape = true;
            Destroy(gameObject);
            Debug.Log("Object destroyed after pressing Escape.");
        }
    }
}
