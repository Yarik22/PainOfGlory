using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] menuPanels;
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenPanel(GameObject panelToOpen)
    {
        CloseAllPanels();
        panelToOpen.SetActive(true);
        var allShakes = FindObjectsByType<ButtonTextShake>(FindObjectsSortMode.None);
        foreach (var shake in allShakes)
        {
            shake.ResetShakeEffect();
        }

    }


    private void CloseAllPanels()
    {
        foreach (GameObject panel in menuPanels)
        {
            panel.SetActive(false);
        }
    }

    public void StartNewGame(string sceneName)
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("XP", 0);
        PlayerPrefs.SetFloat("Attck", 1);
        PlayerPrefs.SetFloat("Projectile", 3);
        PlayerPrefs.Save();
        ChangeScene(sceneName);
    }
}