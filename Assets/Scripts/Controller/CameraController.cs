using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public enum ECameraType
{
    Main,
    QTE
}

public class CameraController : SerializedMonoBehaviour
{
    public static CameraController instance;

    [SerializeField] 
    Dictionary<ECameraType, CinemachineVirtualCamera> cameras;

    [SerializeField] 
    Dictionary<ECameraType, int[]> cameraPriority;

    void Awake()
    {
        instance = this;
    }

    public void ConvertCamera(ECameraType previous, ECameraType next)
    {
        cameras[previous].Priority = cameraPriority[previous][0];
        cameras[next].Priority = cameraPriority[next][1];
    }
}
