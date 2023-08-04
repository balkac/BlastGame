using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private void Awake()
    {
        BoardGenerator.Instance.OnGridPositionsChanged += OnGridPositionsChanged;
        BoardGenerator.Instance.OnGridInitialized += OnGridInitialized;
    }

    private void OnDestroy()
    {
        BoardGenerator.Instance.OnGridPositionsChanged -= OnGridPositionsChanged;
        BoardGenerator.Instance.OnGridInitialized -= OnGridInitialized;
    }

    private void OnGridInitialized(Grid[,] grids)
    {
        if (!CheckConnectedTiles(grids))
        {
            ShuffleBoard();
        }
    }

    private void OnGridPositionsChanged(Grid[,] grids)
    {
        if (!CheckConnectedTiles(grids))
        {
            ShuffleBoard();
        }
    }

    private bool CheckConnectedTiles(Grid[,] grids)
    {
        List<Tile> controlledConnectedTiles = new List<Tile>();

        bool hasConnectedTiles = false;

        foreach (Grid grid in grids)
        {
            if (controlledConnectedTiles.Contains(grid.Tile))
            {
                continue;
            }

            List<Tile> connectedTiles = grid.Tile.GetConnectedTiles();

            if (!hasConnectedTiles && connectedTiles.Count >= 2)
            {
                hasConnectedTiles = true;
            }

            controlledConnectedTiles.AddRange(connectedTiles);

            foreach (Tile tile in connectedTiles)
            {
                tile.ChangeSprite(connectedTiles.Count);
            }
        }

        return hasConnectedTiles;
    }

    private void ShuffleBoard()
    {
        int lengthRow = BoardGenerator.Instance.Grids.GetLength(1);

        for (int i = BoardGenerator.Instance.Grids.Length - 1; i > 0; i--)
        {
            int i0 = i / lengthRow;
            int i1 = i % lengthRow;

            int j = Random.Range(0, i + 1);

            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            Tile tempTile = BoardGenerator.Instance.Grids[i0, i1].Tile;
            Vector2 tempPos = BoardGenerator.Instance.Grids[i0, i1].Position;
            int tempRow = BoardGenerator.Instance.Grids[i0, i1].Row;
            int tempColumn = BoardGenerator.Instance.Grids[i0, i1].Column;

            BoardGenerator.Instance.Grids[i0, i1].Tile.ChangeGrid(BoardGenerator.Instance.Grids[j0, j1].Column,
                BoardGenerator.Instance.Grids[j0, j1].Row,
                BoardGenerator.Instance.Grids[j0, j1].Position
            );
            BoardGenerator.Instance.Grids[i0, i1].Tile = BoardGenerator.Instance.Grids[j0, j1].Tile;

            BoardGenerator.Instance.Grids[j0, j1].Tile.ChangeGrid(tempColumn, tempRow, tempPos);
            BoardGenerator.Instance.Grids[j0, j1].Tile = tempTile;
        }

        Debug.Log("SHUFFLE");

        if (!CheckConnectedTiles(BoardGenerator.Instance.Grids))
        {
            Debug.Log("SHUFFLE AGAIN");

            ShuffleBoard();
        }
    }
}