using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FloorTemplate", menuName = "Floor/Floor Template")]
public class FloorTemplate : SerializedScriptableObject
{
    public GameObject[] floors;
}
