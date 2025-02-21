using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(ObservableEventTrigger))]
public class FeverController : MonoBehaviour
{
    public static FeverController instance;

    public float feverDuration;

    ReactiveProperty<bool> onFever;
    public bool OnFever
    {
        get => onFever.Value;
        set => onFever.Value = value;
    }

    ReactiveProperty<int> feverCount;
    ReactiveProperty<int> feverGauge;

    [SerializeField]
    SpriteRenderer background;

    [SerializeField]
    ParticleSystem disableParticle, enableParticle;

    [SerializeField]
    Image fillAmountCover;

    [SerializeField]
    int maxFeverGauge, maxFeverCount;

    [SerializeField]
    DOTweenAnimation coverAnimation;

    ObservableEventTrigger eventTrigger;

    void Start()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        onFever = new ReactiveProperty<bool>();
        feverCount = new ReactiveProperty<int>();
        feverGauge = new ReactiveProperty<int>();

        onFever.Subscribe(state => CheckFeverState(state));
        feverGauge.Subscribe(value => CheckFeverGauge(value));
        feverCount.Subscribe(value => CheckFeverCount(value));

        eventTrigger = GetComponent<ObservableEventTrigger>();
        eventTrigger.OnPointerDownAsObservable().Subscribe(_ => PlayFever());
    }

    void CheckFeverState(bool state)
    {
        if (state)
            OnFeverMode();
        else
            OffFeverMode();
    }

    void CheckFeverGauge(int value)
    {
        #region Fill Amount
        fillAmountCover.fillAmount = (float)value / (float)maxFeverGauge;
        #endregion

        #region Effect
        if (value >= maxFeverGauge)
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

    void CheckFeverCount(int value)
    {
        if (onFever.Value)
            return;

        if (value >= maxFeverCount)
        {
            int comboCount = ComboController.instance.GetComboCount();

            feverGauge.Value += (int)((float)comboCount / maxFeverCount);

            if (feverGauge.Value >= maxFeverGauge)
            {
                feverGauge.Value = maxFeverGauge;
            }

            ResetFeverCount();
        }
    }

    public void ResetFeverGauge()
    {
        feverGauge.Value = 0;
    }

    void PlayFever()
    {
        if (feverGauge.Value >= maxFeverGauge)
        {
            if (TimeController.instance.OnTime)
                return;

            if (QTEController.instance.OnQTE)
                return;

            if (SuperBoosterController.instance.OnSuperBooster)
                return;

            TimeController.instance.ResetTimeGauge();

            onFever.Value = true;
        }
    }

    void OnFeverMode()
    {
        #region Change State
        GameManager.instance.ChangeInGameState(EInGameState.Fever);
        #endregion

        #region Animation
        AppearFever();
        #endregion

        #region Decrease Fever Count
        StartCoroutine(DecreaseFeverGauge());
        #endregion

        #region Cehck Duration
        StartCoroutine(CheckDuration());
        #endregion
    }

    void AppearFever()
    {
        var sequence = DOTween.Sequence();

        Vector3 targetPos = Vector3.zero;
        float duration = 0.7f;

        sequence.Append(background.DOFade(1f, 0.2f))
                .Append(background.transform.DOLocalMove(targetPos, duration).SetEase(Ease.OutSine))
                .AppendCallback(() => Popup.instance.feverText.Appear());
    }

    void DisappearFever()
    {
        var sequence = DOTween.Sequence();

        Vector3 originPos = new Vector3(32f, 0f, 0f);
        float duration = 1f;

        sequence.Append(background.DOFade(0f, duration));
        sequence.OnComplete(() => background.transform.localPosition = originPos);
    }

    IEnumerator DecreaseFeverGauge()
    {
        while (feverGauge.Value > 0)
        {
            feverGauge.Value--;
            yield return new WaitForSeconds(feverDuration / maxFeverGauge);
        }
    }

    IEnumerator CheckDuration()
    {
        float currentTime = Time.time;

        while (onFever.Value)
        {
            if (currentTime + feverDuration < Time.time)
                onFever.Value = false;

            yield return new WaitForEndOfFrame();
        }
    }

    void OffFeverMode()
    {
        #region Change State
        GameManager.instance.ChangeInGameState(EInGameState.Default);
        #endregion

        #region Animation
        DisappearFever();
        #endregion
    }

    public void AddFeverCount()
    {
        feverCount.Value++;
    }

    public void ResetFeverCount()
    {
        feverCount.Value = 0;
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
