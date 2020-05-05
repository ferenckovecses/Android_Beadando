using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {MainMenu, Character_Creation, World_Creation, Outworld, Dialogue, IngameMenu, NPCbattle, RandomEncounter, BattleSetup, Battle, SpellBook, Win, Lose};

public class Game_Controller : MonoBehaviour
{
    //Level parent és outworld UI, joystick etc
	public GameObject Outworld;
    //Input és mozgás kezelés
    public GameObject MovementController;
    //Battle controller prefab, harc esetén felbontandó
    public GameObject battleControllerPrefab;
    //A fontos adatok eléréshez és tárolásához az adatvezérlő.
    public Data_Controller dataController;
    //Dialógus vezérlő
    public GameObject dialogueController;
    //Főmenü vezérlő
    public GameObject mainMenuController;
    //Karakter létrehozás vezérlő
    public GameObject characterCreationController;
    //Játék közbeni menü vezérlő
    public GameObject ingameMenuController;


    //Dinamikusan létrehozott elemek változói
    GameObject battleController;
    Character_Controller player;

    //A karakterünk és a kreált ellenfél adatai
    GameObject level;
    Character enemy;

    //Játék közben használt scriptek
    Player_Movement_Controller moveScript;
    BattleManager battleScript;

    //Játék aktuális státuszát tartalmazó változó
    public GameState currentState;

    //Új játék esetén takarítás
    public bool needCleanup;

    public bool battleStarted;

    void Start()
    {
        //Képernyőarányok fixálása
        Screen.SetResolution(1920,1080,true);

        //Alaphelyzet beállítása
        needCleanup = false;
        battleStarted = false;

        //Startnál a menüben kezdünk
        currentState = GameState.MainMenu;
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

        //Amikor indul az új játék létre kell hozni a világot
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

        else if(currentState == GameState.RandomEncounter)
        {
            if(!battleStarted)
            {
                enemy = CreateEnemy();
                BattleSetup();
                battleStarted = true;
                StartBattle();
            }

        }

        else if(currentState == GameState.NPCbattle)
        {
            if(!battleStarted)
            {
                BattleSetup();
                battleStarted = true;
                StartBattle();
            }
        }

        else if(currentState == GameState.Dialogue)
        {
            if(!(dialogueController.activeSelf))
            {
                dialogueController.SetActive(true);
            }
        }

        //Ha a battle véget ért és nyertünk
        else if(currentState == GameState.Win)
        {
            battleStarted = false;
            currentState = GameState.Outworld;
            Outworld.SetActive(true);
            enemy = null;
            Destroy(battleController);
        }
        //Ha vesztettünk
        else if(currentState == GameState.Lose)
        {
            battleStarted = false;
            Destroy(battleController);
            Debug.Log("Game Over");
            needCleanup = true;
            currentState = GameState.MainMenu;
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
        player.name = "Player";
    }

    void CreateMoveControl()
    {
        //Movement controller létrehozás
        moveScript = MovementController.GetComponent<Player_Movement_Controller>();
        moveScript.AddPlayer(player);
    }

    Character CreateEnemy()
    {
        enemy = Instantiate(dataController.enemyList[0]) as Character;
        enemy.Init();
        return enemy;
    }

    void StartBattle()
    {
        battleScript = battleController.GetComponent<BattleManager>();
        battleScript.Init(enemy, player.character);
    }

    void BattleSetup()
    {
        Outworld.SetActive(false);
        battleController = Instantiate(battleControllerPrefab, Vector3.zero, Quaternion.identity);
    }

    void CleanUp()
    {
        Destroy(player);
        Destroy(enemy);
        Destroy(level);
        needCleanup = false;
    }

    //Menu gomb függvénye
    public void IngameMenu()
    {
        if(currentState != GameState.Dialogue)
            currentState = GameState.IngameMenu;
    }

    //Heal dialog command
    public void HealPlayer()
    {
        player.character.ResetHP();
    }

    //Battle dialog command
    public void BattleWithNPC(Character NPC)
    {
        enemy = NPC;
        enemy.Init();
        currentState = GameState.NPCbattle;

    }


}
