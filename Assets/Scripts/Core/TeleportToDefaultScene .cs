using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToDefaultScene : MonoBehaviour
{
    public string defaultSceneName = "Graveyard";
    public KeyCode teleportKey = KeyCode.U;

    void Update()
    {
        if (Input.GetKeyDown(teleportKey))
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        SceneManager.LoadScene(defaultSceneName);
    }
}
