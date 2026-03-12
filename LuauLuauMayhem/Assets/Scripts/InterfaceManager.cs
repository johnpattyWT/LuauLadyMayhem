using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text killText;
    public TMP_Text speedText;
    public TMP_Text timerText;
    public TMP_Text gradeText;
    public Slider styleSlider;
    public Rigidbody playerRb;

    [Header("Panels")]
    public GameObject pauseMenu;
    public GameObject resultsMenu;

    private void Update()
    {
        if (playerRb)
        {
            float mph = playerRb.linearVelocity.magnitude * 2.23694f;
            speedText.text = $"{Mathf.RoundToInt(mph)} MPH";
        }

        if (GameCore.Instance.isTimerRunning)
            timerText.text = FormatTime(GameCore.Instance.currentTime);

        killText.text = $"Kills: {GameCore.Instance.globalKills}";
        gradeText.text = GameCore.Instance.currentGrade;
    }

    public void TogglePause()
    {
        bool paused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    private string FormatTime(float t)
    {
        int min = Mathf.FloorToInt(t / 60);
        int sec = Mathf.FloorToInt(t % 60);
        int ms = Mathf.FloorToInt((t * 100) % 100);
        return $"{min:00}:{sec:00}:{ms:00}";
    }
}