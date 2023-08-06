using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(fileName = "TileMoveSettings", menuName = "TileMoveSettings/CreateTileMoveSettings", order = 0)]
public class TileMoveSettingsScriptableObject : ScriptableObject
{
    public Ease Ease = Ease.OutBounce;

    public float Speed = 40f;
}