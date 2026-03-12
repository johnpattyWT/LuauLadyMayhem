using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameCore : MonoBehaviour
{
    public static GameCore Instance { get; private set; }

    [Header("Stats")]
    public int globalKills;
    public int styleScore;
    public string currentGrade = "F";
    public event Action OnKill;

    [Header("Timer")]
    public float currentTime;
    public bool isTimerRunning;
    private List<float> _leaderboard = new List<float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (isTimerRunning) currentTime += Time.deltaTime;
    }

    public void RegisterKill(int points, string grade)
    {
        globalKills++;
        styleScore += points;
        currentGrade = grade;
        OnKill?.Invoke();
    }

    public void SaveAndResetTimer()
    {
        isTimerRunning = false;
        _leaderboard.Add(currentTime);
        _leaderboard.Sort();
        currentTime = 0;
    }

    public void ChangeScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}