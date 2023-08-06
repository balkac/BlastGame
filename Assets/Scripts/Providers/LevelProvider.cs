using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelProvider : Singleton<LevelProvider>
{
    [SerializeField] private LevelProviderScriptableObject _levelProviderScriptableObject;
    
    private LevelSettings _levelSettings;
    public LevelSettings LevelSettings => _levelSettings;

    private List<ETile> _tileList = new List<ETile>();
    public List<ETile> TileList => _tileList;

    public bool TryGetLevelSetting(int level, out LevelSettings levelSettings)
    {
        levelSettings = default;

        foreach (LevelSettings settings in _levelProviderScriptableObject.LevelSettings)
        {
            if (settings.Level == level)
            {
                _levelSettings = settings;

                levelSettings = settings;

                CreateRandomLevelTileTypes();

                return true;
            }
        }

        return false;
    }

    private void CreateRandomLevelTileTypes()
    {
        List<ETile> tiles = Enum.GetValues(typeof(ETile))
            .Cast<ETile>()
            .ToList();

        tiles.Remove(ETile.None);

        for (int i = tiles.Count - 1; i > 0; i--)
        {
            int j = Random.Range(1, tiles.Count);
            (tiles[i], tiles[j]) = (tiles[j], tiles[i]);
        }

        for (int i = 0; i < _levelSettings.NumberOfColors; i++)
        {
            _tileList.Add(tiles[i]);
        }
    }
}