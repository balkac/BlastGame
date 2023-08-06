using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private int _loadLevel = 1;   
    public Action<LevelSettings> OnLevelLoaded { get; set; }

    private void Start()
    {
        if (LevelProvider.Instance.TryGetLevelSetting(_loadLevel, out LevelSettings levelSettings))
        {
            OnLevelLoaded?.Invoke(levelSettings);
        }
    }
}