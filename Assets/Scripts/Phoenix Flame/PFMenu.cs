using System;
using UnityEngine;

public class PFMenu : MonoBehaviour
{
    public Action OnToggleClicked;
    public Action OnBackClicked;


    public void ClickedBack()
    {
        OnBackClicked?.Invoke();
    }


    public void ClickedToggle()
    {
        OnToggleClicked?.Invoke();
    }
}
