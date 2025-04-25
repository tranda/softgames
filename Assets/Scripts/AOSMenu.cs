using UnityEngine;
using UnityEngine.SceneManagement;

public class AOSMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ClickBack()
    {
        Debug.Log("Back clicked!");
        SceneManager.LoadScene("MainScene");
    }
}
