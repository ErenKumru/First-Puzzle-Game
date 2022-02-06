using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfigurations", menuName = "Configurations/SpawnConfiguration", order = 2)]
public class SpawnConfigSO : ScriptableObject
{
    public TileConfigSO tileConfiguration;
    /* Can be extandable with:
     *  Spawn anim
     *  Spawn type/duration
     *  etc.
    */
}
