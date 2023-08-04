using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TileProvider", menuName = "TileProvider/CreateTileProvider", order = 0)]
public class TileProviderScriptableObject : ScriptableObject
{
    public List<Tile> Tiles;
}