using UnityEngine;
using UnityEngine.SceneManagement;

public class PfManager : MonoBehaviour
{
    [SerializeField] private PFMenu pfMenu;
    [SerializeField] private Animator fireAnimator;
    private bool isFireOn = false;


    void Start()
    {
        subscribeToUIEvents();
        ToggleFire();
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
        isFireOn = !isFireOn;
        fireAnimator.SetBool("active", isFireOn);
    }
}
