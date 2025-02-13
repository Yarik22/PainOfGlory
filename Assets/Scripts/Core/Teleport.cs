using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public string sceneToLoad;
    public float teleportDistance = 1.5f;
    public KeyCode teleportKey = KeyCode.F;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= teleportDistance
            && Input.GetKeyDown(teleportKey))
        {
            if (IsMouseOver())
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    bool IsMouseOver()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return GetComponent<Collider2D>().OverlapPoint(mousePos);
    }
}
