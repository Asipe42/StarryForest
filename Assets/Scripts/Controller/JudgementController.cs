using System;
using UnityEngine;

public class JudgementController : MonoBehaviour
{
    public static JudgementController instance;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;
    }

    public void CheckJudgement(EJudgementType type)
    {
        #region Set HP
        SetPlayerHP(type);
        #endregion

        #region Set Combo Count
        SetComboCount(type);
        #endregion

        #region Set Fever Count
        SetFeverCount(type);
        #endregion

        #region Set Time Gauge
        SetTimeGuage();
        #endregion

        #region Set Booster Guage
        SetBoosterGuage(type);
        #endregion

        #region Set QTE Stack
        SetQTEStack(type);
        #endregion

        #region Show Judgement Text
        ShowJudgementText(type);
        #endregion

        #region Show Judgement SFX
        ShowJudgementSFX(type);
        #endregion
    }

    void SetPlayerHP(EJudgementType type)
    {
        float value = JudgementDefine.HP_VALUES[type];

        PlayerController.instance.SetHP(value);

        #region Animation
        HUD.instance.hp.PlayIconAnimation();
        #endregion
    }

    void SetComboCount(EJudgementType type)
    {
        if (type < EJudgementType.Miss)
            ComboController.instance.AddComboCount();
        else
            ComboController.instance.ResetComboCount();
    }

    void SetFeverCount(EJudgementType type)
    {
        if (TimeController.instance.OnTime)
            return;

        if (SuperBoosterController.instance.OnSuperBooster)
            return;

        if (type < EJudgementType.Miss)
        {
            FeverController.instance.AddFeverCount();
        }
        else
        {
            FeverController.instance.ResetFeverCount();
        }
    }

    void SetTimeGuage()
    {
        if (FeverController.instance.OnFever)
            return;

        if (SuperBoosterController.instance.OnSuperBooster)
            return;

        TimeController.instance.SetTimeGauge(BoosterController.instance.BoosterRatio);
    }

    void SetBoosterGuage(EJudgementType type)
    {
        float value = JudgementDefine.BOOSTER_VALUES[type];

        #region Animation
        HUD.instance.booster.PlayIconAnimation();
        #endregion

        #region SpeedUp Text
        #endregion

        BoosterController.instance.SetBoosterValue(value);
    }

    void SetQTEStack(EJudgementType type)
    {
        if (!TimeController.instance.OnTime)
            return;

        if (type == EJudgementType.Perfect)
        {
            #region Vibrate
            Vibrator.Vibrate(50);
            #endregion

            TimeController.instance.QTEStack++;
        }
        else
        {
            TimeController.instance.QTEStack = 0;
            TimeController.instance.EndSlowTime();
        }
    }

    void ShowJudgementText(EJudgementType type)
    {
        JudgeTextPool.instance.SpawnJudgeText(type);
    }

    void ShowJudgementSFX(EJudgementType type)
    {
        if (type > EJudgementType.Miss)
            return;

        string clipName = SFXDefine.JUDGEMENT_CLIP[type];

        SFXHandler.instance.PlaySFX(clipName);
    }

    /*
    void StopCountdown(EJudgementType type)
    {
        if (type == EJudgementType.Miss || type == EJudgementType.Fail)
        {
            if (TimeController.instance.OnCountdown)
            {
                TimeController.instance.EndCountdown();
            }
        }
    }
    */
}
