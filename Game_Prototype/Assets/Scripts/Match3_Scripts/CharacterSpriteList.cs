using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSprites", menuName = "ScriptableObjects/CharacterSprites", order = 1)]
public class CharacterSprites : ScriptableObject
{
    public List<Sprite> characterSprites;
}