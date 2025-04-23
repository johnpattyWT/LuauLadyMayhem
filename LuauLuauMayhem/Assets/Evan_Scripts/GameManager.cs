using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    public static Game instance;

    // 🔊 Event to notify other scripts when a kill is registered
    public event Action OnKillRegistered;

    [Header("Kill Counter")]
    public int globalKillCount = 0;
    public TMP_Text killCountText;

    [Header("Style Meter")]
    public int styleScore = 0;
    public string currentGrade = "F";
    public TMP_Text styleScoreText;
    public TMP_Text styleGradeText;
    public Slider styleScoreSlider;

    [Header("Kill Feed")]
    public TMP_Text killFeedText;
    private List<string> killMessages = new List<string>();
    private float killFeedTimer = 0f;
    private const float killFeedClearTime = 10f;

    private int comboKills = 0;
    private float comboTimer = 0f;
    private const float comboResetTime = 5f;

    public GameObject pausemenu;
    private bool isPaused = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip killClip;
    public AudioClip comboClip;
    public AudioClip rankUpClip;

    private string lastGrade = "F";

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        ResumeGame();
    }

    private void Update()
    {
        // Combo timer
        if (comboKills > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
                ResetCombo();
        }

        // Kill feed timer
        if (killMessages.Count > 0)
        {
            killFeedTimer += Time.deltaTime;
            if (killFeedTimer >= killFeedClearTime)
                ClearKillFeed();
        }

        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void RegisterKill()
    {
        globalKillCount++;
        killCountText.text = "x" + globalKillCount.ToString();

        comboKills++;
        comboTimer = 0f;

        int points = GetPointsForCombo(comboKills);
        styleScore += points;

        // 🔊 Normal kill sound
        if (audioSource && killClip)
            audioSource.PlayOneShot(killClip);

        UpdateGrade();
        AddKillMessage(comboKills);
        killFeedTimer = 0f;

        // 🔊 Combo sound
        if (comboKills >= 2 && audioSource && comboClip)
            audioSource.PlayOneShot(comboClip);

        // ✅ Broadcast the kill event
        OnKillRegistered?.Invoke();
    }

    private int GetPointsForCombo(int combo)
    {
        switch (combo)
        {
            case 1: return 100;
            case 2: return 200;
            case 3: return 300;
            case 4: return 400;
            case 5: return 500;
            case 6: return 1000;
            default: return 100;
        }
    }

    private void UpdateGrade()
    {
        string previousGrade = currentGrade;

        if (styleScore >= 5000) currentGrade = "S";
        else if (styleScore >= 3000) currentGrade = "A";
        else if (styleScore >= 2000) currentGrade = "B";
        else if (styleScore >= 1000) currentGrade = "C";
        else if (styleScore >= 500) currentGrade = "D";
        else currentGrade = "F";

        if (currentGrade != previousGrade && audioSource && rankUpClip)
            audioSource.PlayOneShot(rankUpClip);
    }

    private void UpdateUI()
    {
        styleScoreText.text = styleScore.ToString();
        styleGradeText.text = currentGrade;

        if (styleScoreSlider != null)
        {
            styleScoreSlider.maxValue = 5000;
            styleScoreSlider.value = Mathf.Clamp(styleScore, 0, 5000);
        }

        killFeedText.text = string.Join("\n", killMessages.ToArray());
    }

    private void AddKillMessage(int combo)
    {
        string message = combo switch
        {
            2 => "Double Kill!",
            3 => "Triple Kill!",
            4 => "Quad Kill!",
            5 => "5 Kill Streak!",
            6 => "Unreal!!!",
            _ => combo == 1 ? "Kill!" : null
        };

        if (message != null)
        {
            if (killMessages.Count >= 3)
                killMessages.RemoveAt(0);

            killMessages.Add(message);
        }
    }

    private void ClearKillFeed()
    {
        killMessages.Clear();
        killFeedText.text = "";
        killFeedTimer = 0f;
    }

    private void ResetCombo()
    {
        comboKills = 0;
        comboTimer = 0f;
    }

    public void PauseGame()
    {
        if (pausemenu != null)
            pausemenu.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausemenu != null)
            pausemenu.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void ShowPanel()
    {
        if (pausemenu != null)
            pausemenu.SetActive(true);
    }

    public void HidePanel()
    {
        if (pausemenu != null)
            pausemenu.SetActive(false);
    }
}