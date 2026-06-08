using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [Header("Room Settings")]
    public string nextRoomScene = "Room_02";
    public bool isBossRoom = false;

    [Header("References")]
    public GameObject doorObject;

    private int totalEnemies = 0;
    private int defeatedEnemies = 0;
    private bool roomCleared = false;

    void Start()
    {
        // Count all enemies in room
        totalEnemies = GameObject
            .FindGameObjectsWithTag("Enemy").Length;

        Debug.Log("Room enemies: " + totalEnemies);

        // Close door at start
        if (doorObject != null)
            doorObject.SetActive(true);
    }

    public void EnemyDefeated()
    {
        defeatedEnemies++;
        Debug.Log("Defeated: " + defeatedEnemies +
                  "/" + totalEnemies);

        if (defeatedEnemies >= totalEnemies &&
            !roomCleared)
        {
            roomCleared = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Debug.Log("Room cleared! Door opening...");

        // Enable the door trigger
        if (doorObject != null)
            doorObject.SetActive(true);
    }

    void LoadNextRoom()
    {
        if (!string.IsNullOrEmpty(nextRoomScene))
            SceneManager.LoadScene(nextRoomScene);
    }
}