using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public ETile TileType = ETile.None;

    public Sprite Sprite1;
    public Sprite Sprite2;
    private Sprite _defaultSprite = null;
    public float X => GetComponent<BoxCollider>().size.x;
    public float Y => GetComponent<BoxCollider>().size.y;

    [FormerlySerializedAs("_row")] public int Row;
    [FormerlySerializedAs("_column")] public int Column;

    public Tile Left => Column - 1 >= 0
        ? BoardGenerator.Instance.Grids[Column - 1, Row].Tile
        : null;

    public Tile Right => Column + 1 < BoardGenerator.Instance.N
        ? BoardGenerator.Instance.Grids[Column + 1, Row].Tile
        : null;

    public Tile Top => Row + 1 < BoardGenerator.Instance.M
        ? BoardGenerator.Instance.Grids[Column, Row + 1].Tile
        : null;

    public Tile Bottom => Row - 1 >= 0
        ? BoardGenerator.Instance.Grids[Column, Row - 1].Tile
        : null;

    public Tile[] Neighbours => new[]
    {
        Left,
        Right,
        Top,
        Bottom,
    };

    public void Init(int column, int row)
    {
        Column = column;
        Row = row;
        _defaultSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void MoveBottom()
    {
        BoardGenerator.Instance.Grids[Column, Row].Tile = null;

        Row -= 1;
        
        transform.DOMoveY(BoardGenerator.Instance.Grids[Column, Row].Position.y, 0.1f);

        BoardGenerator.Instance.Grids[Column, Row].Tile = this;

        if (Bottom == null && Row !=0)
        {
            MoveBottom();
        }
    }

    private void Destroy()
    {
        BoardGenerator.Instance.Grids[Column, Row].Tile = null;
        
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on Tile " + Column + "," + Row);

        List<Tile> connectedTiles = GetConnectedTiles();

        foreach (Tile tile in connectedTiles)
        {
            tile.Destroy();
        }

        BoardGenerator.Instance.MoveTiles();
        BoardGenerator.Instance.AddNewTiles();
    }

    // private void Update()
    // {
    //     if (Bottom == null && Row != 0)
    //     {
    //         MoveBottom();
    //     }
    // }

    public List<Tile> GetConnectedTiles(List<Tile> tiles = null)
    {
        var result = new List<Tile>() { this, };

        if (tiles == null)
        {
            tiles = new List<Tile> { this, };
        }
        else
        {
            tiles.Add(this);
        }

        foreach (Tile neighbour in Neighbours)
        {
            if (neighbour == null || tiles.Contains(neighbour) || neighbour.TileType != TileType)
            {
                continue;
            }

            result.AddRange(neighbour.GetConnectedTiles(tiles));
        }

        return result;
    }

    public void ChangeSprite(int connectedCount)
    {
        if (connectedCount >2 && connectedCount <5)
        {
            GetComponent<SpriteRenderer>().sprite = Sprite1;
            return;
        }

        if (connectedCount >= 5)
        {
            GetComponent<SpriteRenderer>().sprite = Sprite2;
            return;
        }
        
        GetComponent<SpriteRenderer>().sprite = _defaultSprite;
    }
}