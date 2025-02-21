using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ScreenEffect : SerializedMonoBehaviour
{
    public static ScreenEffect instance;

    [SerializeField] 
    Dictionary<string, Image> screenImages;

    void Awake()
    {
        instance = this;
    }

    public void PlayScreen(string name)
    {
        screenImages[name].gameObject.SetActive(true);
    }

    public void EndScreen(string name)
    {
        screenImages[name].gameObject.SetActive(false);
    }
}
