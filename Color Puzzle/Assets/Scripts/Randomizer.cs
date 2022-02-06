using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //Optimize and refactor -> Might convert to non-Mono class and be called from the Spawner or necessary class
public class Randomizer : MonoBehaviour
{
    private void Start()
    {
        RandomizeTiles();
    }

    //Refactor
    private void RandomizeTiles()
    {
        /*
         * FindObjectsOfType returns an array in reversed order! To solve it one can:
         *      Arrays: System.Array.Reverse(arrayToReverse)
         *      Lists: listToReverse.Reverse()
         */
        Tile[] tilesArray = FindObjectsOfType<Tile>();
        System.Array.Reverse(tilesArray);

        List<Tile> tempList = new List<Tile>();

        foreach(Tile tile in tilesArray)
        {
            if(tile.tileType != Tile.Type.Pinned)
            {
                tempList.Add(tile);
            }
        }

        //Shuffle both positions and arrays
        ShuffleArray(tempList);

        //Place shuffled tiles in tiles array
        int offset = 0;
        for(int i = 0; i < tilesArray.Length; i++)
        {
            if (tilesArray[i].tileType == Tile.Type.Pinned)
            {
                tilesArray[i].SetIndex(i);
                offset++;
                continue;
            }
            tilesArray[i] = tempList[i - offset];
            tilesArray[i].SetIndex(i);
        }
    }

    //Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    //Except it is not generic. Try converting to generic <T> type
    private void ShuffleArray(List<Tile> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            Tile temp = list[i];
            Vector3 oldPos = temp.transform.position;
            int randomValue = Random.Range(0, i + 1);
            list[i].transform.position = list[randomValue].transform.position;
            list[i] = list[randomValue];
            list[randomValue].transform.position = oldPos;
            list[randomValue] = temp;
        }
    }
}
