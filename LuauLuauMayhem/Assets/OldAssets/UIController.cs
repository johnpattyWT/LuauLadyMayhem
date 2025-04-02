using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {



    // Start is called before the first frame update
    private void Start()
    {
       
    }


    // Function to fade in the Title Screen at the start

    public void OnClickQuitButton()
    {
        print("Quit button was clicked");
        Application.Quit();
    }
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("LevelOne");
    }
  
    public void OnClickHelp()
    {
        SceneManager.LoadScene("HELP");
    }
    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu 1");
    }
    public void OnClickContinue()
    {
        SceneManager.LoadScene("LevelTwo");
    }

}
