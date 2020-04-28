using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Character", order = 1)]
public class Character : ScriptableObject
{
	public string characterName = "Default Name";
	public Sprite battleSprite;

	//Base stats
	public int baseHP=20;
	public int baseAttack=55;
	public int baseDefense=55;
	public int level = 1;
	public int currentXP=0;
	public Elements element;

	//Calculated stats
	int experienceCap;
	int currentHP;
	int currentAttack;
	int currentDefense;

	public void setLevel(int newLevel)
	{
		this.level = newLevel;
		this.currentXP = 0;
		this.experienceCap = level ^ 3;
	}

	public void setName(string newName)
	{
		this.name = newName;
	}

	public void Init()
	{
		this.experienceCap = this.level ^ 3;
		this.currentHP = ((2 * this.baseHP) * this.level) / 100 + this.level + 10;
		this.currentAttack = ((2 * this.baseAttack) * this.level) / 100 + 5;
		this.currentDefense = ((2 * this.baseDefense) * this.level) / 100 + 5;
	}

	public static Character CreateInstance()
	{
		var data = ScriptableObject.CreateInstance<Character>();
		data.Init();
		return data;
	}
}
