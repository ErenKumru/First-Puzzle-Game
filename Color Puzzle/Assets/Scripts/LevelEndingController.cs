using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class LevelEndingController : MonoBehaviour
{
    [SerializeField] private TMP_Text congratulationText;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject nextLevelButton;
    private List<Swapper> swappers;

    private void OnEnable()
    {
        AutoFillTiles();

        foreach(Swapper swapper in swappers)
        {
            swapper.OnTileSwap += CheckLevelEnding; //Registering for the OnTileSwap event in Swapper for each swapper object(a.k.a tile)
        }
    }

    private void CheckLevelEnding()
    {
        if (AllTilesInPlace())
        {
            congratulationText.enabled = true;
            image.SetActive(true);

            nextLevelButton.SetActive(true);
        }
    }

    //Might need refactoring since calling getcomponent each time might be costly
    private bool AllTilesInPlace()
    {
        foreach (Swapper swapper in swappers)
        {
            Tile tile = swapper.GetComponent<Tile>();
            if (!tile.IsInPlace())
            {
                return false;
            }
        }
        return true;
    }

    //Auto fills the swappers by getting all Tile objects with the swapper component in the scene
    private void AutoFillTiles()
    {
        swappers = GetComponentsInChildren<Swapper>()
            .Where(t => t.name.ToLower().Contains("tile"))
            .ToList();
    }
}
