using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player_Data
{
    public float[] position;
    public int playerId;
    public int playerLvl;
    public int playerHP;
    public int levelId;
    public int elementId;
    public string characterName;


    public Player_Data(Character_Controller player, int playerId, int levelId)
    {
        this.characterName = player.character.characterName;
        this.elementId = player.character.GetElementID();
        this.playerId = playerId;
        this.playerLvl = player.character.GetLevel();
        this.playerHP = player.character.GetCurrentHP();
        this.position = new float[3];
        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = 0;
        this.levelId = levelId;
    }
}

