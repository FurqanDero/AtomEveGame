using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("HP Bar")]
    public RectTransform hpBarFill;
    private float hpBarMaxWidth;

    [Header("Room Info")]
    public TextMeshProUGUI roomText;

    [Header("Dash Indicator")]
    public Image dashIndicator;
    public TextMeshProUGUI dashText;

    private PlayerController playerController;

    void Start()
    {
        if (hpBarFill != null)
            hpBarMaxWidth = hpBarFill.sizeDelta.x;

        playerController = Object
            .FindAnyObjectByType<PlayerController>();

        // Set room text
        UpdateRoomText(
            UnityEngine.SceneManagement
            .SceneManager.GetActiveScene().name
        );
    }

    void Update()
    {
        UpdateDashIndicator();
    }

    public void UpdateHP(float current, float max)
    {
        if (hpBarFill == null) return;

        float ratio = current / max;
        hpBarFill.sizeDelta = new Vector2(
            hpBarMaxWidth * ratio,
            hpBarFill.sizeDelta.y
        );

        // Color shifts red as HP drops
        Image fill = hpBarFill.GetComponent<Image>();
        if (fill != null)
            fill.color = Color.Lerp(
                Color.red,
                new Color(0.4f, 0.86f, 1f),
                ratio
            );
    }

    public void UpdateRoomText(string sceneName)
    {
        if (roomText == null) return;

        // Convert scene name to display text
        switch (sceneName)
        {
            case "Room_01":
                roomText.text = "ROOM 1 / 5";
                break;
            case "Room_02":
                roomText.text = "ROOM 2 / 5";
                break;
            case "Room_03":
                roomText.text = "ROOM 3 / 5";
                break;
            case "Room_04":
                roomText.text = "ROOM 4 / 5";
                break;
            case "Room_Boss":
                roomText.text = "BOSS";
                break;
            default:
                roomText.text = sceneName;
                break;
        }
    }

    void UpdateDashIndicator()
    {
        if (dashIndicator == null) return;
        if (playerController == null) return;

        // Visual cooldown — grey when on cooldown
        bool dashReady = !playerController.IsDashing();
        dashIndicator.color = dashReady ?
            new Color(0.4f, 0.86f, 1f) :
            new Color(0.3f, 0.3f, 0.3f);

        if (dashText != null)
            dashText.text = dashReady ?
                "DASH" : "...";
    }
}