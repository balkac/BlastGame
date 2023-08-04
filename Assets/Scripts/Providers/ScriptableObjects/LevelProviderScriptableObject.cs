using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Goal
{
    public ETile TileType;
    public int Count;
}

[Serializable]
public class VisualCondition
{
    public int FirstSpriteConditionCount;
    public int SecondSpriteConditionCount;
    public int ThirdSpiteConditionCount;
}

[Serializable]
public class LevelSettings
{
    public int Level;

    [Range(2, 10)] public int LevelRowCount = 2;
    [Range(2, 10)] public int LevelColumnCount = 2;

    public VisualCondition VisualCondition;

    [Range(2, 6)] public int NumberOfColors = 2;

    public int MovesCount;
    
    public List<Goal> Goals;
}

[CreateAssetMenu(fileName = "LevelSettings", menuName = "LevelSettings/CreateLevelSettings", order = 0)]
public class LevelProviderScriptableObject : ScriptableObject
{
    public List<LevelSettings> LevelSettings;
}