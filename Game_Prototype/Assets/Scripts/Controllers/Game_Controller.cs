using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour
{
    //World UI
	public GameObject worldUI;

    //Input és mozgás kezelés
    public GameObject movementController;

    //Battle controller prefab, harc esetén felbontandó
    public GameObject battleControllerPrefab;

    //A fontos adatok eléréshez és tárolásához az adatvezérlő.
    public Data_Controller dataController;

    //Dialógus vezérlő
    public GameObject dialogueController;

    //Játék közbeni menü vezérlő
    public GameObject ingameMenuController;

    //GameState vezérlő
    GameState_Controller gameState;


    //Dinamikusan létrehozott elemek változói
    GameObject battleController;
    Character_Controller player;

    //A karakterünk és a kreált ellenfél adatai
    GameObject level;
    Character enemy;

    //Játék közben használt scriptek
    Player_Movement_Controller moveScript;
    Battle_Controller battleScript;

    //Játék aktuális státuszát tartalmazó változó
    bool battleStarted;

    void Start()
    {
        //Alaphelyzet beállítása
        battleStarted = false;

        //Adatvezérlő referálása
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
        gameState = GameObject.Find("GameState").GetComponent<GameState_Controller>();
    }


    void Update()
    {
        //Világ létrehozása
        if(gameState.GetGameState() == GameState.NewGame || gameState.GetGameState() == GameState.LoadGame)
        {
            bool ifNewGame;
            if(gameState.GetGameState() == GameState.NewGame)
                ifNewGame = true;
            else
                ifNewGame = false;

            //Létrehozza a játékhoz szükséges dolgokat
            Init(ifNewGame);

            //Átváltunk a játék fázisba
            gameState.ChangeGameState(GameState.Outworld);
        }

        //Ha random ellenféllel találkozunk
        else if(gameState.GetGameState() == GameState.RandomEncounter)
        {
            if(!battleStarted)
            {
                battleStarted = true;
                enemy = CreateEnemy();
                BattleSetup();
                StartBattle();
            }

        }

        //Ha NPC-vel harcot kezdünk
        else if(gameState.GetGameState() == GameState.NPCbattle)
        {
            if(!battleStarted)
            {
                BattleSetup();
                battleStarted = true;
                StartBattle();
            }
        }

        //Ha dialógus játszódik épp
        else if(gameState.GetGameState() == GameState.Dialogue)
        {
            if(!(dialogueController.activeSelf))
            {
                dialogueController.SetActive(true);
            }
        }

        //Ha nyertünk a harcban
        else if(gameState.GetGameState() == GameState.Win)
        {
            battleStarted = false;
            worldUI.SetActive(true);
            enemy = null;
            Destroy(battleController);
        }
        //Ha vesztettünk a harcban
        else if(gameState.GetGameState() == GameState.Lose)
        {
            battleStarted = false;
            Destroy(battleController);
            Destroy(GameObject.Find("Data"));
            Destroy(GameObject.Find("GameState"));
            Debug.Log("Game Over");
            SceneManager.LoadScene("MainMenu");
        }
    }

    //Előkészíti a játékot az indulásra
    void Init(bool firstTime = false)
    {
        enemy = null;
        worldUI.SetActive(true);
        LoadLevel();
        InstantiateCharacter(firstTime);
        CreateMoveControl();
    }

    //Betölti az aktív pályát
    void LoadLevel()
    {
        //Level létrehozás
        level = Instantiate(dataController.levels[dataController.GetLevelId()]) as GameObject;
        level.transform.parent = worldUI.transform;
    }

    //Létrehozza és elhelyezi a megfelelő karaktert
    void InstantiateCharacter(bool firstTime)
    {
        //Karakter elhelyezés
        player = Instantiate(dataController.player[dataController.getPlayerID()]) as Character_Controller;

        //Ha új játékot indítottunk, akkor a spawn pointra rak
        if (firstTime)
        {
            player.transform.position = GetSpawnPoint(level, "GameStartPoint");
            player.character.ResetCharacter();
        }
        //Ellenkező esetben az elmentett pozícióra
        else
        {
            PositionCharacter(player);
            player.character.SetupCharacter(dataController.GetData());
            dataController.DeleteData();
        }
        player.transform.parent = level.transform;

        //Prefab elnevezése
        player.name = "Player";
    }

    //Movement controller létrehozás
    void CreateMoveControl()
    {
        moveScript = movementController.GetComponent<Player_Movement_Controller>();
        moveScript.AddPlayer(player);
    }

    //Generál egy ellenséget
    Character CreateEnemy()
    {
        enemy = Instantiate(dataController.enemyList[0]) as Character;
        enemy.Init();
        return enemy;
    }

    //Elindítja a harcot a karakterünkkel és az enemy változóban lévő ellenséggel
    void StartBattle()
    {
        battleScript = battleController.GetComponent<Battle_Controller>();
        battleScript.Init(enemy, player.character);
    }

    //Megjeleníti a harc képernyőt
    void BattleSetup()
    {
        worldUI.SetActive(false);
        battleController = Instantiate(battleControllerPrefab, Vector3.zero, Quaternion.identity);
    }

    //Menu gomb függvénye
    public void IngameMenu()
    {
        if(gameState.GetGameState() == GameState.Outworld)
            gameState.ChangeGameState(GameState.IngameMenu);
    }

    //Heal dialog parancs
    public void HealPlayer()
    {
        player.character.ResetHP();
    }

    //Battle dialog parancs
    public void BattleWithNPC(Character NPC)
    {
        enemy = NPC;
        enemy.Init();
        gameState.ChangeGameState(GameState.NPCbattle);

    }

    //A kapott karaktert a Data Controllerben eltárolt mentett pozícióra rakja
    void PositionCharacter(Character_Controller player)
    {
        var positions = dataController.GetComponent<Data_Controller>().getPositions();
        Vector3 newPosition;
        newPosition.x = positions[0];
        newPosition.y = positions[1];
        newPosition.z = positions[2];
        player.transform.position = newPosition;
    }

    Vector3 GetSpawnPoint(GameObject level, string pointName)
    {
        return level.transform.Find("SpawnPoints/" + pointName).transform.position;
    }

    public void ChangeLevel(string destination)
    {
        player.transform.parent = worldUI.transform;
        Destroy(level);
        LoadLevel();
        player.transform.parent = level.transform;
        player.transform.position = GetSpawnPoint(this.level, destination);
    }

    public void SaveGame()
    {
        dataController.SaveGame(player);
    }

}
