using UnityEngine;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    public TMP_Text killText;
    public TMP_Text scoreText;
    public TMP_Text gradeText;

    void Start()
    {
        killText.text = "Kills: " + GameResults.LastKillCount;
        scoreText.text = "Style Score: " + GameResults.LastStyleScore;
        gradeText.text = "" + GameResults.LastStyleGrade;
    }
}
