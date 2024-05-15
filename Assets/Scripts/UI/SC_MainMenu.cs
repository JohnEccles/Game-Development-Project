using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source https://www.sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas 
public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject LevelMenu;

    [SerializeField]
    string loadScene;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene);
    }

    public void PlayTutorial()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialLevel");
    }

    public void PlayLevel1()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        LevelMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        LevelMenu.SetActive(false);
    }

    public void LevelButton()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        LevelMenu.SetActive(true);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}