using System;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class AOSMenu : MonoBehaviour
{
    public Action OnResetClicked;
    public Action OnBackClicked;


    public void ClickedBack()
    {
        OnBackClicked?.Invoke();
    }


    public void ClickedReset()
    {
        OnResetClicked?.Invoke();
    }
}
