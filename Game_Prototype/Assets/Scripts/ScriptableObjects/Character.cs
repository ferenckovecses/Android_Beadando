using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Character", order = 1)]
public class Character : ScriptableObject
{
	//Karakter neve
	public string characterName = "Default Name";

	//Harc közben használt sprite
	public Sprite battleSprite;

	//Alap értékek
	public int baseHP=20;
	public int baseAttack=55;
	public int baseDefense=55;
	public int baseLevel = 5;
	public int currentXP=0;
	public Elements element;
	public int baseMovePower = 20;
	public int baseXP = 64;
	
	//Párbeszédek
	public Dialogue dialogue;


	//Dinamikus értékek
	int level = 1;
	int experienceCap = 0;
	int currentHP = 0;
	int currentAttack = 0;
	int currentDefense = 0;
	int currentMaxHP = 0;
	int currentMovepower = 0;

	public Sprite GetSprite()
	{
		return this.battleSprite;
	}

	//Típus beállítása
	public void setElement(Elements element)
	{
		this.element = element;
	}

	//Típus id visszakérése
	public int GetElementID()
	{
		return this.element.elementValue;
	}

	//Elem lekérése
	public Elements GetElement()
	{
		return this.element;
	}

	//Név megváltoztatása
	public void setName(string newName)
	{
		this.characterName = newName;
	}

	//Beállítja a karakter dinamikus statjait
	public void Init()
	{
		this.experienceCap = (int)Mathf.Pow(this.level, 2f);
		this.currentHP = ((2 * this.baseHP) * this.level) / 100 + this.level + 10;
		this.currentAttack = ((2 * this.baseAttack) * this.level) / 100 + 5;
		this.currentDefense = ((2 * this.baseDefense) * this.level) / 100 + 5;
		this.currentXP = 0;
		this.currentMaxHP = this.currentHP;
		this.currentMovepower = this.baseMovePower;
	}

	//Karakter konstruktor
	public static Character CreateInstance()
	{
		var data = ScriptableObject.CreateInstance<Character>();
		data.Init();
		return data;
	}

	//Támadás függvény
	public bool Attack(Character opponent, int damage)
	{
		return opponent.GetDamaged(damage);
	}

	//Sebződés
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

	//Jelenlegi szint lekérése
	public int GetLevel()
	{
		return this.level;
	}

	//Jelenlegi szint beállítása
	public void SetLevel(int newLevel)
	{
		this.level = newLevel;
		Init();
	}

	//Jelenlegi támadás stat lekérése
	public int GetAttack()
	{
		return this.currentAttack;
	}

	//Jelenlegi védelem lekérése
	public int GetDefense()
	{
		return this.currentDefense;
	}

	//Jelenlegi támadás erejének lekérése
	public int GetMovePower()
	{
		return this.currentMovepower;
	}

	//Jelenlegi HP lekérése
	public int GetCurrentHP()
	{
		return this.currentHP;
	}

	//Jelenlegi HP beállítása
	public void SetCurrentHP(int newHP)
	{
		this.currentHP = newHP;
	}

	//Visszaadja a HP textet csatához
	public string GetHPStatus()
	{
		return "HP: " + this.currentHP.ToString() + "/" + this.currentMaxHP.ToString();
	}

	//Visszaadja a Lvl textet csatához
	public string GetLevelText()
	{
		return "Lvl " + this.level.ToString();
	}

	//Visszaadja hogy a karakterünk él-e még
	public bool NotDead()
	{
		return (this.currentHP != 0);
	}

	//XP szerzés
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

	//Szintlépés
	void LevelUp(int newXP)
	{
		//Ha több az XP mint amennyi a level cap-be fér akkor a többletet továbbvisszük.
		int overflowXP = newXP - (this.experienceCap - this.currentXP);
		//A hiányzó HP mennyiséget levonjuk utólag majd level up után
		var healthDiff = this.currentHP - this.currentMaxHP;
		this.level += 1;
		Debug.Log("Level Up! New level: " + this.level);
		this.currentMovepower += 1;
		this.Init();
		this.currentHP = this.currentMaxHP + healthDiff;
		GetXP(overflowXP);
	}

	//XP adás
	public int GiveXP()
	{
		return (int)((1.5 * this.baseXP) / 7);
	}

	//Karakter resetelése
	public void ResetCharacter()
	{
		this.level = this.baseLevel;
		Init();
	}

	//Dialógus lejátszása
	public void TriggerDialogue()
	{
		FindObjectOfType<Dialogue_Controller>().StartDialogue(this);
	}

	//HP resetelése/Full heal
	public void ResetHP()
	{
		this.currentHP = this.currentMaxHP;
	}

}
