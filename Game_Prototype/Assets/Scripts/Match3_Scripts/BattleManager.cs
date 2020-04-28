using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    //Battle Data
    static int normal, fire, water, grass, electric, ice;

    //Game Data
    

    //UI elemek
    public List<Sprite> characterList;
    public List<Sprite> enemyList;
    public List<Sprite> tileList;

    //Script változók
    public static bool needsUpdate;

    // Start is called before the first frame update
    void Start()
    {

        //UI
        GameObject.Find("Player_Sprite").GetComponent<Image>().sprite = characterList[2];

        GameObject.Find("Enemy_Sprite").GetComponent<Image>();
        GameObject.Find("Enemy_Weakness_Icon").GetComponent<Image>();

        //Script
        needsUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
    	//UI frissítés kell
    	if(needsUpdate)
    	{

    	}
    }

    public static void IncreasePoints(int type)
    {
    	switch(type)
    	{
    	case 1: normal++; break;
    	case 2: fire++; break;
    	case 3: water++; break;
    	case 4: grass++; break;
    	case 5: electric++; break;
    	case 6: ice++; break;
    	default: break;
    	}
    }

    //UI függvények

    public void RefreshText()
    {
    	//MatchCounterText.text = "Normal: " + normal + "\nFire: " + fire + "\nWater: " + water + "\nGrass: " + grass + "\nElectric: " + electric + "\nIce: " + ice;
    }
}
