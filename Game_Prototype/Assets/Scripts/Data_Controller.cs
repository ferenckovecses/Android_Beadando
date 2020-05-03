using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Data_Controller : MonoBehaviour
{
	public List<Sprite> characterSprites;
	public List<Character_Controller> player;
	int playerID = 0;
	public List<Character> enemyList;
	public List<GameObject> levels;
	public List<Elements> elements;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCharacter(int id, string name, int elementID)
    {
    	this.playerID = id;
    	this.player[this.playerID].ChangeCharacter(name, elements[elementID]);
    }

    public int getPlayerID()
    {
    	return this.playerID;
    }
}

