using System.Collections.Generic;
using UnityEngine;

public class BoardShuffleBehaviour : MonoBehaviour
{
    private void Awake()
    {
        BoardController.Instance.OnGridPositionsChanged += OnGridPositionsChanged;
        BoardController.Instance.OnGridInitialized += OnGridInitialized;
    }

    private void OnDestroy()
    {
        if (BoardController.Instance == null)
        {
            return;
        }

        BoardController.Instance.OnGridPositionsChanged -= OnGridPositionsChanged;
        BoardController.Instance.OnGridInitialized -= OnGridInitialized;
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
        int lengthRow = BoardController.Instance.Grids.GetLength(1);

        for (int i = BoardController.Instance.Grids.Length - 1; i > 0; i--)
        {
            int i0 = i / lengthRow;
            int i1 = i % lengthRow;

            int j = Random.Range(0, i + 1);

            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            Tile tempTile = BoardController.Instance.Grids[i0, i1].Tile;
            Vector2 tempPos = BoardController.Instance.Grids[i0, i1].Position;
            int tempRow = BoardController.Instance.Grids[i0, i1].Row;
            int tempColumn = BoardController.Instance.Grids[i0, i1].Column;

            BoardController.Instance.Grids[i0, i1].Tile.ChangeGrid(BoardController.Instance.Grids[j0, j1].Column,
                BoardController.Instance.Grids[j0, j1].Row,
                BoardController.Instance.Grids[j0, j1].Position
            );
            BoardController.Instance.Grids[i0, i1].Tile = BoardController.Instance.Grids[j0, j1].Tile;

            BoardController.Instance.Grids[j0, j1].Tile.ChangeGrid(tempColumn, tempRow, tempPos);
            BoardController.Instance.Grids[j0, j1].Tile = tempTile;
        }

        // Debug.Log("SHUFFLE");

        if (!CheckConnectedTiles(BoardController.Instance.Grids))
        {
            // Debug.Log("SHUFFLE AGAIN");

            ShuffleBoard();
        }
    }
}