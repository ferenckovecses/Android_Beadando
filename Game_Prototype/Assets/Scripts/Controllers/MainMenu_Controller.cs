using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        GameObject.Find("Data").GetComponent<Data_Controller>().LoadGame();
        SceneManager.LoadScene("Game");
    }

}
