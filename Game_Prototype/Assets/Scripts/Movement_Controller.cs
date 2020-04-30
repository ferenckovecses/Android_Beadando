using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Character_Controller player;
    public Joystick joystick;

    Vector2 movement;

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

    	if(Input.touchCount > 0)
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
    	}

    	else
    	{
    		if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
    		{
	        	movement.x = Input.GetAxisRaw("Horizontal");
	        	movement.y = 0;
    		}

    		else if(Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
    		{
    			movement.x = 0;
    			movement.y = Input.GetAxisRaw("Vertical");
    		}
    		else
    		{
    			movement.x = 0;
    			movement.y = 0;
    		}

    	}


        player.animator.SetFloat("Horizontal", movement.x);
        player.animator.SetFloat("Vertical", movement.y);
        player.animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    //Movement
    void FixedUpdate()
    {
    	player.rb.MovePosition(player.rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
