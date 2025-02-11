using UnityEngine;
using System.Collections.Generic;

public class AlphaFader : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private float targetAlpha = 0.15f;
    [SerializeField] private float defaultAlpha = 1f;
    [SerializeField] private float lerpSpeed = 2f;

    private Dictionary<SpriteRenderer, float> spriteAlphas = new Dictionary<SpriteRenderer, float>();
    private float currentAlpha;

    void Start()
    {
        currentAlpha = defaultAlpha;
        StoreOriginalAlphas();
    }

    void Update()
    {
        foreach (var sprite in spriteAlphas.Keys)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a = Mathf.MoveTowards(color.a, currentAlpha, lerpSpeed * Time.deltaTime);
                sprite.color = color;
            }
        }
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

    private void StoreOriginalAlphas()
    {
        if (targetObject != null)
        {
            SpriteRenderer[] sprites = targetObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                if (!spriteAlphas.ContainsKey(sprite))
                {
                    spriteAlphas[sprite] = sprite.color.a;
                }
            }
        }
    }
}
