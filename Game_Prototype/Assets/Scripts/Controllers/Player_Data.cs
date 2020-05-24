using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player_Data
{
    public float[] position;
    public int playerId;

    public Player_Data(Character_Controller player, int playerId)
    {
        this.playerId = playerId;
        this.position = new float[3];
        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = 0;
    }
}

