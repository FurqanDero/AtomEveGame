using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Room_01");
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}