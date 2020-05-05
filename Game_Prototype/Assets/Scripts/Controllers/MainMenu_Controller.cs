using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Controller : MonoBehaviour
{
	public GameObject menuPrefab;
	Game_Controller gameController;

	GameObject menuUI;
    // Start is called before the first frame update
    void Start()
    {
    	SetupUI();
    	gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
    	//Ha a menü stateben vagyunk és nem aktív a menü jelenítse meg
    	if(gameController.currentState == GameState.MainMenu && !(menuUI.activeSelf))
    		DisplayMenu();

    	//Ha aktív a menü és nem vagyunk a menü státuszban tüntesse el.
    	else if(gameController.currentState != GameState.MainMenu && (menuUI.activeSelf))
        	HideMenu();
    }

    void DisplayMenu()
    {
    	menuUI.SetActive(true);
    }

    void HideMenu()
    {
    	//Hide Menu elements
    	menuUI.SetActive(false);
    }

    public void StartGame()
    {
    	gameController.currentState = GameState.Character_Creation;
    }

    public void ExitGame()
    {
    	Application.Quit();
    }

    public void LoadGame()
    {
    	Debug.Log("Load Game feature coming soon with a very expensive DLC");
    }

    void SetupUI()
    {
    	menuUI = Instantiate(menuPrefab, Vector3.zero, Quaternion.identity);

    	var startButton = GameObject.Find("Start_Button").GetComponent<Button>();
    	startButton.onClick.AddListener(delegate{StartGame();});

    	var exitButton = GameObject.Find("Exit_Button").GetComponent<Button>();
    	exitButton.onClick.AddListener(delegate{ExitGame();});

    	var loadButton = GameObject.Find("Load_Button").GetComponent<Button>();
    	loadButton.onClick.AddListener(delegate{LoadGame();});
    }

}
