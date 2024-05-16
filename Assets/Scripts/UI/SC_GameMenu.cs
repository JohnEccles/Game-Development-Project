using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

// Source https://www.sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas 
public class SC_GameMenu : MonoBehaviour
{
    public GameObject GameMenu;

    
    // Start is called before the first frame update
    void Start()
    {
        ResumeButton();
    }


    public void GameMenuButton()
    {
        // Show Main Menu
        GameMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        GameMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}