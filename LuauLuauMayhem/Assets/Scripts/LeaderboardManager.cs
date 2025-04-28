using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private List<float> bestTimes = new List<float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveTime(float time)
    {
        bestTimes.Add(time);
        bestTimes.Sort(); // sorts ascending (smallest time first)

        Debug.Log("Time saved! Current Leaderboard:");
        foreach (float t in bestTimes)
        {
            Debug.Log(FormatTime(t));
        }
    }

    public List<float> GetTopTimes(int count = 5)
    {
        return bestTimes.GetRange(0, Mathf.Min(count, bestTimes.Count));
    }

    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}
