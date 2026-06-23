using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [Header("Room Settings")]
    public string nextRoomScene = "Room_02";

    [Header("References")]
    public GameObject doorObject;

    private int totalEnemies = 0;
    private int defeatedEnemies = 0;
    private bool roomCleared = false;

    void Start()
    {
        totalEnemies = GameObject
            .FindGameObjectsWithTag("Enemy").Length;

        Debug.Log("Room enemies: " + totalEnemies);

        // Door starts OFF — disable the whole GameObject
        if (doorObject != null)
            doorObject.SetActive(false);
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

        if (doorObject != null)
        {
            doorObject.SetActive(true);
            Debug.Log("Door active state: " +
                      doorObject.activeSelf);
        }
    }
}