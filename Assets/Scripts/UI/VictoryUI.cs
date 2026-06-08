using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    public GameObject victoryPanel;

    void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void Show()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnPlayAgainPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Room_01");
    }

    public void OnMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}