using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

    //Optimize and refactor
    //Make it so that this class/methods throws exceptions and creates a log file with the failiure info
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject tilesParent; //parent object to spawn tiles under
    [SerializeField] private SpawnConfigSO spawnConfig;
    [SerializeField] private LevelDataSO levelData;

    [Tooltip("Used to offset \"VerticalStripes\" and \"HorizontalStripes\" patterns." + "\n\nWhen set to \"True\" offsets the starting position.")] 
    [SerializeField] private bool offset;
    
    private int NumOfRows;
    private int NumOfColumns;
    private int levelSceneIndexOffset = 0; //subject to change
    private LevelSO level;

    //The Pin
    private GameObject pin;
    //Tile types
    private GameObject swappable;
    private GameObject pinned;

    public string MethodToCall; //Used to store the pattern method selected from the Editor

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        //Initialize the tiles by their respective tile type prefabs
        InitializeTileTypes();

        //Get level info
        level = levelData.levels[SceneManager.GetActiveScene().buildIndex - levelSceneIndexOffset];
        NumOfRows = level.NumOfRows;
        NumOfColumns = level.NumOfColumns;

        //Invoke the chosen pattern method. Spawns tiles via the selected pin pattern
        SpawnPattern();

        tilesParent.SetActive(true);
    }

    private void InitializeTileTypes()
    {
        pin = spawnConfig.tileConfiguration.Pin;
        swappable = spawnConfig.tileConfiguration.Swappable;
        pinned = spawnConfig.tileConfiguration.Pinned;
    }

    private void SpawnPattern()
    {
        typeof(Spawner)
            .GetMethod(MethodToCall, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Invoke(this, new object[0]);
    }

    private void SpawnTile(GameObject tile, int i, int j)
    {
        GameObject newTile = Instantiate(tile, tilesParent.transform);
        if (tile == pinned) Instantiate(pin, newTile.transform);

        //Change the sprite and rename the tile
        SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
        int tileNumber = (i * NumOfColumns) + j;
        spriteRenderer.sprite = level.sprites[tileNumber];
        Tile tileComponent = newTile.GetComponent<Tile>();
        tileComponent.SetId(tileNumber);

        //Calculate the offset of the tiles to spawn contiguously
        float xOffset = spriteRenderer.sprite.bounds.size.x;
        float yOffset = spriteRenderer.sprite.bounds.size.y;

        //Calculate the size of the box collider
        BoxCollider2D boxCollider = newTile.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(xOffset / newTile.transform.lossyScale.x, yOffset / newTile.transform.lossyScale.y);
        boxCollider.offset = Vector2.zero;

        newTile.transform.position = new Vector2(transform.position.x + (j * xOffset), transform.position.y - (i * yOffset));
    }

    private void None()
    {
        Debug.Log("None called successfully");

        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    private void Corners()
    {
        int maxRow = NumOfRows - 1;
        int maxColumn = NumOfColumns - 1;

        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                if ((i == 0 && j == 0) ||
                    (i == 0 && j == maxColumn) ||
                    (i == maxRow && j == 0) ||
                    (i == maxRow && j == maxColumn)) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    //only if numOfTiles is odd
    private void CornersAndCenter()
    {
        int maxRow = NumOfRows - 1;
        int maxColumn = NumOfColumns - 1;

        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                if ((i == 0 && j == 0) ||
                    (i == 0 && j == maxColumn) ||
                    (i == maxRow && j == 0) ||
                    (i == maxRow && j == maxColumn) ||
                    (i == (maxRow / 2) && j == (maxColumn / 2))) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    private void FullRectangle()
    {
        int maxRow = NumOfRows - 1;
        int maxColumn = NumOfColumns - 1;

        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                if ((i == 0 && j <= maxColumn) ||
                    ((i == 1 || i <= maxRow - 1) && (j == 0 || j == maxColumn)) ||
                    (i == maxRow && j <= maxColumn)) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    private void Odds()
    {
        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                if (!IsEven(i, j)) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    private void Evens()
    {
        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                if (IsEven(i, j)) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    //Only works when you have X-by-X (a.k.a Square) image
    private void Cross()
    {
        Debug.Log("Cross is called");
        int crossCount = 0;

        int maxColumn = NumOfColumns - 1;

        for (int i = 0; i < NumOfRows; i++)
        {
            for (int j = 0; j < NumOfColumns; j++)
            {
                if (j == crossCount || j == maxColumn - crossCount) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
            crossCount++;
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    //BUUUGGG!!!!!
    //starts from first column
    private void VerticalStripes()
    {
        int maxRow = NumOfRows - 1;
        int maxColumn = NumOfColumns - 1;

        bool isEven = IsEven(maxRow, maxColumn + 1);
        bool offset;

        if (isEven) offset = !this.offset;
        else offset = this.offset;

        for (int i = 0; i < NumOfRows; i++)
        {
            if(isEven) offset = !offset;
            for (int j = 0; j < NumOfColumns; j++)
            {
                offset = !offset;
                if (offset) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    //starts from first row
    private void HorizontalStripes()
    {
        bool offset = this.offset;

        for (int i = 0; i < NumOfRows; i++)
        {
            offset = !offset;
            for (int j = 0; j < NumOfColumns; j++)
            {
                if (offset) SpawnTile(pinned, i, j);
                else SpawnTile(swappable, i, j);
            }
        }
        //Call Randomizer, Randomizer doesnt neet to be Mono
    }

    //Custom pin pattern with given values
    private void Custom()
    {

    }

    /*
     * Note to Self: 
     *      When using indexes call "IsEven(i, j)"
     *      When using tile count call "IsEven(maxRow, maxCount + 1)"
     */
    private bool IsEven(int i, int j)
    {
        return ((i * NumOfColumns) + j) % 2 == 0;
    }

    private void PatternCreationError(string patternType)
    {
        Debug.LogError("PatternType: \"" + patternType + "\" failed to create pattern! Failiure information will be provided and a log file will be created in later patches.");
        Debug.LogError("Pin Pattern can not be found! Wrong or no pin patterns detected in Spawner class PinPattern." +
                "\nPlease be sure to select write pattern or create a new pattern");
    }
}
