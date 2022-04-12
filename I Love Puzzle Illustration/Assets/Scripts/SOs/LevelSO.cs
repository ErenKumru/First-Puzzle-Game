using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level 0", menuName = "Levels/Level", order = 1)]
public class LevelSO : ScriptableObject
{
    public Sprite[] sprites;
    public int NumOfRows;
    public int NumOfColumns;
    /*  Can have;
     *      reward level info
     *      special level info
     *      background giver info
     *      points info
     *      etc.
     */
}
