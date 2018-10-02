using System;

public class AStarTile : IComparable<AStarTile>
{
    public Tile tile;
    public int startScore, finishScore;
    public AStarTile parent;

    public AStarTile(Tile t)
    {
        tile = t;
    }
    public int CompareTo(AStarTile other)
    {
        if (startScore + finishScore > other.startScore + other.finishScore) return 1;
        if (startScore + finishScore == other.startScore + other.finishScore)
        {
            if (finishScore > other.finishScore) return 1;
            if (finishScore == other.finishScore) return 0;
        }
        return -1;
    }
}
