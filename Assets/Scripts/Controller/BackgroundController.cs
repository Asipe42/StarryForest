using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

[Serializable]
public class BackgroundData
{
    public EChapterType chapterType;
    public Sprite[] layer;
}

public class BackgroundController : SerializedMonoBehaviour
{
    public static BackgroundController instance;

    [SerializeField]
    SpriteRenderer[][] layer;

    [SerializeField]
    BackgroundData[] backgroundData;

    [SerializeField] 
    Image top, bottom;

    Dictionary<EChapterType, Sprite[]> spriteDic;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        #region Init Dictionary
        spriteDic = new Dictionary<EChapterType, Sprite[]>();

        foreach (var data in backgroundData)
            spriteDic.Add(data.chapterType, data.layer);
        #endregion
    }

    public void ConvertBackground(EChapterType chapterType)
    {
        var sequence = DOTween.Sequence();

        float slideInDuration = 1.25f;
        float slideOutDuration = 1.0f;
        float cooltime = 2f;

        sequence.AppendCallback(() => SlideIn(slideInDuration))
                .InsertCallback(slideInDuration + cooltime / 3f, () => ChangeBackgroundSprite(chapterType))
                .InsertCallback(slideInDuration + cooltime / 3f, () => Popup.instance.chapterTitle.SetChapterTitleText(chapterType))
                .InsertCallback(slideInDuration + cooltime, () => SlideOut(slideOutDuration))
                .AppendCallback(() => FloorController.instance.ReplaceFloor(chapterType))
                .AppendCallback(() => HUD.instance.destination.SetIcon(chapterType));
    }

    void SlideIn(float duration)
    {
        Vector2 target = new Vector2(0f, 720f);

        #region Stop Scrolling
        ScrollController.instance.StopScrolling();
        #endregion

        top.rectTransform.DOSizeDelta(target, duration).SetEase(Ease.InQuad);
        bottom.rectTransform.DOSizeDelta(target, duration).SetEase(Ease.InSine);
    }
    
    void ChangeBackgroundSprite(EChapterType type)
    {
        for (int i = 0; i < layer.Length; i++)
        {
            for (int j = 0; j < layer[i].Length; j++)
            {
                layer[i][j].sprite = spriteDic[type][i];
            }
        }
    }

    void SlideOut(float duration)
    {
        Vector2 target = new Vector2(0f, 0f);

        #region Play Scrolling
        ScrollController.instance.PlayScrolling();
        #endregion

        top.rectTransform.DOSizeDelta(target, duration).SetEase(Ease.OutQuad);
        bottom.rectTransform.DOSizeDelta(target, duration).SetEase(Ease.OutQuad);
    }
}
