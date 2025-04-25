using System;
using System.Net.Http;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MWManager : MonoBehaviour
{
    const string liveUrlToGet = "https://private-624120-softgamesassignment.apiary-mock.com/v2/magicwords";
    [SerializeField] private MWMenu mWMenu;

    private MagicWordsData magicWordsData;



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
		//level = 0;
		loadLevelsAsync((result) => {
			magicWordsData = JsonUtility.FromJson<MagicWordsData>( result );

            callback();
		} );
	}

    private void dataLoaded()
    {
        Debug.Log("Data loaded successfully!");
    }

	async void loadLevelsAsync(Action<string> callback)
	{
		var client = new HttpClient();
		var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, liveUrlToGet);
		var response = await client.SendAsync(request);
		response.EnsureSuccessStatusCode();
		var result =  await response.Content.ReadAsStringAsync();
		callback( result );
	}
    
    private void OnDestroy()
    {
        unsubscribeToUIEvents();
    }

    private void subscribeToUIEvents()
    {
        mWMenu.OnResetClicked += Reset;
        mWMenu.OnBackClicked += GoBackToMainMenu;
    }

    private void unsubscribeToUIEvents()
    {
        mWMenu.OnResetClicked -= Reset;
        mWMenu.OnBackClicked -= GoBackToMainMenu;
    }

    private void Reset()
    {
        throw new NotImplementedException();
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

}
