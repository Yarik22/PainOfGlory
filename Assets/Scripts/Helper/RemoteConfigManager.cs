using UnityEngine;
using UnityEngine.UI;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using System.Collections;
using TMPro;

public class RemoteConfigManager : MonoBehaviour
{
    private TMP_Text menuText;
    private Image backgroundImage;

    public struct userAttributes { }
    public struct appAttributes { }

    private void Awake()
    {
        GameObject textObject = GameObject.FindWithTag("Play");
        if (textObject != null)
            menuText = textObject.GetComponent<TMP_Text>();

        GameObject bgObject = GameObject.FindWithTag("Bg");
        if (bgObject != null)
            backgroundImage = bgObject.GetComponent<Image>();

        StartCoroutine(InitializeRemoteConfig());
    }

    private IEnumerator InitializeRemoteConfig()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            yield return UnityServices.InitializeAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void ApplyRemoteConfig(ConfigResponse response)
    {
        if (menuText != null && RemoteConfigService.Instance.appConfig.HasKey("TextColor"))
        {
            string colorHex = RemoteConfigService.Instance.appConfig.GetString("TextColor");
            if (ColorUtility.TryParseHtmlString(colorHex, out Color newTextColor))
            {
                menuText.color = newTextColor;
            }
        }

        if (backgroundImage != null && RemoteConfigService.Instance.appConfig.HasKey("BackgroundImage"))
        {
            string imageUrl = RemoteConfigService.Instance.appConfig.GetString("BackgroundImage");
            StartCoroutine(LoadBackgroundImage(imageUrl));
        }
    }

    private IEnumerator LoadBackgroundImage(string url)
    {
        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success && backgroundImage != null)
            {
                Texture2D texture = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;
                backgroundImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogError("Помилка завантаження зображення: " + request.error);
            }
        }
    }
}
