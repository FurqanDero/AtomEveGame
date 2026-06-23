using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string nextScene = "Room_02";
    private bool lastActiveState;

    void Start()
    {
        lastActiveState = gameObject.activeSelf;
    }

    void Update()
    {
        if (gameObject.activeSelf != lastActiveState)
        {
            Debug.Log("Door active state CHANGED to: " +
                      gameObject.activeSelf +
                      " at frame " + Time.frameCount);
            lastActiveState = gameObject.activeSelf;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Loading: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
    }
}