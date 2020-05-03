using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    Character enemy;
    Character player;
    public GameObject scene;
    GameObject activeScene;
    Game_Controller gameController;
    Image playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        ScoreController.ResetScore();
        gameController = GameObject.Find("Game").GetComponent<Game_Controller>();
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


        var escapeButton = GameObject.Find("Escape_Button").GetComponent<Button>();
        escapeButton.onClick.AddListener(delegate{EscapeBattle();});
    }

    public void EscapeBattle()
    {
        gameController.currentState = GameState.Win;
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
        case 2: water++; break;
        case 3: earth++; break;
        case 4: air++; break;
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
        case 2: water-=amount; break;
        case 3: earth-=amount; break;
        case 4: air-=amount; break;
        case 5: dark-=amount; break;
        case 6: ligth-=amount; break;
        default: break;
        }
    }

}
