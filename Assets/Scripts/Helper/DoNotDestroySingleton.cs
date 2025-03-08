using UnityEngine;

public class DoNotDestroySingleton : MonoBehaviour
{
    // The instance of the Singleton
    public static DoNotDestroySingleton Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // If no instance exists, set this object as the instance
            Instance = this;

            // Prevent this object from being destroyed when changing scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy the duplicate
            Destroy(gameObject);
        }
    }
}
