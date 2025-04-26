using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleScreen;
    public GameObject Main_Menu;
    public CanvasGroup titleCanvasGroup;     // CanvasGroup for the Title Screen fade
    public CanvasGroup mainMenuCanvasGroup;  // CanvasGroup for the Main Menu fade
    public AudioSource audioSource;          // AudioSource for playing sound
    public AudioClip fadeOutSound;           // Audio clip for fade-out sound
    public float fadeDuration = 1f;          // Duration of the fade effect
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure the TitleScreen fades in when the game starts
        TitleScreen.SetActive(true);
        Main_Menu.SetActive(false);
        StartCoroutine(FadeInTitleScreen());
    }
    private IEnumerator FadeInTitleScreen()
    {
        float timeElapsed = 0f;

        // Start with the TitleScreen fully transparent
        titleCanvasGroup.alpha = 0f;

        // Fade in over time
        while (timeElapsed < fadeDuration)
        {
            titleCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the Title Screen is fully visible
        titleCanvasGroup.alpha = 1f;
    }

    // This function will be called when a button is clicked to start fading out the Title Screen
    public void FadeOutTitleScreen()
    {
        // Play the fade-out sound if it's set
        if (fadeOutSound != null)
        {
            audioSource.PlayOneShot(fadeOutSound);
        }

        // Start fading the Title Screen out
        StartCoroutine(FadeOutCoroutine());
    }

    // Coroutine to fade out the Title Screen
    private IEnumerator FadeOutCoroutine()
    {
        float timeElapsed = 0f;
        float startAlpha = titleCanvasGroup.alpha;  // Store initial alpha value

        // Fade out the Title Screen
        while (timeElapsed < fadeDuration)
        {
            titleCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the Title Screen is fully transparent and inactive
        titleCanvasGroup.alpha = 0f;
        TitleScreen.SetActive(false);

        // After fading out the Title Screen, activate and fade in the Main Menu
        StartCoroutine(FadeInMainMenu());
    }

    // Coroutine to fade in the Main Menu after the Title Screen is faded out
    private IEnumerator FadeInMainMenu()
    {
        Main_Menu.SetActive(true);  // Activate the Main Menu

        float timeElapsed = 0f;

        // Start with the Main Menu fully transparent
        mainMenuCanvasGroup.alpha = 0f;

        // Fade in the Main Menu
        while (timeElapsed < fadeDuration)
        {
            mainMenuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the Main Menu is fully visible
        mainMenuCanvasGroup.alpha = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
