using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {MainMenu, Character_Creation, World_Creation, Outworld, IngameMenu, BattleSetup, Battle, Win, Lose};

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

    Player_Movement_Controller moveScript;
    BattleManager battleScript;
    public GameState currentState;
    public bool needCleanup;

    void Start()
    {
        //Képernyőarányok fixálása
        Screen.SetResolution(1920,1080,true);

        //Startnál a menüben kezdünk
        currentState = GameState.MainMenu;

        //Alaphelyzet beállítása
        needCleanup = false;
    }

    void Update()
    {
        
        if(currentState == GameState.MainMenu)
        {
            if(Outworld.activeSelf)
            {
                Outworld.SetActive(false);
            }
        }

        else if(currentState == GameState.Character_Creation)
        {
            
        }

        //Amikor indul a játék létre kell hozni a világot
        else if(currentState == GameState.World_Creation)
        {
            //Ha voltunk már a játékban és visszalépünk, akkor feltakarítunk kreálás előtt
            if(needCleanup)
            {
                CleanUp();
            }
            //Létrehozza a játékhoz szükséges dolgokat
            Init();
            //Átváltunk a játék fázisba
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
        enemy = null;
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
        player = Instantiate(dataController.player[dataController.getPlayerID()]) as Character_Controller;
        player.transform.position = level.transform.Find("SpawnPoint").transform.position;
        player.transform.parent = level.transform;
    }

    void CreateMoveControl()
    {
        //Movement controller létrehozás
        moveScript = MovementController.GetComponent<Player_Movement_Controller>();
        moveScript.AddPlayer(player);
    }

    Character CreateEnemy()
    {
        return enemy = Instantiate(dataController.enemyList[0]) as Character;
    }

    void StartBattle()
    {
        battleScript = battleController.GetComponent<BattleManager>();
        battleScript.Init(enemy, player.character);
    }

    void CleanUp()
    {
        Destroy(player);
        Destroy(enemy);
        Destroy(level);
        needCleanup = false;
    }

    //Only for Debug
    public void TriggerBattle()
    {
        currentState = GameState.BattleSetup;
    }

    public void IngameMenu()
    {
        currentState = GameState.IngameMenu;

    }


}
