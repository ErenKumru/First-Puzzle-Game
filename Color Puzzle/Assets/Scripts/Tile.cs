using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Type tileType;
    private int id;
    private int index;

    /*  Would add:
     *      point
     *      shape
     *      special
     *      reward
     *      etc.
    */

    //extandable
    public enum Type
    {
        Pinned,
        Swappable
    }

    public int GetId()
    {
        return id;
    }

    public void SetId(int newId)
    {
        id = newId;
        name = "Tile " + id;
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public bool IsInPlace()
    {
        return id == index;
    }
}
