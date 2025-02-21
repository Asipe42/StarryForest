using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

[Serializable]
public class TimeData
{
    public int qteStackGoal;
    public float countdownGoal;
    public float maxTimeGauge;
}

[RequireComponent(typeof(ObservableEventTrigger))]
public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    [SerializeField]
    TimeData timeData;

    [SerializeField]
    ParticleSystem disableParticle, enableParticle;

    [SerializeField]
    Volume volume;
    
    [SerializeField]
    VolumeProfile defaultVP, slowTimeVP;

    [SerializeField]
    DOTweenAnimation coverAnimation;

    [SerializeField]
    Image fillAmountCover;

    static float targetTimeScale = 0.5f;
    public float TargetTimeScale
    {
        get => targetTimeScale;
        set => targetTimeScale = value;
    }

    bool onTime;
    public bool OnTime
    {
        get => onTime;
        set => onTime = value;
    }

    ReactiveProperty<float> timeGuage;
    public float TimeGauge
    {
        get => timeGuage.Value;
        set => timeGuage.Value = value;
    }

    ReactiveProperty<int> qteStack;
    public int QTEStack
    {
        get => qteStack.Value;
        set => qteStack.Value = value;
    }

    static float defaultTimeScale;
    static float defaultFixedDeltaTime;

    float currentTime;

    Coroutine countdownCoroutine;

    ObservableEventTrigger eventTrigger;

    void Awake()
    {
        instance = this;

        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;

        qteStack = new ReactiveProperty<int>();
        timeGuage = new ReactiveProperty<float>();

        qteStack.Subscribe(value => CheckQTEStack(value));
        timeGuage.Subscribe(value => CheckTimeGauge(value));

        eventTrigger = GetComponent<ObservableEventTrigger>();
        eventTrigger.OnPointerDownAsObservable().Subscribe(_ => PlaySlowTime());
    }

    void CheckTimeGauge(float value)
    {
        #region Fill Amount
        fillAmountCover.fillAmount = value / timeData.maxTimeGauge;
        #endregion

        #region Effect
        if (value >= timeData.maxTimeGauge)
        {
            if (!enableParticle.isPlaying)
                enableParticle.Play();

            disableParticle.Stop();
        }
        else
        {
            if (!disableParticle.isPlaying)
                disableParticle.Play();

            enableParticle.Stop();
        }
        #endregion
    }

    void PlaySlowTime()
    {
        if (timeGuage.Value >= timeData.maxTimeGauge)
        {
            if (FeverController.instance.OnFever)
                return;

            if (SuperBoosterController.instance.OnSuperBooster)
                return;

            FeverController.instance.ResetFeverGauge();
            ResetTimeGauge();

            StartSlowTime();
        }
    }

    public void ResetTimeGauge()
    {
        timeGuage.Value = 0;
    }

    /*
    public void StartCountdown()
    {
        ResetTimeGauge();

        onCountdown = true;

        countdownCoroutine = StartCoroutine(CountDownLogic());
    }

    public void EndCountdown()
    {
        onCountdown = false;

        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
    }

    IEnumerator CountDownLogic()
    {
        currentTime = 0f;

        timeData.countdownGoal = 15f;

        while (currentTime < timeData.countdownGoal)
        {
            currentTime += Time.deltaTime;

            yield return null;
        }

        StartSlowTime();
    }
    */

    IEnumerator SetChromaticAbberationIntensity()
    {
        ChromaticAberration ca;
        slowTimeVP.TryGet<ChromaticAberration>(out ca);

        float intensity = 0f;

        while (intensity < 0.4f)
        {
            intensity += 0.01f;

            ca.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ResetChromaticAbberationIntensity()
    {
        ChromaticAberration ca;
        slowTimeVP.TryGet<ChromaticAberration>(out ca);

        float intensity = (float)ca.intensity;

        while (intensity > 0)
        {
            intensity -= 0.01f;

            ca.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        volume.profile = defaultVP;
    }

    void StartSlowTime()
    {
        onTime = true;

        #region Change VP
        volume.profile = slowTimeVP;
        StartCoroutine(SetChromaticAbberationIntensity());
        #endregion

        #region Change Gate State
        GameManager.instance.ChangeInGameState(EInGameState.SlowTime);
        #endregion

        #region Show Guide
        HUD.instance.qteStack.ShowQTEStackGuide();
        #endregion

        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * targetTimeScale;
    }

    public void EndSlowTime()
    {
        if (onTime)
        {
            onTime = false;

            #region Change VP
            StartCoroutine(ResetChromaticAbberationIntensity());
            #endregion

            #region Init QTE Stack
            qteStack.Value = 0;
            #endregion

            #region Change Gate State
            GameManager.instance.ChangeInGameState(EInGameState.Default);
            #endregion

            #region Hide Guide
            HUD.instance.qteStack.HideQTEStackGuide();
            #endregion

            #region Reset QTE Stack Text
            HUD.instance.qteStack.ResetQTEStackText();
            #endregion

            Time.timeScale = defaultTimeScale;
            Time.fixedDeltaTime = defaultFixedDeltaTime;
        }
    }

    void CheckQTEStack(float value)
    {
        timeData.qteStackGoal = 5;

        #region Set QTE Stack Text
        HUD.instance.qteStack.SetQTEStackText(value, timeData.qteStackGoal);
        #endregion

        if (timeData.qteStackGoal <= value)
        {
            EndSlowTime();

            QTEController.instance.StartQTE();
        }
    }

    public void SetTimeGauge(float value)
    {
        if (value >= 0.5f)
        {
            TimeGauge += value;
        }
    }

    public void ShowCover()
    {
        string id = "Show";

        coverAnimation.DORestartById(id);
    }

    public void HideCover()
    {
        string id = "Hide";

        coverAnimation.DORestartById(id);
    }
}
