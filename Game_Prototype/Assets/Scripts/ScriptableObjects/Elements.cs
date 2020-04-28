using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ElementName", menuName = "ScriptableObject/Element", order = 2)]
public class Elements : ScriptableObject
{
    public string elementName;
    public Sprite sprite;
    public int value;

    public Sprite returnSprite()
    {
    	return this.sprite;
    }
}
