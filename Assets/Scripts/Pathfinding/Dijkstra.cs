using System;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : Pathfinder
{
    private DijkstraTile[,] map;
    private DijkstraTile start, finish;
    private List<DijkstraTile> unvisitedList;

    public override bool CreatePath(Tile[,] map, Tile start, Tile finish)
    {
        unvisitedList = new List<DijkstraTile>();
        path = new List<Tile>();

        ConvertMap(map);   
        this.start = ConvertTile(start, 1, 0);
        this.map[start.x, start.y] = this.start;
        this.finish = ConvertTile(finish, 1, int.MaxValue);
        this.map[finish.x, finish.y] = this.finish;

        return DijkstraPath();
    }

    private DijkstraTile ConvertTile(Tile tile, int weight, int distanceToStart)
    {
        return new DijkstraTile(tile, weight, distanceToStart);
    }

    private void ConvertMap(Tile[,] map)
    {
        this.map = new DijkstraTile[map.GetLength(0) + 2, map.GetLength(1) + 2];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                this.map[i, j] = new DijkstraTile(map[i, j], 1 , int.MaxValue);
            }
        }
    }

    private bool DijkstraPath()
    {
        unvisitedList.Add(start);
        while(unvisitedList.Count > 0)
        {
            DijkstraTile nextTile = GetClosestTile();
            AddNeighboursToList(nextTile);
            unvisitedList.Remove(nextTile);
            nextTile.visited = true;
        }
        if (finish.visited)
        {
            ReconstructPath();
            return true;
        }
        return false;
    }

    private void ReconstructPath()
    {
        path.Add(finish.tile);
        DijkstraTile nextTile = finish;
        while (nextTile != start)
        {
            nextTile = nextTile.parent;
            path.Add(nextTile.tile);                   
        }
    }

    private void AddNeighboursToList(DijkstraTile previousTile)
    {
        //left
        if (previousTile.tile.x > 1)
            UpdateTileDistance(previousTile.tile.x - 1, previousTile.tile.y, previousTile);

        //right
        if (previousTile.tile.x < map.GetLength(0) - 2)
            UpdateTileDistance(previousTile.tile.x + 1, previousTile.tile.y, previousTile);

        //down
        if (previousTile.tile.y > 1)
            UpdateTileDistance(previousTile.tile.x, previousTile.tile.y - 1, previousTile);

        //top
        if (previousTile.tile.y < map.GetLength(1) - 2)
            UpdateTileDistance(previousTile.tile.x, previousTile.tile.y + 1, previousTile);
    }

    private void UpdateTileDistance(int x, int y, DijkstraTile previousTile)
    {
        DijkstraTile addTile = map[x, y];
        if (addTile.tile.value != TileValue.Floor)
            return;

        int newDistance = previousTile.distanceToStart + addTile.weight;
        if (addTile.distanceToStart > newDistance)
        {
            addTile.distanceToStart = newDistance;
            addTile.parent = previousTile;
        }
        if (!addTile.visited && !unvisitedList.Contains(addTile))
            unvisitedList.Add(addTile);
    }

    private DijkstraTile GetClosestTile()
    {
        unvisitedList.Sort();
        return unvisitedList[0];
}
}
