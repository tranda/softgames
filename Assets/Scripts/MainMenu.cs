using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{

    public void ClickedAceOfShadows()
    {
        SceneManager.LoadScene("Ace of Shadows");
    }

    public void ClickedMagicWords()
    {
        SceneManager.LoadScene("Magic Words");
    }

    public void ClickedPhoenixFlame()
    {
        SceneManager.LoadScene("Phoneix Flame");
    }

}
