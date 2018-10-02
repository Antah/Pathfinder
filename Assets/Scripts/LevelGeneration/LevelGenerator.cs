using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    protected GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    protected int level;

    protected void InstantiateFromArray(GameObject[] prefabs, Vector3 position)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    protected void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    public string seed;
    public bool useRandomSeed;

    public int columns = 10;
    public int rows = 10;

    [Range(0, 100)]
    public int wallsFillPercent;

    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] rubbleTiles;
    public GameObject exit;

    public Tile[,] map;
    private System.Random pseudoRandom;

    public void SetupScene(int level)
    {
        this.level = level;
        SetupParameters();
        boardHolder = new GameObject("BoardHolder");
        GenerateMap();
        InstantiateTiles();
    }

    private void SetupParameters()
    {
        LevelSettings settings = UISettings.instance.GetSettings();

        seed = settings.seed;
        useRandomSeed = settings.useRandomSeed;

        columns = settings.levelWidth;
        rows = settings.levelHeight;
        wallsFillPercent = settings.wallFill;
    }

    private void InstantiateTiles()
    {
        for (int i = 0; i < columns + 2; i++)
        {
            for (int j = 0; j < rows + 2; j++)
            {
                if (map[i, j].value > 0)
                {
                    InstantiateFromArray(wallTiles, i, j);
                }
                else
                {
                    InstantiateFromArray(floorTiles, i, j);
                }
            }
        }

        Vector3 playerPos = new Vector3(columns / 2, rows / 2, 0);
        GameManager.instance.GetPlayer().transform.position = playerPos;
    }

    void GenerateMap()
    {
        map = new Tile[columns + 2, rows + 2];
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        RandomFillMap();
        SmoothMap();
    }

    void RandomFillMap()
    {
        pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < columns + 2; x++)
        {
            for (int y = 0; y < rows + 2; y++)
            {
                if (x == 0 || x == columns + 1 || y == 0 || y == rows + 1)
                {
                    map[x,y] = new Tile(x, y, TileValue.OuterWall);
                }
                else
                {
                    if (pseudoRandom.Next(0, 100) < wallsFillPercent)
                    {
                        map[x, y] = new Tile(x, y, TileValue.Obstacle);
                    }
                    else
                    {
                        map[x, y] = new Tile(x, y, TileValue.Floor);
                    }

                }
            }
        }
    }

    void SmoothMap()
    {
        Tile[,] tmpMap = new Tile[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < columns + 2; x++)
        {
            for (int y = 0; y < rows + 2; y++)
            {
                tmpMap[x, y] = map[x, y];
                if (x == 0 || y == 0 || x == columns + 1 || y == rows + 1)
                    continue;

                int neighbourRoomTiles = CheckNeighbourhood(x, y);

                if (tmpMap[x, y].value > 0 && neighbourRoomTiles >= 3)
                {
                    tmpMap[x, y].value = TileValue.Floor;
                }
                else if (tmpMap[x, y].value == TileValue.Floor && neighbourRoomTiles >= 2)
                {
                    tmpMap[x, y].value = TileValue.Floor;
                }
                else
                {
                    tmpMap[x, y].value = TileValue.Obstacle;
                }
            }
        }
        map = tmpMap;
    }

    private int CheckNeighbourhood(int x, int y)
    {
        int wallCount = 0;

        if (x == 1)
            wallCount++;
        else if (map[x - 1, y].value > 0)
            wallCount ++;

        if (x == columns)
            wallCount++;
        else if (map[x + 1, y].value > 0)
            wallCount ++;

        if (y == 1)
            wallCount++;
        else if (map[x, y - 1].value > 0)
            wallCount ++;

        if (y == rows)
            wallCount++;
        else if (map[x, y + 1].value > 0)
            wallCount ++;

        return 4 - wallCount;
    }
}