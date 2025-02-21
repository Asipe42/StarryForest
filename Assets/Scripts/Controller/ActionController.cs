using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public enum EActionType
{
    Default,
    Jump,
    Slide,
    Downhill
}

public enum EActionKeyState
{
    Inhibition,
    Disable,
    Enable
}

[RequireComponent(typeof(ObservableEventTrigger))]
public class ActionController : MonoBehaviour
{
    public static ActionController instance;

    [SerializeField]
    DOTweenAnimation coverAnimation;

    EActionType currentActionType;
    public EActionType CurrentActionType
    {
        get => currentActionType;
        set => currentActionType = value;
    }

    EActionKeyState currentKeyState;
    public EActionKeyState CurrentKeyState
    {
        get => currentKeyState;
        set => currentKeyState = value;
    }

    JudgementCircleController currentJudgeCircle;
    public JudgementCircleController CurrentJudgeCircle
    {
        get => currentJudgeCircle;
        set => currentJudgeCircle = value;
    }

    bool validJudge;
    public bool ValidJudge
    {
        get => validJudge;
        set => validJudge = value;
    }

    ObservableEventTrigger eventTrigger;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        currentActionType = EActionType.Jump;
        currentKeyState = EActionKeyState.Disable;

        eventTrigger = GetComponent<ObservableEventTrigger>();
        eventTrigger.OnPointerDownAsObservable().Subscribe(_ => PlayAction());
    }

    void PlayAction()
    {
        if (currentKeyState == EActionKeyState.Inhibition)
            return;

        #region Movement
        PlayerController.instance.playerMovement.ExcuteAction(currentActionType);
        #endregion

        #region Animation
        PlayerController.instance.playerAnimation.ChangeAnimationState(currentActionType);
        #endregion

        #region Judge
        if (currentKeyState == EActionKeyState.Enable)
        {
            if (validJudge)
            {
                validJudge = false;

                EJudgementType judgeType = currentJudgeCircle.Judge();

                JudgementController.instance.CheckJudgement(judgeType);
            }
        }
        #endregion
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
