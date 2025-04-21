using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPanels; // Panels 0 to 5
    public GameObject singleEnemySpawnPoint;
    public GameObject[] multiEnemySpawnPoints1;
    public GameObject[] multiEnemySpawnPoints2;
    public GameObject enemyPrefab;

    private int currentPanel = 0;
    private bool canProceed = true;

    void Start()
    {
        ShowPanel(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && canProceed && currentPanel < tutorialPanels.Length)
        {
            canProceed = false;
            StartCoroutine(AdvanceTutorial());
        }
    }

    IEnumerator AdvanceTutorial()
    {
        // Hide current panel
        tutorialPanels[currentPanel].SetActive(false);
        currentPanel++;

        if (currentPanel < tutorialPanels.Length)
        {
            yield return new WaitForSeconds(5f);
            ShowPanel(currentPanel);

            // Panel-specific actions
            if (currentPanel == 2)
            {
                Instantiate(enemyPrefab, singleEnemySpawnPoint.transform.position, Quaternion.identity);
            }
            else if (currentPanel == 3)
            {
                foreach (GameObject point in multiEnemySpawnPoints1)
                {
                    Instantiate(enemyPrefab, point.transform.position, Quaternion.identity);
                }
            }
            else if (currentPanel == 4)
            {
                foreach (GameObject point in multiEnemySpawnPoints2)
                {
                    Instantiate(enemyPrefab, point.transform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            // Last panel was just hidden
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("levelone");
        }

        canProceed = true;
    }

    void ShowPanel(int index)
    {
        if (index >= 0 && index < tutorialPanels.Length)
        {
            tutorialPanels[index].SetActive(true);
        }
    }
}
