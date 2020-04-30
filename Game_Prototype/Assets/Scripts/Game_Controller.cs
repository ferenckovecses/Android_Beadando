using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    //Level parent és outworld UI
	public GameObject Outworld;
    public GameObject MovementController;
    public GameObject battleControllerPrefab;
    public Data_Controller dataController;

	bool battlebool = false;
	bool worldbool = true;

    GameObject battleController;
    Character_Controller player;
    GameObject level;
    Character enemy;

    void Start()
    {
        Init();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
        	worldbool = !worldbool;
        	battlebool = !battlebool;

        	Outworld.SetActive(worldbool);

        	if(battlebool)
        	{
                enemy = Instantiate(dataController.enemyList[0]) as Character;
                battleController = Instantiate(battleControllerPrefab, Vector3.zero, Quaternion.identity);
                BattleManager battleScript = battleController.GetComponent<BattleManager>();
                battleScript.Init(enemy, player.character);

        	}
        	else
        	{
                enemy = null;
                Destroy(battleController);

        	}
        }
    }

    void Init()
    {
        //Level létrehozás
        level = Instantiate(dataController.levels[0]) as GameObject;
        level.transform.parent = Outworld.transform;
        //Karakter elhelyezés
        player = Instantiate(dataController.playerList[0]) as Character_Controller;
        player.transform.position = level.transform.Find("SpawnPoint").transform.position;
        player.transform.parent = level.transform;
        //Movement controller létrehozás
        Player_Movement_Controller moveScript = MovementController.GetComponent<Player_Movement_Controller>();
        moveScript.AddPlayer(player);
    }
}
