using UnityEngine;
using UnityEngine.Tilemaps;

public class TileOpacityChanger : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private float targetAlpha = 0.15f;
    [SerializeField] private float defaultAlpha = 1f;
    [SerializeField] private float lerpSpeed = 2f;

    private float currentAlpha;

    void Start()
    {
        currentAlpha = defaultAlpha;
    }

    void Update()
    {
        Color currentColor = tilemap.GetComponent<Renderer>().material.color;
        currentColor.a = Mathf.MoveTowards(currentColor.a, currentAlpha, lerpSpeed * Time.deltaTime);
        tilemap.GetComponent<Renderer>().material.color = currentColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            currentAlpha = targetAlpha;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            currentAlpha = defaultAlpha;
        }
    }
}
