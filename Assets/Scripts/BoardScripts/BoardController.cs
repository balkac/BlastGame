using System;
using UnityEngine;

public class BoardController : Singleton<BoardController>
{
    private float _tileWidth;

    private float _tileHeight;
    public Grid[,] Grids { private set; get; }

    public Action<Grid[,]> OnGridPositionsChanged;
    public Action<Grid[,]> OnGridInitialized;

    public void CreateBoard(LevelSettings levelSettings)
    {
        _tileWidth = TileProvider.Instance.GetTileWidth();
        _tileHeight = TileProvider.Instance.GetTileHeight();

        Grids = new Grid[levelSettings.LevelColumnCount, levelSettings.LevelRowCount];

        for (int i = 0; i < levelSettings.LevelRowCount; i++)
        {
            for (int j = 0; j < levelSettings.LevelColumnCount; j++)
            {
                Vector2 gridPosition = new Vector2(j * _tileHeight, i * _tileWidth);

                //Grid Generation

                Grid grid = new Grid(j, i, gridPosition);

                Grids[j, i] = grid;

                //Tile Generation

                AddTile(j, i);
            }
        }

        OnGridInitialized?.Invoke(Grids);
    }

    private void AddTile(int column, int row)
    {
        GameObject go = TileProvider.Instance.GetRandomTile();

        Grids[column, row].Tile = go.GetComponent<Tile>();

        go.GetComponent<Tile>().Init(column, row, Grids[column, row].Position);
    }

    public void AddNewTiles()
    {
        foreach (Grid grid in Grids)
        {
            if (grid.Tile == null)
            {
                AddTile(grid.Column, grid.Row);
            }
        }

        OnGridPositionsChanged?.Invoke(Grids);
    }

    public void MoveTiles()
    {
        foreach (Grid grid in Grids)
        {
            if (grid.Tile != null && grid.Tile.Bottom == null && grid.Tile.Row != 0)
            {
                grid.Tile.MoveBottom();
            }
        }
    }
}