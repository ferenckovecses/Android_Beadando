using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue_Controller : MonoBehaviour
{
    //Adathordozók
	Queue<string> dialogues;
    List<string> commands = new List<string>() {"HealPlayer()", "Battle()"};
    Character speaker;
    
    //Vezérlők
    Game_Controller gameController;
    GameState_Controller gameState;

    //UI
    public GameObject dialogueUI;
    GameObject dialogueBox;

    void Start()
    {
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
        gameState = GameObject.Find("GameState").GetComponent<GameState_Controller>();

        dialogues = new Queue<string>();
        
        CreateBox();
        BoxDisplay(false);
    }

    //Létrehozza a dialógus képernyőt
    void CreateBox()
    {
        dialogueBox = Instantiate(dialogueUI, GameObject.Find("UI Canvas").transform);
        var nextButton = GameObject.Find("Dialogue_Button").GetComponent<Button>();
        nextButton.onClick.AddListener(delegate{DisplayNextSentence();}); 
    }

    //Megváltoztatja a dialógus képernyő láthatóságát
    void BoxDisplay(bool status)
    {
        dialogueBox.SetActive(status);
    }

    //Elindítja a megadott karakter dialógusát
    public void StartDialogue(Character character)
    {
    	speaker = character;
        dialogues.Clear();
        foreach(string sentence in speaker.dialogue.sentences)
        {
            dialogues.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    //Megjeleníti a következő dialógust a listán
    public void DisplayNextSentence()
    {
        if(dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = dialogues.Dequeue();

        //Ha az aktuális dialógust tartalmazza a command lista
        if(commands.Contains(sentence))
        {
            //Gyógyítás
            if(sentence == "HealPlayer()")
            {
                gameController.HealPlayer();
            }

            //NPC harc
            else if(sentence == "Battle()")
            {
                gameController.BattleWithNPC(speaker);
            }

            //Következő dialógus megjelenítése
            DisplayNextSentence();
        }
        //Jelenítse meg a beszélgetést
        else
        {
            BoxDisplay(true);
            GameObject.Find("Dialogue_Text").GetComponent<TMP_Text>().text = sentence;
            GameObject.Find("Dialogue_Name").GetComponent<TMP_Text>().text = speaker.characterName;
        }

    }

    //Visszatérés a játékba és a box eltüntetése
    public void EndDialogue()
    {
        gameState.ChangeGameState(GameState.Outworld);
        BoxDisplay(false);
    }

}
