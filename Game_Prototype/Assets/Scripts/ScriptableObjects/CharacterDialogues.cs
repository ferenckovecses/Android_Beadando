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

}
