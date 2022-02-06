using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/LevelData", order = 2)]
public class LevelDataSO : ScriptableObject
{
    public LevelSO[] levels;
}
