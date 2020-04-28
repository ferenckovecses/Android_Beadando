using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
	public Camera OutworldCamera;
	public GameObject Outworld;
	public GameObject Battle;
	bool battlebool = false;
	bool worldbool = true;

	GameObject prefabBattle;
    // Start is called before the first frame update
    void Start()
    {
    	OutworldCamera.enabled = true;
        Outworld.SetActive(worldbool);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
        	worldbool = !worldbool;
        	battlebool = !battlebool;

        	Outworld.SetActive(worldbool);

        	if(battlebool)
        	{
        		prefabBattle = Instantiate(Battle, Vector3.zero, Quaternion.identity);
        	}
        	else
        	{
        		Destroy(prefabBattle);
        	}
        }
    }
}
