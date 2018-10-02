using System;
using System.Collections.Generic;

public class AStar : Pathfinder
{
    private List<AStarTile> openList;
    private List<AStarTile> closedList;
    private AStarTile[,] map;
    private AStarTile start, finish;

    public override bool CreatePath(Tile[,] map, Tile start, Tile finish)
    {
        openList = new List<AStarTile>();
        closedList = new List<AStarTile>();
        path = new List<Tile>();
        
        ConvertMap(map);
        this.start = ConvertTile(start);
        this.map[start.x, start.y] = this.start;
        this.finish = ConvertTile(finish);
        this.map[finish.x, finish.y] = this.finish;

        return AStarPath();
    }

    private AStarTile ConvertTile(Tile tile)
    {
        return new AStarTile(tile);
    }

    private void ConvertMap(Tile[,] map)
    {
        this.map = new AStarTile[map.GetLength(0), map.GetLength(1)];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for( int j = 0; j < map.GetLength(1); j++)
            {
                this.map[i, j] = new AStarTile(map[i,j]);
            }
        }
    }

    private bool AStarPath()
    {
        start.startScore = 0;
        start.finishScore = GetDistance(start.tile, finish.tile);
        start.parent = null;
        closedList.Add(start);
        AddNeighboursToOpenList(start);
        while(openList.Count > 0)
        {
            AStarTile currentTile = GetLowestScoreTile();
            if (currentTile.tile == finish.tile)
            {
                ReconstructPath();
                return true;
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            AddNeighboursToOpenList(currentTile);
        }
        return false;
    }

    private void ReconstructPath()
    {
        AStarTile currentTile = finish;
        path.Add(finish.tile);
        while (currentTile.parent != null)
        {
            path.Add(currentTile.parent.tile);
            currentTile = currentTile.parent;
        }
        path.Reverse();
    }

    private AStarTile GetLowestScoreTile()
    {
        openList.Sort();
        return openList[0];
    }

    private int GetDistance(Tile start, Tile finish)
    {
        return Math.Abs(start.x - finish.x) + Math.Abs(start.y - finish.y);
    }

    private void AddNeighboursToOpenList(AStarTile previousTile)
    {
        //left
        if (previousTile.tile.x > 1)
            AddTiletoOpenList(previousTile.tile.x - 1, previousTile.tile.y, previousTile);
            
        //right
        if (previousTile.tile.x < map.GetLength(0) - 2)
            AddTiletoOpenList(previousTile.tile.x + 1, previousTile.tile.y, previousTile);

        //down
        if (previousTile.tile.y > 1)
            AddTiletoOpenList(previousTile.tile.x, previousTile.tile.y - 1, previousTile);

        //top
        if (previousTile.tile.y < map.GetLength(1) - 2)
            AddTiletoOpenList(previousTile.tile.x, previousTile.tile.y + 1, previousTile);
    }

    private void AddTiletoOpenList(int x, int y, AStarTile prevTile)
    {
        AStarTile addTile = map[x, y];
        if (addTile.tile.value != TileValue.Floor || closedList.Contains(addTile))
            return;

        if (openList.Contains(addTile))
        {
            if (addTile.startScore > prevTile.startScore + 1)
            {
                addTile.startScore = prevTile.startScore + 1;
                addTile.parent = prevTile;
            }
        } else
        {
            openList.Add(addTile);
            addTile.startScore = prevTile.startScore + 1;
            addTile.parent = prevTile;
            addTile.finishScore = GetDistance(addTile.tile, finish.tile);
        }
    }
}
