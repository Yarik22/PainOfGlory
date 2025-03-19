using UnityEngine;
using Unity.Netcode;

public class AutoHostManager : MonoBehaviour
{
    void Start()
    {
        // Check if NetworkManager is properly initialized
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not initialized!");
            return;
        }

        // Check if we are already connected as Server or Client
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Already connected as either Server or Client.");
            return;
        }

        // Try to start the host
        StartHost();
    }

    private void StartHost()
    {
        // Attempt to start the host and check if successful
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Started as Host.");
        }
        else
        {
            Debug.LogError("Failed to start as Host. Trying as Client...");
            StartClient();
        }
    }

    private void StartClient()
    {
        // Attempt to start the client and check if successful
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Started as Client.");
        }
        else
        {
            Debug.LogError("Failed to start as Client.");
        }
    }
}
