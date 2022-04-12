using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileConfigurations", menuName = "Configurations/TileConfiguration", order = 1)]
public class TileConfigSO : ScriptableObject
{
    //Try to find a better way of doing this. More automated and if possible a way that doesn't require modifying code/class when new object type is added.
    public GameObject Pin;
    public GameObject Swappable;
    public GameObject Pinned;
    //public GameObject[] tileTypePrefabs;



    /* Can be extanble with:
     *  Tile shapes[]
     *  Tile colors/lights
     *  Tile anims
     *  Tile size
     *  etc.
     */
}
