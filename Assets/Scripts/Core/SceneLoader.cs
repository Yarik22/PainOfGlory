using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string continueScene;
    [SerializeField] private string menuScene;
    [SerializeField] private GameObject[] menuPanels;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(continueScene);
    }
    public void StopGame()
    {
        SceneManager.LoadScene(menuScene);
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
}