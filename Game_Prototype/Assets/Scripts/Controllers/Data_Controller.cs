using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data_Controller : MonoBehaviour
{
	public List<Sprite> characterSprites;
	public List<Character_Controller> player;
	int playerID = 0;
    int levelId = 0;
    float[] position;
	public List<Character> enemyList;
	public List<GameObject> levels;
	public List<Elements> elements;
    public NPC_Controller interactableCharacter;

    void Start()
    {
        position = new  float[] {0f,0f,0f};
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCharacter(int id, string name, int elementID)
    {
        ChangePlayerId(id);
    	this.player[this.playerID].ChangeCharacter(name, elements[elementID]);
    }

    //Az aktív karakter külső ID-ja a karakter tömbben.
    public int getPlayerID()
    {
    	return this.playerID;
    }

    public void ChangePlayerId(int newId)
    {
        this.playerID = newId;
    }

    //Az aktív pálya ID-ja a páyla tömbben
    public int GetLevelId()
    {
        return this.levelId;
    }

    public void ChangeLevelId(int newId)
    {
        this.levelId = newId;
    }

    public void ChangePosition(float[] data)
    {
        this.position[0] = data[0];
        this.position[1] = data[1];
        this.position[2] = data[2];
    }

    public float[] getPositions()
    {
        return this.position;
    }
}

