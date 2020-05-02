using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {MainMenu, Creation, Outworld, BattleSetup, Battle, Win, Lose};

public class Game_Controller : MonoBehaviour
{
    //Level parent és outworld UI, joystick etc
	public GameObject Outworld;
    //Input és mozgás kezelés
    public GameObject MovementController;
    //Battle controller prefab, harc esetén felbontandó
    public GameObject battleControllerPrefab;
    //A fontos adatok eléréshez és tárolásához.
    public Data_Controller dataController;

    GameObject battleController;
    Character_Controller player;
    //A karakterünk és a kreált ellenfél adatai
    GameObject level;
    Character enemy;

    public GameState currentState;

    void Start()
    {
        Screen.SetResolution(1920,1080,true);
        //Startnál a menüben kezdünk
        currentState = GameState.MainMenu;
    }

    void Update()
    {
        //Amikor először indul a játék akkor létre kell hozni a világot először

        if(currentState == GameState.MainMenu)
        {
            if(Outworld.activeSelf)
            {
                Outworld.SetActive(false);
            }
        }

        else if(currentState == GameState.Creation)
        {
            Init();
            currentState = GameState.Outworld;
        }
        //Outworld fázis
        else if(currentState == GameState.Outworld)
        {
            //Itt történnek dolgok amikor az outworldben vagyunk
            //Itt tud átváltani battle state-re a játék
        }
        //Amikor elkezdődik a csata
        else if(currentState == GameState.BattleSetup)
        {
            Outworld.SetActive(false);
            enemy = CreateEnemy();
            battleController = Instantiate(battleControllerPrefab, Vector3.zero, Quaternion.identity);
            StartBattle();
            currentState = GameState.Battle;
        }

        else if(currentState == GameState.Battle)
        {
            //Do something
        }

        //Ha a battle véget ért és nyertünk
        else if(currentState == GameState.Win)
        {
            currentState = GameState.Outworld;
            Outworld.SetActive(true);
            enemy = null;
            Destroy(battleController);
        }
        //Ha vesztettünk
        else if(currentState == GameState.Win)
        {
            
        }


        //Teszteléshez
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(currentState == GameState.Outworld)
                currentState = GameState.BattleSetup;
            else if(currentState == GameState.Battle)
                currentState = GameState.Win;
        }
    }

    void Init()
    {
        Outworld.SetActive(true);
        LoadLevel();
        InstantiateCharacter();
        CreateMoveControl();
    }

    void LoadLevel()
    {
        //Level létrehozás
        level = Instantiate(dataController.levels[0]) as GameObject;
        level.transform.parent = Outworld.transform;
    }

    void InstantiateCharacter()
    {
        //Karakter elhelyezés
        player = Instantiate(dataController.playerList[0]) as Character_Controller;
        player.transform.position = level.transform.Find("SpawnPoint").transform.position;
        player.transform.parent = level.transform;
    }

    void CreateMoveControl()
    {
        //Movement controller létrehozás
        Player_Movement_Controller moveScript = MovementController.GetComponent<Player_Movement_Controller>();
        moveScript.AddPlayer(player);
    }

    Character CreateEnemy()
    {
        return enemy = Instantiate(dataController.enemyList[0]) as Character;
    }

    void StartBattle()
    {
        BattleManager battleScript = battleController.GetComponent<BattleManager>();
        battleScript.Init(enemy, player.character);
    }

    public void TriggerBattle()
    {
        currentState = GameState.BattleSetup;
    }


}
