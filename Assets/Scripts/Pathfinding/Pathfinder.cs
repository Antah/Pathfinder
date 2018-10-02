using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    protected List<Tile> path;
    protected GameObject pathHolder;
    protected Tile[,] map;
    protected Tile start, finish;
    protected System.Random pseudoRandom;
    public GameObject exit;
    public Material lineMaterial;

    void Awake()
    {
        pseudoRandom = new System.Random((Time.time.ToString().GetHashCode()));
    }

    public virtual void CreatePath(Tile[,] map) {
        ClearPath();
        pathHolder = new GameObject("PathHolder");
        this.map = map;
        this.map = map;
        if (RandomizeStartAndFinish())
        {
            CreatePath(map, start, finish);
            DrawNewPath();
        }
        else
            UISettings.instance.SetMessage("No suitable tiles for start and finish");
    }

    public void ClearPath()
    {
        if (pathHolder != null)
            Destroy(pathHolder.gameObject);
    }

    public bool RandomizeStartAndFinish()
    {
        List<Tile> emptyTiles = GetEmptyTiles(map);
        if (emptyTiles.Count <= 1)
            return false;

        int startIndex, finishIndex;
        startIndex = pseudoRandom.Next(0, emptyTiles.Count);
        finishIndex = pseudoRandom.Next(0, emptyTiles.Count);
        while (startIndex == finishIndex)
            finishIndex = pseudoRandom.Next(0, emptyTiles.Count);

        start = emptyTiles[startIndex];
        finish = emptyTiles[finishIndex];
        return true;
    }

    protected List<Tile> GetEmptyTiles(Tile[,] map)
    {
        List<Tile> emptyTiles = new List<Tile>();
        foreach(Tile t in map)
        {
            if (t.value == TileValue.Floor)
                emptyTiles.Add(t);
        }
        return emptyTiles;
    }

    public virtual bool CreatePath(Tile[,] map, Tile start, Tile finish) { return false; }

    public void DrawNewPath()
    {
        Vector3 startPosition = new Vector3(start.x, start.y, 0f);
        GameManager.instance.player.transform.position = startPosition;

        Vector3 finishPosition = new Vector3(finish.x, finish.y, 0f);
        GameObject finishObject = Instantiate(exit, finishPosition, Quaternion.identity) as GameObject;
        finishObject.transform.parent = pathHolder.transform;
        if(path.Count > 0)
        {
            DrawPathLines();
            UISettings.instance.SetMessage("Path found!");
        }
        else
            UISettings.instance.SetMessage("No path between start and finish");
    }

    public void DrawPathLines()
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 lineStart = new Vector3(path[i].x, path[i].y, -1f);
            Vector3 lineEnd = new Vector3(path[i+1].x, path[i+1].y, -1f);

            DrawLine(lineStart, lineEnd, Color.blue, 2f);
        }
    }

    void DrawLine(Vector3 lineStart, Vector3 lineEnd, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = lineStart;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, lineStart);
        lr.SetPosition(1, lineEnd);
        myLine.transform.parent = pathHolder.transform;
    }
}
