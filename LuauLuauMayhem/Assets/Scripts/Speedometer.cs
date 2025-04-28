using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerSpeedometer : MonoBehaviour
{
    public Rigidbody playerRigidbody; // Drag your player's Rigidbody here
    public TMP_Text speedText;             // Drag your SpeedText UI element here

    private void Update()
    {
        if (playerRigidbody != null && speedText != null)
        {
            float speedMps = playerRigidbody.linearVelocity.magnitude; // meters per second
            float speedMph = speedMps * 2.23694f; // 1 m/s = 2.23694 mph
            speedText.text = Mathf.RoundToInt(speedMph) + " MPH";
        }
    }
}
