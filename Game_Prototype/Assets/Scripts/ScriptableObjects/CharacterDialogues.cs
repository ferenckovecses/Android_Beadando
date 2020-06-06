using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterName", menuName = "ScriptableObject/CharacterDialogues", order = 1)]
public class CharacterDialogues : ScriptableObject
{
	//Párbeszédek
    public int dialogueIndex = 0;
    public List<Dialogue> dialogue;

    public string[] GetActiveDialogues()
    {
    	return this.dialogue[dialogueIndex].sentences;
    }

    public void ResetIndex()
    {
    	this.dialogueIndex = 0;
    }

    public void IncreaseIndex()
    {
    	this.dialogueIndex += 1;
    }

    public void DecreaseIndex()
    {
    	this.dialogueIndex -= 1;
    }

    public void SetIndex(int newIndex)
    {
    	this.dialogueIndex = newIndex;
    }



}
