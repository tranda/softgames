using UnityEngine;
using UnityEngine.SceneManagement;

public class AOSMenu : MonoBehaviour
{
    [SerializeField] private AOSManager aOSManager;



    public void ClickedBack()
    {
        SceneManager.LoadScene("MainScene");
    }


    public void ClickedReset()
    {
        aOSManager.ResetDeck();
    }
}
