using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
	public Character character;
	bool actionRange;
    Data_Controller dataController;

    // Start is called before the first frame update
    void Start()
    {
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
        actionRange = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
        {
    		actionRange = true;
            dataController.interactableCharacter = this;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
    	{
    		actionRange = false;
            dataController.interactableCharacter = null;
    	}
    }

    public bool InRange()
    {
        return this.actionRange;
    }

}
