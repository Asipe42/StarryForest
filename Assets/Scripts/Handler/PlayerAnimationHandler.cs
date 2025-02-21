using System.Collections;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] 
    float walkToRunRatio;

    [SerializeField] 
    Animator animator;
    
    public float WalkToRunRatio
    {
        get => walkToRunRatio;
        set => walkToRunRatio = value;
    }

    string currentState;

    bool onFix;

    public void ChangeAnimationState(string newState, bool fix = false, float fixDuration = 0f)
    {
        if (onFix)
            return;

        #region Set Fixed
        onFix = fix;

        if (onFix)
            StartCoroutine(ReleaseFix(fixDuration));
        #endregion

        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    public void ChangeAnimationState(EActionType actionType, bool fix = false, float fixDuration = 0f)
    {
        if (onFix)
            return;

        #region Set Fixed
        onFix = fix;

        if (onFix)
            StartCoroutine(ReleaseFix(fixDuration));
        #endregion

        string newState = ConvertActionTypeToStateName(actionType);

        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    IEnumerator ReleaseFix(float duration)
    {
        yield return new WaitForSeconds(duration);

        onFix = false;
    }

    public bool CheckNoneAnimation()
    {
        return (currentState == AnimationDefine.STATE_WALK || currentState == AnimationDefine.STATE_RUN) ? true : false;
    }

    string ConvertActionTypeToStateName(EActionType actionType)
    {
        string stateName = "";

        switch (actionType)
        {
            case EActionType.Default:
                stateName = CheckWalkOrRun();
                break;
            case EActionType.Jump:
                stateName = AnimationDefine.STATE_JUMP;
                break;
            case EActionType.Slide:
                stateName = AnimationDefine.STATE_SLIDE;
                break;
            case EActionType.Downhill:
                stateName = AnimationDefine.STATE_DOWNHILL;
                break;
        }

        return stateName;
    }

    string CheckWalkOrRun()
    {
        if (!CheckGround())
            return currentState;

        string stateName = AnimationDefine.STATE_WALK;

        if (BoosterController.instance.BoosterRatio > walkToRunRatio)
        {
            stateName = AnimationDefine.STATE_RUN;
            SetRunAnimationSpeed(BoosterController.instance.BoosterRatio);
        }

        return stateName;
    }

    bool CheckGround()
    {
        return PlayerController.instance.playerCollider.OnGround.Value ? true : false;
    }

    void SetRunAnimationSpeed(float ratio)
    {
        animator.SetFloat(AnimationDefine.PARAMETER_RUN_MULTIFLIER, 1f + ((3f * ratio) - walkToRunRatio));
    }
}
