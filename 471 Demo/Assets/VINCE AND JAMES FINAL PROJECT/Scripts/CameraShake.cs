using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private float shakeMagnitude = 0.1f;  // How much the screen will shake
    private float shakeDuration = 0f;

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPosition + (Random.insideUnitSphere * shakeMagnitude);
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    // Call this method to trigger a shake
    public void Shake(float magnitude, float duration)
    {
        originalPosition = transform.localPosition;
        shakeMagnitude = magnitude;
        shakeDuration = duration;
    }
}

