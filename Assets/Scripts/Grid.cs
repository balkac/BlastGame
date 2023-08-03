using UnityEngine;

public class Grid
{
    public Grid(int column, int row, Vector2 pos)
    {
        Column = column;
        Row = row;
        Position = pos;
    }

    public int Row;

    public int Column;

    public Vector2 Position;

    public Tile Tile = null;
    
}