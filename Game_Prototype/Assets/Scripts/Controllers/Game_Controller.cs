using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {MainMenu, LoadGame, SaveGame, Character_Creation, World_Creation, Outworld, Travelling, Dialogue,
 IngameMenu, NPCbattle, RandomEncounter, BattleSetup, Battle, SpellBook, Win, Lose};

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

    public bool battleStarted;

    void Start()
    {
        //Képernyőarányok fixálása
        Screen.SetResolution(1920,1080,true);

        //Alaphelyzet beállítása
        battleStarted = false;

        //Játék indulásakor a menüben kezdünk
        currentState = GameState.MainMenu;
    }


    void Update()
    {
        //Főmenü fázis
        if(currentState == GameState.MainMenu)
        {

            if(worldUI.activeSelf)
            {
                worldUI.SetActive(false);
            }
        }

        //Világ létrehozása
        else if(currentState == GameState.World_Creation)
        {

            //Létrehozza a játékhoz szükséges dolgokat
            Init(true);

            //Átváltunk a játék fázisba
            currentState = GameState.Outworld;
        }

        //Ha random ellenféllel találkozunk
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

        //Ha NPC-vel harcot kezdünk
        else if(currentState == GameState.NPCbattle)
        {
            if(!battleStarted)
            {
                BattleSetup();
                battleStarted = true;
                StartBattle();
            }
        }

        //Ha dialógus játszódik épp
        else if(currentState == GameState.Dialogue)
        {
            if(!(dialogueController.activeSelf))
            {
                dialogueController.SetActive(true);
            }
        }

        //Ha nyertünk a harcban
        else if(currentState == GameState.Win)
        {
            battleStarted = false;
            currentState = GameState.Outworld;
            worldUI.SetActive(true);
            enemy = null;
            Destroy(battleController);
        }
        //Ha vesztettünk a harcban
        else if(currentState == GameState.Lose)
        {
            battleStarted = false;
            Destroy(battleController);
            Debug.Log("Game Over");
            currentState = GameState.MainMenu;
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
        battleScript = battleController.GetComponent<BattleManager>();
        battleScript.Init(enemy, player.character);
    }

    //Megjeleníti a harc képernyőt
    void BattleSetup()
    {
        worldUI.SetActive(false);
        battleController = Instantiate(battleControllerPrefab, Vector3.zero, Quaternion.identity);
    }
    //Tiszta lappal kezdéshez
    public void CleanUp()
    {
        dialogueController.GetComponent<Dialogue_Controller>().EndDialogue();
        Destroy(player);
        enemy = null;
        Destroy(level);
    }

    //Menu gomb függvénye
    public void IngameMenu()
    {
        if(currentState == GameState.Outworld)
            currentState = GameState.IngameMenu;
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
        currentState = GameState.NPCbattle;

    }

    //Elmenti a játékállást
    public void SaveGame()
    {
        var oldState = currentState;
        currentState = GameState.SaveGame;
        var playerId = dataController.GetComponent<Data_Controller>().getPlayerID();
        var levelId = dataController.GetComponent<Data_Controller>().GetLevelId();
        Save_Controller.SaveGame(player,playerId,levelId);
        currentState = oldState;
    }

    //Betölti a mentett játékállást
    public void LoadGame()
    {
        var oldState = currentState;
        currentState = GameState.SaveGame;
        Player_Data data = Save_Controller.LoadGame();
        if(data != null)
        {
            dataController.GetComponent<Data_Controller>().ChangePosition(data.position);
            dataController.GetComponent<Data_Controller>().ChangePlayerId(data.playerId);
            dataController.GetComponent<Data_Controller>().ChangeLevelId(data.levelId);
            CleanUp();
            Init(false);
            player.character.SetLevel(data.playerLvl);
            player.character.setElement(dataController.elements[data.elementId]);
            player.character.SetCurrentHP(data.playerHP);
            currentState = GameState.Outworld;
        }

        else
        {
            Debug.Log("Betöltés Sikertelen!");
            currentState = oldState;
        }


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


}
