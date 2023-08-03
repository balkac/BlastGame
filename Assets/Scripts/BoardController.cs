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

            if (!hasConnectedTiles && connectedTiles.Count>=2)
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
        Debug.Log("SHUFFLE");
    }
}