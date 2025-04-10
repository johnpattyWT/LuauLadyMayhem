using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    public static Game instance;

    [Header("Kill Counter")]
    public int globalKillCount = 0;
    public TMP_Text killCountText; // Drag the UI Text element (TextMeshProUGUI) here in the Inspector

    private void Awake()
    {
        // Ensure only one instance exists.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterKill()
    {
        globalKillCount++;
        if (killCountText != null)
            killCountText.text = globalKillCount.ToString();
    }
}