using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string continueScene;
    [SerializeField] private string menuScene;
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
}