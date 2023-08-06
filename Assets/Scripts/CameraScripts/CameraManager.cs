using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Awake()
    {
        LevelManager.Instance.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance == null)
        {
            return;
        }
        
        LevelManager.Instance.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelSettings levelSettings)
    {
        transform.position = new Vector3((levelSettings.LevelColumnCount - 1) * TileProvider.Instance.GetTileHeight() / 2,
            (levelSettings.LevelRowCount - 1) * TileProvider.Instance.GetTileWidth()/ 2, transform.position.z);
    }
}