using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TMP_Text timerText;   // Drag your Timer UI Text here
    public bool isRunning = false;
    private float currentTime = 0f;

    private void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime % 60F);
        int milliseconds = Mathf.FloorToInt((currentTime * 100F) % 100F);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void StartTimer()
    {
        currentTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
        LeaderboardManager.Instance.SaveTime(currentTime);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
