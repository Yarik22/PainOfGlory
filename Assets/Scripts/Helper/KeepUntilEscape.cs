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
        GameObject networkObj = GameObject.FindGameObjectWithTag("Network");
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape) && !shouldDestroyOnEscape)
        {
            if (networkObj != null)
                Destroy(networkObj);
            shouldDestroyOnEscape = true;
            Destroy(gameObject);
            Debug.Log("Object destroyed after pressing Escape.");
        }
    }
}
