using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Game_Controller : MonoBehaviour
{
    //World UI
	public GameObject worldUI;

    //Input és mozgás kezelés
    public GameObject movementController;

    //Battle controller prefab, harc esetén felbontandó
    public GameObject battleControllerPrefab;

    //A fontos adatok eléréshez és tárolásához az adatvezérlő.
    Data_Controller dataController;

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
    public bool battleStarted;

    //HUD UI elemek
    public TMP_Text LvlText;
    public TMP_Text LvlStatus;
    public TMP_Text HPStatus;
    public Slider playerHPSlider;
    public Slider playerXpSlider;

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

        //Ha vége a harcnak
        else if(gameState.GetGameState() == GameState.BattleEnded)
        {
            gameState.ChangeGameState(GameState.Outworld);
            battleStarted = false;
            worldUI.SetActive(true);
            enemy = null;
            RefreshHUD();


            if(battleController.GetComponent<Battle_Controller>().GetBattleState() == BattleState.Lose)
            {
                Faint();
            }

            if(battleController.GetComponent<Battle_Controller>().GetBattleState() == BattleState.Escaped)
            {
                string message = "Sikeresen elmenekültél a harcból!";
                GameObject.Find("Dialogue").GetComponent<Dialogue_Controller>().DisplayText(message);
            }

            Destroy(battleController);

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

        //Élet és XP slider beállítása
        RefreshHUD();
    }

    //Movement controller létrehozás
    void CreateMoveControl()
    {
        moveScript = movementController.GetComponent<Player_Movement_Controller>();
        moveScript.AddPlayer(player);
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
        RefreshHUD();
    }

    //Battle dialog parancs
    public void BattleWithNPC(Character NPC)
    {
        enemy = NPC;
        enemy.ResetCharacter();
        gameState.ChangeGameState(GameState.NPCbattle);

    }

    public void BattleWithEncounter(Character encounter)
    {
        enemy = encounter;
        gameState.ChangeGameState(GameState.RandomEncounter);
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

    public Character_Controller GetPlayer()
    {
        return this.player;
    }

    void Faint()
    {
        dataController.ChangeLevelId(0);
        ChangeLevel("FaintSpawnPoint"); 
        HealPlayer();
        string textToDisplay = "A harcban alulmaradtam, de a falu gyógyítója megmentett.";
        dialogueController.GetComponent<Dialogue_Controller>().DisplayText(textToDisplay);
    }

    //Frissíti a HUD élet és xp státuszát
    void RefreshHUD()
    {
        LvlText.text = player.character.GetLevelText();
        HPStatus.text = player.character.GetCurrentHP().ToString() + "/" + player.character.GetMaxHP().ToString();
        LvlStatus.text = player.character.GetCurrentXP().ToString() + "/" + player.character.GetMaxXP().ToString();

        playerHPSlider.value = player.character.GetCurrentHP();
        playerHPSlider.maxValue = player.character.GetMaxHP();
        playerXpSlider.value = player.character.GetCurrentXP();
        playerXpSlider.maxValue = player.character.GetMaxXP();
    }

}
