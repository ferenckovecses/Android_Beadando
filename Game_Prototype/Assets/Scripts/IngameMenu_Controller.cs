using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu_Controller : MonoBehaviour
{
	public GameObject menuPrefab;
	Game_Controller gameController;
	bool dispalyed = false;
	GameObject menuUI;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.currentState == GameState.IngameMenu && dispalyed == false)
        {
        	DisplayMenu();
        }
    }

    void DisplayMenu()
    {
		dispalyed = true;

		menuUI = Instantiate(menuPrefab, Vector3.zero, Quaternion.identity);

		var returnButton = GameObject.Find("Return_Button").GetComponent<Button>();
    	returnButton.onClick.AddListener(delegate{HideMenu();});

    	var exitButton = GameObject.Find("Exit_Button").GetComponent<Button>();
    	exitButton.onClick.AddListener(delegate{MainMenu();});

    	var saveButton = GameObject.Find("Save_Button").GetComponent<Button>();
    	saveButton.onClick.AddListener(delegate{SaveGame();});
    }

    public void HideMenu()
    {
    	Destroy(menuUI);
    	dispalyed = false;
    	gameController.currentState = GameState.Outworld;
    }

    public void SaveGame()
    {
    	Debug.Log("Save feature coming Soon, just like your momma!");
    }

    public void MainMenu()
    {
    	SaveGame();
    	dispalyed = false;
    	Destroy(menuUI);
    	gameController.needCleanup = true;
    	gameController.currentState = GameState.MainMenu;
    }
}
