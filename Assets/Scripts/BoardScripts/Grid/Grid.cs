using UnityEngine;

public class Grid
{
    public Grid(int column, int row, Vector2 pos)
    {
        Column = column;
        Row = row;
        Position = pos;
    }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Vector2 Position { get; private set; }
    public Tile Tile { get; set; }
}