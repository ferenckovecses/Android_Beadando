using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ElementName", menuName = "ScriptableObject/Element", order = 2)]
public class Elements : ScriptableObject
{
    public string elementName;
    public Sprite sprite;
    public int elementValue;
    public List<Elements> weakTo;

    public Sprite returnSprite()
    {
    	return this.sprite;
    }

    public int getValue()
    {
    	return this.elementValue;
    }

    public float isSuperEffective(int attackValue)
	{
		foreach (Elements element in weakTo)
    	{
    		if(element.getValue() == attackValue)
    		{
    			return 1.5f;
    		}
    	}
    	return 1f;
    }
}
