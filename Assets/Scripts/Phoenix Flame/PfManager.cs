using UnityEngine;
using UnityEngine.SceneManagement;

public class PfManager : MonoBehaviour
{
    [SerializeField] private PFMenu pfMenu;



    void Start()
    {
        subscribeToUIEvents();
    }

    private void OnDestroy()
    {
        unsubscribeToUIEvents();
    }

    private void subscribeToUIEvents()
    {
        pfMenu.OnToggleClicked += ToggleFire;
        pfMenu.OnBackClicked += GoBackToMainMenu;
    }

    private void unsubscribeToUIEvents()
    {
        pfMenu.OnToggleClicked -= ToggleFire;
        pfMenu.OnBackClicked -= GoBackToMainMenu;
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void ToggleFire()
    {
        // Implement the logic to toggle the fire effect on or off
        Debug.Log("Fire effect toggled.");
    }
}
