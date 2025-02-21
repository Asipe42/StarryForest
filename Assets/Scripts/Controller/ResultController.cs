using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ResultController : MonoBehaviour
{
    [SerializeField]
    Image panel;

    [SerializeField]
    Image sign;

    [SerializeField]
    RectTransform chapterCard, mushroomCard;

    [SerializeField]
    TextMeshProUGUI chapterCardName, mushroomCardName, mushroomCardValueText;

    [SerializeField]
    Image chpaterCardIcon, mushroomCardIcon;

    [SerializeField]
    Slider chapterCardSlider;

    [SerializeField]
    Sprite cardFront;

    [SerializeField]
    GameObject fail;

    public static ResultController instance;

    void Awake()
    {
        instance = this;
    }

    [ContextMenu("Test")]
    public void ShowResultDirecting()
    {
        StartCoroutine(ShowResultDirectingLogic());
    }

    IEnumerator ShowResultDirectingLogic()
    {
        #region Show Panel
        panel.gameObject.SetActive(true);
        #endregion

        yield return new WaitForSeconds(0.1f);

        #region Hide UI
        HUD.instance.HideHUD();
        ActionController.instance.HideCover();
        FeverController.instance.HideCover();
        TimeController.instance.HideCover();
        #endregion

        #region Show Sign
        sign.gameObject.SetActive(true);
        #endregion

        yield return new WaitForSeconds(4f);

        fail.SetActive(true);

        /*
        #region Show Card
        PlayAppearSequence();
        #endregion

        yield return new WaitForSeconds(10f);

        ScreenEffect.instance.PlayScreen(ScreenDefine.FADE);

        yield return new WaitForSeconds(2.5f);

        GameManager.instance.GoToLobby();
        */
    }

    void PlayAppearSequence()
    {
        var appearSequence = DOTween.Sequence();

        float intervalTime = 0.3f;
        float[] cardPositionX = { -380, 380 };

        appearSequence.OnStart(() => SFXHandler.instance.PlaySFX(SFXDefine.RESULT));

        appearSequence.Append(chapterCard.DOAnchorPosX(cardPositionX[0], 1.5f).SetEase(Ease.OutQuint))
                      .Insert(intervalTime, mushroomCard.DOAnchorPosX(cardPositionX[1], 1.5f).SetEase(Ease.OutQuint));

        appearSequence.OnComplete(() => PlayRotateSequence());
    }

    void PlayRotateSequence()
    {
        var rotateSequence = DOTween.Sequence();

        float intervalTime = 2.75f;

        rotateSequence.Append(chapterCard.DORotate(new Vector3(0f, 180f, 0f), 0.5f, RotateMode.FastBeyond360)
                                         .SetEase(Ease.InSine)
                                         .OnPlay(() => SFXHandler.instance.PlaySFX(SFXDefine.ROTATION))
                                         .OnUpdate(() => ShowCardInfo(chapterCard)))
                      .Insert(intervalTime, mushroomCard.DORotate(new Vector3(0f, 180f, 0f), 0.5f, RotateMode.FastBeyond360)
                                                        .SetEase(Ease.InSine)
                                                        .OnPlay(() => SFXHandler.instance.PlaySFX(SFXDefine.ROTATION))
                                                        .OnUpdate(() => ShowCardInfo(mushroomCard)));
    }

    void ShowCardInfo(RectTransform card)
    {
        if (card.eulerAngles.y > 90f)
        {
            card.GetComponent<Image>().sprite = cardFront;

            for (int i = 0; i < card.childCount; i++)
            {
                card.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void SetProgressSlider()
    {
        chapterCardSlider.DOValue(ProgressController.instance.ProgressRatio, 2f);
    }

    public void SetMushroomCount()
    {
        mushroomCardValueText.DOCounter(0, GameManager.instance.saveData.mushroom, 2f);
    }
}
