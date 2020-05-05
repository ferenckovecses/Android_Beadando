using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Character_Controller player;
    public Joystick joystick;

    Vector2 movement;

    //Input
    void Update()
    {

    	if(Input.touchCount > 0)
    	{
    		//movement.x = joystick
    	}
    	else
    	{
	        movement.x = Input.GetAxisRaw("Horizontal");
        	movement.y = Input.GetAxisRaw("Vertical");
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
