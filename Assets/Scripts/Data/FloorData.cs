using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FloorData", menuName = "Floor/Floor Data")]
public class FloorData: SerializedScriptableObject
{
    public Dictionary<EChapterType, Dictionary<EDifficultyType, FloorTemplate>> candidate;

    public Dictionary<EDifficultyType, float> difficultyTable;

    public Dictionary<EChapterType, GameObject> defaultFloors;
}
