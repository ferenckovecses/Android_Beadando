using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter_Controller : MonoBehaviour
{

	public List<Encounter> encounterList;
	int safety = 0;
	public int safetyCap = 200;
	int steps = 0;
	public int encounterCap = 100;
	bool isSafetyOn = false;
	bool isInside = false;
	Player_Movement_Controller movement;
	Game_Controller gameController;

	//Növeli a lépésszámot, illetve 
	void IncreaseSteps()
	{
		//Ha nincs encounter védettségünk harc után
		if(!isSafetyOn)
		{
			this.steps += 1;
		}

		//Ha van védettségünk
		else
		{
			safety+=1;
			if(safety == safetyCap)
			{
				isSafetyOn = false;
				ResetSafety();
			}
		}
		
		//Ha elértük a szintet hogy encounter capünk legyen
		if(steps == encounterCap)
		{
			GetEncounterChance();
		}
	}

	//Generál egy számot ami alapján eldönti, hogy legyen-e encounter, vagy megússzuk
	void GetEncounterChance()
	{
		//Ha elértük az encounter capet, akkor esélyt kap hogy encounterünk legyen 
		int chance = (UnityEngine.Random.Range(1,100)) % 15;
		//Ha lesz encounter
		if(chance == 0)
		{
			int id = UnityEngine.Random.Range(0,(encounterList.Count - 1));
			gameController.BattleWithEncounter(GenerateEncounter(id));
			this.isSafetyOn = true;
		}

		ResetSteps();
	}

	public void ResetSteps()
	{
		this.steps = 0;
	}

	public void ResetSafety()
	{
		this.safety = 0;
	}

	Character GenerateEncounter(int id)
	{
		Character encounter = encounterList[id].enemy;
		int newLevel = UnityEngine.Random.Range(encounterList[id].minLvl, encounterList[id].maxLvl);
		Debug.Log(newLevel);
		encounter.SetLevel(newLevel);

		return encounter;
	}

	void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
        {
        	isInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
    	{
    		isInside = false;
    	}
    }

    void Start()
    {
    	movement = GameObject.Find("Movement").GetComponent<Player_Movement_Controller>();
    	gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
    }

	void Update()
	{
		//Ha a triggeren belül vagyunk és mozgunk
		if(isInside && movement.IsTheCharacterMoving())
		{
			IncreaseSteps();
		}
	}


}
