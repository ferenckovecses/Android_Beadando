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
	public int baseMovePower = 20;
	public int baseXP = 64;


	//Calculated stats
	int experienceCap = 0;
	int currentHP = 0;
	int currentAttack = 0;
	int currentDefense = 0;
	int currentMaxHP = 0;

	public void setLevel(int newLevel)
	{
		this.level = newLevel;
		this.currentXP = 0;
		this.experienceCap = level ^ 3;
	}

	public void setElement(Elements element)
	{
		this.element = element;
	}

	public void setName(string newName)
	{
		this.characterName = newName;
	}

	public void Init()
	{
		this.experienceCap = (int)Mathf.Pow(this.level, 2f);
		this.currentHP = ((2 * this.baseHP) * this.level) / 100 + this.level + 10;
		this.currentAttack = ((2 * this.baseAttack) * this.level) / 100 + 5;
		this.currentDefense = ((2 * this.baseDefense) * this.level) / 100 + 5;
		this.currentXP = 0;
		currentMaxHP = currentHP;
	}

	public static Character CreateInstance()
	{
		var data = ScriptableObject.CreateInstance<Character>();
		data.Init();
		return data;
	}

	public bool Attack(Character opponent, int damage)
	{
		return opponent.GetDamaged(damage);
	}

	public bool GetDamaged(int damage)
	{
		if(damage >= this.currentHP)
		{
			this.currentHP = 0;
			return true;
		}

		else
		{
			this.currentHP -= damage;
			return false;
		}
	}

	public int GetLevel()
	{
		return this.level;
	}

	public int GetAttack()
	{
		return this.currentAttack;
	}

	public int GetDefense()
	{
		return this.currentDefense;
	}

	public int GetMovePower()
	{
		return this.baseMovePower;
	}

	public Elements GetElement()
	{
		return this.element;
	}

	public string GetHPStatus()
	{
		return "HP: " + this.currentHP.ToString() + "/" + this.currentMaxHP.ToString();
	}

	public string GetLevelText()
	{
		return "Lvl " + this.level.ToString();
	}

	public bool NotDead()
	{
		return (this.currentHP != 0);
	}

	public void GetXP(int newXP)
	{
		if (this.level < 100)
		{
			//Ha kevesebb mint amennyi a level uphoz kell akkor csak növeljük a current xp-t
			if(newXP < (this.experienceCap - this.currentXP))
			{
				this.currentXP += newXP;
			}

			else
			{
				LevelUp(newXP);
			}
		}
		
	}

	void LevelUp(int newXP)
	{
		//Ha több az XP mint amennyi a level cap-be fér akkor a többletet továbbvisszük.
		int overflowXP = newXP - (this.experienceCap - this.currentXP);
		//A hiányzó HP mennyiséget levonjuk utólag majd level up után
		var healthDiff = this.currentHP - this.currentMaxHP;
		this.level += 1;
		Debug.Log("Level Up! New level: " + this.level);
		this.baseMovePower += 1;
		this.Init();
		this.currentHP = this.currentMaxHP + healthDiff;
		GetXP(overflowXP);
	}

	public int GiveXP()
	{
		return (int)((1.5 * this.baseXP) / 7);
	}

	public void ResetCharacter()
	{
		this.level = 5;
		this.baseMovePower = 5;
		Init();
	}

}
