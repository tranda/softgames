using System;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class MWMenu : MonoBehaviour
{
    public Action OnNextClicked;
    public Action OnResetClicked;
    public Action OnBackClicked;


    public void ClickedBack()
    {
        OnBackClicked?.Invoke();
    }

    public void ClickedNext()
    {
        OnNextClicked?.Invoke();
    }

    public void ClickedReset()
    {
        OnResetClicked?.Invoke();
    }
}
