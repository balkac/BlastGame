using System;

public class LevelManager : Singleton<LevelManager>
{
    public Action<LevelSettings> OnLevelLoaded { get; set; }

    private void Start()
    {
        if (LevelProvider.Instance.TryGetLevelSetting(1, out LevelSettings levelSettings))
        {
            OnLevelLoaded?.Invoke(levelSettings);
        }
    }
}