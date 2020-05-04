using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
	Character character;
	bool talk;
    // Start is called before the first frame update
    void Start()
    {
        talk = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(talk && Input.GetKeyDown(KeyCode.Space))
        {
        	Debug.Log("Szeretlek Lilla! <3 ");
        }
    }

    public void StartDialog()
    {

    }

    public void StartBattle()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
    		talk = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
    	{
    		talk = false;
    	}
    }
}
