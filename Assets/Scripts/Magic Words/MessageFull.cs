using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MessageFull : MonoBehaviour
{
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
}
