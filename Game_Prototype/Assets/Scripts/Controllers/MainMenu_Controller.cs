using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour
{

    public void StartGame()
    {
        GameObject.Find("GameState").GetComponent<GameState_Controller>().ChangeGameState(GameState.CharacterCreation);
        SceneManager.LoadScene("CharacterCreation");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        GameObject.Find("GameState").GetComponent<GameState_Controller>().ChangeGameState(GameState.LoadGame);
        GameObject.Find("Data").GetComponent<Data_Controller>().LoadGame();
        SceneManager.LoadScene("Game");
    }

}
