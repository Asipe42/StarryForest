using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UI_Destination : SerializedMonoBehaviour
{
    [SerializeField]
    Image left, right;

    [SerializeField]
    Dictionary<EChapterType, Sprite> chapterIcon;

    void Start()
    {
        SetIcon(GameManager.instance.gameData.chapterType);
    }

    public void SetIcon(EChapterType currentType)
    {
        left.sprite = chapterIcon[currentType];
        right.sprite = chapterIcon[(EChapterType)((int)currentType + 1)];
    }
}
