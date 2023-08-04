using UnityEngine;

public class TileProvider : Singleton<TileProvider>
{
    [SerializeField] private TileProviderScriptableObject _tileProvider;
    private GameObject GetTilePrefab(ETile tileType)
    {
        foreach (Tile tile in _tileProvider.Tiles)
        {
            if (tile.TileType == tileType)
            {
                return tile.gameObject;
            }
        }

        return null;
    }

    public float GetTileWidth()
    {
        return _tileProvider.Tiles[0].Width;
    }

    public float GetTileHeight()
    {
        return _tileProvider.Tiles[0].Height;
    }

    public GameObject GetRandomTile()
    {
        int random = Random.Range(0, LevelProvider.Instance.TileList.Count);

        GameObject go = Instantiate(GetTilePrefab(LevelProvider.Instance.TileList[random]), transform);

        return go;
    }
}