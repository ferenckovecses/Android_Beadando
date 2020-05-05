using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {PlayerTurn, EnemyTurn, Endgame};

public class BattleManager : MonoBehaviour
{

    public BattleState currentBattleState;
    Character enemy;
    Character player;
    public GameObject scene;
    GameObject activeScene;
    Game_Controller gameController;
    Data_Controller dataController;
    Image playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        ScoreController.ResetScore();
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
        currentBattleState = BattleState.PlayerTurn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Character enemy, Character player)
    {
        this.enemy = enemy;
        this.player = player;
        activeScene = Instantiate(scene) as GameObject;
        activeScene.transform.parent = gameObject.transform;
        SetupUI();
    }

    public void SetupUI()
    {
        GameObject.Find("Player_Sprite").GetComponent<Image>().sprite = player.battleSprite;
        GameObject.Find("Enemy_Sprite").GetComponent<Image>().sprite = enemy.battleSprite;
        GameObject.Find("Player_Name").GetComponent<TMP_Text>().text = player.characterName;
        GameObject.Find("Enemy_Name").GetComponent<TMP_Text>().text = enemy.characterName;
        GameObject.Find("Player_Type_Image").GetComponent<Image>().sprite = player.element.returnSprite();
        GameObject.Find("Enemy_Type_Image").GetComponent<Image>().sprite = enemy.element.returnSprite();
        GameObject.Find("Player_HP").GetComponent<TMP_Text>().text = player.GetHPStatus();
        GameObject.Find("Enemy_HP").GetComponent<TMP_Text>().text = enemy.GetHPStatus();
        GameObject.Find("Player_Lvl").GetComponent<TMP_Text>().text = player.GetLevelText();
        GameObject.Find("Enemy_Lvl").GetComponent<TMP_Text>().text = enemy.GetLevelText();

        var escapeButton = GameObject.Find("Escape_Button").GetComponent<Button>();
        escapeButton.onClick.AddListener(delegate{EscapeBattle();});
    }

    public void EscapeBattle()
    {
        if(gameController.currentState == GameState.NPCbattle)
            Debug.Log("There's no escape option in NPC Battles!");
        else
            gameController.currentState = GameState.Win;
    }

    public int CalculateDamage(int typeID)
    {
        int damage;
        if(currentBattleState == BattleState.PlayerTurn)
        {
            float baseDamage = (((((2 * player.GetLevel()) / 5) + 2) * player.baseMovePower * (player.GetAttack() / enemy.GetDefense())) / 50 ) + 2;
            float sameTypeBonus = (player.GetElement().elementValue == typeID) ? 1.5f : 1;
            float effectivity = enemy.GetElement().isSuperEffective(typeID);
            float randomRoll = UnityEngine.Random.Range(0.85f, 1f);
            damage = (int)(baseDamage * randomRoll * sameTypeBonus * effectivity);
        }

        else
        {
            float baseDamage = (((((2 * enemy.GetLevel()) / 5) + 2) * enemy.baseMovePower * (enemy.GetAttack() / player.GetDefense())) / 50 ) + 2;
            float sameTypeBonus = (typeID == -1) ? 1.5f : 1;
            float effectivity = player.GetElement().isSuperEffective(typeID);
            float randomRoll = UnityEngine.Random.Range(0.85f, 1f);
            damage = (int)(baseDamage * randomRoll * sameTypeBonus * effectivity);
        }
        return damage;
    }

    public void Attack(int typeID = -1)
    {
        bool result;
        //Ha a mi körünk van
        if(currentBattleState == BattleState.PlayerTurn)
        {
            result = player.Attack(enemy, CalculateDamage(typeID));
            if(result)
            {
                currentBattleState = BattleState.Endgame;
            }

        }
        //Ha az ellenség köre van
        else if(currentBattleState == BattleState.EnemyTurn)
        {
            result = enemy.Attack(player, CalculateDamage(typeID));
            if(result)
            {
                currentBattleState = BattleState.Endgame;
            }
        }
        ResetHP();
    }

    void ResetHP()
    {
        GameObject.Find("Player_HP").GetComponent<TMP_Text>().text = player.GetHPStatus();
        GameObject.Find("Enemy_HP").GetComponent<TMP_Text>().text = enemy.GetHPStatus();
    }

    public void ChangeTurn()
    {
        if(currentBattleState == BattleState.PlayerTurn)
        {
            currentBattleState = BattleState.EnemyTurn;
        }
        else if(currentBattleState == BattleState.EnemyTurn)
        {
            currentBattleState = BattleState.PlayerTurn;
        }

        else
        {
            if(player.NotDead())
            {
                player.GetXP(enemy.GiveXP());
                gameController.currentState = GameState.Win;
            }

            else
                gameController.currentState = GameState.Lose;
        }
    }

}
//A megszerzett elemi pontokat kezeli.
public static class ScoreController
{
    public static int fire, water, earth, air, dark, ligth;

    public static void ResetScore()
    {
        fire = 0;
        water = 0;
        earth = 0;
        air = 0;
        dark = 0;
        ligth = 0;
    }

    public static void IncreasePoints(int type)
    {
        switch(type)
        {
        case 1: fire++; break;
        case 2: air++; break;
        case 3: water++; break;
        case 4: earth++; break;
        case 5: dark++; break;
        case 6: ligth++; break;
        default: break;
        }
    }

    public static void DecreasePoint(int type, int amount)
    {
        switch(type)
        {
        case 1: fire-=amount; break;
        case 2: air-=amount; break;
        case 3: water-=amount; break;
        case 4: earth-=amount; break;
        case 5: dark-=amount; break;
        case 6: ligth-=amount; break;
        default: break;
        }
    }

}
