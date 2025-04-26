using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.TextCore;

public class EmojiManager : MonoBehaviour
{
    // string spriteAsset = "Sprite Assets/EmojiSpriteAsset";
    string spriteAsset = "Sprite Assets/emojis";
    
    // Singleton instance
    public static EmojiManager Instance { get; private set; }
    
    private Dictionary<string, Sprite> emojiCache = new Dictionary<string, Sprite>();
    private Dictionary<string, string> emojiUrlMapping = new Dictionary<string, string>();
    
    // TMP Sprite Asset that will contain all emoji sprites
    public TMP_SpriteAsset emojiSpriteAsset;
    
    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create sprite asset if not assigned
        if (emojiSpriteAsset == null)
        {
            // Try to load the default sprite asset
            emojiSpriteAsset = Resources.Load<TMP_SpriteAsset>(spriteAsset);

            // If still null, create a new one
            if (emojiSpriteAsset == null)
            {
                emojiSpriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
                emojiSpriteAsset.name = spriteAsset;

                // Initialize texture and material for the sprite asset
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.clear);
                texture.Apply();

                Material material = new Material(Shader.Find("TextMeshPro/Sprite"));
                material.name = spriteAsset + " Material";
                material.mainTexture = texture;

                emojiSpriteAsset.spriteSheet = texture;
                emojiSpriteAsset.material = material;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.AssetDatabase.CreateAsset(emojiSpriteAsset, "Assets/Resources/" + spriteAsset + ".asset");
                    UnityEditor.AssetDatabase.AddObjectToAsset(material, emojiSpriteAsset);
                    UnityEditor.AssetDatabase.SaveAssets();
                }
#endif
            }
        }
        
        // Force update lookup tables
        emojiSpriteAsset.UpdateLookupTables();
    }
    
    
    public void LoadAllEmojis(List<emojie> emojies)
    {
        foreach (var emoji in emojies)
        {
            emojiUrlMapping.Add(emoji.name, emoji.url);
        }

        foreach (var emoji in emojiUrlMapping)
        {
            StartCoroutine(DownloadEmoji(emoji.Key, emoji.Value));
        }
    }
    
    private IEnumerator DownloadEmoji(string emojiName, string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                // Add to cache
                emojiCache[emojiName] = sprite;

                // Register the sprite with TextMeshPro
                RegisterSpriteWithTMP(emojiName, sprite);

                // Notify any registered text components that emojis have been updated
                NotifyEmojiUpdate();
                Debug.Log($"Downloaded and registered emoji: {emojiName}");
            }
            else
            {
                Debug.LogError($"Failed to download emoji {emojiName}: {webRequest.error}");
            }
        }
    }
    
    // Method to register sprites with TextMeshPro
    public void RegisterSpriteWithTMP(string emojiName, Sprite sprite)
    {
        if (sprite == null || emojiSpriteAsset == null)
        {
            Debug.LogError("Sprite or Sprite Asset is null. Cannot register sprite.");
            return;
        }
        // Create sprite glyph
        TMP_SpriteGlyph spriteGlyph = new TMP_SpriteGlyph();
        spriteGlyph.sprite = sprite;
        spriteGlyph.metrics = new GlyphMetrics(
            sprite.rect.width, 
            sprite.rect.height, 
            0, 
            sprite.rect.height * 0.8f, // Baseline adjustment
            sprite.rect.width
        );
        spriteGlyph.glyphRect = new GlyphRect(
            (int)sprite.rect.x, 
            (int)sprite.rect.y, 
            (int)sprite.rect.width, 
            (int)sprite.rect.height
        );
        
        // Create sprite character
        TMP_SpriteCharacter spriteChar = new TMP_SpriteCharacter();
        spriteChar.name = emojiName;
        spriteChar.glyph = spriteGlyph;
        // Set appropriate Unicode value or ID
        spriteChar.glyphIndex = (uint)emojiSpriteAsset.spriteCharacterTable.Count + 1;
        spriteChar.unicode = (uint)emojiSpriteAsset.spriteCharacterTable.Count + 1000; // Incremental ID
        spriteChar.scale = 1.0f;
        spriteChar.textAsset = emojiSpriteAsset;

        
        // Add to sprite asset
        emojiSpriteAsset.spriteCharacterTable.Add(spriteChar);
        emojiSpriteAsset.spriteGlyphTable.Add(spriteGlyph);

        int glyphIndex = emojiSpriteAsset.spriteGlyphTable.Count;
        // Critical: Create a TMP_Sprite info for the sprite
        TMP_Sprite spriteInfo = new TMP_Sprite();
        spriteInfo.id = glyphIndex;
        spriteInfo.name = emojiName;
        spriteInfo.x = (int)sprite.rect.x;
        spriteInfo.y = (int)sprite.rect.y;
        spriteInfo.width = (int)sprite.rect.width;
        spriteInfo.height = (int)sprite.rect.height;
        spriteInfo.xOffset = 0;
        spriteInfo.yOffset = 0;
        spriteInfo.xAdvance = (int)sprite.rect.width;
        spriteInfo.scale = 1.0f;

        List<TMP_Sprite> spriteInfoList = emojiSpriteAsset.spriteInfoList;
            if (spriteInfoList == null)
            {
                // If list is null, create a new one
                spriteInfoList = new List<TMP_Sprite>();
            }
            spriteInfoList.Add(spriteInfo);
            emojiSpriteAsset.spriteInfoList = spriteInfoList;
        
        // Mark sprite asset as dirty to ensure changes are saved
        emojiSpriteAsset.UpdateLookupTables();
    }
    
    // List of text components to notify when emojis are updated
    private List<EmojiTextComponent> registeredTextComponents = new List<EmojiTextComponent>();
    
    public void RegisterTextComponent(EmojiTextComponent textComponent)
    {
        if (!registeredTextComponents.Contains(textComponent))
        {
            registeredTextComponents.Add(textComponent);
            
            // Assign sprite asset to text component
            textComponent.spriteAsset = emojiSpriteAsset;
        }
    }
    
    public void UnregisterTextComponent(EmojiTextComponent textComponent)
    {
        if (registeredTextComponents.Contains(textComponent))
        {
            registeredTextComponents.Remove(textComponent);
        }
    }
    
    private void NotifyEmojiUpdate()
    {
        foreach (var textComponent in registeredTextComponents)
        {
            if (textComponent != null)
            {
                textComponent.SetTextWithEmojis(textComponent.text);
            }
        }
    }
    
    public Sprite GetEmojiSprite(string emojiName)
    {
        if (emojiCache.TryGetValue(emojiName, out Sprite sprite))
        {
            return sprite;
        }
        
        // If not cached, try to download
        if (emojiUrlMapping.ContainsKey(emojiName))
        {
            StartCoroutine(DownloadEmoji(emojiName, emojiUrlMapping[emojiName]));
        }
        
        return null;
    }
    
    public bool IsEmojiAvailable(string emojiName)
    {
        return emojiCache.ContainsKey(emojiName);
    }
}