using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MWManager : MonoBehaviour
{
    const string liveUrlToGet = "https://private-624120-softgamesassignment.apiary-mock.com/v2/magicwords";
    [SerializeField] private MWMenu mWMenu;
    [SerializeField] private Conversation conversation;

    private MagicWordsData magicWordsData;
    private int downloadedAvatarsCount;
    private int totalAvatarsCount;



    void Start()
    {
        subscribeToUIEvents();
        Init();
    }

    private void Init()
    {
        loadOnlineData(dataLoaded);
    }

    private void loadOnlineData(Action callback)
    {
        loadLevelsAsync((result) =>
        {
            magicWordsData = JsonUtility.FromJson<MagicWordsData>(result);
            callback();
        });
    }

    private void dataLoaded()
    {
        Debug.Log("Data loaded successfully!");
        cacheEmojies(magicWordsData.emojies);
        cacheAvatars(magicWordsData.avatars);

        conversation.Init(magicWordsData);
    }

    private void allAvatarsDownloaded()
    {
        if (downloadedAvatarsCount == totalAvatarsCount)
        {
            Debug.Log("All avatars downloaded successfully!");
            conversation.DisplayConversation();
        }
    }
    private void NextConversation()
    {
        conversation.NextConversation();
    }

    private void cacheAvatars(List<avatar> avatars)
    {
        totalAvatarsCount = avatars.Count;
        downloadedAvatarsCount = 0;
        foreach (var avatar in avatars)
        {
            Debug.Log($"Avatar: {avatar.name}, URL: {avatar.url}, Position: {avatar.position}");
            StartCoroutine(downloadAvatar(avatar));
        }
    }

    private void cacheEmojies(List<emojie> emojies)
    {
        // EmojiManager.Instance.LoadAllEmojis(emojies);
    }

    async void loadLevelsAsync(Action<string> callback)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, liveUrlToGet);
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        callback(result);
    }

    private void OnDestroy()
    {
        unsubscribeToUIEvents();
    }

    private void subscribeToUIEvents()
    {
        mWMenu.OnResetClicked += Reset;
        mWMenu.OnBackClicked += GoBackToMainMenu;
        mWMenu.OnNextClicked += NextConversation;
    }

    private void unsubscribeToUIEvents()
    {
        mWMenu.OnNextClicked -= NextConversation;
        mWMenu.OnBackClicked -= GoBackToMainMenu;
        mWMenu.OnResetClicked -= Reset;
    }

    private void Reset()
    {
        conversation.Reset();
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator downloadAvatar(avatar avatar)
    {

        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(avatar.url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                avatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                Debug.Log($"Downloaded avatar: {avatar.name}");
            }
            else
            {
                Debug.LogError($"Failed to download avatar {avatar.name}: {webRequest.error}");
            }
            downloadedAvatarsCount++;
            allAvatarsDownloaded();
            yield return null; // Yield to avoid blocking the main thread
        }
    }
}
