using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class MessageFull : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private EmojiTextComponent textComponent;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image avatarImage;

    public void SetText(string text)
    {
        textComponent.SetTextWithEmojis(text);
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetAvatar(Sprite avatar)
    {
        avatarImage.sprite = avatar;
    }

    public void ShowMessage()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void HideMessage()
    {
        if (gameObject.activeSelf)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
