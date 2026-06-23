using UnityEngine;

public class CameraShakeBoss : MonoBehaviour
{
    private Vector3 originalPos;
    private float shakeTimer;
    private float shakeIntensity;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            transform.localPosition = originalPos +
                (Vector3)Random.insideUnitCircle *
                shakeIntensity;

            if (shakeTimer <= 0)
                transform.localPosition = originalPos;
        }
    }

    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeTimer = duration;
    }
}