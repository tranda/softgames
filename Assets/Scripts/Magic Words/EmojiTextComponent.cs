using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class EmojiTextComponent : TextMeshProUGUI
{
    // private void Awake()
    // {
    //     base.Awake();
    //     if (EmojiManager.Instance != null)
    //     {
    //         EmojiManager.Instance.RegisterTextComponent(this);
    //     }
    // }

    // private void OnDestroy()
    // {
    //     if (EmojiManager.Instance != null)
    //     {
    //         EmojiManager.Instance.UnregisterTextComponent(this);
    //     }
    //     base.OnDestroy();
    // }
    
    public void SetTextWithEmojis(string text)
    {
        string parsedText = ParseEmojis(text);
        this.text = parsedText;
    }
    
    private string ParseEmojis(string input)
    {
        // Example regex to find emoji codes like {sad}
        string pattern = @"{([a-zA-Z0-9_]+)}";
        
        return Regex.Replace(input, pattern, match => {
            string emojiName = match.Groups[1].Value;
            // if (EmojiManager.Instance != null && EmojiManager.Instance.IsEmojiAvailable(emojiName))
            {
                return $"<sprite name=\"{emojiName}\">";
            }
            // return match.Value;
        });
    }
}