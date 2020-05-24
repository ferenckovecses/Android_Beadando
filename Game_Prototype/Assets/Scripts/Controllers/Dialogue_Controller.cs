using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue_Controller : MonoBehaviour
{
	public Queue<string> dialogues;
	public GameObject dialogueUI;
    GameObject dialogueBox;
    public List<string> commands;
    Character speaker;
    Game_Controller gameController;


    // Start is called before the first frame update
    void Start()
    {
        commands = new List<string>() {"HealPlayer()", "Battle()"};
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
        dialogues = new Queue<string>();
        dialogueBox = Instantiate(dialogueUI, GameObject.Find("Outworld_Canvas").transform);
        var nextButton = GameObject.Find("Dialogue_Button").GetComponent<Button>();
        nextButton.onClick.AddListener(delegate{DisplayNextSentence();}); 
        dialogueBox.SetActive(false);
    }

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

    public void DisplayNextSentence()
    {
        if(dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = dialogues.Dequeue();

        if(commands.Contains(sentence))
        {
            if(sentence == "HealPlayer()")
            {
                gameController.HealPlayer();
                sentence = dialogues.Dequeue();
                GameObject.Find("Dialogue_Text").GetComponent<TMP_Text>().text = sentence;
            }
            else if(sentence == "Battle()")
            {
                gameController.BattleWithNPC(speaker);
                sentence = dialogues.Dequeue();
                GameObject.Find("Dialogue_Text").GetComponent<TMP_Text>().text = sentence;

            }
        }
        //Jelenítse meg a beszélgetést
        else
        {
            dialogueBox.SetActive(true);
            GameObject.Find("Dialogue_Text").GetComponent<TMP_Text>().text = sentence;
            GameObject.Find("Dialogue_Name").GetComponent<TMP_Text>().text = speaker.characterName;
        }

    }

    public void EndDialogue()
    {
        gameController.currentState = GameState.Outworld;
        dialogueBox.SetActive(false);
    }

}
