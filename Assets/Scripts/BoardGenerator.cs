using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoardGenerator : Singleton<BoardGenerator>
{
    private BoardController _boardController;
    
    public List<Tile> TilePrefabs;

    public int M;

    public int N;

    public Grid[,] Grids;

    private float _xdiff;

    private float _ydiff;

    public Action<Grid[,]> OnGridPositionsChanged;
    public Action<Grid[,]> OnGridInitialized;
    private void Awake()
    {
        _xdiff = TilePrefabs[0].X;
        _ydiff = TilePrefabs[0].Y;

        Grids = new Grid[N, M];

        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Vector2 gridPosition = new Vector2(j * _ydiff, i * _xdiff);

                //Grid Generation

                Grid grid = new Grid(j, i, gridPosition);

                Grids[j, i] = grid;

                //Tile Generation

                AddTile(j, i);
            }
        }
        
        OnGridInitialized?.Invoke(Grids);
    }

    private GameObject GetRandomTile()
    {
        int random = Random.Range(0, TilePrefabs.Count);

        GameObject go = Instantiate(TilePrefabs[random].gameObject, transform);

        return go;
    }

    private void AddTile(int column, int row, bool isNew = false)
    {
        GameObject go = GetRandomTile();

        Grids[column, row].Tile = go.GetComponent<Tile>();

        go.transform.position = Grids[column, row].Position;

        if (isNew)
        {
            go.name = string.Format("Tile : {0} , {1} + [NEW]", column.ToString(), row.ToString());
        }
        else
        {
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
                AddTile(grid.Column,grid.Row,true);
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