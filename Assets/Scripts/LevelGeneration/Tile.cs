using System;

public enum TileValue { Floor = 0, OuterWall = 9, Obstacle = 1 }

public class Tile
{
    public int x;
    public int y;
    public TileValue value;

    public Tile(int x, int y, TileValue v)
    {
        this.x = x;
        this.y = y;
        value = v;
    }
}
