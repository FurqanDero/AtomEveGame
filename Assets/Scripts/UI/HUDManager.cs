using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("HP Bar")]
    public RectTransform hpBarFill;
    private float hpBarMaxWidth;

    [Header("Room Info")]
    public TMPro.TextMeshProUGUI roomText;

    void Start()
    {
        if (hpBarFill != null)
            hpBarMaxWidth = hpBarFill.sizeDelta.x;
    }

    public void UpdateHP(float current, float max)
    {
        if (hpBarFill == null) return;

        float ratio = current / max;
        hpBarFill.sizeDelta = new Vector2(
            hpBarMaxWidth * ratio,
            hpBarFill.sizeDelta.y
        );
    }

    public void UpdateRoomText(string text)
    {
        if (roomText != null)
            roomText.text = text;
    }
}