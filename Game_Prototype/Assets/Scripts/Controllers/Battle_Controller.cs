using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {PlayerTurn, EnemyTurn, Endgame, Win, Lose, Escaped};

public class Battle_Controller : MonoBehaviour
{

    BattleState currentBattleState;
    Character enemy;
    Character player;
    GameState_Controller gameState;
    Data_Controller dataController;
    Image playerSprite;

    public Slider playerHPSlider;
    public Slider playerXpSlider;
    public Slider enemyHPSlider;
    public Slider enemyXpSlider;

    // Start is called before the first frame update
    void Start()
    {
        ScoreController.ResetScore();
        gameState = GameObject.Find("GameState").GetComponent<GameState_Controller>();
        dataController = GameObject.Find("Data").GetComponent<Data_Controller>();
        currentBattleState = BattleState.PlayerTurn;
    }

    public void Init(Character enemy, Character player)
    {
        this.enemy = enemy;
        this.player = player;
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
        GameObject.Find("Player_Type_Name").GetComponent<TMP_Text>().text = player.element.returnName();
        GameObject.Find("Enemy_Type_Name").GetComponent<TMP_Text>().text = enemy.element.returnName();
        GameObject.Find("Player_HP").GetComponent<TMP_Text>().text = player.GetHPStatus();
        GameObject.Find("Enemy_HP").GetComponent<TMP_Text>().text = enemy.GetHPStatus();
        GameObject.Find("Player_Lvl").GetComponent<TMP_Text>().text = player.GetLevelText();
        GameObject.Find("Enemy_Lvl").GetComponent<TMP_Text>().text = enemy.GetLevelText();

        SetupBar();

        var escapeButton = GameObject.Find("Escape_Button").GetComponent<Button>();
        escapeButton.onClick.AddListener(delegate{EscapeBattle();});
    }

    public void EscapeBattle()
    {
        if(gameState.GetGameState() == GameState.NPCbattle)
        {
            Debug.Log("NPC harcokból nincs menekülés");
        }
        else if(gameState.GetGameState() == GameState.RandomEncounter)
        {
            currentBattleState = BattleState.Escaped;
            gameState.ChangeGameState(GameState.BattleEnded);
        }

    }

    public int CalculateDamage(int typeID)
    {
        int damage;
        if(currentBattleState == BattleState.PlayerTurn)
        {
            float baseDamage = (((((2 * player.GetLevel()) / 5) + 2) * player.GetMovePower() * (player.GetAttack() / enemy.GetDefense())) / 50 ) + 2;
            float sameTypeBonus = (player.GetElement().elementValue == typeID) ? 1.5f : 1;
            float effectivity = enemy.GetElement().isSuperEffective(typeID);
            float randomRoll = UnityEngine.Random.Range(0.85f, 1f);
            damage = (int)(baseDamage * randomRoll * sameTypeBonus * effectivity);
        }

        else
        {
            float baseDamage = (((((2 * enemy.GetLevel()) / 5) + 2) * enemy.GetMovePower() * (enemy.GetAttack() / player.GetDefense())) / 50 ) + 2;
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
        SetHealth();
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
                SetupBar();
                currentBattleState = BattleState.Win;
                gameState.ChangeGameState(GameState.BattleEnded);
            }

            else
            {
                currentBattleState = BattleState.Lose;
                gameState.ChangeGameState(GameState.BattleEnded);
            }
        }
    }

    public BattleState GetBattleState()
    {
        return this.currentBattleState;
    }

    void SetHealth()
    {
        playerHPSlider.value = player.GetCurrentHP();
        enemyHPSlider.value = enemy.GetCurrentHP();
    }

    void SetupBar()
    {
        playerHPSlider.value = player.GetCurrentHP();
        playerHPSlider.maxValue = player.GetMaxHP();
        playerXpSlider.value = player.GetCurrentXP();
        playerXpSlider.maxValue = player.GetMaxXP();

        enemyHPSlider.value = enemy.GetCurrentHP();
        enemyHPSlider.maxValue = enemy.GetMaxHP();
        enemyXpSlider.value = enemy.GetCurrentXP();
        enemyXpSlider.maxValue = enemy.GetMaxXP();
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
