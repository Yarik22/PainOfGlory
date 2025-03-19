using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button disconnectButton;

    private void Start()
    {
        // Зв'язати кнопки з методами
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        disconnectButton.onClick.AddListener(StopNetwork);
    }

    // Запустити хоста
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host Started");
    }

    // Запустити клієнта
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Started");
    }

    // Відключити мережу
    private void StopNetwork()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("Host Stopped");
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("Client Stopped");
        }
    }
}
