using DG.Tweening;
using UnityEngine;

public class TileMoveSettings : Singleton<TileMoveSettings>
{
    [SerializeField] private TileMoveSettingsScriptableObject _tileMoveSettingsScriptableObject;

    public Ease GetTileMoveEase()
    {
        return _tileMoveSettingsScriptableObject.Ease;
    }

    public float GetTileMoveSpeed()
    {
        return _tileMoveSettingsScriptableObject.Speed;
    }
}