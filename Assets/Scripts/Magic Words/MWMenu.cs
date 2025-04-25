using System;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class MWMenu : MonoBehaviour
{
    public Action OnResetClicked;
    public Action OnBackClicked;


    public void ClickedBack()
    {
        // SceneManager.LoadScene("MainScene");
        OnBackClicked?.Invoke();
    }


    public void ClickedReset()
    {
        // aOSManager.ResetDeck();
        OnResetClicked?.Invoke();
    }
}
