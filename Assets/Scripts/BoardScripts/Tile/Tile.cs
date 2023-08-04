using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public ETile TileType = ETile.None;

    public Sprite FirstIcon;

    public Sprite SecondIcon;

    public Sprite ThirdIcon;

    private Sprite _defaultSprite = null;
    public float Width => GetComponent<BoxCollider>().size.x;
    public float Height => GetComponent<BoxCollider>().size.y;

    public int Row;

    public int Column;

    public Tile Left => Column - 1 >= 0
        ? BoardGenerator.Instance.Grids[Column - 1, Row].Tile
        : null;

    public Tile Right => Column + 1 < LevelProvider.Instance.LevelSettings.LevelColumnCount
        ? BoardGenerator.Instance.Grids[Column + 1, Row].Tile
        : null;

    public Tile Top => Row + 1 < LevelProvider.Instance.LevelSettings.LevelRowCount
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


        transform.DOMoveY(BoardGenerator.Instance.Grids[Column, Row].Position.y, 40f).SetSpeedBased(true);

        BoardGenerator.Instance.Grids[Column, Row].Tile = this;

        if (Bottom == null && Row != 0)
        {
            MoveBottom();
        }
    }

    public void ChangeGrid(int column, int row, Vector2 position)
    {
        Column = column;

        Row = row;

        transform.DOMove(position, 1f);

        gameObject.name = string.Format("Tile : {0} , {1} + [SHUFFLE]", column.ToString(), row.ToString());
    }

    private void Destroy()
    {
        BoardGenerator.Instance.Grids[Column, Row].Tile = null;

        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (!BoardGenerator.Instance.CanClick)
            return;

        Debug.Log("Clicked on Tile " + Column + "," + Row);

        List<Tile> connectedTiles = GetConnectedTiles();

        if (connectedTiles.Count < 2)
        {
            return;
        }

        foreach (Tile tile in connectedTiles)
        {
            tile.Destroy();
        }

        BoardGenerator.Instance.MoveTiles();
        BoardGenerator.Instance.AddNewTiles();
    }

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
        if (connectedCount >= LevelProvider.Instance.LevelSettings.VisualCondition.FirstSpriteConditionCount
            && connectedCount < LevelProvider.Instance.LevelSettings.VisualCondition.SecondSpriteConditionCount)
        {
            GetComponent<SpriteRenderer>().sprite = FirstIcon;
        }
        else if (connectedCount >= LevelProvider.Instance.LevelSettings.VisualCondition.SecondSpriteConditionCount
                 && connectedCount < LevelProvider.Instance.LevelSettings.VisualCondition.ThirdSpiteConditionCount)
        {
            GetComponent<SpriteRenderer>().sprite = SecondIcon;
        }
        else if (connectedCount >= LevelProvider.Instance.LevelSettings.VisualCondition.ThirdSpiteConditionCount)
        {
            GetComponent<SpriteRenderer>().sprite = ThirdIcon;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = _defaultSprite;
        }
    }
}