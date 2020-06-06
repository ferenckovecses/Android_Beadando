using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IngameMenu_Controller : MonoBehaviour
{
	public GameObject menuPrefab;
    GameState_Controller gameState;
	bool dispalyed = false;
	GameObject menuUI;

    //Kreálunk egy GameState referenciát
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState_Controller>();
    }

    void Update()
    {
        //Ha menü fázisban vagyunk és még nincs megjelenítve a menü
        if(gameState.GetGameState() == GameState.IngameMenu && dispalyed == false)
        {
        	DisplayMenu();
        }
    }

    //Megjeleníti a menüt
    void DisplayMenu()
    {
        dispalyed = true;

        menuUI = Instantiate(menuPrefab, Vector3.zero, Quaternion.identity);

        SetupButtons();
		
    }

    //Gombok és onclick eventek beállítása
    void SetupButtons()
    {
        var returnButton = GameObject.Find("Return_Button").GetComponent<Button>();
        returnButton.onClick.AddListener(delegate{BackToGame();});

        var exitButton = GameObject.Find("Exit_Button").GetComponent<Button>();
        exitButton.onClick.AddListener(delegate{MainMenu();});

        var saveButton = GameObject.Find("Save_Button").GetComponent<Button>();
        saveButton.onClick.AddListener(delegate{SaveGame();});
    }

    //Eltünteti a menüt
    public void HideMenu()
    {
    	Destroy(menuUI);
    	dispalyed = false;
    }

    //Visszalép a játékba
    public void BackToGame()
    {
        HideMenu();
        gameState.ChangeGameState(GameState.Outworld);
    }

    //Elmenti a játékot
    public void SaveGame()
    {
    	GameObject.Find("Game").GetComponent<Game_Controller>().SaveGame();
    }

    //Visszalép a főmenübe miután feltakarított maga után
    public void MainMenu()
    {
        HideMenu();
        Destroy(GameObject.Find("Data"));
        Destroy(GameObject.Find("GameState"));
    	SceneManager.LoadScene("MainMenu");
    }
}
