using UnityEngine;

public class MaulerBossManager : MonoBehaviour
{
    private MaulerTwin[] twins;
    private int twinsDefeated = 0;

    void Start()
    {
        twins = Object
    .FindObjectsByType<MaulerTwin>(
        FindObjectsInactive.Exclude);

        Debug.Log("Mauler Twins found: " +
                  twins.Length);
    }

    public void OnTwinDefeated(MaulerTwin defeatedTwin)
    {
        twinsDefeated++;
        Debug.Log("Twins defeated: " + twinsDefeated +
                  "/2");

        if (twinsDefeated >= 2)
        {
            TriggerVictory();
            return;
        }

        // Enrage the surviving twin
        foreach (MaulerTwin twin in twins)
        {
            if (twin != null &&
                twin != defeatedTwin &&
                twin.currentState !=
                MaulerTwin.TwinState.Death)
            {
                twin.EnableEnrage();
            }
        }
    }

    void TriggerVictory()
    {
        Debug.Log("BOTH TWINS DEFEATED — VICTORY!");

        VictoryUI victoryUI =
            Object.FindAnyObjectByType<VictoryUI>();
        if (victoryUI != null)
            victoryUI.Show();
    }
}