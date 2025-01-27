using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTextShake : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject textComponent;
    [SerializeField] private float shakeIntensity = 5f;
    [SerializeField] private float shakeSpeed = 50f;

    private RectTransform textTransform;
    private Vector3 originalPosition;
    private bool isShaking = false;

    [SerializeField] private Texture2D customCursor;
    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<Text>().gameObject;
        }

        textTransform = textComponent.GetComponent<RectTransform>();
        originalPosition = textTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isShaking = true;
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isShaking = false;
        textTransform.anchoredPosition = originalPosition;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (isShaking)
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * shakeIntensity - (shakeIntensity / 2);
            float offsetY = Mathf.PerlinNoise(0, Time.time * shakeSpeed) * shakeIntensity - (shakeIntensity / 2);

            textTransform.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }
    }
}
