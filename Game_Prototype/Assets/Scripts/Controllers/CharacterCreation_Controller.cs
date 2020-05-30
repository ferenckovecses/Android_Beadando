using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterCreation_Controller : MonoBehaviour
{
    //Adathordozók
	string[] signList;
	Data_Controller dataController;
	int signID;
	int spriteID;

    //UI elemek
	TMP_Text sign;
	InputField nameField;
    GameObject firstPanel;
    GameObject secondPanel;
    Image spriteImage;

    void Start()
    {
        //Adatvezérlő referálása
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();

        //Változók alapértelmezése
        signID = 0;
        spriteID = 0;
        signList = new string[12] {"Aries","Leo","Sagittarius","Gemini","Libra","Aquarius","Cancer",
        "Scorpio","Pisces","Taurus","Virgo","Capricorn"};

        //Dinamikus UI szegmensek referálása
        firstPanel = GameObject.Find("First_Part");
        spriteImage = GameObject.Find("Character_Image").GetComponent<Image>();
        nameField = GameObject.Find("Name_Field").GetComponent<InputField>();

        secondPanel = GameObject.Find("Second_Part");
        sign = GameObject.Find("Sign_Text").GetComponent<TMP_Text>();
        secondPanel.SetActive(false);


        //Elemek alapértékeinek megjelenítése
        RefreshSprite();
        RefreshName();
    }

    //Lapozás a létrehozás képernyők között
    public void ContinueCreation()
    {
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
        RefreshSign();
    }

    //Frissíti a sprite képet
    void RefreshSprite()
    {
    	spriteImage.sprite = dataController.player[spriteID].character.GetSprite();
    }

    //Frissíti a csillagjegy textet
    void RefreshSign()
    {
    	sign.text = signList[signID];
    }

    //Default nevek megjelenítése placeholdernek
    void RefreshName()
    {
    	switch(spriteID)
    	{
    		case 0: nameField.text = "James"; break;
    		case 1: nameField.text = "Jane"; break;
    		default: nameField.text = " "; break;
    	}
    }

    //Csillagjegy/Elemválasztó 
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

    //Sprite választó
    public void ModifySprite(int input)
    {
    	if(spriteID+input >= dataController.player.Count)
    		spriteID = 0;
		else if(spriteID+input < 0)
			spriteID = dataController.player.Count-1;
		else
			spriteID += input;

		RefreshSprite();
		RefreshName();
    }

    //A csillagjegyből elemental ID-t ad vissza
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

    //Karakter adatainak eltárolása az adattárba és átlépés a játék scene-be
    public void CreateCharacter()
    {
        dataController.SetCharacter(spriteID,nameField.text,GetElementId(signID));
        SceneManager.LoadScene("Game");
    }


}
