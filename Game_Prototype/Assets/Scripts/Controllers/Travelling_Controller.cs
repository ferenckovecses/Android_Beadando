using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travelling_Controller : MonoBehaviour
{
	bool actionRange;
	public int mapID;
	public string spawnPointName;
	Data_Controller dataController;
	Game_Controller gameController;

    // Start is called before the first frame update
    void Start()
    {
    	gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
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
        	gameController.currentState = GameState.Travelling;
        	dataController.ChangeLevelId(this.mapID);
        	gameController.ChangeLevel(this.spawnPointName);
        	gameController.currentState = GameState.Outworld;
        }
    }

    public bool InRange()
    {
        return this.actionRange;
    }
}
