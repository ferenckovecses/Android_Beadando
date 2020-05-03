using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character_Creation : MonoBehaviour
{
	public GameObject UI_Prefab;
	public string[] signList;
	GameObject firstPanel;
	GameObject secondPanel;
	Game_Controller gameController;
	Data_Controller dataController;
	GameObject UI;
	Image spriteImage;
	bool needToCreate;

	int signID;
	int spriteID;
	TMP_Text sign;
	InputField nameField;

    // Start is called before the first frame update
    void Start()
    {
    	needToCreate = true;
        //Szükséges kontrollerek referálása
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(gameController.currentState == GameState.Character_Creation && needToCreate)
    	{
    		needToCreate = false;
    		Init();
    	}
    }

    public void ContinueCreation()
    {
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
    }

    public void CreateCharacter()
    {
    	dataController.SetCharacter(spriteID,nameField.text,GetElementId(signID));
    	Destroy(UI);
    	needToCreate = true;
    	gameController.currentState = GameState.World_Creation;
    }

    public void Init()
    {
    	signID = 0;
    	spriteID = 0;

    	//UI létrehozása prefabból
    	UI = Instantiate(UI_Prefab, Vector3.zero, Quaternion.identity);

    	//UI szegmensek megszerzése
        firstPanel = GameObject.Find("First_Part");
        secondPanel = GameObject.Find("Second_Part");

        spriteImage = GameObject.Find("Character_Image").GetComponent<Image>();
        sign = GameObject.Find("Sign_Text").GetComponent<TMP_Text>();
        nameField = GameObject.Find("Name_Field").GetComponent<InputField>();

        //Gombok referálása
        var continueBtn = GameObject.Find("Continue_Button").GetComponent<Button>();
		var spriteNext = GameObject.Find("Next_Sprite_Button").GetComponent<Button>();
		var spritePrev = GameObject.Find("Prev_Sprite_Button").GetComponent<Button>();
		var createCharacter = GameObject.Find("Create_Button").GetComponent<Button>();
		var signNext = GameObject.Find("Next_Sign_Button").GetComponent<Button>();
		var signPrev = GameObject.Find("Prev_Sign_Button").GetComponent<Button>();

		//Gomb eventek beállítása
		continueBtn.onClick.AddListener(delegate{ContinueCreation();});
		createCharacter.onClick.AddListener(delegate{CreateCharacter();});

		signNext.onClick.AddListener(delegate{ModifySign(1);});
		signPrev.onClick.AddListener(delegate{ModifySign(-1);});

		spriteNext.onClick.AddListener(delegate{ModifySprite(1);});
		spritePrev.onClick.AddListener(delegate{ModifySprite(-1);});

		RefreshSign();
		RefreshSprite();
		RefreshName();

        //Második szegmens eltüntetése
        secondPanel.SetActive(false);
    }

    void RefreshSprite()
    {
    	spriteImage.sprite = dataController.characterSprites[spriteID];
    }

    void RefreshSign()
    {
    	sign.text = signList[signID];
    }

    void RefreshName()
    {
    	switch(spriteID)
    	{
    		case 0: nameField.text = "James"; break;
    		case 1: nameField.text = "Jane"; break;
    		default: nameField.text = " "; break;
    	}
    }

    public void ModifySign(int input)
    {
    	if(signID+input >= signList.Length)
    		signID = 0;
		else if(signID+input < 0)
			signID = signList.Length-1;
		else
			signID += input;

		RefreshSign();
    }

    public void ModifySprite(int input)
    {
    	if(spriteID+input >= dataController.characterSprites.Count)
    		spriteID = 0;
		else if(spriteID+input < 0)
			spriteID = dataController.characterSprites.Count-1;
		else
			spriteID += input;

		RefreshSprite();
		RefreshName();
    }

    int GetElementId(int id)
    {
    	if(id >= 0 && id <= 2)
    		return 0;
    	else if(id >= 3 && id <= 5)
    		return 1;
    	else if(id >= 6 && id <= 8)
    		return 2;
    	else if(id >= 9 && id <= 11)
    		return 3;
		else
			return 0;
    }


}
