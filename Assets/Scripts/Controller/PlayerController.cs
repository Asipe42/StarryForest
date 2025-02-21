using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum EHitState
{
    KnockDown,
    StandUp,
    Retry
}

public enum EFallState
{
    ComeUp,
    StandUp,
    Retry,
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public PlayerAnimationHandler playerAnimation { get; private set; }
    public PlayerMovementHandler playerMovement { get; private set; }
    public PlayerCollideHandler playerCollider { get; private set; }
    public PlayerData playerData { get; private set; }

    Rigidbody2D rigid;

    Vector3 defaultPos;
    float defaultGravityScale;

    bool onInvincible;
    bool onHit;
    bool onFall;

    EHitState currentHitState;
    EFallState currentFallState;

    float hpDecreaseModifier = 1f;

    void Awake()
    {
        InitProperty();
    }

    void Start()
    {
        DecreaseHP();
    }

    void InitProperty()
    {
        instance = this;

        playerAnimation = GetComponent<PlayerAnimationHandler>();
        playerMovement = GetComponent<PlayerMovementHandler>();
        playerCollider = GetComponent<PlayerCollideHandler>();
        rigid = GetComponent<Rigidbody2D>();

        playerData = new PlayerData();

        defaultPos = transform.position;
        defaultGravityScale = rigid.gravityScale;
    }

    void Update()
    {
        CheckHP();
    }

    void CheckHP()
    {
        #region Clamp HP
        playerData.hp = Mathf.Clamp(playerData.hp, 0, playerData.maxHP);
        #endregion

        #region Interlock HP Slider
        HUD.instance.hp.SetHpSliderValue(playerData.hp);
        #endregion

        if (playerData.hp <= 0)
        {
            CheckLife(playerData.life);
        }
    }

    void RecoverHP()
    {
        if (playerData.life > 0)
        {
            playerData.life--;
            playerData.hp = playerData.maxHP;

            HUD.instance.hp.SetIconFillAmount(playerData.life, playerData.maxLife);
        }
    }

    void CheckLife(int value)
    {
        if (value <= 0)
            StartCoroutine(Dead());
        else
            RecoverHP();
    }

    [ContextMenu("Test")]
    void Test()
    {
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        if (onHit || onFall)
        {
            yield return new WaitUntil(() => !onHit && !onFall);

            #region On Invincible
            onInvincible = true;
            #endregion

            #region No Gravity
            SetGravityScale(0f);
            #endregion

            yield return new WaitForSeconds(0.5f);
        }

        #region On Invincible
        onInvincible = true;
        #endregion

        #region Inhibit Action
        ActionController.instance.CurrentKeyState = EActionKeyState.Inhibition;
        #endregion

        #region Stop Scrolling
        ScrollController.instance.StopScrolling();
        #endregion

        #region Blood Screen
        ScreenEffect.instance.PlayScreen(ScreenDefine.BLOOD);
        #endregion

        #region Animation
        playerAnimation.ChangeAnimationState(AnimationDefine.STATE_HIT, true, 100f);
        #endregion

        #region GameOver
        GameManager.instance.gameData.inGameState.Value = EInGameState.GameOver;
        #endregion
    }

    public void DecreaseHP()
    {
        StartCoroutine(DecreaseHPLogic(0.5f, 0.1f));
    }

    IEnumerator DecreaseHPLogic(float value, float cooltime)
    {
        while (playerData.hp > 0)
        {
            playerData.hp -= value * hpDecreaseModifier;

            yield return new WaitForSeconds(cooltime);
        }
    }

    public void Hit(float damage, string clipName)
    {
        if (onInvincible)
            return;

        onHit = true;

        #region On Invincible
        onInvincible = true;
        #endregion

        #region Vibrate
        Vibrator.Vibrate(50);
        #endregion

        #region Decrease HP
        playerData.hp -= damage;
        #endregion

        #region Play SFX
        SFXHandler.instance.PlaySFX(clipName);
        #endregion

        #region Set Judgement
        JudgementController.instance.CheckJudgement(EJudgementType.Fail);
        #endregion

        SetHitState(EHitState.KnockDown);
    }

    public void Fall(float damage, string clipName)
    {
        if (onInvincible)
            return;

        onFall = true;

        #region Vibrate
        Vibrator.Vibrate(50);
        #endregion

        #region Decrease HP
        playerData.hp -= damage;
        #endregion

        #region Play SFX
        SFXHandler.instance.PlaySFX(clipName);
        #endregion

        #region Set Judgement
        JudgementController.instance.CheckJudgement(EJudgementType.Fail);
        #endregion

        SetFallState(EFallState.ComeUp);
    }

    void SetHitState(EHitState state)
    {
        currentHitState = state;

        CheckHitState();
    }

    void SetFallState(EFallState state)
    {
        currentFallState = state;

        CheckFallState();
    }

    void CheckHitState()
    {
        switch (currentHitState)
        {
            case EHitState.KnockDown:
                KnockDown();
                break;
            case EHitState.StandUp:
                StandUp();
                break;
            case EHitState.Retry:
                Retry();
                break;
        }
    }

    void CheckFallState()
    {
        switch (currentFallState)
        {
            case EFallState.ComeUp:
                ComeUp();
                break;
            case EFallState.StandUp:
                StandUp();
                break;
            case EFallState.Retry:
                Retry();
                break;
        }
    }

    void KnockDown()
    {
        StartCoroutine(KnockDownLogic());
    }

    IEnumerator KnockDownLogic()
    {
        float waitTime = 3f;

        #region Inhibit Action
        ActionController.instance.CurrentKeyState = EActionKeyState.Inhibition;
        #endregion

        #region Stop Scrolling
        ScrollController.instance.StopScrolling();
        #endregion

        #region Blood Screen
        ScreenEffect.instance.PlayScreen(ScreenDefine.BLOOD);
        #endregion

        #region Knockback
        playerMovement.Knockback();
        #endregion

        #region Animation
        playerAnimation.ChangeAnimationState(AnimationDefine.STATE_HIT, true, waitTime);
        #endregion

        yield return new WaitForSeconds(waitTime);

        SetHitState(EHitState.StandUp);
    }

    void StandUp()
    {
        StartCoroutine(StandUpLogic());
    }

    IEnumerator StandUpLogic()
    {
        float waitTime = 2f;
        float delay = 0.5f;
        float duration = 1.5f;

        #region Animation
        playerAnimation.ChangeAnimationState(AnimationDefine.STATE_STANDUP, true, waitTime);
        #endregion

        #region Comback
        StartCoroutine(ComebackLogic(delay, duration));
        #endregion

        yield return new WaitForSeconds(waitTime);

        SetHitState(EHitState.Retry);
    }

    void ComeUp()
    {
        StartCoroutine(ComeUpLogic());
    }

    IEnumerator ComeUpLogic()
    {
        float waitTime = 4f;
        float delay = 1f;
        float duration = 0.1f;

        #region On Invincible
        onInvincible = true;
        #endregion

        #region Inhibit Action
        ActionController.instance.CurrentKeyState = EActionKeyState.Inhibition;
        #endregion

        #region Stop Scrolling
        ScrollController.instance.StopScrolling();
        #endregion

        #region Animation
        playerAnimation.ChangeAnimationState(AnimationDefine.STATE_HIT, true, waitTime);
        #endregion

        #region Comback
        StartCoroutine(ComebackLogic(delay, duration));
        #endregion

        yield return new WaitForSeconds(waitTime);

        SetFallState(EFallState.StandUp);
    }

    void Retry()
    {
        #region Off Invincible
        StartCoroutine(SetInvincibileDuration(4f));
        #endregion

        #region Animation
        playerAnimation.ChangeAnimationState(AnimationDefine.STATE_WALK);
        #endregion

        #region Disable Action
        ActionController.instance.CurrentKeyState = EActionKeyState.Disable;
        #endregion

        #region Play Scrolling
        ScrollController.instance.PlayScrolling();
        #endregion

        onHit = false;
        onFall = false;
    }

    IEnumerator ComebackLogic(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        transform.DOMove(defaultPos, duration).SetEase(Ease.OutCubic);
    }

    public void SetInvincible(bool state)
    {
        onInvincible = state;
    }

    public IEnumerator SetInvincibileDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        onInvincible = false;
    }

    public void SetGravityScale(float value)
    {
        rigid.gravityScale = value;
    }

    public void InitGravityScale()
    {
        rigid.gravityScale = defaultGravityScale;
    }

    public void SetHP(float value)
    {
        playerData.hp += value;
    }

    public Vector3 GetPlayerPos()
    {
        Vector3 pos = transform.position;

        return pos;
    }

    public void SetHPDecreaseModifier(float value)
    {
        hpDecreaseModifier = value;
    }
}
