using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ELayerType
{
    Back,
    Front,
    Floor,
}

[CreateAssetMenu(fileName = "ScrollData", menuName = "Scroll/Scroll Data")]
public class ScrollData: SerializedScriptableObject
{
    public Dictionary<ELayerType, float> scrollSpeedDic;

    public float boosterModifier = 1f;
    public float superBoosterModifier = 1f;
    public float validation = 1f;
}
