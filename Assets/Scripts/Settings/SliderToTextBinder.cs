using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToTextBinder : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValueText;

    private void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        sliderValueText.text = Mathf.RoundToInt(value).ToString();
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(UpdateText);
    }
}