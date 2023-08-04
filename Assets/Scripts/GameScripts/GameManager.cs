public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        LevelManager.Instance.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelSettings levelSettings)
    {
        StartGame(levelSettings);
    }

    private void StartGame(LevelSettings levelSettings)
    {
        BoardGenerator.Instance.CreateBoard(levelSettings);
    }
}