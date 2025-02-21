using UnityEngine;
using DG.Tweening;
using System;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public UI_Booster booster;
    public UI_Progress progress;
    public UI_HP hp;
    public UI_ComboText comboText;
    public UI_SlowTime slowTime;
    public UI_SuperBooster superBooster;
    public UI_QTEStack qteStack;
    public UI_QTECorrect qteCorrect;
    public UI_SpeedUpText speedUpText;
    public UI_Destination destination;

    void Awake()
    {
        InitProperty();
        ShowHUD();
    }

    void InitProperty()
    {
        instance = this;
    }

    public void ShowHUD()
    {
        string id = "Show";

        var sequence = DOTween.Sequence();

        sequence.InsertCallback(0f, () => hp.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id))
                .InsertCallback(0f, () => booster.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id))
                .InsertCallback(0f, () => comboText.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id))
                .InsertCallback(0f, () => progress.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id));
    }

    public void HideHUD()
    {
        string id = "Hide";

        var sequence = DOTween.Sequence();

        sequence.InsertCallback(0f, () => hp.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id))
                .InsertCallback(0f, () => booster.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id))
                .InsertCallback(0f, () => comboText.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id))
                .InsertCallback(0f, () => progress.gameObject.GetComponent<DOTweenAnimation>().DORestartById(id));
    }
}
