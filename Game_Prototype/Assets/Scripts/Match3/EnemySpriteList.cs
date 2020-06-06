using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpriteList", menuName = "ScriptableObjects/EnemySpriteList", order = 1)]
public class EnemySpriteList : ScriptableObject
{
    public List<Sprite> enemySpriteList;
}
