using System;
using DG.Tweening;
using UnityEngine;

public class BoardGenerator : Singleton<BoardGenerator>
{
    public Ease Ease = Ease.Linear;

    public float Speed = 50f;

    private BoardController _boardController;

    public Grid[,] Grids;

    public bool CanClick = true;

    private float _tileWidth;

    private float _tileHeight;

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

    private void AddTile(int column, int row, bool isNew = false)
    {
        GameObject go = TileProvider.Instance.GetRandomTile();

        Grids[column, row].Tile = go.GetComponent<Tile>();

        if (isNew)
        {
            CanClick = false;

            go.transform.position = Grids[column, row].Position + 40 * Vector2.up;

            go.transform.DOMove(Grids[column, row].Position, Speed).SetSpeedBased(true).SetEase(Ease).OnComplete(() =>
            {
                CanClick = true;
            });

            go.name = string.Format("Tile : {0} , {1} + [NEW]", column.ToString(), row.ToString());
        }
        else
        {
            go.transform.position = Grids[column, row].Position;

            go.name = string.Format("Tile : {0} , {1}", column.ToString(), row.ToString());
        }

        go.GetComponent<Tile>().Init(column, row);
    }

    public void AddNewTiles()
    {
        foreach (Grid grid in Grids)
        {
            if (grid.Tile == null)
            {
                AddTile(grid.Column, grid.Row, true);
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