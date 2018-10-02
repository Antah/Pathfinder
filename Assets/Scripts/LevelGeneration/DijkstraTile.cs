using System;

public class DijkstraTile : IComparable<DijkstraTile>
{
    public Tile tile;
    public int weight, distanceToStart;
    public DijkstraTile parent;
    public bool visited;

    public DijkstraTile(Tile t, int w, int d)
    {
        tile = t;
        weight = w;
        distanceToStart = d;
        parent = null;
        visited = false;
    }

    public int CompareTo(DijkstraTile other)
    {
        if (distanceToStart > other.distanceToStart) return 1;
        if (distanceToStart == other.distanceToStart) return 0;
        return -1;
    }
}
