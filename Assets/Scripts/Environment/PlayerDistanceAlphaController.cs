using UnityEngine;

public class PlayerDistanceAlphaController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float minAlpha = 0f;
    [SerializeField] private float maxAlpha = 1f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        float t = Mathf.Clamp01(1 - (distance / maxDistance));
        float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
    }
}
