using System;
using System.Collections;
using UnityEngine;
using UniRx;

[Serializable]
public class BoosterData
{
    public float maxValue;
    public float slowTimeConditionRatio;
}

public class BoosterController : MonoBehaviour
{
    public static BoosterController instance;

    [SerializeField]
    BoosterData boosterData;

    ReactiveProperty<float> boosterValue;
    public float BoosterValue
    {
        get => boosterValue.Value;
        set
        {
            boosterValue.Value = value;
            boosterValue.Value = Mathf.Clamp(boosterValue.Value, 1f, boosterData.maxValue);
        }
    }

    float boosterRatio;
    public float BoosterRatio
    {
        get
        {
            boosterRatio = (BoosterValue - 1f) / (boosterData.maxValue - 1f);
            return boosterRatio;
        }
        private set => boosterRatio = value;
    }

    bool onCountdown;
    public bool OnCountdown
    {
        get => onCountdown;
        set => onCountdown = value;
    }

    void Start()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        boosterValue = new ReactiveProperty<float>(1f);

        boosterValue.Subscribe(value => BoosterCallback(value));
    }

    void BoosterCallback(float value)
    {
        SetBoosterSliderValue(value);
        SetScrollModifier(value);
        SetScreenParticleModifier(value);
        SetBoosterParticle();
        CheckSpeedUpTextCondition();
    }

    void CheckSpeedUpTextCondition()
    {
        HUD.instance.speedUpText.ShowSpeedUpText(BoosterRatio);
    }

    void SetBoosterSliderValue(float value)
    {
        HUD.instance.booster.SetSliderValue(BoosterRatio);

        HUD.instance.booster.SetSecondaryFillAmount(BoosterRatio);
    }

    void SetScrollModifier(float value)
    {
        ScrollController.instance.SetScrollModifier(value);
    }

    void SetScreenParticleModifier(float value)
    {
        ScreenParticleController.instance.SetParticleModifier(value);
    }

    void SetBoosterParticle()
    {
        HUD.instance.booster.SetBoosterParticle(BoosterRatio);
    }

    /*
    void CheckSlowTimeCondition()
    {
        if (FeverController.instance.OnFever)
            return;

        if (SuperBoosterController.instance.OnSuperBooster)
            return;

        if (boosterData.slowTimeConditionRatio < BoosterRatio)
        {
            if (onCountdown)
                return;

            onCountdown = true;

            #region Start Countdown
            TimeController.instance.StartCountdown();
            #endregion

            #region Show Icon
            HUD.instance.slowTime.ShowIcon();
            #endregion
        }
        else
        {
            if (!onCountdown)
                return;

            onCountdown = false;

            #region End Countdown
            TimeController.instance.EndCountdown();
            #endregion

            #region End Slow Time
            TimeController.instance.EndSlowTime();
            #endregion

            #region Hide Icon
            HUD.instance.slowTime.HideIcon();
            #endregion
        }
    }
    */

    public void SetBoosterValue(float value)
    {
        BoosterValue += value;
    }

    public void ResetBoosterGradually()
    {
        float value = 0.001f;

        StartCoroutine(ResetBoosterGraduallyLogic(value));
    }

    IEnumerator ResetBoosterGraduallyLogic(float value)
    {
        float modifier = 1f;
        float cooltime = 1f;
        float time = 0f;

        while (boosterValue.Value > 0.95f)
        {
            boosterValue.Value -= value * modifier;

            time += Time.deltaTime;

            if (time > cooltime)
            {
                time = 0;
                cooltime++;
                modifier++;
            }

            yield return null;
        }

        SuperBoosterController.instance.EndSuperBooster();
    }

    public float GetMaxValue()
    {
        return boosterData.maxValue;
    }
}
