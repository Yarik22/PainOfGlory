using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChecker : MonoBehaviour
{
    [SerializeField] private float checkInterval = 1f;

    void Start()
    {
        InvokeRepeating(nameof(CheckForEnemies), checkInterval, checkInterval);
    }

    void CheckForEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            Debug.Log("No enemies left! Returning to Graveyard...");
            SceneManager.LoadScene("Graveyard");
        }
    }
}
