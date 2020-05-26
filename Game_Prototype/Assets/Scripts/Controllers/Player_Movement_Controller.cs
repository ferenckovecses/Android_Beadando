using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Controller : MonoBehaviour
{
    float moveSpeed = 5f;
    public Character_Controller player;

    public Joystick joystick;

    Vector2 movement;

    Game_Controller gameController;
    Data_Controller dataController;
    public void AddPlayer(Character_Controller player)
    {
        this.player = player;
    }

    public void AddJoystick(Joystick joystick)
    {
        this.joystick = joystick;
    }

    //Input
    void Update()
    {

		if(joystick.Horizontal >= 0.2 || joystick.Horizontal <= -0.2)
		{
        	movement.x = joystick.Horizontal;
        	movement.y = 0;
		}

		else if(joystick.Vertical >= 0.2 || joystick.Vertical <= -0.2)
		{
			movement.x = 0;
			movement.y = joystick.Vertical;
		}
        
		else
		{
			movement.x = 0;
			movement.y = 0;
		}		

        if(gameController.currentState == GameState.Outworld)
        {
            player.animator.SetFloat("Horizontal", movement.x);
            player.animator.SetFloat("Vertical", movement.y);
            player.animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    //Movement
    void FixedUpdate()
    {
        if(gameController.currentState == GameState.Outworld)
    	   player.rb.MovePosition(player.rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Start()
    {
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
    }

    public void ActionButton()
    {
        if(dataController.interactableCharacter != null 
            && dataController.interactableCharacter.InRange() 
            && gameController.currentState != GameState.Dialogue)
        {
            gameController.currentState = GameState.Dialogue;
            dataController.interactableCharacter.character.TriggerDialogue();
        }
    }
}
