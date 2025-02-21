using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



[CreateAssetMenu(fileName = "QTEData", menuName = "QTE/QTE Data")]
public class QTEData : SerializedScriptableObject
{
    public Dictionary<int, EQTEInputType[]> QTEPatterns;
    public int maxCount;
}
