using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndUIManager : MonoBehaviour
{
    [Header("Primary Panel Pair")]
    public GameObject panelToToggle;
    public GameObject panelToUntoggle;

    [Header("Secondary Panel Pair")]
    public GameObject secondaryPanelToToggle;
    public GameObject secondaryPanelToUntoggle;

    /// <summary>
    /// Reloads the current active scene.
    /// </summary>
    public void ResetCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1f; // Resume time

    }

    /// <summary>
    /// Loads a scene by its name.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Toggles the primary panel and untoggles the assigned secondary one.
    /// </summary>
    public void TogglePanel()
    {
        if (panelToToggle != null)
        {
            bool shouldActivate = !panelToToggle.activeSelf;
            panelToToggle.SetActive(shouldActivate);
        }

        if (panelToUntoggle != null)
        {
            panelToUntoggle.SetActive(false);
        }
    }

    /// <summary>
    /// Toggles the secondary panel and untoggles its paired one.
    /// </summary>
    public void ToggleSecondaryPanel()
    {
        if (secondaryPanelToToggle != null)
        {
            bool shouldActivate = !secondaryPanelToToggle.activeSelf;
            secondaryPanelToToggle.SetActive(shouldActivate);
        }

        if (secondaryPanelToUntoggle != null)
        {
            secondaryPanelToUntoggle.SetActive(false);
        }
    }
}
 