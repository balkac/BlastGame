using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    [SerializeField] private ETile _tileType = ETile.None;

    [SerializeField] private Sprite _firstIcon;

    [SerializeField] private Sprite _secondIcon;

    [SerializeField] private Sprite _thirdIcon;
    public ETile TileType => _tileType;
    public int Row { get; private set; }
    public int Column { get; private set; }

    public float Width => GetComponent<BoxCollider>().size.x;
    public float Height => GetComponent<BoxCollider>().size.y;

    public Tile Left => Column - 1 >= 0
        ? BoardController.Instance.Grids[Column - 1, Row].Tile
        : null;

    public Tile Right => Column + 1 < LevelProvider.Instance.LevelSettings.LevelColumnCount
        ? BoardController.Instance.Grids[Column + 1, Row].Tile
        : null;

    public Tile Top => Row + 1 < LevelProvider.Instance.LevelSettings.LevelRowCount
        ? BoardController.Instance.Grids[Column, Row + 1].Tile
        : null;

    public Tile Bottom => Row - 1 >= 0
        ? BoardController.Instance.Grids[Column, Row - 1].Tile
        : null;

    private Tile[] Neighbours => new[]
    {
        Left,
        Right,
        Top,
        Bottom,
    };
    
    private Sprite _defaultSprite = null;

    private Tween _initMoveTween = null;

    private Tween _moveBottomTween = null;
    
    private Tween _shuffleTween = null;
    
    private bool _isShuffling = false;

    private Dictionary<int, Sprite> _conditionsToSprites = new Dictionary<int, Sprite>();
    public void Init(int column, int row, Vector3 position)
    {
        gameObject.name = string.Format("Tile : {0} , {1}", column.ToString(), row.ToString());

        Column = column;
        Row = row;

        _conditionsToSprites.Add(LevelProvider.Instance.LevelSettings.VisualCondition.FirstSpriteConditionCount,_firstIcon);
        _conditionsToSprites.Add(LevelProvider.Instance.LevelSettings.VisualCondition.SecondSpriteConditionCount,_secondIcon);
        _conditionsToSprites.Add(LevelProvider.Instance.LevelSettings.VisualCondition.ThirdSpiteConditionCount,_thirdIcon);
        _defaultSprite = GetComponent<SpriteRenderer>().sprite;
        
        transform.position = position + 40 * Vector3.up;

        _initMoveTween = transform.DOMove(position, 1f).SetEase(TileMoveSettings.Instance.GetTileMoveEase()).OnComplete(() =>
        {
            _initMoveTween = null;
        });
    }

    public void MoveBottom()
    {
        _initMoveTween?.Kill();

        _initMoveTween = null;
        
        _moveBottomTween?.Kill();

        _moveBottomTween = null;

        BoardController.Instance.Grids[Column, Row].Tile = null;

        Row -= 1;

        gameObject.name = string.Format("Tile : {0} , {1}", Column.ToString(), Row.ToString());

        BoardController.Instance.Grids[Column, Row].Tile = this;
        
        _moveBottomTween = transform.DOMoveY(BoardController.Instance.Grids[Column, Row].Position.y,
            TileMoveSettings.Instance.GetTileMoveSpeed()).SetSpeedBased(true).OnComplete(
            () =>
            {
                _moveBottomTween = null;
            });

        if (Bottom == null && Row != 0)
        {
            MoveBottom();
        }
    }
    
    public void ChangeGrid(int column, int row, Vector2 position)
    {
        Column = column;

        Row = row;

        _isShuffling = true;
        
        _shuffleTween?.Kill();
            
        _shuffleTween = transform.DOMove(position, 1f).OnComplete(() =>
        {
            _isShuffling = false;

            _shuffleTween = null;
        });

        gameObject.name = string.Format("Tile : {0} , {1}", column.ToString(), row.ToString());
    }

    private void Destroy()
    {
        BoardController.Instance.Grids[Column, Row].Tile = null;

        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (_isShuffling)
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

        BoardController.Instance.MoveTiles();
        BoardController.Instance.AddNewTiles();
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
            if (neighbour == null || tiles.Contains(neighbour) || neighbour._tileType != _tileType)
            {
                continue;
            }

            result.AddRange(neighbour.GetConnectedTiles(tiles));
        }

        return result;
    }

    public void ChangeSprite(int connectedCount)
    {
        Sprite sprite = _defaultSprite;
        
        foreach (KeyValuePair<int,Sprite> conditionToSprite in _conditionsToSprites)
        {
            if (connectedCount > conditionToSprite.Key)
            {
                _conditionsToSprites.TryGetValue(conditionToSprite.Key ,out Sprite conditionSprite);

                sprite = conditionSprite;
            }
        }

        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}