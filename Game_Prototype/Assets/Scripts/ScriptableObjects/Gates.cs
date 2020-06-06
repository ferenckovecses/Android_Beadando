using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Gate_Name", menuName = "ScriptableObject/Gates", order = 3)]
public class Gates : ScriptableObject
{
    public string gateName;
    public int stageID;
}
