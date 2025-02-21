using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(ObservableEventTrigger))]
public class SuperBoosterController : MonoBehaviour
{
    public static SuperBoosterController instance;

    [SerializeField] 
    Image leftCover, rightCover;

    [SerializeField]
    Image leftPanel, rightPanel;

    [SerializeField]
    ObservableEventTrigger eventTriggerLeft, eventTriggerRight;

    [SerializeField]
    DOTweenAnimation leftCoverAnimation, rightCoverAnimation;

    [SerializeField]
    SpriteRenderer background;

    bool onTouchTweenLeft, onTouchTweenRight;

    bool onSuperBooster;
    public bool OnSuperBooster
    {
        get => onSuperBooster;
        set => onSuperBooster = value;
    }

    enum ESuperBoosterTouchType
    {
        None,
        Left,
        Right
    }

    ESuperBoosterTouchType touchType = ESuperBoosterTouchType.None;

    void Awake()
    {
        instance = this;

        eventTriggerLeft.OnPointerDownAsObservable().Subscribe(eventData => LeftTouch());
        eventTriggerRight.OnPointerDownAsObservable().Subscribe(eventData => RightTouch());
    }

    void LeftTouch()
    {
        if (touchType == ESuperBoosterTouchType.None || touchType == ESuperBoosterTouchType.Right)
        {
            if (!onTouchTweenLeft)
            {
                leftCover.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0f), 0.2f, vibrato: 5).SetEase(Ease.OutBounce)
                   .OnStart(() => onTouchTweenLeft = true)
                   .OnComplete(() => onTouchTweenLeft = false);

                touchType = ESuperBoosterTouchType.Left;

                BoosterController.instance.SetBoosterValue(0.05f);

                SFXHandler.instance.PlaySFX(SFXDefine.POP_1);
            }
        }
    }

    void RightTouch()
    {
        if (touchType == ESuperBoosterTouchType.None || touchType == ESuperBoosterTouchType.Left)
        {
            if (!onTouchTweenRight)
            {
                rightCover.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0f), 0.2f, vibrato: 5).SetEase(Ease.OutBounce)
                   .OnStart(() => onTouchTweenRight = true)
                   .OnComplete(() => onTouchTweenRight = false);

                touchType = ESuperBoosterTouchType.Right;

                BoosterController.instance.SetBoosterValue(0.05f);

                SFXHandler.instance.PlaySFX(SFXDefine.POP_2);
            }
        }
    }

    public void StartSuperBooster()
    {
        onSuperBooster = true;

        #region Input Enable
        leftPanel.raycastTarget = true;
        rightPanel.raycastTarget = true;
        #endregion

        #region Animation
        AppearFever();
        #endregion

        #region Change Game State
        GameManager.instance.ChangeInGameState(EInGameState.Fever);
        #endregion

        #region ScreenEffect
        ScreenEffect.instance.PlayScreen(ScreenDefine.FOCUS);
        #endregion

        #region Cover Animation
        leftCoverAnimation.DORestartById("Show");
        rightCoverAnimation.DORestartById("Show");
        #endregion

        #region Set HP Decrease Modifier
        PlayerController.instance.SetHPDecreaseModifier(0f);
        #endregion

        #region Set HP Particle
        HUD.instance.hp.SetHPParticle(true);
        #endregion

        #region Set Scroll Modifier
        ScrollController.instance.SetSuperBoosterModifier(1.25f);
        #endregion

        #region Set Invincible
        PlayerController.instance.SetInvincible(true);
        #endregion

        #region Set Gravity
        PlayerController.instance.SetGravityScale(0f);
        #endregion

        #region Inhibit Action Key
        ActionController.instance.CurrentKeyState = EActionKeyState.Inhibition;
        #endregion

        #region Decrease Booster Gauge
        BoosterController.instance.ResetBoosterGradually();
        #endregion
    }

    public void EndSuperBooster()
    {
        onSuperBooster = false;

        StartCoroutine(PlayerController.instance.SetInvincibileDuration(1f));

        #region Show Button
        ActionController.instance.ShowCover();
        FeverController.instance.ShowCover();
        TimeController.instance.ShowCover();
        #endregion

        #region Input Disable
        leftPanel.raycastTarget = false;
        rightPanel.raycastTarget = false;
        #endregion

        #region Animation
        DisappearFever();
        #endregion

        #region Reset HP Decrease Modifier
        PlayerController.instance.SetHPDecreaseModifier(1f);
        #endregion
 
        #region Reset HP Particle
        HUD.instance.hp.SetHPParticle(false);
        #endregion

        #region Reset Scroll Modifier
        ScrollController.instance.SetSuperBoosterModifier(1f);
        #endregion

        #region Cover Animation
        leftCoverAnimation.DORestartById("Hide");
        rightCoverAnimation.DORestartById("Hide");
        #endregion

        #region Change Game State
        GameManager.instance.ChangeInGameState(EInGameState.Default);
        #endregion

        #region ScreenEffect
        ScreenEffect.instance.EndScreen(ScreenDefine.FOCUS);
        #endregion

        #region Set Invincible
        PlayerController.instance.SetInvincible(false);
        #endregion

        #region Set Gravity
        PlayerController.instance.InitGravityScale();
        #endregion

        #region Inhibit Action Key
        ActionController.instance.CurrentKeyState = EActionKeyState.Disable;
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
}
